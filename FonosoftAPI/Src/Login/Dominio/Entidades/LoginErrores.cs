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
        public static LoginErrores NombreUsuarioInexistente = new LoginErrores("5", "Nombre de usuario inexiste");
        public static LoginErrores ContraseniaNullOrEmpty = new LoginErrores("6", "Ingrese la contraseña");
        public static LoginErrores UsuarioContraseniaErronea = new LoginErrores("7", "Usuario o contraseña erroneos");
        public static LoginErrores NuevaContraseniaNullOrEmpty = new LoginErrores("8", "Ingrese la nueva contraseña");
        public static LoginErrores RepetirContraseniaNullOrEmpty = new LoginErrores("9", "Ingrese la contraseña a repetir");
        public static LoginErrores NoCoincidenContrasenias = new LoginErrores("10", "No coinciden contraseñas");
    }
}