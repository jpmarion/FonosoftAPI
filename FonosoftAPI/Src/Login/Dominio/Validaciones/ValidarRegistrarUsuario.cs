using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarRegistrarUsuario : IValidar
    {
        private readonly IUsuario _usuario;
        private readonly ILoginRepo _loginRepo;
        private AValidarUsuario _validarUsuarioNombreUsuario = new ValidarUsuarioNombreUsuario();
        private AValidarUsuario _validarUsuarioEmail = new ValidarUsuarioEmail();
        private AValidarUsuario _validarNombreUsuariExiste;
        public ValidarRegistrarUsuario(IUsuario usuario, ILoginRepo loginRepo)
        {
            _usuario = usuario;
            _loginRepo = loginRepo;
            _validarUsuarioNombreUsuario.ProximaValidacion(_validarUsuarioEmail);
            _validarNombreUsuariExiste = new ValidarUsuarioNombreUsuarioExiste(_loginRepo); 
            _validarUsuarioEmail.ProximaValidacion(_validarNombreUsuariExiste);
        }

        public IError Validar()
        {
            return _validarUsuarioNombreUsuario.EsValido(_usuario);
        }
    }
}