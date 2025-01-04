using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorRoute : AbstractValidator<Route>
{
    public ValidatorRoute()
    {
        RuleFor(m => m.Origem)
            .NotNull().NotEmpty().WithName(MessageConst.OriginRequired)
            .MaximumLength(100).WithMessage(MessageConst.OriginMaxPermitted);

        RuleFor(m => m.Destino)
            .NotNull().NotEmpty().WithName(MessageConst.DestinyRequired)
            .MaximumLength(150).WithMessage(MessageConst.DestinyMaxPermitted);

        RuleFor(u => u.Valor)
            .GreaterThan(0).WithMessage(MessageConst.ValueRequired);
    }
}