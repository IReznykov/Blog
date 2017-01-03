using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace WpfApplication.Converters
{
	/// <summary>
	/// Base generic convertor. Contains some transform methods.
	/// </summary>
	public abstract class BaseGenericConverter<T>
	{
		protected IList<T> Convert(object[] values, CultureInfo culture)
		{
			var tValues = new List<T>(values.Length);
			foreach (var value in values)
			{
				if (value == null || value.Equals(DependencyProperty.UnsetValue))
					continue;
				try
				{
					tValues.Add(ConvertMethod(value, culture));
				}
				catch (InvalidCastException)
				{
					// ignore it, some wrong input value
					Debug.Assert(false, $"value \"{value?.ToString()??"null"}\", type {value?.GetType().FullName??"null"} should be convertible to type {typeof(T).FullName}");
					continue;
				}
			}

			return tValues;
		}

		protected bool Convert(object value, CultureInfo culture, ref T tValue)
		{
			if (value == null || value.Equals(DependencyProperty.UnsetValue))
				return false;

			try
			{
				tValue = ConvertMethod(value, culture);
			}
			catch (InvalidCastException)
			{
				// ignore it, some wrong input value
				Debug.Assert(false, $"value \"{value?.ToString() ?? "null"}\", type {value?.GetType().FullName ?? "null"} should be convertible to type {typeof(T).FullName}");
				return false;
			}
			return true;
		}

		protected bool ApplyParameter(object parameter, CultureInfo culture, ref T tResult)
		{
			if (ApplyParameterMethod == null ||
				parameter == null ||
				parameter.Equals(DependencyProperty.UnsetValue))
			{
				return false;
			}

			try
			{
				var tParameter = ConvertMethod(parameter, culture);
				tResult = ApplyParameterMethod(tResult, tParameter);
			}
			catch (InvalidCastException)
			{
				// ignore it, some wrong input value
				Debug.Assert(false, $"parameter \"{parameter?.ToString() ?? "null"}\", type {parameter?.GetType().FullName ?? "null"}should be convertible to type {typeof(T).FullName}");
				return false;
			}
			return true;
		}

		protected abstract Func<object, CultureInfo, T> ConvertMethod { get; }

		protected virtual Func<T, T, T> ApplyParameterMethod => null;

	}
}
