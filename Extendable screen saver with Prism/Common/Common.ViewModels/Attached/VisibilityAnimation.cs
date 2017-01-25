using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using Ikc5.ScreenSaver.Common.Models.Types;

namespace Ikc5.ScreenSaver.Common.Views.Attached
{
	/// <summary>
	/// Supplies attached properties that provides visibility of animations
	/// http://www.codeproject.com/Articles/57175/WPF-How-To-Animate-Visibility-Property
	/// </summary>
	public static class VisibilityAnimation
	{
		/// <summary>
		/// List of hooked objects
		/// </summary>
		private static readonly Dictionary<FrameworkElement, bool> HookedElements = new Dictionary<FrameworkElement, bool>();

		/// <summary>
		/// VisibilityAnimation static ctor
		/// </summary>
		static VisibilityAnimation()
		{
			// Here we "register" on Visibility property "before change" event
			UIElement.VisibilityProperty.AddOwner(
				typeof(FrameworkElement),
				new FrameworkPropertyMetadata(Visibility.Visible, VisibilityChanged, CoerceVisibility));
		}

		#region DependencyProperties

		/// <summary>
		/// Using a DependencyProperty as the backing store for AnimationType.
		/// </summary>
		public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.RegisterAttached(
			"AnimationType",
			typeof(AnimationType),
			typeof(VisibilityAnimation),
			new FrameworkPropertyMetadata(AnimationType.None, new PropertyChangedCallback(OnAnimationTypePropertyChanged)));

		/// <summary>
		/// Get AnimationType attached property
		/// </summary>
		/// <param name="obj">Dependency object</param>
		/// <returns>AnimationType value</returns>
		public static AnimationType GetAnimationType(DependencyObject obj)
		{
			return (AnimationType)obj.GetValue(AnimationTypeProperty);
		}

		/// <summary>
		/// Set AnimationType attached property
		/// </summary>
		/// <param name="obj">Dependency object</param>
		/// <param name="value">New value for AnimationType</param>
		public static void SetAnimationType(DependencyObject obj, AnimationType value)
		{
			obj.SetValue(AnimationTypeProperty, value);
		}

		/// <summary>
		/// AnimationType property changed
		/// </summary>
		/// <param name="dependencyObject">Dependency object</param>
		/// <param name="e">Arguments</param>
		private static void OnAnimationTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var frameworkElement = dependencyObject as FrameworkElement;

			if (frameworkElement == null)
			{
				return;
			}

			// If AnimationType is set to True on this framework element, 
			if (GetAnimationType(frameworkElement) != AnimationType.None)
			{
				// Add this framework element to hooked list
				HookVisibilityChanges(frameworkElement);
			}
			else
			{
				// Otherwise, remove it from the hooked list
				UnHookVisibilityChanges(frameworkElement);
			}
		}

		/// <summary>
		/// Using a DependencyProperty as the backing store for AnimationDuration.
		/// </summary>
		public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.RegisterAttached(
			"AnimationDuration",
			typeof(int),
			typeof(VisibilityAnimation),
			new FrameworkPropertyMetadata(AnimationDuration));

		/// <summary>
		/// Animation duration
		/// </summary>
		private const int AnimationDuration = 500;

		/// <summary>
		/// Get AnimationDuration attached property
		/// </summary>
		/// <param name="obj">Dependency object</param>
		/// <returns>Animation duration value</returns>
		public static int GetAnimationDuration(DependencyObject obj)
		{
			return (int)obj.GetValue(AnimationDurationProperty);
		}

		/// <summary>
		/// Set AnimationDuration attached property
		/// </summary>
		/// <param name="obj">Dependency object</param>
		/// <param name="value">New value for AnimationDuration</param>
		public static void SetAnimationDuration(DependencyObject obj, int value)
		{
			obj.SetValue(AnimationDurationProperty, value);
		}

		#endregion

		/// <summary>
		/// Add framework element to list of hooked objects
		/// </summary>
		/// <param name="frameworkElement">Framework element</param>
		private static void HookVisibilityChanges(FrameworkElement frameworkElement)
		{
			HookedElements.Add(frameworkElement, false);
		}

		/// <summary>
		/// Remove framework element from list of hooked objects
		/// </summary>
		/// <param name="frameworkElement">Framework element</param>
		private static void UnHookVisibilityChanges(FrameworkElement frameworkElement)
		{
			if (HookedElements.ContainsKey(frameworkElement))
			{
				HookedElements.Remove(frameworkElement);
			}
		}

		/// <summary>
		/// Visibility changed
		/// </summary>
		/// <param name="dependencyObject">Dependency object</param>
		/// <param name="e">e</param>
		private static void VisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			// Ignore
		}

		/// <summary>
		/// Coerce visibility
		/// </summary>
		/// <param name="dependencyObject">Dependency object</param>
		/// <param name="baseValue">Base value</param>
		/// <returns>Coerced value</returns>
		private static object CoerceVisibility(DependencyObject dependencyObject, object baseValue)
		{
			// Make sure object is a framework element
			FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
			if (frameworkElement == null)
			{
				return baseValue;
			}

			// Cast to type safe value
			Visibility visibility = (Visibility)baseValue;

			// If Visibility value hasn't change, do nothing.
			// This can happen if the Visibility property is set using data binding and the binding source has changed 
			// but the new visibility value hasn't changed.
			if (visibility == frameworkElement.Visibility)
			{
				return baseValue;
			}

			// If element is not hooked by our attached property, stop here
			if (!IsHookedElement(frameworkElement))
			{
				return baseValue;
			}

			// Update animation flag
			// If animation already started - don't restart it (otherwise, infinite loop)
			if (UpdateAnimationStartedFlag(frameworkElement))
			{
				return baseValue;
			}

			// If we get here, it means we have to start fade in or fade out animation. 
			// In any case return value of this method will be Visibility.Visible, to allow the animation.
			var doubleAnimation = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromMilliseconds(GetAnimationDuration(frameworkElement)))
			};

			// When animation completes, set the visibility value to the requested value (baseValue)
			doubleAnimation.Completed += (sender, eventArgs) =>
			{
				if (visibility == Visibility.Visible)
				{
					// In case we change into Visibility.Visible, the correct value is already set
					// So just update the animation started flag
					UpdateAnimationStartedFlag(frameworkElement);
				}
				else
				{
					// This will trigger value coercion again 
					// but UpdateAnimationStartedFlag() function will return true this time, 
					// thus animation will not be triggered. 
					if (BindingOperations.IsDataBound(frameworkElement, UIElement.VisibilityProperty))
					{
						// Set visibility using bounded value
						var bindingValue = BindingOperations.GetBinding(frameworkElement, UIElement.VisibilityProperty);
						if (bindingValue != null)
							BindingOperations.SetBinding(frameworkElement, UIElement.VisibilityProperty, bindingValue);
					}
					else
					{
						// No binding, just assign the value
						frameworkElement.Visibility = visibility;
					}
				}
			};

			if (visibility == Visibility.Collapsed || visibility == Visibility.Hidden)
			{
				// Fade out by animating opacity
				doubleAnimation.From = 1.0;
				doubleAnimation.To = 0.0;
			}
			else
			{
				// Fade in by animating opacity
				doubleAnimation.From = 0.0;
				doubleAnimation.To = 1.0;
			}

			// Start animation
			frameworkElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

			// Make sure the element remains visible during the animation
			// The original requested value will be set in the completed event of the animation
			return Visibility.Visible;
		}

		/// <summary>
		/// Check if framework element is hooked with AnimationType property
		/// </summary>
		/// <param name="frameworkElement">Framework element to check</param>
		/// <returns>Is the framework element hooked?</returns>
		private static bool IsHookedElement(FrameworkElement frameworkElement)
		{
			return HookedElements.ContainsKey(frameworkElement);
		}

		/// <summary>
		/// Update animation started flag or a given framework element
		/// </summary>
		/// <param name="frameworkElement">Given framework element</param>
		/// <returns>Old value of animation started flag</returns>
		private static bool UpdateAnimationStartedFlag(FrameworkElement frameworkElement)
		{
			var animationStarted = HookedElements[frameworkElement];
			HookedElements[frameworkElement] = !animationStarted;

			return animationStarted;
		}
	}
}