using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predavaci.Queries;

public record GetPredavaciZaPredmetQuery(
    int PredmetId)
    : IRequest<List<PredavacDto>>;

public class GetPredavaciZaPredmetQueryHandler
    : IRequestHandler<
        GetPredavaciZaPredmetQuery,
        List<PredavacDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPredavaciZaPredmetQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PredavacDto>> Handle(
        GetPredavaciZaPredmetQuery request,
        CancellationToken cancellationToken)
    {
        var predavanja =
            await _unitOfWork.Predavaci
                .GetZaPredmet(
                    request.PredmetId);

        return predavanja
            .Select(x => new PredavacDto
            {
                KorisnikId =
                    x.Predavac.KorisnikId,

                Ime =
                    x.Predavac.Ime,

                Prezime =
                    x.Predavac.Prezime,

                Email =
                    x.Predavac.Email,

                Biografija =
                    x.Predavac.Biografija,

                CenaPoSatu =
                    x.Predavac.CenaPoSatu,

                GodineIskustva =
                    x.GodineIskustva,

                Nivo =
                    x.Nivo.ToString(),

                PredmetId =
                    x.PredmetId
            })
            .ToList();
    }
}