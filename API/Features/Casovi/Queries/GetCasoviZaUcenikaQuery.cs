using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Casovi.Queries;

public record GetCasoviZaUcenikaQuery(
    int UcenikId)
    : IRequest<List<CasDto>>;

public class GetCasoviZaUcenikaQueryHandler
    : IRequestHandler<
        GetCasoviZaUcenikaQuery,
        List<CasDto>>
{
    private readonly IUnitOfWork _uow;

    public GetCasoviZaUcenikaQueryHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<CasDto>> Handle(
        GetCasoviZaUcenikaQuery request,
        CancellationToken cancellationToken)
    {
        var casovi =
            await _uow.Casovi.GetZaUcenika(
                request.UcenikId);

        return casovi
            .Select(x => new CasDto
            {
                CasId = x.CasId,

                UcenikIme =
                    x.Ucenik.Ime + " " +
                    x.Ucenik.Prezime,

                PredavacIme =
                    x.Predavac.Ime + " " +
                    x.Predavac.Prezime,

                PredmetNaziv =
                    x.Predmet.Naziv,

                Datum =
                    x.Datum,

                VremePocetka =
                    x.VremePocetka,

                VremeZavrsetka =
                    x.VremeZavrsetka,

                Status =
                    x.Status.ToString()
            })
            .ToList();
    }
}