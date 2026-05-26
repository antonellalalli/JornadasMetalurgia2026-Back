using HandlebarsDotNet;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

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
            Console.WriteLine($"{_config["USEREMAIL"]}");
            Console.WriteLine($"{_config["EMAILPASS"]}");
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "confirmacion.html");
            string templateText = await File.ReadAllTextAsync(templatePath);

            var template = Handlebars.Compile(templateText);
            var data = new { StudentName = studentName, Id = id };
            string html = template(data);



            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Jornadas Metalurgia", _config["USEREMAIL"]));
            email.To.Add(new MailboxAddress(studentName, recipient));
            email.Subject = "Confirmación de Inscripción";
            email.Body = new TextPart("html") { Text = html };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["USEREMAIL"], _config["EMAILPASS"]);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
    }
}
