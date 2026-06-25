using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Commands;

public record OdbijCasCommand(
    int CasId)
    : IRequest<bool>;

public class OdbijCasCommandHandler
    : IRequestHandler<OdbijCasCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hubContext;

    public OdbijCasCommandHandler(
        IUnitOfWork uow,
        IHubContext<NotificationHub> hubContext)
    {
        _uow = uow;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(
        OdbijCasCommand request,
        CancellationToken cancellationToken)
    {
        var cas =
            await _uow.Casovi.GetByIdAsync(
                request.CasId);

        if (cas == null)
            return false;

        cas.Status =
            StatusCasa.Odbijen;

        await _uow.SaveChangesAsync();

        if (UserConnections.Connections.TryGetValue(
            cas.UcenikId,
            out var connectionId))
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    "Vaš zahtev za čas je odbijen.");
        }

        return true;
    }
}