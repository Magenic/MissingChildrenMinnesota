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
		private MCMCollectionViewControllerDataSource _dataSource;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public MCMCollectionViewController(IntPtr handle)
            : base(handle)
        {
			_loginService = new LoginService ();
			this.Title = "MCM";
        }
			
        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			if (_loginService.LoggedIn == false) {
				this.PerformSegue (_loginSegue, this);
			}
            
			_dataSource = new MCMCollectionViewControllerDataSource ();
			this.CollectionView.Source = _dataSource;
			var layout = new UICollectionViewFlowLayout ();
			layout.SectionInset = new UIEdgeInsets (10,20,10,20);
			this.CollectionView.SetCollectionViewLayout (layout, true);
			this.CollectionView.Delegate = new MCMCollectionViewDelegateFlowLayout ();
			this.CollectionView.ReloadData ();
        }
			
        #endregion

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }
    }
}