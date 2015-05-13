
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
        private List<DataObjects.IDCheckListItem> _idCheckListItems;
        private TextView _pageTitleTextView;
        private ListView _idCheckListListView;

        public Boolean[] CheckBoxState;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);
            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.IDChecklist);

            GetCheckListItems();
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
                    CreateAndShowDialog("Save Clicked", "Menu");
                    return true;

                case Resource.Id.menu_cancel_info:
                    CreateAndShowDialog("Cancel Clicked", "Menu");
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void GetCheckListItems()
        {
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Loading");
            progressDialog.SetMessage("Please Wait...");
            progressDialog.Show();

            Task<List<DataObjects.IDCheckListItem>> listItems = GetIDCheckListItems();
            listItems.Wait();

            _idCheckListItems = listItems.Result;
            
            var idCheckListListView = FindViewById<ListView>(Resource.Id.IDCheckListListView);
            idCheckListListView.Adapter = new IDCheckListListViewAdapter(this, _idCheckListItems);
            idCheckListListView.ItemClick += idCheckListListView_ItemClick;

            progressDialog.Dismiss();
        }

        void idCheckListListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedIDCheck = ((IDCheckListListViewAdapter)(((ListView)e.Parent).Adapter))[e.Position] as DataObjects.IDCheckListItem;
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        private Task<List<DataObjects.IDCheckListItem>> GetIDCheckListItems()
        {

            Task<List<DataObjects.IDCheckListItem>> t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.IDCheckListItem>()
                .OrderBy(_ => _.DisplayOrder)
                .ToListAsync());

            return t;
        }

        public Task<DataObjects.IDCheckListItem> GetIDCheckListItem(int idCheckListItemId)
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

