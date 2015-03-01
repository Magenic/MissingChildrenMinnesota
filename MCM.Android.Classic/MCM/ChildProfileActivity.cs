
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

namespace MCM
{
    [Activity(Label = "@string/childprofile_layout_label")]			
	public class ChildProfileActivity : Activity
	{
        private DataObjects.Child _child;
        private TextView _pageTitleTextView;
        private Button _addPhotoButton ;
        private Button _childBasicsButton;
        private Button _measurementsButton;
        private Button _physicalDetailsButton;
        private Button _doctorInfoButton;
        private Button _dentalInfoButton;
        private Button _medicalAlertInfoButton;
        private Button _distinguishingFeaturesButton;
        private Button _idChecklistButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

			SetContentView (Resource.Layout.ChildProfile);

			_addPhotoButton = FindViewById<Button>(Resource.Id.AddPhotoButton);
			_childBasicsButton = FindViewById<Button>(Resource.Id.ChildBasicsButton);
			_measurementsButton = FindViewById<Button>(Resource.Id.MeasurementsButton);
			_physicalDetailsButton = FindViewById<Button>(Resource.Id.PhysicalDetailsButton);
			_doctorInfoButton = FindViewById<Button>(Resource.Id.DoctorInfoButton);
			_dentalInfoButton = FindViewById<Button>(Resource.Id.DentalInfoButton);
			_medicalAlertInfoButton = FindViewById<Button>(Resource.Id.MedicalAlertInfoButton);
			_distinguishingFeaturesButton = FindViewById<Button>(Resource.Id.DistinguishingFeaturesButton);
			_idChecklistButton = FindViewById<Button>(Resource.Id.IDChecklistButton);
            _pageTitleTextView = FindViewById<TextView>(Resource.Id.textView1);

			_addPhotoButton.Click += HandleAddPhotoButton;
			_childBasicsButton.Click += HandleChildBasicsButton;
			_measurementsButton.Click += HandleMeasurementsButton;
			_physicalDetailsButton.Click += HandlePhysicalDetailsButton;
			_doctorInfoButton.Click += HandleDoctorInfoButton;
			_dentalInfoButton.Click += HandleDentalInfoButton;
			_medicalAlertInfoButton.Click += HandleMedicalAlertInfoButton;
			_distinguishingFeaturesButton.Click += HandleDistinguishingFeaturesButton;
			_idChecklistButton.Click += HandleIDChecklistButton;

            InitializeDisplay();
		}

        private void InitializeDisplay()
        {
            if (string.IsNullOrWhiteSpace(_child.Id))
            {
                _addPhotoButton.Enabled = false;
                _measurementsButton.Enabled = false;
                _physicalDetailsButton.Enabled = false;
                _doctorInfoButton.Enabled = false;
                _dentalInfoButton.Enabled = false;
                _medicalAlertInfoButton.Enabled = false;
                _distinguishingFeaturesButton.Enabled = false;
                _idChecklistButton.Enabled = false;
                HandleChildBasicsButton(this, new EventArgs());
            }
            else
            {
                _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
            }

        }

		private void HandleAddPhotoButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(AddPhotoActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleChildBasicsButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(ChildBasicsActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
			StartActivity (activity);
		}

		private void HandleMeasurementsButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(MeasurementsActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandlePhysicalDetailsButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(PhysicalDetailsActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleDoctorInfoButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(DoctorInfoActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleDentalInfoButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(DentalInfoActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleMedicalAlertInfoButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(MedicalAlertInfoActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleDistinguishingFeaturesButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(DistinguishingFeaturesActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}

		private void HandleIDChecklistButton (object sender, EventArgs ea)
		{
			var activity = new Intent (this, typeof(IDChecklistActivity));
			//activity.PutExtra ("MyData", "Data from Activity1");
			StartActivity (activity);
		}
	}
}

