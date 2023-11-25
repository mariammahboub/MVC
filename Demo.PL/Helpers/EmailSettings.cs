using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		//YOu cant Send Email Without Account  on Email Server 
		public static void  SendEmail(Email email)
		{
			// (Host ,Port )
			var Client =  new SmtpClient("smtp.gmail.com",587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("omniasabty@gmail.com", "tvwkglvacdxshdqp");
			Client.Send("omniasabty@gmail.com", email.Recipients, email.Subject, email.Body);
		}
	}
}
