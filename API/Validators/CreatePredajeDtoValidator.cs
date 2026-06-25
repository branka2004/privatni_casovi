using FluentValidation;
using PrivateLessons.Application.DTOs;

namespace API.Validators;

public class CreatePredajeDtoValidator
    : AbstractValidator<CreatePredajeDto>
{
    public CreatePredajeDtoValidator()
    {
        RuleFor(x => x.PredmetId)
            .GreaterThan(0)
            .WithMessage("Morate izabrati predmet.");

        RuleFor(x => x.GodineIskustva)
            .GreaterThan(0)
            .WithMessage("Godine iskustva moraju biti veće od 0.");
    }
}