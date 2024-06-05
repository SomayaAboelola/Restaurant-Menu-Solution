using DALayer.Entities;
using System.Net;
using System.Net.Mail;

namespace PLayer.Helper
{
	public static class EmailSetting
	{

		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("somaymagdy290@gmail.com", "hcqbcrnzsdjvsczm");

			Client.Send("somaymagdy290@gmail.com", email.To, email.Subject, email.Body);

		}
	}
}
