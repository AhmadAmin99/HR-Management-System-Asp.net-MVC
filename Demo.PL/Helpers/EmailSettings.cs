using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("ahmedamin4449@gmail.com", "zxjvloeveydftmsx");
			client.Send("ahmedamin4449@gmail.com",email.Recipients, email.Subject, email.Body);
		}
	}
}
