using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Commands;

public record CreateCasCommand(
    int UcenikId,
    int PredavacId,
    int PredmetId,
    DateTime Datum,
    TimeSpan VremePocetka,
    TimeSpan VremeZavrsetka)
    : IRequest<Cas?>;

public class CreateCasCommandHandler
    : IRequestHandler<CreateCasCommand, Cas?>
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hubContext;

    public CreateCasCommandHandler(
        IUnitOfWork uow,
        IHubContext<NotificationHub> hubContext)
    {
        _uow = uow;
        _hubContext = hubContext;
    }

    public async Task<Cas?> Handle(
        CreateCasCommand request,
        CancellationToken cancellationToken)
    {
        var postojiTermin =
            await _uow.Casovi.PostojiTermin(
                request.PredavacId,
                request.Datum,
                request.VremePocetka,
                request.VremeZavrsetka);

        if (postojiTermin)
            return null;

        var cas = new Cas
        {
            UcenikId = request.UcenikId,
            PredavacId = request.PredavacId,
            PredmetId = request.PredmetId,
            Datum = request.Datum,
            VremePocetka = request.VremePocetka,
            VremeZavrsetka = request.VremeZavrsetka,
            Status = StatusCasa.NaCekanju
        };

        await _uow.Casovi.AddAsync(cas);

        await _uow.SaveChangesAsync();

        var connectionId =
            UserConnections.Connections
                .GetValueOrDefault(
                    request.PredavacId);

        if (connectionId != null)
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    "Imate novi zahtev za čas.");
        }

        return cas;
    }
}