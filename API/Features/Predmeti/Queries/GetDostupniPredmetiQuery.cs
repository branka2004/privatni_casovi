using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Queries;

public record GetDostupniPredmetiQuery(
    int PredavacId)
    : IRequest<List<PredmetDto>>;

public class GetDostupniPredmetiQueryHandler
    : IRequestHandler<
        GetDostupniPredmetiQuery,
        List<PredmetDto>>
{
    private readonly IUnitOfWork _uow;

    public GetDostupniPredmetiQueryHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<PredmetDto>> Handle(
        GetDostupniPredmetiQuery request,
        CancellationToken cancellationToken)
    {
        var sviPredmeti =
            await _uow.Predmeti.GetAllAsync();

        var mojiPredmeti =
            (await _uow.Predaje.GetAllAsync())
            .Where(x => x.PredavacId == request.PredavacId)
            .Select(x => x.PredmetId)
            .ToList();

        return sviPredmeti
            .Where(x => !mojiPredmeti.Contains(x.PredmetId))
            .Select(x => new PredmetDto
            {
                PredmetId = x.PredmetId,
                Naziv = x.Naziv,
                Oblast = x.Oblast
            })
            .ToList();
    }
}