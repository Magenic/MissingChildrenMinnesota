
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/idchecklist_layout_label")]			
	public class IDChecklistActivity : Activity
	{
        private GlobalVars _globalVars;

        private DataObjects.Child _child;
        private List<DataObjects.ChildCheckListItem> _childIDCheckItems;
        private List<DataObjects.IDCheckListItem> _idCheckListItems;

        private ProgressDialog _progressDialog;
        private TextView _pageTitleTextView;
        private ListView _idCheckListListView;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);
            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.IDChecklist);

            GetItems();

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.IDCheckListTitleTextView);
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_save_cancel, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_save_info:
                    MaintainChildIDCheckList();
                    return true;

                case Resource.Id.menu_cancel_info:
                    CancelChanges();
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void GetItems()
        {
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Loading");
            progressDialog.SetMessage("Please Wait...");
            progressDialog.Show();

            GetChildIDCheckItems();
            GetIDListItems();

            _idCheckListListView = FindViewById<ListView>(Resource.Id.IDCheckListListView);
            _idCheckListListView.Adapter = new IDCheckListListViewAdapter(this, _idCheckListItems, _childIDCheckItems);

            progressDialog.Dismiss();
        }

        private void GetChildIDCheckItems()
        {
            Task<List<DataObjects.ChildCheckListItem>> listItems = GetChildIDCheckListItems();
            listItems.Wait();

            _childIDCheckItems = listItems.Result;
        }

        private void GetIDListItems()
        {
            Task<List<DataObjects.IDCheckListItem>> listItems = GetIDCheckListItems();
            listItems.Wait();

            _idCheckListItems = listItems.Result;
        }

        private void CancelChanges()
        {
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Canceling Changes");
            progressDialog.SetMessage("Please Wait...");
            progressDialog.Show();

            _idCheckListListView = FindViewById<ListView>(Resource.Id.IDCheckListListView);
            _idCheckListListView.Adapter = new IDCheckListListViewAdapter(this, _idCheckListItems, _childIDCheckItems);

            progressDialog.Dismiss();
        }

        private void MaintainChildIDCheckList()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving ID Check List");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();

            try
            {
                //foreach(var checkBoxState in ((IDCheckListListViewAdapter)_idCheckListListView.Adapter).CheckBoxStates)
                Parallel.ForEach(((IDCheckListListViewAdapter)_idCheckListListView.Adapter).CheckBoxStates, checkBoxState =>
                {
                    var childIDCheckItem = _childIDCheckItems.FirstOrDefault<DataObjects.ChildCheckListItem>(_ => _.IDCheckListItemId == checkBoxState.Key);
                    if (checkBoxState.Value)
                    {
                        if (childIDCheckItem == null)
                        {
                            childIDCheckItem = new DataObjects.ChildCheckListItem();
                            childIDCheckItem.ChildId = _child.Id;
                            childIDCheckItem.IDCheckListItemId = checkBoxState.Key;
                            childIDCheckItem.Save(this);
                            _childIDCheckItems.Add(childIDCheckItem);
                        }
                    }
                    else
                    {
                        if (childIDCheckItem != null)
                        {
                            int pos = _childIDCheckItems.IndexOf(childIDCheckItem);
                            childIDCheckItem.Delete(this);
                            _childIDCheckItems.RemoveAt(pos);
                        }
                    }
                });
                //};
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(string.Format("Unable to save ID Check List: {0}", ex.Message), "Save ID Check List");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        private Task<List<DataObjects.ChildCheckListItem>> GetChildIDCheckListItems()
        {

            Task<List<DataObjects.ChildCheckListItem>> t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.ChildCheckListItem>()
                .ToListAsync());

            return t;
        }

        private Task<List<DataObjects.IDCheckListItem>> GetIDCheckListItems()
        {

            Task<List<DataObjects.IDCheckListItem>> t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.IDCheckListItem>()
                .OrderBy(_ => _.DisplayOrder)
                .ToListAsync());

            return t;
        }

        private Task<DataObjects.IDCheckListItem> GetIDCheckListItem(int idCheckListItemId)
        {

            Task<List<DataObjects.IDCheckListItem>> t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.IDCheckListItem>()
                .Where(_ => _.IDCheckListItemId == idCheckListItemId)
                .ToListAsync());
            t.Wait();

            Task<DataObjects.IDCheckListItem> t2 = Task.Run( () => {
                if (t != null && t.Result != null && t.Result.Count > 0)
                {
                    return t.Result[0];
                }
                else
                {
                    return null;
                }}
                );

            return t2;
        }
    }
}

