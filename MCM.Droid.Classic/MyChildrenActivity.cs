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

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Newtonsoft.Json;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/mychildren_layout_label")]			
	public class MyChildrenActivity : Activity
	{
        private GlobalVars _globalVars;
        private List<DataObjects.Child> _children;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);

            GetChildren();

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.MyChildren);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_mychildren, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add_child:
                    var newChild = new DataObjects.Child();
                    newChild.UserAccount = _globalVars.UserInfo;
                    var activity = new Intent(this, typeof(ChildProfileActivity));
                    activity.PutExtra("Child", JsonConvert.SerializeObject(newChild));
                    StartActivity (activity);
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        private async void GetChildren()
        {
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Loading");
            progressDialog.SetMessage("Please Wait...");
            progressDialog.Show();

            _children = await GetChildrenList();
            var myChildListView = FindViewById<ListView>(Resource.Id.ChildrenListView);
            myChildListView.Adapter = new MyChildrenListViewAdapter(this, _children);
            myChildListView.ItemClick += childClick;

            progressDialog.Dismiss();
            
            CreateAndShowDialog(_children.Count.ToString(), " Children Found");
        }

        void childClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedChild = ((MyChildrenListViewAdapter)(((ListView)e.Parent).Adapter))[e.Position] as DataObjects.Child;
            var activity = new Intent(this, typeof(ChildProfileActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(selectedChild));
            StartActivity(activity);
        }

        private Task<List<DataObjects.Child>> GetChildrenList()
        {
            Task<List<DataObjects.Child>> children = Task.Factory.StartNew(() => _globalVars.MobileServiceClient.GetTable<DataObjects.Child>()
                                                                                                                .Where(_ => _.UserAccount == _globalVars.UserInfo)
                                                                                                                .OrderByDescending(c => c.BirthDate)
                                                                                                                .ToListAsync()
                                                                                                                .Result);
            
            return children;
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

