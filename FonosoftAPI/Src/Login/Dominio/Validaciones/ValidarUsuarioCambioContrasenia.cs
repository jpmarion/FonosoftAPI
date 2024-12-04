using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarUsuarioCambioContrasenia : AValidarUsuario
    {
        public ValidarUsuarioCambioContrasenia()
        {
        }

        public override IError EsValido(IUsuario usuario)
        {
            LoginErrores? loginErrores = null;
            if (string.IsNullOrEmpty(usuario.NuevaContrasenia!.Trim()))
            {
                loginErrores = LoginErrores.NuevaContraseniaNullOrEmpty;
            }
            else if (String.IsNullOrEmpty(usuario.RepetirContrasenia!.Trim()))
            {
                loginErrores = LoginErrores.RepetirContraseniaNullOrEmpty;
            }
            else if (usuario.NuevaContrasenia != usuario.RepetirContrasenia)
            {
                loginErrores = LoginErrores.NoCoincidenContrasenias;
            }

            if (loginErrores != null)
            {
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
    }
}