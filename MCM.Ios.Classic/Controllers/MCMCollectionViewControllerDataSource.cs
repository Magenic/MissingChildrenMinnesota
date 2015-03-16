using System;
using Foundation;
using UIKit;

namespace MCM.Ios.Classic
{
	public class MCMCollectionViewControllerDataSource : UICollectionViewSource
	{
		private readonly string _cellId = "mcmCell";

		public MCMCollectionViewControllerDataSource ()
		{	}

		public override nint NumberOfSections(UICollectionView collectionView) {
			return 3;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) {

			if (section == 0) {
				return 1;
			} 

			return 2;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (MCMCollectionViewCell)collectionView.DequeueReusableCell (_cellId, indexPath);

			if (indexPath.Section == 0) {
				cell.CellType = MCMCollectionViewCellType.MyChildren;
			} else if (indexPath.Section == 1) {
				switch (indexPath.Row) {
				case 0:
					cell.CellType = MCMCollectionViewCellType.HomeDNA;
					break;
				case 1:
					cell.CellType = MCMCollectionViewCellType.SafetyForChildren;
					break;
				default:
					break;
				}
			} else {
				switch (indexPath.Row) {
				case 0:
					cell.CellType = MCMCollectionViewCellType.MissingChild;
					break;
				case 1:
					cell.CellType = MCMCollectionViewCellType.AboutMCM;
					break;
				default:
					break;
				}
			}

			return cell;
		}

	}
}

