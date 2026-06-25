using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Commands;

public record OtkaziCasCommand(
    int CasId,
    string Otkazao)
    : IRequest<bool>;

public class OtkaziCasCommandHandler
    : IRequestHandler<OtkaziCasCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hubContext;

    public OtkaziCasCommandHandler(
        IUnitOfWork uow,
        IHubContext<NotificationHub> hubContext)
    {
        _uow = uow;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(
        OtkaziCasCommand request,
        CancellationToken cancellationToken)
    {
        var cas =
            await _uow.Casovi.GetByIdAsync(
                request.CasId);

        if (cas == null)
            return false;

        cas.Status = StatusCasa.Otkazan;

        await _uow.SaveChangesAsync();

        int korisnikZaNotifikaciju;
        string poruka;

        if (request.Otkazao == "Ucenik")
        {
            korisnikZaNotifikaciju =
                cas.PredavacId;

            poruka =
                "Učenik je otkazao čas.";
        }
        else
        {
            korisnikZaNotifikaciju =
                cas.UcenikId;

            poruka =
                "Predavač je otkazao čas.";
        }

        var connectionId =
            UserConnections.Connections
                .GetValueOrDefault(
                    korisnikZaNotifikaciju);

        if (connectionId != null)
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    poruka);
        }

        return true;
    }
}