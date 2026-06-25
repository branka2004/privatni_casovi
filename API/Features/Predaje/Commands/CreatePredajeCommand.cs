using MediatR;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predaje.Commands;

public record CreatePredajeCommand(
    int PredavacId,
    int PredmetId,
    int GodineIskustva,
    int Nivo)
    : IRequest<bool>;

public class CreatePredajeCommandHandler
    : IRequestHandler<CreatePredajeCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public CreatePredajeCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(
        CreatePredajeCommand request,
        CancellationToken cancellationToken)
    {
        var postoji =
            (await _uow.Predaje.GetAllAsync())
            .Any(x =>
                x.PredavacId == request.PredavacId &&
                x.PredmetId == request.PredmetId);

        if (postoji)
            return false;

        var predaje = new PrivateLessons.Domain.Entities.Predaje
        {
            PredavacId = request.PredavacId,
            PredmetId = request.PredmetId,
            GodineIskustva = request.GodineIskustva,
            Nivo = (NivoPredmeta)request.Nivo
        };

        await _uow.Predaje.AddAsync(predaje);

        await _uow.SaveChangesAsync();

        return true;
    }
}