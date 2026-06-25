using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Commands;

public record OznaciKaoOdrzanCommand(
    int CasId)
    : IRequest<bool>;

public class OznaciKaoOdrzanCommandHandler
    : IRequestHandler<OznaciKaoOdrzanCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<NotificationHub> _hubContext;

    public OznaciKaoOdrzanCommandHandler(
        IUnitOfWork uow,
        IHubContext<NotificationHub> hubContext)
    {
        _uow = uow;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(
        OznaciKaoOdrzanCommand request,
        CancellationToken cancellationToken)
    {
        var cas =
            await _uow.Casovi.GetByIdAsync(
                request.CasId);

        if (cas == null)
            return false;

        cas.Status =
            StatusCasa.Odrzan;

        await _uow.SaveChangesAsync();

        var connectionId =
            UserConnections.Connections
                .GetValueOrDefault(
                    cas.UcenikId);

        if (connectionId != null)
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    "Vaš čas je označen kao održan.");
        }

        return true;
    }
}