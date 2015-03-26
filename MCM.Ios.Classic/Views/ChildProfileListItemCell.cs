using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class ChildProfileListItemCell : UITableViewCell
	{
		public ChildProfileListItemCell (IntPtr handle) : base (handle)
		{ }

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			this.ImageView.ClipsToBounds = true;
			this.ImageView.Layer.MasksToBounds = true;
			this.ImageView.Layer.CornerRadius = 5;
			this.ImageView.BackgroundColor = MCMExtensions.MCMPurple ();
		}

		public void updateWithImage(UIImage image) {
			this.ImageView.Image = image;
		}
	}
}
