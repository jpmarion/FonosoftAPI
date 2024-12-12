namespace FonosoftAPI.Modelos.LoginController
{
    public class RqsModificarContrasenia
    {
        public String? NombreUsuario { get; set; }
        public String? Contrasenia { get; set; }
        public String? NuevaContrasenia { get; set; }
        public String? RepetirContrasenia { get; set; }
    }
}