using MediatR;
using Microsoft.EntityFrameworkCore;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Infrastructure.Data;

namespace API.Features.Predavaci.Queries;

public record GetPredmetiPredavacaQuery(
    int PredavacId)
    : IRequest<List<PredmetPredavacaDto>>;

public class GetPredmetiPredavacaQueryHandler
    : IRequestHandler<
        GetPredmetiPredavacaQuery,
        List<PredmetPredavacaDto>>
{
    private readonly PrivateLessonsDbContext _context;

    public GetPredmetiPredavacaQueryHandler(
        PrivateLessonsDbContext context)
    {
        _context = context;
    }

    public async Task<List<PredmetPredavacaDto>> Handle(
        GetPredmetiPredavacaQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Predaje
            .Include(x => x.Predmet)
            .Where(x =>
                x.PredavacId == request.PredavacId)
            .Select(x => new PredmetPredavacaDto
            {
                PredmetId = x.PredmetId,
                Naziv = x.Predmet.Naziv,
                Oblast = x.Predmet.Oblast,
                GodineIskustva = x.GodineIskustva,
                Nivo = x.Nivo.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}