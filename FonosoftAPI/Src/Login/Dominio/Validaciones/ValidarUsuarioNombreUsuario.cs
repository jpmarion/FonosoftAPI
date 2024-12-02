using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarUsuarioNombreUsuario : AValidarUsuario
    {
        public override IError EsValido(IUsuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.NombreUsuario!.Trim()))
            {
                LoginErrores loginErrores= LoginErrores.NombreUsuarioNullOrEmpty;

                IError error =new Error();
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