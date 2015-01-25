﻿using System;
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

namespace MCM
{
    [Activity(Label = "@string/main_layout_label", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		const string applicationURL = @"";
		const string applicationKey = @"";

		private MobileServiceUser _user;

		//Mobile Service Client reference
		private MobileServiceClient _client;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Enable the ActionBar
            RequestWindowFeature(WindowFeatures.ActionBar);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			try
			{
				//MobileServiceAuthenticationProvider provder = null;
				// Create the Mobile Service Client instance, using the provided
				// Mobile Service URL and key
				//_client = new MobileServiceClient (applicationURL, applicationKey);

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
				_user = await _client.LoginAsync(this, provider);
				CreateAndShowDialog(string.Format("you are now logged in - {0}", _user.UserId), "Logged in!");
			}
			catch (Exception ex)
			{
				CreateAndShowDialog(ex, "Authentication failed");
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
            NavigateToMCM();
        }

		private void HandleLoginBypass (object sender, EventArgs ea)
		{
			NavigateToMCM ();
		}

		private async void Login(MobileServiceAuthenticationProvider provider)
		{
			await Authenticate(provider);
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


