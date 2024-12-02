using System.Text.RegularExpressions;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarUsuarioEmail : AValidarUsuario
    {
        public override IError EsValido(IUsuario usuario)
        {
            LoginErrores? loginErrores = null;
            if (string.IsNullOrEmpty(usuario.Email!.Trim()))
            {
                loginErrores = LoginErrores.EmailNullOrEmpty;


            }
            else if (!EsEmailValido(usuario.Email))
            {
                loginErrores = LoginErrores.EmailNoValido;
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

        private bool EsEmailValido(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}