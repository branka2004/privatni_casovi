using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predmeti.Queries;

public class GetAllPredmetiQuery
    : IRequest<IEnumerable<PredmetDto>>
{
}

public class GetAllPredmetiQueryHandler
    : IRequestHandler<
        GetAllPredmetiQuery,
        IEnumerable<PredmetDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllPredmetiQueryHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<PredmetDto>>
        Handle(
            GetAllPredmetiQuery request,
            CancellationToken cancellationToken)
    {
        var predmeti =
            await _uow.Predmeti.GetAllAsync();

        return predmeti.Select(x =>
            new PredmetDto
            {
                PredmetId = x.PredmetId,
                Naziv = x.Naziv,
                Oblast = x.Oblast
            });
    }
}