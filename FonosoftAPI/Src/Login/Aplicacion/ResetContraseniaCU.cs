using System.Text;
using System.Text.RegularExpressions;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Aplicacion
{
    public class ResetContraseniaCU<T> : AEjecutarCUAsync<T>
    {
        private readonly ILoginRepo _loginRepo;
        private readonly IValidar _validar;
        private readonly IAes _aes;
        private readonly IUsuario _usuario;
        private readonly IAuthEmail _authEmail;

        public ResetContraseniaCU(IResponse<T> response,
                                  ILoginRepo loginRepo,
                                  IValidar validar,
                                  IAes aes,
                                  IUsuario usuario,
                                  IAuthEmail authEmail) : base(response)
        {
            _loginRepo = loginRepo;
            _validar = validar;
            _aes = aes;
            _usuario = usuario;
            _authEmail = authEmail;
        }

        public override IError Especificaciones()
        {
            return _validar.Validar();
        }

        public override async Task<IResponse<T>> Proceso()
        {
            _usuario.NombreUsuario = _aes.Encriptar(_usuario.NombreUsuario!);

            IUsuario usuarioBuscar = (IUsuario)_usuario.Clone();
            usuarioBuscar = await _loginRepo.BuscarUsuarioXNombreUsuario(usuarioBuscar);

            _usuario.Id = usuarioBuscar.Id;
            string nuevaContrasenia = GenerarTextoAleatorio(8);
            _usuario.Contrasenia = _aes.Encriptar(nuevaContrasenia);
            await _loginRepo.ModificarContrasenia(_usuario);

            string htmlBody = GetBodyResetContrasenia();
            htmlBody = Regex.Replace(htmlBody, "@contrasenia", nuevaContrasenia);
            ICollection<string> lstEmail = [_aes.Desencriptar(usuarioBuscar.Email!)];
            _authEmail.EnviarEmailRegistro(lstEmail, htmlBody);

            _response.Data = (T)_usuario;

            return _response;
        }

        private string GetBodyResetContrasenia()
        {
            string nombreArchivo = "ResetContrasenia.html";
            string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Html", nombreArchivo);
            string contenido = File.ReadAllText(rutaArchivo);

            return contenido;
        }
        private static string GenerarTextoAleatorio(int longitud)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder resultado = new StringBuilder(longitud);
            Random random = new Random();

            for (int i = 0; i < longitud; i++)
            {
                char caracterAleatorio = caracteres[random.Next(caracteres.Length)];
                resultado.Append(caracterAleatorio);
            }

            return resultado.ToString();
        }

        public override void BeginTransaction()
        {
            _loginRepo.BeginTransaction();
        }
        public override void CommitTransaction()
        {
            _loginRepo.CommitTransaction();
        }
        public override async Task ConexionOpen()
        {
            await _loginRepo.ConexionOpen();
        }
        public override void RollBackTransaction()
        {
            _loginRepo.RollbackTransaction();
        }
    }
}