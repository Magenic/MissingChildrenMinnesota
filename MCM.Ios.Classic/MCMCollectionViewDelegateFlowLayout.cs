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
				return new CGSize (width, height);
			default: 
				return new CGSize (width / 2, height);
				break;
			}
		}
	}
}