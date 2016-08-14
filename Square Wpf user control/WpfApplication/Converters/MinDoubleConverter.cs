using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfApplication.Converters
{
	/// <summary>
	/// Returns minimal value of input double values.
	/// </summary>
	[ValueConversion(typeof(double[]), typeof(double))]
	public class MinDoubleConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType,
			object parameter, CultureInfo culture)
		{
			Debug.Assert(targetType.IsAssignableFrom(typeof(double)),
				$"targetType should be {typeof(double).FullName}");

			// values:
			if (values == null || values.Length <= 0)
				return DependencyProperty.UnsetValue;

			var tValues = Convert(values, culture);

			// check that all input values were not wrong
			if (tValues.Count <= 0)
				return DependencyProperty.UnsetValue;

			// return aggregate value
			var tResult = tValues.Min();

			// check parameter and round resulting value
			var roundDigits = 0;
			if (Convert(parameter, culture,
				(value1, culture1) => Math.Max(0, System.Convert.ToInt32(value1)),
				ref roundDigits))
			{
				tResult = Math.Round(tResult, roundDigits);
			}

			return System.Convert.ChangeType(tResult, targetType);
		}

		public object[] ConvertBack(object value, Type[] targetTypes,
			object parameter, CultureInfo culture)
		{
			// no back conversion
			return null;
		}

		private static IList<double> Convert(object[] values, CultureInfo culture)
		{
			var tValues = new List<double>(values.Length);
			foreach (var value in values)
			{
				double result = 0;

				if (Convert(value, culture,
					(value1, culture1) =>
					{
						var result1 = System.Convert.ToDouble(value1);
						return double.IsNaN(result1) ? 0 : result1;
					},
					ref result))
				{
					tValues.Add(result);
				}
			}

			return tValues;
		}

		private static bool Convert<T>(object value, CultureInfo culture,
			Func<object, CultureInfo, T> convertFunc, ref T tResult)
		{
			try
			{
				if (value == null || value.Equals(DependencyProperty.UnsetValue))
					return false;
				tResult = convertFunc(value, culture);
			}
			catch (FormatException ex)
			{
				// ignore it, some wrong input value
				Debug.Assert(false, DebugMessage<T>(value, ex));
			}
			catch (InvalidCastException ex)
			{
				// ignore it, some wrong input value
				Debug.Assert(false, DebugMessage<T>(value, ex));
			}
			catch (OverflowException ex)
			{
				// ignore it, some wrong input value
				Debug.Assert(false, DebugMessage<T>(value, ex));
			}
			return true;
		}

		private static string DebugMessage<T>(object value, Exception ex)
		{
			return
				$"value \"{value?.ToString() ?? "null"}\", type {value?.GetType().FullName ?? "null"}" +
				$" should be convertible to type {typeof(T).FullName}. Exception: {ex.Message}";
		}
	}
}
