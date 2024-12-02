namespace FonosoftAPI.Src.Login.Dominio.Interface
{
    public interface IUsuario : ICloneable
    {
        public int Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Contrasenia { get; set; }
    }
}