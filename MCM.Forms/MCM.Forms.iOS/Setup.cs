using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MCM.Forms.Helpers;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.CrossCore.IoC;

namespace MCM.Forms
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override void InitializeIoC()
		{
			base.InitializeIoC();

			Mvx.RegisterSingleton<IUiContext>(new UiContext());
		}

		protected override IMvxApplication CreateApp()
		{
			return new McmApp();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override IMvxTouchViewPresenter CreatePresenter()
		{
			return new MvxPagePresenter(Window, Mvx.Resolve<IUiContext>());
		}
	}
}