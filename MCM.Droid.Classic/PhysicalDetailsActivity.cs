
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
    [Activity(Label = "@string/physicaldetails_layout_label")]			
	public class PhysicalDetailsActivity : Activity
	{
        private bool _childUpdated = false;

        private DataObjects.Child _child;
        private TextView _pageTitleTextView;

        private EditText _orgHairColorText;
        private EditText _orgEyeColorText;
        private CheckBox _orgGlassesCheckBox;
        private CheckBox _orgContactsCheckBox;
        private EditText _orgSkinToneText;
        private EditText _orgRaceText;

        private EditText _hairColorText;
        private EditText _eyeColorText;
        private CheckBox _glassesCheckBox;
        private CheckBox _contactsCheckBox;
        private EditText _skinToneText;
        private EditText _raceText;
        
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.PhysicalDetails);

            _pageTitleTextView = FindViewById<TextView>(Resource.Id.PhysicalDetailTitleTextView);
            _hairColorText = FindViewById<EditText>(Resource.Id.HairColorText);
            _eyeColorText = FindViewById<EditText>(Resource.Id.EyeColorText);
            _glassesCheckBox = FindViewById<CheckBox>(Resource.Id.GlassesCheckBox);
            _contactsCheckBox = FindViewById<CheckBox>(Resource.Id.ContactsCheckBox);
            _skinToneText = FindViewById<EditText>(Resource.Id.SkinToneText);
            _raceText = FindViewById<EditText>(Resource.Id.RaceText);

            InitializeDisplay();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_save_cancel, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            _childUpdated = false;

            switch (item.ItemId)
            {
                case Resource.Id.menu_save_info:
                    SavePhysicalDetail();
                    return true;

                case Resource.Id.menu_cancel_info:
                    //if (!((string.IsNullOrWhiteSpace(_orgHairColorText.Text) ? string.Empty : _orgHairColorText.Text.Trim()).Equals((string.IsNullOrWhiteSpace(_hairColorText.Text) ? string.Empty : _hairColorText.Text.Trim()), StringComparison.OrdinalIgnoreCase)) ||
                    //    !((string.IsNullOrWhiteSpace(_orgEyeColorText.Text) ? string.Empty : _orgEyeColorText.Text.Trim()).Equals((string.IsNullOrWhiteSpace(_eyeColorText.Text) ? string.Empty : _eyeColorText.Text.Trim()), StringComparison.OrdinalIgnoreCase)) ||
                    //    !_orgGlassesCheckBox.Checked.Equals(_glassesCheckBox.Checked) ||
                    //    !_orgContactsCheckBox.Checked.Equals(_contactsCheckBox.Checked) ||
                    //    !((string.IsNullOrWhiteSpace(_orgSkinToneText.Text) ? string.Empty : _orgSkinToneText.Text.Trim()).Equals((string.IsNullOrWhiteSpace(_skinToneText.Text) ? string.Empty : _skinToneText.Text.Trim()), StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    ///
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

        private async void SavePhysicalDetail()
        {
            var _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Saving Physical Details");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                _child.HairColor = string.IsNullOrWhiteSpace(_hairColorText.Text) ? null : _hairColorText.Text.Trim();
                _child.EyeColor = string.IsNullOrWhiteSpace(_eyeColorText.Text) ? null : _eyeColorText.Text.Trim();
                _child.Glasses = _glassesCheckBox.Checked;
                _child.Contacts = _contactsCheckBox.Checked;
                _child.SkinTone = string.IsNullOrWhiteSpace(_skinToneText.Text) ? null : _skinToneText.Text.Trim();
                _child.RaceEthnicity = string.IsNullOrWhiteSpace(_raceText.Text) ? null : _raceText.Text.Trim();
                await _child.Save(this);
                _childUpdated = true;
            }
            catch
            {
                CreateAndShowDialog("Unable to save Physical Details.", "Save Physical Details");
            }
            finally
            {
                _progressDialog.Dismiss();
            }
        }

        private void InitializeDisplay()
        {
            _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
            _hairColorText.Text = _child.HairColor;
            _eyeColorText.Text = _child.EyeColor;
            _glassesCheckBox.Checked = _child.Glasses;
            _contactsCheckBox.Checked = _child.Contacts;
            _skinToneText.Text = _child.SkinTone;
            _raceText.Text = _child.RaceEthnicity;

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgHairColorText = _hairColorText;
            _orgEyeColorText = _eyeColorText;
            _orgGlassesCheckBox = _glassesCheckBox;
            _orgContactsCheckBox = _contactsCheckBox;
            _orgSkinToneText = _skinToneText;
            _orgRaceText = _raceText;
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

