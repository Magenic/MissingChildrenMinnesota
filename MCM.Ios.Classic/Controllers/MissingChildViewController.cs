using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class MissingChildViewController : UIViewController
	{
		public MissingChildViewController (IntPtr handle) : base (handle)
		{ }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "Missing Child?";
			textView.Editable = false;
			var point = new CoreGraphics.CGPoint (0, -200);
			textView.SetContentOffset (point, false);
			this.AutomaticallyAdjustsScrollViewInsets = false;
		}
	}
}
