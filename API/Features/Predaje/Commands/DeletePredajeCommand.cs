using MediatR;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predaje.Commands;

public record DeletePredajeCommand(
    int PredavacId,
    int PredmetId)
    : IRequest<bool>;

public class DeletePredajeCommandHandler
    : IRequestHandler<DeletePredajeCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeletePredajeCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(
    DeletePredajeCommand request,
    CancellationToken cancellationToken)
    {
        var postojiCas =
            await _uow.Casovi
                .PostojiCasZaPredmet(
                    request.PredavacId,
                    request.PredmetId);

        if (postojiCas)
            return false;

        var predaje =
            await _uow.Predaje
                .GetPredavanje(
                    request.PredavacId,
                    request.PredmetId);

        if (predaje == null)
            return false;

        _uow.Predaje.Delete(predaje);

        await _uow.SaveChangesAsync();

        return true;
    }
}