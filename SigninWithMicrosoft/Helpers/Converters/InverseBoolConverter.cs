﻿namespace SigninWithMicrosoft.Helpers.Converters;

public class InverseBoolConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null) return false;
		return !(bool)value;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return false;
	}
}
