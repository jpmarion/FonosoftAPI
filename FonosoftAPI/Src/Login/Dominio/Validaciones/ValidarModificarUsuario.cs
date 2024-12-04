using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarModificarUsuario : IValidar
    {
        private readonly AValidarUsuario _validarUsuarioNombreUsuario = new ValidarUsuarioNombreUsuario();
        private readonly AValidarUsuario _validarUsuarioNombreUsuarioInexistente;
        private readonly AValidarUsuario _validarUsuarioContrasenia = new ValidarUsuarioContrasenia();
        private readonly AValidarUsuario _validarUsuarioCambioContrasenia = new ValidarUsuarioCambioContrasenia();
        private readonly IUsuario _usuario;
        private readonly ILoginRepo _loginRepo;
        private readonly IAes _aes;

        public ValidarModificarUsuario(IUsuario usuario, ILoginRepo loginRepo, IAes aes)
        {
            _usuario = usuario;
            _loginRepo = loginRepo;
            _aes = aes;

            _validarUsuarioNombreUsuarioInexistente = new ValidarUsuarioNombreUsuarioInexistente(_loginRepo, _aes);
            _validarUsuarioNombreUsuario.ProximaValidacion(_validarUsuarioNombreUsuarioInexistente);
            _validarUsuarioNombreUsuarioInexistente.ProximaValidacion(_validarUsuarioContrasenia);
            _validarUsuarioContrasenia.ProximaValidacion(_validarUsuarioCambioContrasenia);
        }

        public IError Validar()
        {
            return _validarUsuarioNombreUsuario.EsValido(_usuario);
        }
    }
}