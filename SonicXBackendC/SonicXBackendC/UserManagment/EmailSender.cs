using System;
using System.Net;
using System.Net.Mail;

namespace SonicXBackendC.UserManagement
{
    public class EmailSender
    {
        private const string Host = "smtp.gmail.com";
        private const int Port = 587;
        private const string Username = "your_email";
        private const string Password = "your_app_password";

        private static EmailSender instance;

        private EmailSender() { }

        public static EmailSender GetInstance()
        {
            if (instance == null)
            {
                instance = new EmailSender();
            }
            return instance;
        }

        public bool SendVerificationEmail(string recipientEmail, string verificationCode)
        {
            using (var client = new SmtpClient(Host, Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Username, Password);
                client.EnableSsl = true;

                var message = new MailMessage(Username, recipientEmail)
                {
                    Subject = "E-posta Doğrulama Kodu",
                    Body = $"Salam,\n\nE-posta doğrulama kodunuz: {verificationCode}"
                };

                try
                {
                    client.Send(message);
                    Console.WriteLine("Doğrulama kodu e-posta olarak gönderildi ");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("E-posta gönderilirken hata oluştu: " + e.Message);
                    return false;
                }
            }
        }
    }
}