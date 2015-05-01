using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using MCM.Forms.Helpers;

namespace MCM.Forms
{
    [Activity(
		Label = "Missing Children of Minnesota"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

		public override void InitializationComplete()
		{
			StartActivity(typeof(MvxNavigationActivity));
		}
    }
}