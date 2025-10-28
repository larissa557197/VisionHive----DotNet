using System.Data;
using FluentValidation;
using VisionHive.Application.DTO.Request;

namespace VisionHive.Application.DTO.Validators;

public class PatioRequestValidator : AbstractValidator<PatioRequest>
{
    public PatioRequestValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("Nome do pátio é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(p => p.LimiteMotos)
            .GreaterThan(0)
            .WithMessage("O limite de motos deve ser maior que zero.");

        RuleFor(p => p.FilialId)
            .NotEmpty()
            .WithMessage("O Id da filial é obrigatório.");
    }
}