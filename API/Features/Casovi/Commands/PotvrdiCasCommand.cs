using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Commands;

public record PotvrdiCasCommand(
    int CasId)
    : IRequest<bool>;

public class PotvrdiCasCommandHandler
    : IRequestHandler<PotvrdiCasCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hubContext;

    public PotvrdiCasCommandHandler(
        IUnitOfWork uow,
        IHubContext<NotificationHub> hubContext)
    {
        _uow = uow;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(
        PotvrdiCasCommand request,
        CancellationToken cancellationToken)
    {
        var cas =
            await _uow.Casovi.GetByIdAsync(
                request.CasId);

        if (cas == null)
            return false;

        cas.Status =
            StatusCasa.Zakazan;

        await _uow.SaveChangesAsync();

        if (UserConnections.Connections.TryGetValue(
            cas.UcenikId,
            out var connectionId))
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    "Vaš čas je potvrđen.");
        }

        return true;
    }
}