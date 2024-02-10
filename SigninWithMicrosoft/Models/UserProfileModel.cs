namespace SigninWithMicrosoft.Models;

public class UserProfileModel
{
	public string? Id { get; set; }
	public string? DisplayName { get; set; }
	public string? GivenName { get; set; }
	public string? Surname { get; set; }
	public string? UserPrincipalName { get; set; }
	public string? BusinessPhones { get; set; }
	public string? MobilePhone { get; set; }
	public string? JobTitle { get; set; }
	public string? OfficeLocation { get; set; }
	public string? PreferredLanguage { get; set; }
	public string? Mail { get; set; }
	public string? AccessToken { get; set; }
	public DateTimeOffset? AccessTokenExpiresOn { get; set; }
	public byte[]? UserPhotoSource { get; set; }
	public string? UserNameInitial { get; set; }
	public Color? InitialBgColor { get; set; }
	public Color? InitialFgColor { get; set; }
	public ImageSource? ProfilePicture { get; set; }
	public bool IsProfilePicture { get; set; }
	public string? ExceptionMessage { get; set; }
}
