using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Queries;

public record GetPredmetiZaPredavacaQuery(
    int PredavacId)
    : IRequest<List<PredmetDto>>;

public class GetPredmetiZaPredavacaQueryHandler
    : IRequestHandler<
        GetPredmetiZaPredavacaQuery,
        List<PredmetDto>>
{
    private readonly IPredavacRepository _repository;

    public GetPredmetiZaPredavacaQueryHandler(
        IPredavacRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PredmetDto>> Handle(
        GetPredmetiZaPredavacaQuery request,
        CancellationToken cancellationToken)
    {
        var predaje =
            await _repository.GetPredmete(
                request.PredavacId);

        return predaje
            .Select(x => new PredmetDto
            {
                PredmetId = x.Predmet.PredmetId,
                Naziv = x.Predmet.Naziv,
                Oblast = x.Predmet.Oblast
            })
            .ToList();
    }
}