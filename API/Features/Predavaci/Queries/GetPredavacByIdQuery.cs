using MediatR;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predavaci.Queries;

public record GetPredavacByIdQuery(
    int PredavacId)
    : IRequest<PredavacDto?>;

public class GetPredavacByIdQueryHandler
    : IRequestHandler<GetPredavacByIdQuery, PredavacDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPredavacByIdQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PredavacDto?> Handle(
        GetPredavacByIdQuery request,
        CancellationToken cancellationToken)
    {
        var predavac =
            await _unitOfWork.Predavaci.GetByIdAsync(
                request.PredavacId);

        if (predavac == null)
            return null;

        return new PredavacDto
        {
            KorisnikId = predavac.KorisnikId,
            Ime = predavac.Ime,
            Prezime = predavac.Prezime,
            Email = predavac.Email,
            Biografija = predavac.Biografija,
            CenaPoSatu = predavac.CenaPoSatu
        };
    }
}