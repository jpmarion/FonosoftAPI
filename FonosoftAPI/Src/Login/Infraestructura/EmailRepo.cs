using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Infraestructura
{
    public class EmailRepo : IAuthEmail
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _usuario;
        private readonly string _contrasenia;
        private readonly bool _habilitaSSL;

        public EmailRepo(string Host, int Port, string Usuario, string Contrasenia, bool HabilitaSSL)
        {
            _host = Host;
            _port = Port;
            _usuario = Usuario;
            _contrasenia = Contrasenia;
            _habilitaSSL = HabilitaSSL;
        }
        public async Task EnviarEmailRegistro(ICollection<string> EmailDestino, string Cuerpo)
        {
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(_usuario);
            foreach (string item in EmailDestino)
            {
                correo.To.Add(item);
            }
            correo.Subject = "Registrar usuario";
            correo.IsBodyHtml = true;
            correo.Body = Cuerpo;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = _host;
            smtp.Port = _port;
            smtp.Credentials = new NetworkCredential(_usuario, _contrasenia);
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = _habilitaSSL;
            smtp.SendAsync(correo, "send");
        }
    }
}