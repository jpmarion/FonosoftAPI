using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;


namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarUsuarioNombreUsuarioInexistente : AValidarUsuario
    {
        private readonly ILoginRepo _loginRepo;
        private readonly IAes _aes;

        public ValidarUsuarioNombreUsuarioInexistente(ILoginRepo loginRepo, IAes aes)
        {
            _loginRepo = loginRepo;
            _aes = aes;
        }

        public override IError EsValido(IUsuario usuario)
        {
            if (!ValidarUsuarioNombreUsuarioExiste(usuario))
            {
                LoginErrores loginErrores = LoginErrores.NombreUsuarioInexistente;

                IError error = new Error();
                error.NroError = loginErrores.NroError;
                error.MsgError = loginErrores.MsgError;

                return error;
            }

            if (_validador != null)
            {
                return _validador.EsValido(usuario);
            }
            return null!;
        }

        private bool ValidarUsuarioNombreUsuarioExiste(IUsuario usuario)
        {
            IUsuario usuarioBuscar = (IUsuario)usuario.Clone();
            usuarioBuscar.NombreUsuario = _aes.Encriptar(usuario.NombreUsuario!);

            usuarioBuscar = _loginRepo.BuscarUsuarioXNombreUsuario(usuarioBuscar).GetAwaiter().GetResult();

            if (usuarioBuscar == null)
            {
                return false;
            }

            return true;
        }
    }
}