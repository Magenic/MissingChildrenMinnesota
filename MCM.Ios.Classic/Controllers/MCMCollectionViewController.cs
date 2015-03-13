using System;
using System.Drawing;
using Foundation;
using UIKit;

namespace MCM.Ios.Classic.Controllers
{
	public partial class MCMCollectionViewController : UICollectionViewController
    {
		private readonly ILoginService _loginService;
		private readonly string _loginSegue = "loginSegue";
		private readonly string _cellId = "myChildrenCell";

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public MCMCollectionViewController(IntPtr handle)
            : base(handle)
        {
			_loginService = new LoginService ();
        }
			
        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			if (_loginService.LoggedIn == false) {
				this.PerformSegue (_loginSegue, this);
			}
            // Perform any additional setup after loading the view, typically from a nib.
        }
			
        #endregion

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }

		#region UICollectionView methods

		public virtual nint NumberOfSections(UICollectionView collectionView) {
			return 2;
		}

		public virtual nint NumberOfItemsInSection(UICollectionView collectionView, nint section) {

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
			} else {
				switch (indexPath.Row) {
				case 0:
					cell.CellType = MCMCollectionViewCellType.HomeDNA;
					break;
				case 1:
					cell.CellType = MCMCollectionViewCellType.SafetyForChildren;
					break;
				case 2:
					cell.CellType = MCMCollectionViewCellType.MissingChild;
					break;
				case 3:
					cell.CellType = MCMCollectionViewCellType.AboutMCM;
					break;
				default:
					break;
				}
			}

			return cell;
		}

		#endregion
    }
}