using CargoManagement.Api.DTOs;
using FluentValidation;

namespace CargoManagement.Api.Validators;

public class ManifestValidator : AbstractValidator<CreateManifestDto>
{
    public ManifestValidator()
    {
        RuleFor(x => x.CargoId)
            .GreaterThan(0).WithMessage("CargoId deve ser maior que zero.");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("Número é obrigatório.")
            .MaximumLength(50).WithMessage("Número deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Despachante)
            .NotEmpty().WithMessage("Despachante é obrigatório.")
            .MaximumLength(100).WithMessage("Despachante deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Observacoes)
            .MaximumLength(500).WithMessage("Observações deve ter no máximo 500 caracteres.")
            .When(x => x.Observacoes is not null);
    }
}
