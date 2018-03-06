using System.Diagnostics;

namespace TheWorld.Service
{
	public class DebugMailService : IMailService
	{
		public void SendMail(string to, string from, string subject, string body)
		{
			Debug.WriteLine($"Sending mail");
			Debug.WriteLine($"To: {to}\n From: {from}\n Subject: {subject}\n Message: {body}");
		}
	}
}