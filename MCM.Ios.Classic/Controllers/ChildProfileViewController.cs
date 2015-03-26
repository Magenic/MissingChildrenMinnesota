using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MCM.Ios.Classic
{
	partial class ChildProfileViewController : UITableViewController
	{
		private readonly string _listItemCellId = "itemListCell";
		private readonly string _addPhotoCellId = "addPhotoCell";

		public ChildProfileViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "Child Profile";
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 8;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Row == 0) {
				return tableView.DequeueReusableCell (_addPhotoCellId);
			} else {
				var cell = tableView.DequeueReusableCell (_listItemCellId) as ChildProfileListItemCell;

				switch (indexPath.Row) {
				case 1:
					cell.TextLabel.Text = "Child Basics";
					cell.updateWithImage (new UIImage ("253-person"));
					break;
				case 2:
					cell.TextLabel.Text = "Measurements";
					cell.updateWithImage (new UIImage ("186-ruler"));
					break;
				case 3:
					cell.TextLabel.Text = "Physical Details";
					break;
				case 4:
					cell.TextLabel.Text = "Doctor Info";
					break;
				case 5:
					cell.TextLabel.Text = "Dental Info";
					break;
				case 6:
					cell.TextLabel.Text = "Medical Alert Info";
					break;
				case 7:
					cell.TextLabel.Text = "Distinguishing Features";
					break;
				case 8:
					cell.TextLabel.Text = "I.D. Checklist";
					break;
				default:
					break;
				}	

				return cell;
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Row == 0) {
				return 100.0f;
			} else {
				return 44.0f;
			}
		}
	}
}
