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

namespace MCM.Ios.Classic
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton HotmailLoginButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (HotmailLoginButton != null) {
				HotmailLoginButton.Dispose ();
				HotmailLoginButton = null;
			}
		}
	}
}
