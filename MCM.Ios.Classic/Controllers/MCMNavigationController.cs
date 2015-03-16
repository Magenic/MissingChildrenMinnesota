using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class MCMNavigationController : UINavigationController
	{
		public MCMNavigationController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.NavigationBar.Translucent = false;
		}

		public override UIStatusBarStyle PreferredStatusBarStyle() {
			return UIStatusBarStyle.LightContent;
		}
	}
}
