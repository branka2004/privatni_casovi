using MediatR;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Commands;

public record DeletePredmetCommand(
    int PredmetId)
    : IRequest<bool>;

public class DeletePredmetCommandHandler
    : IRequestHandler<
        DeletePredmetCommand,
        bool>
{
    private readonly IUnitOfWork _uow;

    public DeletePredmetCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(
        DeletePredmetCommand request,
        CancellationToken cancellationToken)
    {
        var predmet =
            await _uow.Predmeti.GetByIdAsync(
                request.PredmetId);

        if (predmet == null)
            return false;

        _uow.Predmeti.Delete(predmet);

        await _uow.SaveChangesAsync();

        return true;
    }
}