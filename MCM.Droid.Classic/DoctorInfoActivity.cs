
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
    [Activity(Label = "@string/doctorinfo_layout_label")]			
	public class DoctorInfoActivity : Activity
	{
        private bool _childUpdated = false;

        private DataObjects.Child _child;
        private TextView _pageTitleTextView;

        private EditText _orgDoctorNameText;
        private EditText _orgDoctorClinicNameText;
        private EditText _orgDoctorAddressLine1Text;
        private EditText _orgDoctorAddressLine2Text;
        private EditText _orgDoctorCityText;
        private EditText _orgDoctorStateText;
        private EditText _orgDoctorPostalCodeText;
        private EditText _orgDoctorPhoneNumberText;

        private EditText _doctorNameText;
        private EditText _doctorClinicNameText;
        private EditText _doctorAddressLine1Text;
        private EditText _doctorAddressLine2Text;
        private EditText _doctorCityText;
        private EditText _doctorStateText;
        private EditText _doctorPostalCodeText;
        private EditText _doctorPhoneNumberText;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.DoctorInfo);

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.DoctorInfoTitleTextView);
            _doctorNameText = FindViewById<EditText>(Resource.Id.DoctorNameText);
            _doctorClinicNameText = FindViewById<EditText>(Resource.Id.DoctorClinicNameText);
            _doctorAddressLine1Text = FindViewById<EditText>(Resource.Id.DoctorAddressLine1Text);
            _doctorAddressLine2Text = FindViewById<EditText>(Resource.Id.DoctorAddressLine2Text);
            _doctorCityText = FindViewById<EditText>(Resource.Id.DoctorCityText);
            _doctorStateText = FindViewById<EditText>(Resource.Id.DoctorStateText);
            _doctorPostalCodeText = FindViewById<EditText>(Resource.Id.DoctorZipText);
            _doctorPhoneNumberText = FindViewById<EditText>(Resource.Id.DoctorPhoneNumber);

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
                    SaveDoctorInfo();
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

        private async void SaveDoctorInfo()
        {
            var _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving Doctor Info");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _child.DoctorName = string.IsNullOrWhiteSpace(_doctorNameText.Text) ? null : _doctorNameText.Text.Trim();
                _child.DoctorClinicName = string.IsNullOrWhiteSpace(_doctorClinicNameText.Text) ? null : _doctorClinicNameText.Text.Trim();
                _child.DoctorAddress1 = string.IsNullOrWhiteSpace(_doctorAddressLine1Text.Text) ? null : _doctorAddressLine1Text.Text.Trim();
                _child.DoctorAddress2 = string.IsNullOrWhiteSpace(_doctorAddressLine2Text.Text) ? null : _doctorAddressLine2Text.Text.Trim();
                _child.DoctorCity = string.IsNullOrWhiteSpace(_doctorCityText.Text) ? null : _doctorCityText.Text.Trim();
                _child.DoctorState = string.IsNullOrWhiteSpace(_doctorStateText.Text) ? null : _doctorStateText.Text.Trim();
                _child.DoctorPostalCode = string.IsNullOrWhiteSpace(_doctorPostalCodeText.Text) ? null : _doctorPostalCodeText.Text.Trim();
                _child.DoctorPhoneNumber = string.IsNullOrWhiteSpace(_doctorPhoneNumberText.Text) ? null : _doctorPhoneNumberText.Text.Trim();
                await _child.Save(this);
                _childUpdated = true;
            }
            catch
            {
                CreateAndShowDialog("Unable to save Doctor Info.", "Save Doctor Info");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void InitializeDisplay()
        {
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
            _doctorNameText.Text = _child.DoctorName;
            _doctorClinicNameText.Text = _child.DoctorClinicName;
            _doctorAddressLine1Text.Text = _child.DoctorAddress1;
            _doctorAddressLine2Text.Text = _child.DoctorAddress2;
            _doctorCityText.Text = _child.DoctorCity;
            _doctorStateText.Text = _child.DoctorState;
            _doctorPostalCodeText.Text = _child.DoctorPostalCode;
            _doctorPhoneNumberText.Text = _child.DoctorPhoneNumber;

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgDoctorNameText = _doctorNameText;
            _orgDoctorClinicNameText = _doctorClinicNameText;
            _orgDoctorAddressLine1Text = _doctorAddressLine1Text;
            _orgDoctorAddressLine2Text = _doctorAddressLine2Text;
            _orgDoctorCityText = _doctorCityText;
            _orgDoctorStateText = _doctorStateText;
            _orgDoctorPostalCodeText = _doctorPostalCodeText;
            _orgDoctorPhoneNumberText = _doctorPhoneNumberText;
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

