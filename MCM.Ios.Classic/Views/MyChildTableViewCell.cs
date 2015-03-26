using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class MyChildTableViewCell : UITableViewCell
	{
		public MyChildTableViewCell (IntPtr handle) : base (handle)
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			imageView.ClipsToBounds = true;
			imageView.Layer.MasksToBounds = true;
			imageView.Layer.CornerRadius = imageView.Bounds.Size.Width/2;
		}

		public void updateWithChild(MyChild child) {
			nameLabel.Text = child.Name;
			ageLabel.Text = child.Age.ToString() + " years old";
		}

		public void updateImage(UIImage image) {
			this.imageView.Image = image;
		}
	}
}
	