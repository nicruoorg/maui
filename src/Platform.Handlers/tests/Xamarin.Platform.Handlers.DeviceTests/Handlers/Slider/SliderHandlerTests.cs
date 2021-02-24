﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using Xamarin.Platform.Handlers.DeviceTests.Stubs;
using Xunit;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	[Category(TestCategory.Slider)]
	public partial class SliderHandlerTests : HandlerTestBase<SliderHandler>
	{
		[Theory(DisplayName = "Percent Value Initializes Correctly")]
		[InlineData(0, 1, 0)]
		[InlineData(0, 1, 0.5)]
		[InlineData(0, 1, 1)]
		[InlineData(0, 100, 0)]
		[InlineData(0, 100, 1)]
		[InlineData(0, 100, 5)]
		[InlineData(0, 100, 50)]
		[InlineData(0, 100, 100)]
		[InlineData(0, 100, 10000)]
		[InlineData(0, 100, -10000)]
		[InlineData(0, 10000, 10000)]
		[InlineData(0, 10000, -10000)]
		public async Task PercentValueInitializesCorrectly(double min, double max, double value)
		{
			var expectedValue = Math.Clamp(value, min, max);
			var expectedPercent = (expectedValue - min) / (max - min);

			var slider = new SliderStub()
			{
				Maximum = max,
				Minimum = min,
				Value = value
			};

			Assert.Equal(min, slider.Minimum);
			Assert.Equal(max, slider.Maximum);
			Assert.Equal(expectedValue, slider.Value);

			var native = await GetValueAsync(slider, (handler) =>
			{
				return new
				{
					Min = GetNativeMinimum(handler),
					Max = GetNativeMaximum(handler),
					Val = GetNativeProgress(handler),
				};
			});
			var nativePercent = (native.Val - native.Min) / (native.Max - native.Min);

			Assert.Equal(expectedPercent, nativePercent, 5);
		}

		[Fact(DisplayName = "Thumb Color Initializes Correctly")]
		public async Task ThumbColorInitializesCorrectly()
		{
			var slider = new SliderStub()
			{
				ThumbColor = Color.Purple
			};

			await ValidateNativeThumbColor(slider, Color.Purple);
		}

#if !__ANDROID__ // Android native control behavior works differently; see SliderHandlerTests.Android.cs
		[Fact(DisplayName = "Value Initializes Correctly")]
		public async Task ValueInitializesCorrectly()
		{
			var slider = new SliderStub()
			{
				Maximum = 1,
				Minimum = 0,
				Value = 0.5
			};

			await ValidatePropertyInitValue(slider, () => slider.Value, GetNativeProgress, slider.Value);
		}

		[Fact(DisplayName = "Maximum Value Initializes Correctly")]
		public async Task MaximumInitializesCorrectly()
		{
			var slider = new SliderStub()
			{
				Maximum = 1
			};

			await ValidatePropertyInitValue(slider, () => slider.Maximum, GetNativeMaximum, 1);
		}
#endif
	}
}
