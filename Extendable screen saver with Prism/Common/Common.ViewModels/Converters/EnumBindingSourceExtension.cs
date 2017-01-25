using System;
using System.Windows.Markup;

namespace Ikc5.ScreenSaver.Common.Views.Converters
{
	/// <summary>
	/// This extension will simply take an enum Type and then create a bindable
	/// list of enum values for my control.
	/// Code is taken from: #2 The Better Way
	/// http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
	/// </summary>
	public class EnumBindingSourceExtension : MarkupExtension
	{
		private Type _enumType;

		public EnumBindingSourceExtension()
		{
		}

		public EnumBindingSourceExtension(Type enumType)
		{
			EnumType = enumType;
		}

		public Type EnumType
		{
			get { return _enumType; }
			set
			{
				if (value != _enumType)
				{
					if (null != value)
					{
						var enumType = Nullable.GetUnderlyingType(value) ?? value;

						if (!enumType.IsEnum)
							throw new ArgumentException("Type must be for an Enum.");
					}
					_enumType = value;
				}
			}
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (null == _enumType)
				throw new InvalidOperationException("The EnumType must be specified.");
			var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
			var enumValues = Enum.GetValues(actualEnumType);
			if (actualEnumType == _enumType)
				return enumValues;
			var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
			enumValues.CopyTo(tempArray, 1);
			return tempArray;
		}
	}
}