using System;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/main_layout_label", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        //Mobile Service Client reference
        private MobileServiceUser _user;
        private GlobalVars _globalVars;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _globalVars = ((GlobalVars)this.Application);
            _globalVars.ApplicationURL = @"";
            _globalVars.ApplicationKey = @"";

            // Enable the ActionBar
            RequestWindowFeature(WindowFeatures.ActionBar);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			try
			{
				//MobileServiceAuthenticationProvider provder = null;
				// Create the Mobile Service Client instance, using the provided
				// Mobile Service URL and key
                _globalVars.MobileServiceClient = new MobileServiceClient(_globalVars.ApplicationURL, _globalVars.ApplicationKey);

				// Get our button from the layout resource,
				// and attach an event to it
				Button hotmailLoginButton = FindViewById<Button>(Resource.Id.HotmailLoginButton);
				Button gmailLoginButton = FindViewById<Button>(Resource.Id.GmailLoginButton);
				Button twitterLoginButton = FindViewById<Button>(Resource.Id.TwitterLoginButton);
				Button bypassLoginButton = FindViewById<Button>(Resource.Id.BypassLoginButton);

				hotmailLoginButton.Click += HandleLoginMicrosoft;
				gmailLoginButton.Click += HandleLoginGmail;
				twitterLoginButton.Click += HandleLoginTwitter;
				bypassLoginButton.Click += HandleLoginBypass;

			}
			catch (Java.Net.MalformedURLException) 
			{
				CreateAndShowDialog (new Exception ("There was an error creating the Mobile Service. Verify the URL"), "Error");
			} 
			catch (Exception e) 
			{
				CreateAndShowDialog (e, "Error");
			}
		}

		private async Task Authenticate(MobileServiceAuthenticationProvider provider)
		{
			try
			{
                _user = await _globalVars.MobileServiceClient.LoginAsync(this, provider);
                _globalVars.UserInfo = _user.UserId;
                _globalVars.AuthenticationToken = _user.MobileServiceAuthenticationToken;
			}
			catch (Exception ex)
			{
				CreateAndShowDialog(ex, "Authentication Error");
			}
		}

		private void HandleLoginMicrosoft (object sender, EventArgs ea)
		{
			Login(MobileServiceAuthenticationProvider.MicrosoftAccount);
		}

		private void HandleLoginGmail (object sender, EventArgs ea)
		{
			Login(MobileServiceAuthenticationProvider.Google);
		}

		private void HandleLoginTwitter (object sender, EventArgs ea)
		{
			Login(MobileServiceAuthenticationProvider.Twitter);
        }

		private void HandleLoginBypass (object sender, EventArgs ea)
		{
            //this is just for testing
            //_globalVars.UserInfo = Guid.NewGuid().ToString();
            //sample data in database has UserAccount = '12345'
            _globalVars.UserInfo = "12345";
            _globalVars.AuthenticationToken = AndroidEnvironment.AndroidLogAppName;

            NavigateToMCM();
		}

		private async void Login(MobileServiceAuthenticationProvider provider)
		{
			await Authenticate(provider);
            if (_user != null && !string.IsNullOrWhiteSpace(_user.UserId))
            {
                CreateAndShowDialog(string.Format("you are now logged in - {0}", _user.UserId), "Logged in!");
                NavigateToMCM();
            }
            else
            {
                CreateAndShowDialog("Unable to Authenticate User", "Not Authenticated");
            }
		}

		private void NavigateToMCM()
		{
			SetContentView (Resource.Layout.MCM);

			Button myChildrenButton = FindViewById<Button>(Resource.Id.MyChildrenButton);
			Button homeDNAButton = FindViewById<Button>(Resource.Id.HomeDNAButton);
			Button safetyForChildrenButton = FindViewById<Button>(Resource.Id.SafetyForChildrenButton);
			Button missingChildButton = FindViewById<Button>(Resource.Id.MissingChildButton);
			Button aboutMCMButton = FindViewById<Button>(Resource.Id.AboutMCMButton);

			myChildrenButton.Click += HandleMyChildrenButton;
			homeDNAButton.Click += HandleHomeDNAButton;
			safetyForChildrenButton.Click += HandleSafetyForChildrenButton;
			missingChildButton.Click += HandleMissingChildButton;
			aboutMCMButton.Click += HandleAboutMCMButton;
		}

		private void HandleMyChildrenButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(MyChildrenActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleHomeDNAButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(HomeDNAActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleSafetyForChildrenButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(SafetyForChildrenActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleMissingChildButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(MissingChildActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleAboutMCMButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(AboutMCMActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void CreateAndShowDialog (Exception exception, String title)
		{
			CreateAndShowDialog (exception.Message, title);
		}

		private void CreateAndShowDialog (string message, string title)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);

			builder.SetMessage (message);
			builder.SetTitle (title);
			builder.Create ().Show ();
		}
	}
}

