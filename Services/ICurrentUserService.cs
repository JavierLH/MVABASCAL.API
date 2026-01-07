namespace SistemaAduanero.API.Services
{
    public interface ICurrentUserService
    {
        int UsuarioId { get; }
        int ClienteId { get; }
        string Username { get; }
        string Rol { get; }
    }
}