using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitTestLibrary
{
	public class ObjectFactoryTests
	{
		[Fact]
		public void GetObject_Should_ReturnNonEmptyObject()
		{
			object liteObject = null;
			var exception = Record.Exception(() => liteObject = (new ObjectFactory()).GetObject());

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);
		}

		[Fact]
		public void GetObject_Should_ReturnObjectWithTwoProperties()
		{
			object liteObject = null;
			var exception = Record.Exception(() => liteObject = (new ObjectFactory()).GetObject());

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);

			var propertyInfos = TypeDescriptor.GetProperties(liteObject.GetType());
			Assert.Equal(propertyInfos.Count, ObjectFactory.PropertyNames.Length);

			for (var pos = 0; pos < ObjectFactory.PropertyNames.Length; pos++)
			{
				var property = propertyInfos[ObjectFactory.PropertyNames[pos]];
				Assert.NotNull(property);
				Assert.Equal(property.PropertyType, ObjectFactory.PropertyTypes[pos]);
			}
		}

		[Fact]
		public void GetObject_Should_ReturnDynamicWithTwoProperties()
		{
			dynamic liteObject = null;
			var exception = Record.Exception(() => liteObject = (new ObjectFactory()).GetObject());

			Assert.Null(exception);
			Assert.NotNull(liteObject);

			// check correct properties
			exception = Record.Exception(() =>
			{
				var name = liteObject.Name;
				Assert.Equal(name, null);
			});
			Assert.Null(exception);

			exception = Record.Exception(() =>
			{
				var count = liteObject.Count;
				Assert.Equal(count, new int());
			});
			Assert.Null(exception);

			// check wrong property
			exception = Record.Exception(() =>
			{
				var wrong = liteObject.Wrong;
				Assert.NotNull(wrong);
			});
			Assert.NotNull(exception);
			Assert.Equal(exception.Message, "'LiteObject' does not contain a definition for 'Wrong'");
		}

		[Fact]
		public void GetObject_Should_AssignValuesViaReflection()
		{
			const string name = "TestName1";
			const int count = 10;

			object liteObject = null;
			var exception = Record.Exception(() => liteObject = (new ObjectFactory()).GetObjectViaReflection(name, count));

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);

			var propertyInfos = TypeDescriptor.GetProperties(liteObject.GetType());
			var values = new object[] { name, count };
			for (var pos = 0; pos < ObjectFactory.PropertyNames.Length; pos++)
			{
				var property = propertyInfos[ObjectFactory.PropertyNames[pos]];
				Assert.NotNull(property);
				Assert.Equal(property.GetValue(liteObject), values[pos]);
			}
		}

		[Fact]
		public void GetObject_Should_AssignValuesViaDynamic()
		{
			const string name = "TestName2";
			const int count = 20;

			dynamic liteObject = null;
			var exception = Record.Exception(() => liteObject = (new ObjectFactory()).GetObjectViaDynamic(name, count));

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);
			Assert.Equal(liteObject.Name, name);
			Assert.Equal(liteObject.Count, count);
		}

	}
}
