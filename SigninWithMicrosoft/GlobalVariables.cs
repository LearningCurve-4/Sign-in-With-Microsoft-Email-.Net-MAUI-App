namespace SigninWithMicrosoft;

public class GlobalVariables
{
	public static string pageFolder = "SigninWithMicrosoft.Views.Pages.";
	public const string headerBarFillColor = "HeaderBarFillColor";
	public const string footerBarFillColor = "FooterBarFillColor";
	public static string SignInUserEmail = string.Empty;
	public static string SignInUserName = string.Empty;
}

public class MsAuthClient
{
	public static object? ParentWindow { get; set; }

	public const string Id = "";  //Enter your <client-id> here you received when registering the client application in Microsoft Entra ID
	public const string IosKeychainSecurityGroups = "com.microsoft.adalcache";
	public static readonly string[] DefaultScopes = { "User.Read" }; //constant array
	public const string Authority = null;
	public const string SigninPolicy = null;
	public const string DataHost = "auth";
	public const string DataScheme = "msal" + Id;
	public const string GraphDataUri = "https://graph.microsoft.com/v1.0/me";
	public const string GraphPhotoUri = "https://graph.microsoft.com/beta/me/photo/$value";
}
