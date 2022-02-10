using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSMailMessage
{
	class Program
	{
		static void Main(string[] args)
		{
			var emailMethods = new EmailMethods();
			var exchangeServer = new ExchangeService();
			var emailMessage = emailMethods.CreateEmailMessage(
				exchangeServer, "New Subject", "Interesting text", "illya@ireznykov.com");

			emailMethods.SetReceivedBy(emailMessage, "ews@example.com");
			emailMethods.SetDateTimeCreated(emailMessage, DateTime.Now.AddDays(-1));
			emailMethods.SetDateTimeSent(emailMessage, DateTime.Now.AddHours(-22));
			emailMethods.SetDateTimeReceived(emailMessage, DateTime.Now.AddHours(-20));

			Console.WriteLine($"Mail.Subject = {emailMessage.Subject}");
			Console.WriteLine($"Mail.From = {emailMessage.From}");
			Console.WriteLine($"Mail.ReceivedBy = {emailMessage.ReceivedBy}");
			Console.WriteLine($"Mail.DateTimeCreated = {emailMessage.DateTimeCreated}");
			Console.WriteLine($"Mail.DateTimeSent = {emailMessage.DateTimeSent}");
			Console.WriteLine($"Mail.DateTimeReceived = {emailMessage.DateTimeReceived}");

			Console.ReadKey();
		}
	}
}
