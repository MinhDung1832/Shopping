using System.Net;
using System.Net.Mail;

namespace Shopping.Areas.Admin.Repository
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true, //bật bảo mật
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("minhdung1832@gmail.com", "Redsun180302")
			};

			return client.SendMailAsync(
				new MailMessage(from: "minhdung1832@gmail.com",
								to: email,
								subject,
								message
								));
		}
	}
}
