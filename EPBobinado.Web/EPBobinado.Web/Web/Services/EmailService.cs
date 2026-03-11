using System.Net;
using System.Net.Mail;

namespace Web.Services
{
    /// <summary>
    /// Servicio de envío de correos via SMTP.
    /// Configurar en appsettings.json bajo la sección "Email".
    /// </summary>
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarRecuperacionAsync(string destinatario, string nombre, string enlace)
        {
            var sec  = _config.GetSection("Email");
            var host = sec.GetValue<string>("SmtpHost")  ?? throw new InvalidOperationException("SmtpHost no configurado.");
            var port = sec.GetValue<int>("SmtpPort");
            var user = sec.GetValue<string>("SmtpUser")  ?? throw new InvalidOperationException("SmtpUser no configurado.");
            var pass = sec.GetValue<string>("SmtpPass")  ?? throw new InvalidOperationException("SmtpPass no configurado.");
            var from = sec.GetValue<string>("FromEmail") ?? user;
            var fromName = sec.GetValue<string>("FromName") ?? "EP Bobinado";

            var asunto  = "Recuperación de contraseña — EP Bobinado";
            var cuerpo  = GenerarHtml(nombre, enlace);

            using var smtp   = new SmtpClient(host, port);
            smtp.Credentials = new NetworkCredential(user, pass);
            smtp.EnableSsl   = true;

            var mensaje = new MailMessage
            {
                From       = new MailAddress(from, fromName),
                Subject    = asunto,
                Body       = cuerpo,
                IsBodyHtml = true
            };
            mensaje.To.Add(destinatario);

            await smtp.SendMailAsync(mensaje);
        }

        private static string GenerarHtml(string nombre, string enlace) => $"""
            <!DOCTYPE html>
            <html lang="es">
            <head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"></head>
            <body style="margin:0;padding:0;background:#f1f5f9;font-family:'Segoe UI',Arial,sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#f1f5f9;padding:40px 0;">
                <tr><td align="center">
                  <table width="520" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,.08);">

                    <!-- Header -->
                    <tr><td style="background:#0f172a;padding:28px 32px;text-align:center;">
                      <div style="font-size:36px;margin-bottom:6px;">🔧</div>
                      <div style="color:#f59e0b;font-size:22px;font-weight:800;letter-spacing:2px;">EP BOBINADO</div>
                      <div style="color:#94a3b8;font-size:12px;margin-top:2px;">Mi Taller</div>
                    </td></tr>

                    <!-- Body -->
                    <tr><td style="padding:36px 32px;">
                      <h2 style="color:#0f172a;font-size:20px;font-weight:700;margin:0 0 12px;">Hola, {nombre} 👋</h2>
                      <p style="color:#475569;font-size:15px;line-height:1.6;margin:0 0 24px;">
                        Recibimos una solicitud para restablecer la contraseña de tu cuenta.
                        Hacé clic en el botón de abajo para crear una nueva contraseña.
                      </p>

                      <!-- CTA Button -->
                      <table cellpadding="0" cellspacing="0" style="margin:0 auto 24px;">
                        <tr><td style="background:#f59e0b;border-radius:10px;text-align:center;">
                          <a href="{enlace}" style="display:inline-block;padding:14px 32px;color:#0f172a;font-size:15px;font-weight:700;text-decoration:none;letter-spacing:.3px;">
                            🔑 Restablecer contraseña
                          </a>
                        </td></tr>
                      </table>

                      <p style="color:#94a3b8;font-size:13px;text-align:center;margin:0 0 8px;">
                        Este enlace expira en <strong>30 minutos</strong>.
                      </p>
                      <p style="color:#94a3b8;font-size:13px;text-align:center;margin:0;">
                        Si no solicitaste este cambio, ignorá este correo.
                      </p>
                    </td></tr>

                    <!-- Divider -->
                    <tr><td style="padding:0 32px;"><hr style="border:none;border-top:1px solid #e2e8f0;margin:0;"></td></tr>

                    <!-- Footer -->
                    <tr><td style="padding:20px 32px;text-align:center;">
                      <p style="color:#cbd5e1;font-size:12px;margin:0;">
                        © {DateTime.Now.Year} EP Bobinado — Este correo fue generado automáticamente, no respondas a este mensaje.
                      </p>
                    </td></tr>

                  </table>
                </td></tr>
              </table>
            </body>
            </html>
            """;
    }
}
