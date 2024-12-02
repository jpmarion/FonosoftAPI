namespace FonosoftAPI.Src.Login.Infraestructura.Interface
{
    public interface IAuthEmail
    {
        Task EnviarEmailRegistro(ICollection<string> EmailDestino, string Cuerpo);
    }
}