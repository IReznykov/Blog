using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EWSMailMessage
{
	public class EmailMethods : IEmailMethods
	{
		public EmailMessage CreateEmailMessage(ExchangeService service,
			 string subject,
			 string body,
			 string address)
		{
			if (service == null ||
				string.IsNullOrWhiteSpace(subject) ||
				string.IsNullOrWhiteSpace(address))
				return null;

			var emailMessage = new EmailMessage(service)
			{
				Subject = subject,
				Body = body,
				ItemClass = "IPM.Note",
				From = address
			};

			return emailMessage;
		}

		public void SetReceivedBy(EmailMessage emailMessage, string address)
		{
			if (emailMessage == null ||
				string.IsNullOrWhiteSpace(address))
				return;
			SetProperty(emailMessage, EmailMessageSchema.ReceivedBy, new EmailAddress(address));
			return;
		}

		public void SetDateTimeCreated(EmailMessage emailMessage, DateTime dateTime)
		{
			if (emailMessage == null)
				return;
			SetProperty(emailMessage, ItemSchema.DateTimeCreated, dateTime);
			return;
		}

		public void SetDateTimeSent(EmailMessage emailMessage, DateTime dateTime)
		{
			if (emailMessage == null)
				return;
			SetProperty(emailMessage, ItemSchema.DateTimeSent, dateTime);
			return;
		}

		public void SetDateTimeReceived(EmailMessage emailMessage, DateTime dateTime)
		{
			if (emailMessage == null)
				return;
			SetProperty(emailMessage, ItemSchema.DateTimeReceived, dateTime);
			return;
		}

		#region Set property

		private bool SetProperty(EmailMessage message,
			 PropertyDefinition propertyDefinition,
			 object value)
		{
			if (message == null)
				return false;

			// get value of PropertyBag property – that is wrapper
			// over dictionary of inner message’s properties
			var members = message.GetType().FindMembers(
				MemberTypes.Property,
				BindingFlags.NonPublic | BindingFlags.Instance,
				PartialName,
				"PropertyBag");
			if (members.Length < 1)
				return false;

			var propertyInfo = members[0] as PropertyInfo;
			if (propertyInfo == null)
				return false;

			var bag = propertyInfo.GetValue(message, null);
			members = bag.GetType().FindMembers(
				MemberTypes.Property,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
				PartialName,
				"Properties");
			if (members.Length < 1)
				return false;

			// get dictionary of properties values
			var properties = ((PropertyInfo)members[0]).GetMethod.Invoke(bag, null);
			var dictionary = properties as Dictionary<PropertyDefinition, object>;
			if (dictionary == null)
				return false;

			dictionary[propertyDefinition] = value;
			return true;
		}

		private bool PartialName(MemberInfo info, Object part)
		{
			// Test whether the name of the candidate member contains the
			// specified partial name.
			return info.Name.Contains(part.ToString());
		}

		#endregion
	}
}
