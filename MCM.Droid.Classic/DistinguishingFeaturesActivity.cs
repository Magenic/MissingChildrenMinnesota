
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
    [Activity(Label = "@string/distinguishingfeatures_layout_label")]			
	public class DistinguishingFeaturesActivity : Activity
	{
        private GlobalVars _globalVars;

        private DataObjects.Child _child;
        private List<DataObjects.DistinguishingFeature> _features;
        private TextView _pageTitleTextView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);
            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            GetFeatures();

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.DistinguishingFeatures);
            _pageTitleTextView = FindViewById<TextView>(Resource.Id.textView1);
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //MenuInflater.Inflate(Resource.Menu.menu_save_cancel, menu);
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

        private async void GetFeatures()
        {
            ProgressDialog progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Loading");
            progressDialog.SetMessage("Please Wait...");
            progressDialog.Show();

            _features = await GetFeaturesList();
            var childFeaturesListView = FindViewById<ListView>(Resource.Id.DistinguishingFeaturesListView);
            childFeaturesListView.Adapter = new DistinguishingFeaturesListViewAdapter(this, _features);
            childFeaturesListView.ItemClick += childFeaturesListView_ItemClick;

            progressDialog.Dismiss();
        }

        void childFeaturesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedFeature = ((DistinguishingFeaturesListViewAdapter)(((ListView)e.Parent).Adapter))[e.Position] as DataObjects.DistinguishingFeature;
            //var activity = new Intent(this, typeof(ChildProfileActivity));
            //activity.PutExtra("Child", JsonConvert.SerializeObject(selectedFeature));
            //StartActivityForResult(activity, 0);
        }

        private Task<List<DataObjects.DistinguishingFeature>> GetFeaturesList()
        {
            System.Diagnostics.Debug.WriteLine("Creating task for feature list.");
            Task<List<DataObjects.DistinguishingFeature>> features = Task.Factory.StartNew(() => _globalVars.MobileServiceClient.GetTable<DataObjects.DistinguishingFeature>()
                                                                                                                .Where(_ => _.ChildId == _child.Id)
                                                                                                                .ToListAsync()
                                                                                                                .Result);
//                                                                                                                .OrderByDescending(c => c.BodyChartNbr)

            return features;
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

