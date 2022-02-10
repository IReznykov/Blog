using Microsoft.Exchange.WebServices.Data;
using System;

namespace EWSMailMessage
{
	public interface IEmailMethods
	{
		EmailMessage CreateEmailMessage(ExchangeService service, string subject, string body, string address);
		void SetDateTimeCreated(EmailMessage emailMessage, DateTime dateTime);
		void SetDateTimeReceived(EmailMessage emailMessage, DateTime dateTime);
		void SetDateTimeSent(EmailMessage emailMessage, DateTime dateTime);
		void SetReceivedBy(EmailMessage emailMessage, string address);
	}
}