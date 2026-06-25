using FluentValidation;
using PrivateLessons.Application.DTOs;

namespace API.Validators;

public class CreatePredmetDtoValidator
    : AbstractValidator<CreatePredmetDto>
{
    public CreatePredmetDtoValidator()
    {
        RuleFor(x => x.Naziv)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Oblast)
            .NotEmpty()
            .MaximumLength(100);
    }
}