namespace SigninWithMicrosoft
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			InitializeRouting();
		}

		public void InitializeRouting()
		{
			Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
		}
	}
}
