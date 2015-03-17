using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MCM.Ios.Classic
{
	public class MCMCollectionViewDelegateFlowLayout : UICollectionViewDelegateFlowLayout
	{
		public override CGSize GetSizeForItem (UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			nfloat width = collectionView.Frame.Size.Width;
			nfloat height = 145.0f;

			switch (indexPath.Section) {
			case 0:				
				return new CGSize (width - 20, height);
			default: 
				var size = new CGSize (width / 2, height);
				size.Width -= 40;
				return size;
			}
		}

		public override UIEdgeInsets GetInsetForSection (UICollectionView collectionView, UICollectionViewLayout layout, nint section) {
			return new UIEdgeInsets (10,20,10,20);
		}
	}
}