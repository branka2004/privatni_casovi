using FluentValidation;
using PrivateLessons.Application.DTOs;

namespace API.Validators;

public class CreateCasDtoValidator
    : AbstractValidator<CreateCasDto>
{
    public CreateCasDtoValidator()
    {
        RuleFor(x => x.UcenikId)
            .GreaterThan(0)
            .WithMessage("Učenik nije izabran.");

        RuleFor(x => x.PredavacId)
            .GreaterThan(0)
            .WithMessage("Predavač nije izabran.");

        RuleFor(x => x.PredmetId)
            .GreaterThan(0)
            .WithMessage("Predmet nije izabran.");

        RuleFor(x => x.VremePocetka)
            .NotEqual(TimeSpan.Zero)
            .WithMessage("Morate uneti vreme početka.");

        RuleFor(x => x.VremeZavrsetka)
            .NotEqual(TimeSpan.Zero)
            .WithMessage("Morate uneti vreme završetka.");

        RuleFor(x => x.VremeZavrsetka)
            .GreaterThan(x => x.VremePocetka)
            .WithMessage(
                "Vreme završetka mora biti kasnije od vremena početka.");
    }
}