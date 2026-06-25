using FluentValidation;
using PrivateLessons.Application.DTOs;

namespace API.Validators;

public class CreatePredavacDtoValidator : AbstractValidator<CreatePredavacDto>
{
    public CreatePredavacDtoValidator()
    {
        RuleFor(x => x.Ime)
            .NotEmpty()
            .WithMessage("Ime je obavezno.");

        RuleFor(x => x.Prezime)
            .NotEmpty()
            .WithMessage("Prezime je obavezno.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email je obavezan.")
            .EmailAddress()
            .WithMessage("Email nije ispravnog formata.");

        RuleFor(x => x.Lozinka)
            .NotEmpty()
            .WithMessage("Lozinka je obavezna.")
            .MinimumLength(6)
            .WithMessage("Lozinka mora imati najmanje 6 karaktera.");

        RuleFor(x => x.CenaPoSatu)
            .GreaterThan(0)
            .WithMessage("Cena po satu mora biti veća od 0.");
    }
}