
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
    [Activity(Label = "@string/medicalalertinfo_layout_label")]			
	public class MedicalAlertInfoActivity : Activity
	{
        private bool _childUpdated = false;

        private DataObjects.Child _child;
        private TextView _pageTitleTextView;

        private EditText _orgMedicalAlertText;
        private EditText _medicalAlertText;
        
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));
            _childUpdated = false;

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.MedicalAlertInfo);

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.MedicalAlertInfoTextView);
            _medicalAlertText = FindViewById<EditText>(Resource.Id.MedicalAlertText);

            InitializeDisplay();
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
                    SaveMedicalAlertInfo();
                    return true;

                case Resource.Id.menu_cancel_info:
                    //    ///TODO: dialog to ask to discard changes
                    //    ///

                    //    //replace fields with original values
                    //}

                    InitializeDisplay();
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void Finish()
        {
            if (_childUpdated)
            {
                Intent returnIntent = new Intent();
                returnIntent.PutExtra("Child", JsonConvert.SerializeObject(_child));
                this.SetResult(Result.Ok, returnIntent);
            }
            base.Finish();
        }

        private async void SaveMedicalAlertInfo()
        {
            var _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving Medical Alert Info");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _child.MedicalAlertInfo = string.IsNullOrWhiteSpace(_medicalAlertText.Text) ? null : _medicalAlertText.Text.Trim();
                await _child.Save(this);
                _childUpdated = true;
            }
            catch
            {
                CreateAndShowDialog("Unable to save Medical Alert Info.", "Save Medical Alert Info");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void InitializeDisplay()
        {
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
            _medicalAlertText.Text = _child.MedicalAlertInfo;

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgMedicalAlertText = _medicalAlertText;
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

