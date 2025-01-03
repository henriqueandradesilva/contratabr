namespace Domain.Common.Consts;

public class MessageConst
{
    #region Consts

    public const string MessageEmpty = "Nenhuma mensagem disponível.";

    public const string ActionNotPermitted = "Ação não permitida.";

    #endregion

    #region Route

    public const string OriginRequired = "O campo Origem é obrigatório.";

    public const string OriginMaxPermitted = "O máximo permitido para o campo Origem é de 100 caracteres.";

    public const string DestinyRequired = "O campo Destino é obrigatório.";

    public const string DestinyMaxPermitted = "O máximo permitido para o campo Destino é de 100 caracteres.";

    public const string ValueRequired = "O campo Valor é obrigatório.";

    public const string RouteCreated = "Rota cadastrada com sucesso.";

    public const string RouteUpdated = "Rota modificada com sucesso.";

    public const string RouteNotExist = "Nenhuma Rota foi encontrada.";

    public const string RouteExist = "Este Rota já existe.";

    #endregion
}