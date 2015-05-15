using Android.Content;
using Android.Runtime;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using MCM.Forms.Helpers;
using System;

namespace MCM.Forms
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{
				Mvx.Error(e.ToString());
			};

			AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
			{
				Mvx.Error(e.ToString());
			};
        }

        protected override IMvxApplication CreateApp()
        {
            return new McmApp();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var presenter = new MvxPagePresenter();
			Mvx.RegisterSingleton<IMvxPageNavigationHost>(presenter);
			return presenter;
		}
    }
}