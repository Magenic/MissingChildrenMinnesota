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
		private readonly string _aboutMCMSegue = "aboutMCMSegue";
		private readonly string _homeDNASegue = "homeDNASegue";
		private readonly MCMCollectionViewDelegate _delegate;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public MCMCollectionViewController(IntPtr handle)
            : base(handle)
        {
			_loginService = new LoginService ();
			this._delegate = new MCMCollectionViewDelegate ();
        }
			
        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			if (_loginService.LoggedIn == false) {
				this.PerformSegue (_loginSegue, this);
			}
            
			this.CollectionView.Source = new MCMCollectionViewControllerDataSource ();
			this.CollectionView.Delegate = _delegate;
			_delegate.ViewController = new WeakReference (this);
			this.CollectionView.ReloadData ();
        }
			
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.Title = "MCM";
		}
			
        #endregion

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
			this.Title = "";
        }

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath) {

			if (indexPath.Section == 0) {

			} else if (indexPath.Section == 1) {
				switch (indexPath.Row) {
				case 0:
					performHomeDNASegue ();
					break;
				case 1:
					break;
				default:
					break;
				}
			} else if (indexPath.Section == 2) {
				switch (indexPath.Row) {
				case 0:
					break;
				case 1:
					performAboutMCMSegue ();
					break;
				default:
					break;
				}
			}
		}

		private void performAboutMCMSegue() {
			this.PerformSegue (_aboutMCMSegue, this);
		}

		private void performHomeDNASegue() {
			this.PerformSegue (_homeDNASegue, this);
		}
    }
}