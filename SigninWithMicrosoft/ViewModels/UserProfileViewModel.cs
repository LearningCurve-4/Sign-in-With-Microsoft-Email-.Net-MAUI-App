namespace SigninWithMicrosoft.ViewModels;

public class UserProfileViewModel : BaseViewModel
{
	static UserProfileModel? userDetail = null;
	public UserProfileModel? UserDetail
	{
		get => userDetail;
		set
		{
			if (userDetail == value) { return; }
			userDetail = value;
			OnPropertyChanged();
		}
	}
}
