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
	}
}
