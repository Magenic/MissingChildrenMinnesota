using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace MCM.Droid.Classic
{
    [Activity(Label = "AddDistinguishingFeatureActivity")]
    public class AddDistinguishingFeatureActivity : Activity
    {
        private bool _featureAddedOrUpdated = false;
        private DataObjects.Child _child;
        private DataObjects.DistinguishingFeature _feature;

        private ProgressDialog _progressDialog;
        private TextView _pageTitleTextView;

        private EditText _orgDistinguishingFeatureText;
        private EditText _orgBodyChartNbrText;

        private EditText _distinguishingFeatureText;
        private EditText _bodyChartNbrText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));
            _feature = JsonConvert.DeserializeObject<DataObjects.DistinguishingFeature>(Intent.GetStringExtra("Feature"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            SetContentView(Resource.Layout.AddDistinguishingFeature);

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.AddDistinguishingFeatureTitleTextView);
            _distinguishingFeatureText = FindViewById<EditText>(Resource.Id.DistinguishingFeatureText);
            _bodyChartNbrText = FindViewById<EditText>(Resource.Id.BodyChartNbrText);

            InitializeDisplay();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_save_cancel_delete, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_save_info:
                    SaveFeature();
                    return true;

                case Resource.Id.menu_cancel_info:
                    //    ///TODO: dialog to ask to discard changes
                    //    ///

                    //    //replace fields with original values
                    //}

                    InitializeDisplay();
                    return true;

                case Resource.Id.menu_delete_info:
                    this.SetResult(Result.Ok, null);
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void Finish()
        {
            if (_featureAddedOrUpdated)
            {
                Intent returnIntent = new Intent();
                returnIntent.PutExtra("Feature", JsonConvert.SerializeObject(_feature));
                this.SetResult(Result.Ok, returnIntent);
            }
            base.Finish();
        }

        private void SaveFeature()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving Doctor Info");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _feature.Feature = string.IsNullOrWhiteSpace(_distinguishingFeatureText.Text) ? null : _distinguishingFeatureText.Text.Trim();
                _feature.BodyChartNbr = string.IsNullOrWhiteSpace(_bodyChartNbrText.Text) ? 0 : int.Parse(_bodyChartNbrText.Text.Trim());
                _feature.Save(this);
                _featureAddedOrUpdated = true;
                SaveOriginalValues();
                InitializeDisplay();
            }
            catch
            {
                CreateAndShowDialog("Unable to save Distinguishing Feature.", "Save Distinguishing Feature");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void DeleteChild()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Removing Feature Information");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {

                _feature.Delete(this);

                Finish();
            }
            catch
            {
                CreateAndShowDialog("Unable to remove Feature.", "Remove Feature");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void InitializeDisplay()
        {
            if (string.IsNullOrWhiteSpace(_feature.Id))
            {
                _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Add ", string.Concat("Add ", _child.FirstName, "'s "));
            }
            else
            {
                _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Add ", _child.FirstName + "'s ");
                _distinguishingFeatureText.Text = _feature.Feature;
                _bodyChartNbrText.Text = _feature.BodyChartNbr.ToString();
            }

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgDistinguishingFeatureText = _distinguishingFeatureText;
            _orgBodyChartNbrText = _bodyChartNbrText;
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