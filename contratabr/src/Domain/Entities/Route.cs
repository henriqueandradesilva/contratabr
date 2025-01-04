using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class Route : BaseGenericEntity
{
    public Route()
    {

    }


    public Route(
        long id,
        string origem,
        string destino,
        double valor) : base(id)
    {
        Origem = origem;
        Destino = destino;
        Valor = valor;
    }

    public string Origem { get; private set; }

    public string Destino { get; private set; }

    public double Valor { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Route> _validator
        = new Validators.ValidatorRoute();

    #endregion

    #region Extensions

    public void AddValor(
        double valor)
    {
        Valor = valor;
    }

    #endregion
}