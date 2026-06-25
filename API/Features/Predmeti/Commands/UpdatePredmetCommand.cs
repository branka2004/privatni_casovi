using MediatR;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Commands;

public record UpdatePredmetCommand(
    int PredmetId,
    string Naziv,
    string Oblast)
    : IRequest<bool>;

public class UpdatePredmetCommandHandler
    : IRequestHandler<
        UpdatePredmetCommand,
        bool>
{
    private readonly IUnitOfWork _uow;

    public UpdatePredmetCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(
        UpdatePredmetCommand request,
        CancellationToken cancellationToken)
    {
        var predmet =
            await _uow.Predmeti.GetByIdAsync(
                request.PredmetId);

        if (predmet == null)
            return false;

        predmet.Naziv = request.Naziv;
        predmet.Oblast = request.Oblast;

        await _uow.SaveChangesAsync();

        return true;
    }
}