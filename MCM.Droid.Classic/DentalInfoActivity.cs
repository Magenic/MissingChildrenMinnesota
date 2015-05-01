
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
    [Activity(Label = "@string/dentalinfo_layout_label")]			
	public class DentalInfoActivity : Activity
	{
        private bool _childUpdated = false;

        private DataObjects.Child _child;
        private TextView _pageTitleTextView;

        private EditText _orgDentalNameText;
        private EditText _orgDentalClinicNameText;
        private EditText _orgDentalAddressLine1Text;
        private EditText _orgDentalAddressLine2Text;
        private EditText _orgDentalCityText;
        private EditText _orgDentalStateText;
        private EditText _orgDentalPostalCodeText;
        private EditText _orgDentalPhoneNumberText;

        private EditText _dentalNameText;
        private EditText _dentalClinicNameText;
        private EditText _dentalAddressLine1Text;
        private EditText _dentalAddressLine2Text;
        private EditText _dentalCityText;
        private EditText _dentalStateText;
        private EditText _dentalPostalCodeText;
        private EditText _dentalPhoneNumberText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));
            _childUpdated = false;

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            SetContentView(Resource.Layout.DentalInfo);

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.DentalInfoTitleTextView);
            _dentalNameText = FindViewById<EditText>(Resource.Id.DentalNameText);
            _dentalClinicNameText = FindViewById<EditText>(Resource.Id.DentalClinicNameText);
            _dentalAddressLine1Text = FindViewById<EditText>(Resource.Id.DentalAddressLine1Text);
            _dentalAddressLine2Text = FindViewById<EditText>(Resource.Id.DentalAddressLine2Text);
            _dentalCityText = FindViewById<EditText>(Resource.Id.DentalCityText);
            _dentalStateText = FindViewById<EditText>(Resource.Id.DentalStateText);
            _dentalPostalCodeText = FindViewById<EditText>(Resource.Id.DentalZipText);
            _dentalPhoneNumberText = FindViewById<EditText>(Resource.Id.DentalPhoneNumber);

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
                    SaveDentalInfo();
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

        private async void SaveDentalInfo()
        {
            var _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving Dental Info");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _child.DentistName = string.IsNullOrWhiteSpace(_dentalNameText.Text) ? null : _dentalNameText.Text.Trim();
                _child.DentistClinicName = string.IsNullOrWhiteSpace(_dentalClinicNameText.Text) ? null : _dentalClinicNameText.Text.Trim();
                _child.DentistAddress1 = string.IsNullOrWhiteSpace(_dentalAddressLine1Text.Text) ? null : _dentalAddressLine1Text.Text.Trim();
                _child.DentistAddress2 = string.IsNullOrWhiteSpace(_dentalAddressLine2Text.Text) ? null : _dentalAddressLine2Text.Text.Trim();
                _child.DentistCity = string.IsNullOrWhiteSpace(_dentalCityText.Text) ? null : _dentalCityText.Text.Trim();
                _child.DentistState = string.IsNullOrWhiteSpace(_dentalStateText.Text) ? null : _dentalStateText.Text.Trim();
                _child.DentistPostalCode = string.IsNullOrWhiteSpace(_dentalPostalCodeText.Text) ? null : _dentalPostalCodeText.Text.Trim();
                _child.DentistPhoneNumber = string.IsNullOrWhiteSpace(_dentalPhoneNumberText.Text) ? null : _dentalPhoneNumberText.Text.Trim();
                await _child.Save(this);
                _childUpdated = true;
            }
            catch
            {
                CreateAndShowDialog("Unable to save Dental Info.", "Save Dental Info");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void InitializeDisplay()
        {
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
            _dentalNameText.Text = _child.DentistName;
            _dentalClinicNameText.Text = _child.DentistClinicName;
            _dentalAddressLine1Text.Text = _child.DentistAddress1;
            _dentalAddressLine2Text.Text = _child.DentistAddress2;
            _dentalCityText.Text = _child.DentistCity;
            _dentalStateText.Text = _child.DentistState;
            _dentalPostalCodeText.Text = _child.DentistPostalCode;
            _dentalPhoneNumberText.Text = _child.DentistPhoneNumber;

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgDentalNameText = _dentalNameText;
            _orgDentalClinicNameText = _dentalClinicNameText;
            _orgDentalAddressLine1Text = _dentalAddressLine1Text;
            _orgDentalAddressLine2Text = _dentalAddressLine2Text;
            _orgDentalCityText = _dentalCityText;
            _orgDentalStateText = _dentalStateText;
            _orgDentalPostalCodeText = _dentalPostalCodeText;
            _orgDentalPhoneNumberText = _dentalPhoneNumberText;
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
