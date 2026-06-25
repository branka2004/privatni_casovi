using FluentValidation;
using PrivateLessons.Application.DTOs;

namespace API.Validators;

public class CreateNoviPredmetDtoValidator
    : AbstractValidator<CreateNoviPredmetDto>
{
    public CreateNoviPredmetDtoValidator()
    {
        RuleFor(x => x.Naziv)
            .NotEmpty()
            .WithMessage("Naziv predmeta je obavezan.");

        RuleFor(x => x.Oblast)
            .NotEmpty()
            .WithMessage("Oblast je obavezna.");

        RuleFor(x => x.GodineIskustva)
            .GreaterThan(0)
            .WithMessage("Godine iskustva moraju biti veće od 0.");
    }
}