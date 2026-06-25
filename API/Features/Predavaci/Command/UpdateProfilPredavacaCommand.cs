using MediatR;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Predavaci.Commands;

public record UpdateProfilPredavacaCommand(
    int PredavacId,
    string Biografija,
    decimal CenaPoSatu)
    : IRequest<bool>;

public class UpdateProfilPredavacaCommandHandler
    : IRequestHandler<UpdateProfilPredavacaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfilPredavacaCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateProfilPredavacaCommand request,
        CancellationToken cancellationToken)
    {
        var predavac =
            await _unitOfWork.Predavaci.GetByIdAsync(
                request.PredavacId);

        if (predavac == null)
            return false;

        predavac.Biografija =
            request.Biografija;

        predavac.CenaPoSatu =
            request.CenaPoSatu;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}