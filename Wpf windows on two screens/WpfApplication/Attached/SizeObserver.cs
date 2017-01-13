using System.Windows;
using Ikc5.TypeLibrary;

namespace WpfApplication.Attached
{
	/// <summary>
	/// An attached behavior that has ObservedWidth and ObservedHeight attached properties. 
	/// It also has an Observe property that is used to do the initial hook-up. Usage looks like this:
	///	<UserControl>
    ///		SizeObserver.Observe="True"
    ///		SizeObserver.ObservedWidth="{Binding Width, Mode=OneWayToSource}"
    ///		SizeObserver.ObservedHeight="{Binding Height, Mode=OneWayToSource}"
    /// </UserControl>
	/// http://stackoverflow.com/questions/1083224/pushing-read-only-gui-properties-back-into-viewmodel
	/// </summary>
	public static class SizeObserver
	{
		public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
			"Observe",
			typeof(bool),
			typeof(SizeObserver),
			new FrameworkPropertyMetadata(OnObserveChanged));

		public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
			"ObservedWidth",
			typeof(double),
			typeof(SizeObserver));

		public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
			"ObservedHeight",
			typeof(double),
			typeof(SizeObserver));

		public static bool GetObserve(FrameworkElement frameworkElement)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			return (bool)frameworkElement.GetValue(ObserveProperty);
		}

		public static void SetObserve(FrameworkElement frameworkElement, bool observe)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			frameworkElement.SetValue(ObserveProperty, observe);
		}

		public static double GetObservedWidth(FrameworkElement frameworkElement)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			return (double)frameworkElement.GetValue(ObservedWidthProperty);
		}

		public static void SetObservedWidth(FrameworkElement frameworkElement, double observedWidth)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			frameworkElement.SetValue(ObservedWidthProperty, observedWidth);
		}

		public static double GetObservedHeight(FrameworkElement frameworkElement)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			return (double)frameworkElement.GetValue(ObservedHeightProperty);
		}

		public static void SetObservedHeight(FrameworkElement frameworkElement, double observedHeight)
		{
			frameworkElement.ThrowIfNull(nameof(frameworkElement));
			frameworkElement.SetValue(ObservedHeightProperty, observedHeight);
		}

		private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var frameworkElement = dependencyObject as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}

			if ((bool)e.NewValue)
			{
				frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
				UpdateObservedSizesForFrameworkElement(frameworkElement);
			}
			else
			{
				frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
			}
		}

		private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
		}

		private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
		{
			// WPF 4.0 onwards
			frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
			frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);

			// WPF 3.5 and prior
			////SetObservedWidth(frameworkElement, frameworkElement.ActualWidth);
			////SetObservedHeight(frameworkElement, frameworkElement.ActualHeight);
		}
	}
}
