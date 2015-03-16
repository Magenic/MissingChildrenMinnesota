using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class MCMCollectionViewCell : UICollectionViewCell
	{
		public MCMCollectionViewCell (IntPtr handle) : base (handle)
		{
		}

		public void updateLabel(string labelText) {
			mcmCellLabel.Text = labelText;
		}

		public void updateImage(UIImage image) {
			mcmCellImageView.Image = image;
		}

		private MCMCollectionViewCellType _cellType;
		public MCMCollectionViewCellType CellType {
			get {
				return _cellType;
			}
			set {
				updateCellBasedOnType (value);
			}
		}

		private void updateCellBasedOnType(MCMCollectionViewCellType cellType) {
			_cellType = cellType;

			switch (cellType) {
			case MCMCollectionViewCellType.MyChildren:
				updateImage (new UIImage ("cheerful_children.jpg"));
				updateLabel ("My Children");
				break;
			case MCMCollectionViewCellType.HomeDNA:
				updateImage (new UIImage ("dna_boy.jpg"));
				updateLabel ("Home DNA");
				break;
			case MCMCollectionViewCellType.SafetyForChildren:
				updateImage (new UIImage ("safety_for_children.jpg"));
				updateLabel ("Safety for Children");
				break;
			case MCMCollectionViewCellType.MissingChild:
				updateImage (new UIImage ("empty-bike.jpg"));
				updateLabel ("Missing Child?");
				break;
			case MCMCollectionViewCellType.AboutMCM:
				updateImage (new UIImage ("about-mcm.jpg"));
				updateLabel ("About MCM");
				break;
			default:
				break;
			}
		}
	}

	public enum MCMCollectionViewCellType
	{
		MyChildren,
		HomeDNA,
		SafetyForChildren,
		MissingChild,
		AboutMCM
	}
}
