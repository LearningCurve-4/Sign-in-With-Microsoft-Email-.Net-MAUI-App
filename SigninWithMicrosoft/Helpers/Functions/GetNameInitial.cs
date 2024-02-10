namespace SigninWithMicrosoft.Helpers.Functions;

public class GetNameInitial
{
	public static string? Function(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) { return null; }
		string[] totWords = name.Split(' ');
		if (totWords.Length == 1)
		{
			return totWords[0][0].ToString();  //take first character of the first word from the name
		}
		return $"{totWords[0][0]}{totWords[1][0]}";  //take first character of the first two words from the name
	}
}
