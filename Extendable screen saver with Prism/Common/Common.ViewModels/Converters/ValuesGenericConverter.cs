using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ikc5.ScreenSaver.Common.Views.Converters
{
	/// <summary>
	/// Returns aggregate value of input values.
	/// </summary>
	public abstract class ValuesGenericConverter<T> : BaseGenericConverter<T>, IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.Assert(targetType.IsAssignableFrom(typeof(T)), $"targetType should be {typeof(T).FullName}");

			// values:
			if (values == null || values.Length <= 0)
				return DependencyProperty.UnsetValue;

			var tValues = Convert(values, culture);

			// check that all input values were wrong
			if (tValues.Count <= 0)
				return DependencyProperty.UnsetValue;

			// return aggregate value
			var tResult = AggregateMethod(tValues);

			ApplyParameter(parameter, culture, ref tResult);
			return System.Convert.ChangeType(tResult, targetType);

		}

		public object[] ConvertBack(object value, Type[] targetTypes,
			   object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}

		protected abstract Func<IEnumerable<T>, T> AggregateMethod { get; }

	}
}
