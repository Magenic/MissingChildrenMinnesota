using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class myChildrenCell : UICollectionViewCell
	{
		public myChildrenCell (IntPtr handle) : base (handle)
		{
		}

		public void updateLabel(string labelText) {
			mcmCellLabel.Text = labelText;
		}

		public void updateImage(UIImage image) {
			mcmCellImageView.Image = image;
		}
	}
}
