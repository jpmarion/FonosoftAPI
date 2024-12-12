using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Infraestructura.Interface
{
    public interface ILoginRepo : ISqlRepo
    {
        Task<IUsuario> BuscarUsuarioXNombreUsuario(IUsuario usuario);
        Task<IUsuario> BuscarUsuarioXId(IUsuario usuario);
        Task ModificarContrasenia(IUsuario usuario);
        Task<IUsuario> RegistrarUsuario(IUsuario usuario);
    }
}