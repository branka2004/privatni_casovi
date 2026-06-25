using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predavaci.Queries;

public record GetAllPredavaciQuery()
    : IRequest<List<PredavacDto>>;

public class GetAllPredavaciQueryHandler
    : IRequestHandler<GetAllPredavaciQuery, List<PredavacDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPredavaciQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PredavacDto>> Handle(
        GetAllPredavaciQuery request,
        CancellationToken cancellationToken)
    {
        var predavaci =
            await _unitOfWork.Predavaci.GetAllAsync();

        return predavaci
            .Select(x => new PredavacDto
            {
                KorisnikId = x.KorisnikId,
                Ime = x.Ime,
                Prezime = x.Prezime,
                Email = x.Email,
                Biografija = x.Biografija,
                CenaPoSatu = x.CenaPoSatu
            })
            .ToList();
    }
}