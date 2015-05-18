using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MCM.Core.Helpers;
using MCM.Forms.Helpers;
using UIKit;

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