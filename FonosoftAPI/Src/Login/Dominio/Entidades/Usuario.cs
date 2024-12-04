using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Entidades
{
    public class Usuario : IUsuario
    {
        public int Id { get; set; }

        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Contrasenia { get; set; }
        public string? NuevaContrasenia { get; set; }
        public string? RepetirContrasenia { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}