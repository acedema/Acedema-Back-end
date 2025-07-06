using API.Models.Request;
using Microsoft.Extensions.Configuration;
using SendGrid; 
using SendGrid.Helpers.Mail;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class LogicaUtilitarios
    {
        private readonly IConfiguration _configuration;

        public LogicaUtilitarios(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Encriptar(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public string GenerarPassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var result = new char[length];
            var buffer = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[buffer[i] % chars.Length];
            }

            return new string(result);
        }

        public async Task<bool> EnviarPasswordAsync(string nombre, string correo, string password)
        {
            try
            {
                var apiKey = _configuration["SendGrid:ApiKey"];
                var fromEmail = _configuration["SendGrid:FromEmail"];
                var fromName = _configuration["SendGrid:FromName"];

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var to = new EmailAddress(correo, nombre);
                var subject = "Sus credenciales de ACEDEMA.COM";
                var plainTextContent = "Se adjuntan las credenciales para el sitio web de ACEDEMA";
                var htmlContent = GenerarContenidoHtml(correo, password);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private string GenerarContenidoHtml(string correo, string password)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Credenciales de acceso</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                    <table width='100%' cellspacing='0' cellpadding='0'>
                        <tr>
                            <td align='center'>
                                <table width='500px' cellspacing='0' cellpadding='20' style='background-color: #ffffff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
                                    <tr>
                                        <td align='center'>
                                            <h1 style='color: #333; font-size: 32px; margin-bottom: 10px;'>ACEDEMA</h1>
                                            <p style='color: #555; font-size: 18px;'>Estas son tus credenciales para acceder al sitio web de <b>ACEDEMA</b>.</p>
                                            <div style='background-color: #f8f8f8; padding: 20px; margin: 20px auto; border-radius: 8px; width: 80%; border: 1px solid #ddd; text-align: left;'>
                                                <p style='font-size: 16px; color: #333;'><b>Usuario:</b> {correo} </p>
                                                <p style='font-size: 16px; color: #333;'><b>Contraseña:</b> {password} </p>
                                            </div>
                                            <p style='color: #d9534f; font-size: 14px; text-align: left; padding: 0 20px;'>
                                                ⚠ No compartas estas credenciales con nadie. Guárdalas en un lugar seguro o cámbialas en el sitio web.
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
        }

        /// <summary>
        /// Envía un correo con un enlace para restablecer la contraseña.
        /// </summary>
        /// <param name="correo">Correo del usuario que solicitó la recuperación.</param>
        /// <param name="urlRecuperacion">URL que contiene el token JWT para restablecer la contraseña.</param>
        /// <returns>True si el correo fue enviado exitosamente, false si hubo error.</returns>
        public async Task<bool> EnviarCorreoRecuperacionAsync(string correo, string urlRecuperacion)
        {
            try
            {
                var apiKey = _configuration["SendGrid:ApiKey"];
                var fromEmail = _configuration["SendGrid:FromEmail"];
                var fromName = _configuration["SendGrid:FromName"];

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var to = new EmailAddress(correo);
                var subject = "Recuperación de contraseña - ACEDEMA";
                var plainTextContent = $"Haz clic en el siguiente enlace para restablecer tu contraseña: {urlRecuperacion}";
                var htmlContent = GenerarHtmlRecuperacion(urlRecuperacion);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Genera el HTML del correo de recuperación de contraseña.
        /// </summary>
        /// <param name="url">URL que contiene el token para restablecer contraseña.</param>
        /// <returns>Cadena con el contenido HTML.</returns>
        private string GenerarHtmlRecuperacion(string url)
        {
            return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>Restablecer contraseña</title>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
            <table width='100%' cellspacing='0' cellpadding='0'>
                <tr>
                    <td align='center'>
                        <table width='500px' cellspacing='0' cellpadding='20' style='background-color: #ffffff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
                            <tr>
                                <td align='center'>
                                    <h1 style='color: #333; font-size: 28px;'>Recuperación de contraseña</h1>
                                    <p style='color: #555; font-size: 16px;'>Hemos recibido una solicitud para restablecer tu contraseña.</p>
                                    <a href='{url}' style='display: inline-block; margin-top: 20px; padding: 12px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Restablecer contraseña</a>
                                    <p style='color: #999; font-size: 12px; margin-top: 20px;'>Si no realizaste esta solicitud, puedes ignorar este mensaje.</p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>";
        }



        public bool VerificarPassword(string passwordIngresada, string hashGuardado)
        {
            string hashIngresado = Encriptar(passwordIngresada); // usando tu función existente
            return hashIngresado == hashGuardado;
        }

        public static string ValidarPersona(ReqRegistrarPersona req)
        {
            if (req.Persona == null)
                return "La información de la persona es requerida.";

            if (req.Persona.NumCedula <= 0)
                return "La cédula es inválida.";

            if (string.IsNullOrWhiteSpace(req.Persona.Correo))
                return "El correo es obligatorio.";

            if (string.IsNullOrWhiteSpace(req.Persona.PrimerNombre))
                return "El primer nombre es obligatorio.";

            if (string.IsNullOrWhiteSpace(req.Persona.PrimerApellido))
                return "El primer apellido es obligatorio.";

            if (req.Persona.IdRol <= 0)
                return "Debe seleccionar un rol válido.";

            return null; // Todo bien
        }

    }
}

