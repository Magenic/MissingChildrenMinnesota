using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class AboutMCMViewController : UIViewController
	{
		public AboutMCMViewController (IntPtr handle) : base (handle)
		{ }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			textView.Editable = false;
			var point = new CoreGraphics.CGPoint (0, -200);
			textView.SetContentOffset (point, false);
			this.AutomaticallyAdjustsScrollViewInsets = false;
		}
	}
}
