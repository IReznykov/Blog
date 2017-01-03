using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApplication.Converters
{
	/// <summary>
	/// Accept two values and returns result of the binary operation between them.
	/// </summary>
	public abstract class OperationGenericConverter<T> : BaseGenericConverter<T>, IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.Assert(targetType.IsAssignableFrom(typeof(T)), $"targetType should be {typeof(T).FullName}");
			
			// values:
			if (values == null || values.Length <= 0)
				return DependencyProperty.UnsetValue;

			var tValues = Convert(values, culture);

			// check that all input values were wrong
			if (tValues.Count < 2)
				return DependencyProperty.UnsetValue;

			// return min value
			var tResult = BinaryMethod(tValues[0], tValues[1]);

			ApplyParameter(parameter, culture, ref tResult);
			return System.Convert.ChangeType(tResult, targetType);

		}

		public object[] ConvertBack(object value, Type[] targetTypes,
			   object parameter, CultureInfo culture)
		{
			return null;
		}

		protected abstract Func<T, T, T> BinaryMethod { get; }

	}
}
