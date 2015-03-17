using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class HomeDNAViewController : UIViewController
	{
		public HomeDNAViewController (IntPtr handle) : base (handle)
		{	}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "Home DNA";
		}
	}
}
