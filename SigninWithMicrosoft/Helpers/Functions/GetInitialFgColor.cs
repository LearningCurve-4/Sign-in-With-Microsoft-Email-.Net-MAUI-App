namespace SigninWithMicrosoft.Helpers.Functions;

public class GetInitialFgColor
{
	public static Color? Function(Color bgColor)
	{
		Color fgColor = Colors.White;
		if (bgColor != null)
		{
			if ((double)(bgColor.Red * .3 + bgColor.Green * .59 + bgColor.Blue * .11) > 0.5)
			{
				fgColor = Colors.Black;
			}
		}
		return fgColor;
	}
}
