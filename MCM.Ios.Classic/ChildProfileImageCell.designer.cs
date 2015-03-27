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
	[Register ("ChildProfileImageCell")]
	partial class ChildProfileImageCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView childImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel childNameLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (childImageView != null) {
				childImageView.Dispose ();
				childImageView = null;
			}
			if (childNameLabel != null) {
				childNameLabel.Dispose ();
				childNameLabel = null;
			}
		}
	}
}
