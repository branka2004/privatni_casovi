using MediatR;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Commands;

public class CreatePredmetCommand
    : IRequest<int>
{
    public string Naziv { get; set; } = string.Empty;

    public string Oblast { get; set; } = string.Empty;
}

public class CreatePredmetCommandHandler
    : IRequestHandler<CreatePredmetCommand, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePredmetCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(
        CreatePredmetCommand request,
        CancellationToken cancellationToken)
    {
        var predmet =
            new Predmet
            {
                Naziv = request.Naziv,
                Oblast = request.Oblast
            };

        await _uow.Predmeti.AddAsync(predmet);

        await _uow.SaveChangesAsync();

        return predmet.PredmetId;
    }
}