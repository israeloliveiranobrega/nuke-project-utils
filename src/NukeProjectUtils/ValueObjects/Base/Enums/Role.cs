namespace NukeProjectUtils.ValueObjects.Base.Enums;

public enum Role
{
    /// <summary>
    /// O padrão. Acesso apenas aos seus próprios dados.
    /// </summary>
    User = 0,
    /// <summary>
    /// Usuário com identidade validada (gov.br, foto com RG). 
    /// Tem mais confiança na plataforma e limites maiores de anúncio.
    /// </summary>
    VerifiedUser = 1,

    /// <summary>
    /// Equipe de atendimento. Pode visualizar usuários, resetar senhas, mas não pode excluir dados.
    /// </summary>
    Support = 10,

    /// <summary>
    /// Gerente do sistema. Pode criar/banir usuários e alterar configurações globais.
    /// </summary>
    Admin = 20,

    /// <summary>
    /// (Opcional) Desenvolvedor/Dono. Acesso irrestrito a logs, banco de dados e endpoints de debug.
    /// </summary>
    Master = 99
}