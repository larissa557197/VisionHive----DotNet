using FluentValidation;
using VisionHive.Application.DTO.Request;

namespace VisionHive.Application.DTO.Validators;

public class MotoRequestValidator : AbstractValidator<MotoRequest>
{
    public MotoRequestValidator()
    {
        RuleFor(m => m.Placa)
            .NotEmpty().WithMessage("Placa não pode ser nula.")
            .Matches(@"^[A-Z]{3}\d[A-Z0-9]\d{2}$")
            .WithMessage("Placa inválida. Use o formato padrão Mercosul (ex:ABC1D23");

        RuleFor(m => m.Chassi)
            .NotEmpty().WithMessage("Chassi é obrigatório.")
            .Length(17).WithMessage("Chassi deve ter exatamente 17 caracteres.");

        RuleFor(m => m.NumeroMotor)
            .NotEmpty().WithMessage("Número do motor é obrigatório.")
            .MaximumLength(30).WithMessage("Número do motor deve ter no máximo 30 caracteres.");

        RuleFor(m => m.FilialId)
            .NotEmpty().WithMessage("FilialId é orbigatório.");

        RuleFor(m => m.PatioId)
            .NotEmpty().WithMessage("PatioId é obrigatório.");

    }
}