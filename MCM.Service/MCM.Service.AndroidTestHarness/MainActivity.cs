using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Service.AndroidTestHarness
{
    [Activity(Label = "MCM.Service.AndroidTestHarness", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MobileServiceClient mobileService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mobileService = new MobileServiceClient("https://missingchildrenminnesota-dev.azure-mobile.net/", "Put Key Here");

            // Get our button from the layout resource,
            // and attach an event to it
            Button btnMicrosoft = FindViewById<Button>(Resource.Id.btnMicrosoft);

            btnMicrosoft.Click += btnMicrosoft_Click;

            Button btnGoogle = FindViewById<Button>(Resource.Id.btnGoogle);

            btnGoogle.Click += btnGoogle_Click;

            Button btnTwitter = FindViewById<Button>(Resource.Id.btnTwitter);

            btnTwitter.Click += btnTwitter_Click;
        }

        private async void btnTwitter_Click(object sender, EventArgs e)
        {
            try
            {
                var user = await this.mobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);

                Toast.MakeText(this, user.ToString(), ToastLength.Short);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short);
            }
        }

        private async void btnGoogle_Click(object sender, EventArgs e)
        {
            try
            {
                var user = await this.mobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Google);

                Toast.MakeText(this, user.ToString(), ToastLength.Short);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short);
            }
        }

        private async void btnMicrosoft_Click(object sender, EventArgs e)
        {
            try
            {
                var user = await this.mobileService.LoginAsync(this, MobileServiceAuthenticationProvider.MicrosoftAccount);

                Toast.MakeText(this, user.ToString(), ToastLength.Short);

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short);
            }
        }
    }
}

