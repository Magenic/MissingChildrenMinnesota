
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
    [Activity(Label = "@string/measurements_layout_label")]
    public class MeasurementsActivity : Activity
    {
        private DataObjects.Child _child;
        //private DataObjects.ChildMeasurement _measurements;
        private DataObjects.ChildMeasurement _measurement;
        private NumberPicker _poundPicker;
        private NumberPicker _ouncePicker;
        private NumberPicker _feetPicker;
        private NumberPicker _inchPicker;

        private ProgressDialog _progressDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            SetContentView(Resource.Layout.Measurements);

            _poundPicker = FindViewById<NumberPicker>(Resource.Id.pounds);
            _ouncePicker = FindViewById<NumberPicker>(Resource.Id.ounces);
            _feetPicker = FindViewById<NumberPicker>(Resource.Id.feet);
            _inchPicker = FindViewById<NumberPicker>(Resource.Id.inches);

            //_measurements = DataObjects.ChildMeasurement.GetChildMeasurement(((GlobalVars)this.Application).MobileServiceClient, _child.Id);
            _measurement = new DataObjects.ChildMeasurement();
            _measurement.ChildId = _child.Id;

            InitializePickers();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_save_cancel, menu);
            return true;
        }

        private void InitializePickers()
        {
            _poundPicker.MaxValue = 250;
            _poundPicker.MinValue = 0;
            _poundPicker.Value = string.IsNullOrEmpty(_measurement.Id) ? 50 : _measurement.Pounds;

            _ouncePicker.MaxValue = 15;
            _ouncePicker.MinValue = 0;
            _ouncePicker.Value = _measurement.Ounces;

            _feetPicker.MaxValue = 6;
            _feetPicker.MinValue = 0;
            _feetPicker.Value = string.IsNullOrEmpty(_measurement.Id) ? 3 : _measurement.Feet;

            _inchPicker.MaxValue = 11;
            _inchPicker.MinValue = 0;
            _inchPicker.Value = _measurement.Inches;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_save_info:
                    SaveMeasurementsInfo();
                    return true;

                case Resource.Id.menu_cancel_info:
                    CancelMeasurements();
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void CancelMeasurements()
        {
            this.SetResult(Result.Canceled);
            Finish();

        }

        private async void SaveMeasurementsInfo()
        {
            var _progressDialog = new ProgressDialog(this);
            if (string.IsNullOrEmpty(_measurement.Id))
            {
                _progressDialog.SetTitle("Adding Child Information");
            }
            else
            {
                _progressDialog.SetTitle("Updating Child Information");
            }
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _measurement.Pounds = _poundPicker.Value;
                _measurement.Ounces = _ouncePicker.Value;
                _measurement.Feet = _feetPicker.Value;
                _measurement.Inches = _inchPicker.Value;
                _measurement.MeasurementDate = DateTime.Now;
                await _measurement.Save(this);
            }
            catch
            {
                CreateAndShowDialog("Unable to save Child Measurement.", "Save Child Measurement");
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
    }
}

