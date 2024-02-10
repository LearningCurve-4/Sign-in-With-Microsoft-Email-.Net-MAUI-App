using Android.App;
using Android.Content;

namespace SigninWithMicrosoft.Platforms.Android;

[Activity(Exported = true)]
[IntentFilter([Intent.ActionView],
						 Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault],
						 DataHost = MsAuthClient.DataHost,
						 DataScheme = MsAuthClient.DataScheme)]

public class MsalActivity : BrowserTabActivity
{
}
