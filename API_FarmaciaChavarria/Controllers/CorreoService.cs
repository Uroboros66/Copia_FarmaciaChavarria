using System.Diagnostics;
using System.Net;
using System.Net.Mail;
namespace API_FarmaciaChavarria.Controllers
{
    public class CorreoService
    {
        private readonly IConfiguration _config;

        public CorreoService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarPin(string destino, int pin)
        {
            var remitente = _config["Correo:Remitente"];
            var clave = _config["Correo:Clave"];
            var smtpHost = _config["Correo:Smtp"];
            var puerto = int.Parse(_config["Correo:Puerto"]!);

            Debug.WriteLine($"Hola {remitente}");
            var mensaje = new MailMessage(remitente!, destino)
            {
                Subject = "Recuperación de PIN",
                Body = $"Hola,\n\nTu nuevo PIN es: {pin}\n\nUtilízalo para acceder a tu cuenta.",
                IsBodyHtml = false
            };

            using var smtp = new SmtpClient(smtpHost, puerto)
            {
                Credentials = new NetworkCredential(remitente, clave),
                EnableSsl = true
            };

            smtp.Send(mensaje);
        }
    }
}