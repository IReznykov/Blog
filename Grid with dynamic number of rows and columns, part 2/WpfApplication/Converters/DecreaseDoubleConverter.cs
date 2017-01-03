using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApplication.Converters
{
	/// <summary>
	/// Substract two double elements.
	/// </summary>
	[ValueConversion(typeof(double), typeof(double))]
	public class DecreaseDoubleConverter : ValueGenericConverter<double>
	{
		protected override Func<object, CultureInfo, double> ConvertMethod =>
				(value, culture) =>
				{
					if (value == null)
						return 0;
					var result = System.Convert.ToDouble(value);
					return double.IsNaN(result) ? 0 : result;
				};

		protected override Func<double, double, double> ApplyParameterMethod =>
			(value, parameter) => Math.Max(Math.Round(value - parameter, 2), 0);
	}
}
