using System.Net.Mail;
using System.Net;
using Hangfire;
using Hangfire.MemoryStorage;
using KidProEdu.Domain.Entities;
using KidProEdu.Application.Services;
using System.Net.Mime;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace KidProEdu.Application.Utils
{
    public static class SendEmailUtil
    {
        public static async Task SendEmail(string toEmail, string subject, string body)
        {
            // Thông tin tài khoản email của bạn
            string fromEmail = "kidproedu6868@gmail.com";
            string password = "xpty whcp ubjt ogaz";

            // Thông tin người nhận
            //string toEmail = email;

            // Tạo đối tượng MailMessage
            MailMessage mail = new MailMessage(fromEmail, toEmail);

            // Tiêu đề email
            mail.Subject = subject;

            // Nội dung email
            mail.Body = body;

            // Cấu hình SmtpClient để gửi email thông qua SMTP của Gmail
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587; // Port của SMTP Gmail
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);
            smtpClient.EnableSsl = true; // Kích hoạt SSL

            try
            {
                // Gửi email
                 await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
            }
            finally
            {
                // Giải phóng tài nguyên
                mail.Dispose();
                smtpClient.Dispose();
            }
        }

        /*public async static Task SendEmailJob()
        {
            // Khởi tạo Hangfire
            *//*GlobalConfiguration.Configuration.UseMemoryStorage();
            using (var server = new BackgroundJobServer())
            {
                // Lập lịch công việc gửi email
                //RecurringJob.AddOrUpdate("send-email-job", () => SendEmail("hello"), "0 0 * * *");
                RecurringJob.AddOrUpdate("send-email-job1", () => SendEmail("tkchoi1312@gmail.com"), "*A/5 * * * * *");

                Console.WriteLine("Hangfire Server started. ");
                Console.ReadKey();
            }*//*
            //RecurringJob.AddOrUpdate("send-email-job", () => SendEmail("tkchoi1312@gmail.com"), "*A/5 * * * * *");
            //RecurringJob.AddOrUpdate("send-email-job", () => SendEmail("tkchoi1312@gmail.com"), "0 0 * * *");

        }*/

        public static async Task SendEmailWithAttachment(string recipient, string subject, string body, MimePart attachment)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("KidProEdu", "kidproedu6868@gmail.com"));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            /*var attachment = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(attachmentStream),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentFileName
            };*/
            builder.Attachments.Add(attachment);

            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("kidproedu6868@gmail.com", "xpty whcp ubjt ogaz");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
