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
    }
}