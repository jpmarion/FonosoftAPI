using System.Text;
using System.Text.RegularExpressions;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Aplicacion
{
    public class RegistrarUsuarioCU<T> : AEjecutarCUAsync<T>
    {
        private readonly ILoginRepo _loginRepo;
        private IUsuario _usuario;
        private readonly IValidar _validar;
        private readonly IAes _aes;
        private readonly IAuthEmail _authEmail;

        public RegistrarUsuarioCU(IResponse<T> response,
                                  ILoginRepo loginRepo,
                                  IUsuario usuario,
                                  IValidar validar,
                                  IAes aes,
                                  IAuthEmail authEmail) : base(response)
        {
            _loginRepo = loginRepo;
            _usuario = usuario;
            _validar = validar;
            _aes = aes;
            _authEmail = authEmail;
        }

        public override IError Especificaciones()
        {
            return _validar.Validar();
        }

        public override async Task<IResponse<T>> Proceso()
        {
            string email = _usuario.Email!;
            string contrasenia = GenerarTextoAleatorio(8);

            _usuario.NombreUsuario = _aes.Encriptar(_usuario.NombreUsuario!);
            _usuario.Email = _aes.Encriptar(_usuario.Email!);
            _usuario.Contrasenia = _aes.Encriptar(contrasenia);

            _usuario = await _loginRepo.RegistrarUsuario(_usuario);

            IResponse<T> response = new Response<T>();

            if (_usuario != null)
            {
                string htmlBody = GetBodyConfirmarRegistro();
                htmlBody = Regex.Replace(htmlBody, "@contrasenia", contrasenia);

                ICollection<string> lstEmail = [email];
                _authEmail.EnviarEmailRegistro(lstEmail, htmlBody);
    
                response.Data = (T)_usuario;
            }

            return response;
        }

        private string GetBodyConfirmarRegistro()
        {
            string nombreArchivo = "ConfirmarRegistro.html";
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