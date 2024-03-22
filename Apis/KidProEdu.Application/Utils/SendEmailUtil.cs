using System.Net.Mail;
using System.Net;
using Hangfire;

namespace KidProEdu.Application.Utils
{
    public static class SendEmailUtil
    {
        public static async Task SendEmail(string email)
        {
            // Thông tin tài khoản email của bạn
            string fromEmail = "nghiapdse150938@fpt.edu.vn";
            string password = "thanthienchuyenmon";

            // Thông tin người nhận
            string toEmail = email;

            // Tạo đối tượng MailMessage
            MailMessage mail = new MailMessage(fromEmail, toEmail);

            // Tiêu đề email
            mail.Subject = "Test email from C#";

            // Nội dung email
            mail.Body = "This is a test email sent from C#.";

            // Cấu hình SmtpClient để gửi email thông qua SMTP của Gmail
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
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

        public async static Task AutoSendEmail()
        {
            // Khởi tạo Hangfire
            //GlobalConfiguration.Configuration.UseMemoryStorage();
            using (var server = new BackgroundJobServer())
            {
                // Lập lịch công việc gửi email
                RecurringJob.AddOrUpdate("send-email-job", () => SendEmail("hello"), Cron.Daily);

                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
