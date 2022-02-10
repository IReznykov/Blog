using FluentAssertions;
using Microsoft.Exchange.WebServices.Data;
using NUnit.Framework;
using System;

namespace EWSMailMessage.Tests
{
	public class EmailMethodsTests
	{
		#region Null checks

		[TestCase]
		public void EmailMethods_ShouldReturn_NullIfServiceNull()
		{
			// arrange
			ExchangeService service = null;
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			// act
			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// assert
			emailMessage.Should().BeNull();
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldReturn_NullIfSubjectNullOrEmpty(string subject)
		{
			// arrange
			var service = new ExchangeService();
			var body = string.Empty;
			var address = string.Empty;
			var emailMethods = new EmailMethods();

			// act
			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// assert
			emailMessage.Should().BeNull();
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldReturn_NullIfAddressNullOrEmpty(string address)
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = string.Empty;
			var emailMethods = new EmailMethods();

			// act
			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// assert
			emailMessage.Should().BeNull();
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldReturn_MessageIfBodyNullOrEmpty(string body)
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			// act
			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// assert
			emailMessage.Should().NotBeNull();
		}

		[TestCase]
		public void EmailMethods_ShouldReturn_MessageIfParametersNotNull()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			// act
			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// assert
			emailMessage.Should().NotBeNull();
		}

		#endregion

		#region Properties

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldReturn_NullReceiveByIfEmptyValue(string receivedBy)
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetReceivedBy(emailMessage, receivedBy);

			//DatatServiceObjectProperty
			// assert
			emailMessage.Should().NotBeNull();
			Assert.Throws<ServiceObjectPropertyException>(
				() => { var receiveByAddress = emailMessage.ReceivedBy; });
		}

		[TestCase]
		public void EmailMethods_ShouldSet_ReceiveBy()
		{
			// arrange
			var receivedBy = Constant.Recepeint;
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetReceivedBy(emailMessage, receivedBy);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.ReceivedBy.Address.Should().BeEquivalentTo(receivedBy);
		}

		[TestCase]
		public void EmailMethods_ShouldUpdate_ReceiveBy()
		{
			// arrange
			var receivedByFirst = "first@example.com";
			var receivedBySecond = "second@example.com";
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetReceivedBy(emailMessage, receivedByFirst);
			emailMethods.SetReceivedBy(emailMessage, receivedBySecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.ReceivedBy.Address.Should().BeEquivalentTo(receivedBySecond);
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldNotUpdate_ReceiveByByEmptyValue(string receivedBySecond)
		{
			// arrange
			var receivedByFirst = "first@example.com";
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetReceivedBy(emailMessage, receivedByFirst);
			emailMethods.SetReceivedBy(emailMessage, receivedBySecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.ReceivedBy.Address.Should().BeEquivalentTo(receivedByFirst);
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("\t")]
		[TestCase("   ")]
		[TestCase("  \t")]
		public void EmailMethods_ShouldUpdate_NullReceiveBy(string receivedByFirst)
		{
			// arrange
			var receivedBySecond = "second@example.com";
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetReceivedBy(emailMessage, receivedByFirst);
			emailMethods.SetReceivedBy(emailMessage, receivedBySecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.ReceivedBy.Address.Should().BeEquivalentTo(receivedBySecond);
		}

		[TestCase]
		public void EmailMethods_ShouldSet_DateTimeCreated()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTime = DateTime.Now.AddHours(-1);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeCreated(emailMessage, dateTime);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeCreated.Should().Be(dateTime);
		}

		[TestCase]
		public void EmailMethods_ShouldUpdate_DateTimeCreated()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTimeFirst = DateTime.Now.AddHours(-1);
			var dateTimeSecond = DateTime.Now.AddHours(-10);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeCreated(emailMessage, dateTimeFirst);
			emailMethods.SetDateTimeCreated(emailMessage, dateTimeSecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeCreated.Should().Be(dateTimeSecond);
		}

		[TestCase]
		public void EmailMethods_ShouldSet_DateTimeSent()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTime = DateTime.Now.AddHours(-1);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeSent(emailMessage, dateTime);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeSent.Should().Be(dateTime);
		}

		[TestCase]
		public void EmailMethods_ShouldUpdate_DateTimeSent()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTimeFirst = DateTime.Now.AddHours(-1);
			var dateTimeSecond = DateTime.Now.AddHours(-10);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeSent(emailMessage, dateTimeFirst);
			emailMethods.SetDateTimeSent(emailMessage, dateTimeSecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeSent.Should().Be(dateTimeSecond);
		}

		[TestCase]
		public void EmailMethods_ShouldSet_DateTimeReceived()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTime = DateTime.Now.AddHours(-1);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeReceived(emailMessage, dateTime);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeReceived.Should().Be(dateTime);
		}

		[TestCase]
		public void EmailMethods_ShouldUpdate_DateTimeReceived()
		{
			// arrange
			var service = new ExchangeService();
			var subject = Constant.Subject;
			var body = Constant.Body;
			var address = Constant.Address;
			var dateTimeFirst = DateTime.Now.AddHours(-1);
			var dateTimeSecond = DateTime.Now.AddHours(-10);
			var emailMethods = new EmailMethods();

			var emailMessage = emailMethods.CreateEmailMessage(service, subject, body, address);

			// act
			emailMethods.SetDateTimeReceived(emailMessage, dateTimeFirst);
			emailMethods.SetDateTimeReceived(emailMessage, dateTimeSecond);

			// assert
			emailMessage.Should().NotBeNull();
			emailMessage.DateTimeReceived.Should().Be(dateTimeSecond);
		}

		#endregion

		private static class Constant
		{
			public const string Subject = "Subject";
			public const string Body = "Interesting text";
			public const string Address = "illya@ireznykov.com";
			public const string Recepeint = "ews@example.com";
		}
	}
}
