using System;
using UIKit;
using Foundation;

namespace MCM.Ios.Classic.Controllers
{
	[Register ("MCMCollectionViewController")]
	public class MCMNavController : UINavigationController
	{
		public MCMNavController (IntPtr handle)
			: base(handle)
		{
			this.NavigationBar.Translucent = false;
		}

		public override UIStatusBarStyle PreferredStatusBarStyle() {
			return UIStatusBarStyle.LightContent;
		}
	}
}