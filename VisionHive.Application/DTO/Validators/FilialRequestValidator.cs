using FluentValidation;
using VisionHive.Application.DTO.Request;

namespace VisionHive.Application.DTO.Validators;

public class FilialRequestValidator : AbstractValidator<FilialRequest>
{
    public FilialRequestValidator()
    {
        RuleFor(f => f.Nome)
            .NotEmpty().WithMessage("Nome da filial é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(f => f.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.")
            .MaximumLength(100).WithMessage("O bairro deve ter no máximo 100 caracteres.");

        RuleFor(f => f.Cnpj)
            .NotEmpty().WithMessage("O CNPJ é obrigatório.")
            .Matches(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}")
            .WithMessage("Formato de CNJPJ inválido. Use o formato 00.000.000/0000-00.");
    }
}