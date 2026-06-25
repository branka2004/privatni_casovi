using MediatR;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predaje.Commands;

public record CreateNoviPredmetCommand(
    int PredavacId,
    string Naziv,
    string Oblast,
    int GodineIskustva,
    int Nivo)
    : IRequest<bool>;

public class CreateNoviPredmetCommandHandler
    : IRequestHandler<CreateNoviPredmetCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public CreateNoviPredmetCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(
        CreateNoviPredmetCommand request,
        CancellationToken cancellationToken)
    {
        var predmet = new Predmet
        {
            Naziv = request.Naziv,
            Oblast = request.Oblast
        };

        await _uow.Predmeti.AddAsync(predmet);

        await _uow.SaveChangesAsync();

        var predaje =
            new PrivateLessons.Domain.Entities.Predaje
            {
                PredavacId = request.PredavacId,
                PredmetId = predmet.PredmetId,
                GodineIskustva =
                    request.GodineIskustva,
                Nivo =
                    (NivoPredmeta)request.Nivo
            };

        await _uow.Predaje.AddAsync(predaje);

        await _uow.SaveChangesAsync();

        return true;
    }
}