using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FonosoftAPI.Src.Login.Dominio.Entidades
{
    public class LoginErrores
    {
        private string? _nroError;

        public LoginErrores(string nroError, string msgError)
        {
            _nroError = nroError;
            MsgError = msgError;
        }

        public string NroError
        {
            get { return "LoginErrores" + _nroError; }
            set => _nroError = value;
        }
        public string? MsgError { get; set; }

        public static LoginErrores NombreUsuarioNullOrEmpty = new LoginErrores("1", "Ingrese el nombre de ususario");
        public static LoginErrores EmailNullOrEmpty = new LoginErrores("2", "Ingrese el email");
        public static LoginErrores EmailNoValido = new LoginErrores("3", "Formato email incorrecto");
        public static LoginErrores NombreUsuarioExistente = new LoginErrores("4", "Nombre usuario existente");
    }
}