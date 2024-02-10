namespace SigninWithMicrosoft.Services;

public class MsAuthService
{
	static IPublicClientApplication? MsAuthClientApp = null;

	public static void InitializeMsAuthClient()
	{
		if (string.IsNullOrEmpty(MsAuthClient.Id))  //Provide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.
		{
			Shell.Current.DisplayAlert("Error:", "Provide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.", "Ok");
			return;
		}

		MsAuthClientApp = PublicClientApplicationBuilder.Create(MsAuthClient.Id)
							.WithIosKeychainSecurityGroup(MsAuthClient.IosKeychainSecurityGroups)
							//.WithB2CAuthority($"{MsAuthClient.Authority}{MsAuthClient.SigninPolicy}")
							.WithRedirectUri($"{MsAuthClient.DataScheme}://{MsAuthClient.DataHost}")
							.Build();
	}

	public static async Task<UserProfileModel> OnSignin()
	{
		UserProfileModel? MsUser = new();

		if (string.IsNullOrEmpty(MsAuthClient.Id))  //Provide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.
		{
			MsUser.ExceptionMessage = "Error:\nProvide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.";
			return MsUser;
		}

		AuthenticationResult? MsAuthResult = null;
		IEnumerable<IAccount> msAccounts = await MsAuthClientApp!.GetAccountsAsync();
		try
		{
			try
			{
				IAccount firstMsAccount = msAccounts.FirstOrDefault()!;
				MsAuthResult = await MsAuthClientApp
										.AcquireTokenSilent(MsAuthClient.DefaultScopes, firstMsAccount)
										.ExecuteAsync();
			}
			catch (MsalUiRequiredException)
			{
				try
				{
					MsAuthResult = await MsAuthClientApp
											.AcquireTokenInteractive(MsAuthClient.DefaultScopes)
											//.WithPrompt(Prompt.ForceLogin) //to force password screen
											.WithPrompt(Prompt.SelectAccount)
											.WithParentActivityOrWindow(MsAuthClient.ParentWindow)
											.WithUseEmbeddedWebView(true)
											.ExecuteAsync();
				}
				catch (Exception ex)
				{
					MsUser!.ExceptionMessage = ex.Message;
				}
			}

			if (MsAuthResult != null)
			{
				MsUser!.UserPrincipalName = MsAuthResult?.Account?.Username ?? "";
				MsUser!.AccessToken = MsAuthResult?.AccessToken ?? "";
				MsUser!.AccessTokenExpiresOn = MsAuthResult?.ExpiresOn ?? DateTimeOffset.MinValue;

				try
				{
					//Get Ms user profile data
					HttpClient clientProfileData = new();
					HttpRequestMessage message = new(HttpMethod.Get, MsAuthClient.GraphDataUri);
					message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", MsAuthResult?.AccessToken);
					HttpResponseMessage responseProfileData = await clientProfileData.SendAsync(message);

					if (responseProfileData.IsSuccessStatusCode)
					{
						string json = await responseProfileData.Content.ReadAsStringAsync();

						//Deserialize Json
						string jsonToString = json.Replace("{", "").Replace("\"", "").Replace("}", "");
						string[] r = jsonToString.Split(',');
						foreach (string i in r)
						{
							if (i.StartsWith("@odata.") || string.IsNullOrEmpty(i)) { continue; }

							string[] fv = i.Split(':');
							switch (fv[0])
							{
								case "displayName":
									MsUser!.DisplayName = fv[1];
									MsUser.UserNameInitial = GetNameInitial.Function(fv[1]);
									MsUser.InitialBgColor = GetInitialBgColor.Function(fv[1]);
									MsUser.InitialFgColor = GetInitialFgColor.Function(MsUser.InitialBgColor);
									GlobalVariables.SignInUserName = fv[1];
									break;
								case "surname":
									MsUser!.Surname = fv[1];
									break;
								case "givenName":
									MsUser!.GivenName = fv[1];
									break;
								case "id":
									MsUser!.Id = fv[1];
									break;
								case "userPrincipalName":
									MsUser!.UserPrincipalName = fv[1];
									GlobalVariables.SignInUserEmail = fv[1];
									break;
								case "businessPhones":
									MsUser!.BusinessPhones = fv[1];
									break;
								case "jobTitle":
									MsUser!.JobTitle = fv[1];
									break;
								case "mail":
									MsUser!.Mail = fv[1];
									break;
								case "mobilePhone":
									MsUser!.MobilePhone = fv[1];
									break;
								case "officeLocation":
									MsUser!.OfficeLocation = fv[1];
									break;
								case "preferredLanguage":
									MsUser!.PreferredLanguage = fv[1];
									break;
							}
						}

						//Get Ms user profile picture
						HttpClient clientProfilePicture = new();
						clientProfilePicture.DefaultRequestHeaders.Add("Authorization", "Bearer " + MsAuthResult!.AccessToken);
						clientProfilePicture.DefaultRequestHeaders.Add("Accept", "application/json");
						using var responseProfilePicture = await clientProfilePicture.GetAsync(MsAuthClient.GraphPhotoUri);
						if (responseProfilePicture.IsSuccessStatusCode)
						{
							var stream = await responseProfilePicture.Content.ReadAsStreamAsync();
							byte[] bytes = new byte[stream.Length];
							stream.Read(bytes, 0, (int)stream.Length);
							Stream userPhotoStream = new MemoryStream(bytes);

							MemoryStream photoStream = new();
							userPhotoStream.CopyTo(photoStream);
							userPhotoStream.Dispose();
							photoStream.Position = 0;
							MsUser!.UserPhotoSource = photoStream.ToArray();
							MsUser.ProfilePicture = ImageSource.FromStream(() => new MemoryStream(photoStream.ToArray()));
							MsUser.IsProfilePicture = MsUser.UserPhotoSource != null;
						}
					}
					else
					{
						MsUser!.ExceptionMessage = "API call to MsGraph failed.";
					}
				}
				catch (Exception ex)
				{
					MsUser!.ExceptionMessage = ex.Message;
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(MsUser!.ExceptionMessage))
				{
					MsUser.ExceptionMessage = "Acquire token interactive failed.";
				}
			}
		}
		catch (Exception ex)
		{
			MsUser!.ExceptionMessage = ex.Message;
		}
		return MsUser;
	}

	public static async Task<UserProfileModel> OnSignOut()
	{
		UserProfileModel? MsUser = new();

		if (string.IsNullOrEmpty(MsAuthClient.Id))  //Provide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.
		{
			await Shell.Current.DisplayAlert("Error:", "Provide your <client-id> in GlobalVariables.cs > MsAuthClient.Id, that you received when registering the client application in Microsoft Entra ID.", "Ok");
			return MsUser;
		}

		var msAccounts = await MsAuthClientApp!.GetAccountsAsync();
		try
		{
			while (msAccounts.Any())
			{
				await MsAuthClientApp.RemoveAsync(msAccounts.FirstOrDefault());
				msAccounts = await MsAuthClientApp.GetAccountsAsync();
			}
			GlobalVariables.SignInUserEmail = string.Empty;
			GlobalVariables.SignInUserName = string.Empty;
		}
		catch (Exception ex)
		{
			MsUser!.ExceptionMessage = ex.Message;
		}
		return MsUser!;
	}
}
