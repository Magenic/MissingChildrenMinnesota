using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;

namespace MCM.Ios.Classic
{
	partial class MyChildrenViewController : UITableViewController
	{
		private readonly IList<MyChild> _children;

		public MyChildrenViewController (IntPtr handle) : base (handle)
		{
			_children = new List<MyChild> ();
			_children.Add (new MyChild (18, "Napolean Dynamite"));
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "My Children";
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {
					
			});
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("childCell");
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 1;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			if (_children.Count > 0) {
				return _children.Count;
			} else {
				var noChildrenLabel = new UILabel (new CoreGraphics.CGRect (0, 0, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height));
				noChildrenLabel.Text = "You haven't added your children yet.";
				noChildrenLabel.TextAlignment = UITextAlignment.Center;
				noChildrenLabel.Lines = 0;
				noChildrenLabel.Font = UIFont.BoldSystemFontOfSize (15);
				noChildrenLabel.SizeToFit ();
				this.TableView.BackgroundView = noChildrenLabel;
				this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

				return 0;
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 100.0f;
		}
	}
}
