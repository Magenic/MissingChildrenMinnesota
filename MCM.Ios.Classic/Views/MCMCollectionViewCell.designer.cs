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
	[Register ("MCMCollectionViewCell")]
	partial class MCMCollectionViewCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView mcmCellImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel mcmCellLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (mcmCellImageView != null) {
				mcmCellImageView.Dispose ();
				mcmCellImageView = null;
			}
			if (mcmCellLabel != null) {
				mcmCellLabel.Dispose ();
				mcmCellLabel = null;
			}
		}
	}
}
