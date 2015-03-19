using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class MyChildrenViewController : UITableViewController
	{
		public MyChildrenViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "My Children";

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {
					
			});
		}
	}
}
