// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic.Controllers
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnGoogle { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnHotmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnTwitter { get; set; }

		[Action ("btnGoogle_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnGoogle_TouchUpInside (UIButton sender);

		[Action ("btnHotmail_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnHotmail_TouchUpInside (UIButton sender);

		[Action ("btnTwitter_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnTwitter_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnGoogle != null) {
				btnGoogle.Dispose ();
				btnGoogle = null;
			}
			if (btnHotmail != null) {
				btnHotmail.Dispose ();
				btnHotmail = null;
			}
			if (btnTwitter != null) {
				btnTwitter.Dispose ();
				btnTwitter = null;
			}
		}
	}
}
