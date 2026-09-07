using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class EmailService
    {
        public bool EnviarCorreoRecuperacion(
            string correoDestino,
            string enlaceRecuperacion)
        {
            try
            {
                MailMessage mensaje = new MailMessage();

                mensaje.From = new MailAddress(
                    "mentesana.sistema@gmail.com",
                    "Mente Sana"
                );

                mensaje.To.Add(correoDestino);

                mensaje.Subject = "Recuperación de contraseña - Mente Sana";

                mensaje.Body =
                    "Hola,\n\n" +
                    "Hemos recibido una solicitud para recuperar " +
                    "la contraseña de tu cuenta de Mente Sana.\n\n" +
                    "Para cambiar tu contraseña, ingresa al siguiente enlace:\n\n" +
                    enlaceRecuperacion +
                    "\n\n" +
                    "Este enlace será válido durante 30 minutos.\n\n" +
                    "Si no solicitaste este cambio, puedes ignorar este correo.\n\n" +
                    "Saludos,\n" +
                    "Equipo Mente Sana";

                mensaje.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();

                smtp.Send(mensaje);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Error enviando correo: " + ex.Message
                );

                return false;
            }
        }
    }
}
