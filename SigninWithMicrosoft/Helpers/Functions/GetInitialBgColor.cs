namespace SigninWithMicrosoft.Helpers.Functions;

public class GetInitialBgColor
{
	public static Color Function(string name)
	{
		Color initialColor = Colors.White;
		if (!string.IsNullOrEmpty(name)) 
		{
			initialColor = name[..1] switch
			{
				"A" => Colors.DarkBlue,
				"B" => Colors.DarkCyan,
				"C" => Colors.DarkGoldenrod,
				"D" => Colors.DarkGray,
				"E" => Colors.DarkGreen,
				"F" => Colors.DarkKhaki,
				"G" => Colors.DarkMagenta,
				"H" => Colors.DarkOliveGreen,
				"I" => Colors.DarkOrange,
				"J" => Colors.DarkOrchid,
				"K" => Colors.DarkRed,
				"L" => Colors.DarkSalmon,
				"M" => Colors.DarkSeaGreen,
				"N" => Colors.DarkSlateBlue,
				"O" => Colors.DarkSlateGray,
				"P" => Colors.DarkTurquoise,
				"Q" => Colors.DarkViolet,
				"R" => Colors.DeepPink,
				"S" => Colors.DeepSkyBlue,
				"T" => Colors.DimGray,
				"U" => Colors.DodgerBlue,
				"V" => Colors.Tan,
				"W" => Colors.Teal,
				"X" => Colors.Thistle,
				"Y" => Colors.Tomato,
				"Z" => Colors.Turquoise,
				_ => Colors.Black,
			};
		}
		return initialColor;
	}
}
