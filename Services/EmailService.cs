using HandlebarsDotNet;
using SendGrid.Helpers.Mail;

namespace Jornadas_Metalurgia_2026.Services

{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService( IConfiguration config)
        {
            _config = config;
        }


        public async Task SendInscriptionMail (string recipient, string studentName, int id)
        {
        
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "confirmacion.html");
            string templateText = await File.ReadAllTextAsync(templatePath);

            var template = Handlebars.Compile(templateText);

            var html = template(new { StudentName = studentName, Id = id });

            var client = new SendGridClient(_config["SENDGRID_API_KEY"]);

            var from = new EmailAddress("jornadasmetalurgia@gmail.com", "Jornadas Metalurgia");
            var to = new EmailAddress(recipient);

            var msg = MailHelper.CreateSingleEmail(from, to, "Confirmación de Inscripción", null, html);
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new Exception($"Error al enviar el mail {response.StatusCode} - {body}");
            }
        }
    }
}
