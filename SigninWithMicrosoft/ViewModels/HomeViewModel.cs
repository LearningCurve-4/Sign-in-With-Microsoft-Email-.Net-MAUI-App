namespace SigninWithMicrosoft.ViewModels;

public class HomeViewModel : BaseViewModel
{
	public HomeViewModel()
	{
		MsAuthService.InitializeMsAuthClient();
	}

	bool isSignin = false;
	public bool IsSignin
	{
		get => isSignin;
		set
		{
			if (isSignin != value)
			{
				isSignin = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsSignout));
			}
		}
	}
	public bool IsSignout => !IsSignin;

	public UserProfileModel? MsUser { get; set; } = null;

	public ICommand OnSigninCommand => new Command<string>(async (str) =>
	{
		IsBusy = true;
		MsUser = await MsAuthService.OnSignin();
		if (MsUser.ExceptionMessage == null) { IsSignin = true; }
		else { await Shell.Current.DisplayAlert("Error:", MsUser.ExceptionMessage, "OK"); }
		OnPropertyChanged(nameof(MsUser));
		IsBusy = false;
	}, (str) => IsNotBusy);

	public ICommand OnSignOutCommand => new Command<string?>(async (str) =>
	{
		IsBusy = true;
		IsSignin = false;
		MsUser = await MsAuthService.OnSignOut(); MsUser = null;
		OnPropertyChanged(nameof(MsUser));
		IsBusy = false;
	}, (str) => IsNotBusy);

	public Command UserProfileCommand => new Command<string>((str) =>
	{
		IsBusy = true;
		_ = new UserProfileViewModel { UserDetail = MsUser };
		GoToPageCommand.Execute(str);
		IsBusy = false;
	}, (str) => IsNotBusy);
}
