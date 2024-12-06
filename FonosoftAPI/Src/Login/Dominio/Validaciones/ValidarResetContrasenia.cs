using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarResetContrasenia : IValidar
    {
        private readonly IUsuario _usuario;
        private readonly ILoginRepo _loginRepo;
        private readonly IAes _aes;
        private readonly AValidarUsuario _validarUsuarioNombreUsuario = new ValidarUsuarioNombreUsuario();
        private readonly AValidarUsuario _validarUsuarioNombreUsuarioInexistente;

        public ValidarResetContrasenia(IUsuario usuario, ILoginRepo loginRepo, IAes aes)
        {
            _usuario = usuario;
            _loginRepo = loginRepo;
            _aes = aes;

            _validarUsuarioNombreUsuarioInexistente = new ValidarUsuarioNombreUsuarioInexistente(_loginRepo, _aes);
            _validarUsuarioNombreUsuario.ProximaValidacion(_validarUsuarioNombreUsuarioInexistente);
        }

        public IError Validar()
        {
            return _validarUsuarioNombreUsuario.EsValido(_usuario);
        }
    }
}