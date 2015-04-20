
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
        private DataObjects.ChildMeasurement _measurements;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Measurements);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_save_cancel, menu);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            _measurements = DataObjects.ChildMeasurement.GetChildMeasurement(((GlobalVars)this.Application).MobileServiceClient, _child.Id);

            InitializePickers();
            return true;
        }

        private void InitializePickers()
        {
            var poundPicker = FindViewById<NumberPicker>(Resource.Id.pounds);
            poundPicker.MaxValue = 250;
            poundPicker.MinValue = 0;
            poundPicker.Value = _measurements.Pounds;

            var ouncePicker = FindViewById<NumberPicker>(Resource.Id.ounces);
            ouncePicker.MaxValue = 16;
            ouncePicker.MinValue = 0;
            ouncePicker.Value = _measurements.Ounces;

            var feetPicker = FindViewById<NumberPicker>(Resource.Id.feet);
            feetPicker.MaxValue = 6;
            feetPicker.MinValue = 0;
            feetPicker.Value = _measurements.Feet;

            var inchPicker = FindViewById<NumberPicker>(Resource.Id.inches);
            inchPicker.MaxValue = 11;
            inchPicker.MinValue = 0;
            inchPicker.Value = _measurements.Inches;
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
            try
            {
                var poundPicker = FindViewById<NumberPicker>(Resource.Id.pounds);
                _measurements.Pounds = poundPicker.Value;
                
                var ouncePicker = FindViewById<NumberPicker>(Resource.Id.ounces);
                _measurements.Ounces = ouncePicker.Value;

                var feetPicker = FindViewById<NumberPicker>(Resource.Id.feet);
                _measurements.Feet = feetPicker.Value;

                var inchPicker = FindViewById<NumberPicker>(Resource.Id.inches);
                _measurements.Inches = inchPicker.Value;

                await _measurements.Save(this);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                Finish();
            }
        }

        //private void CreateAndShowDialog(string message, string title)
        //{
        //    AlertDialog.Builder builder = new AlertDialog.Builder(this);

        //    builder.SetMessage(message);
        //    builder.SetTitle(title);
        //    builder.Create().Show();
        //}
    }
}

