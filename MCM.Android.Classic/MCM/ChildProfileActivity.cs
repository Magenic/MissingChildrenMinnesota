
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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

			SetContentView (Resource.Layout.ChildProfile);

			Button addPhotoButton = FindViewById<Button>(Resource.Id.AddPhotoButton);
			Button childBasicsButton = FindViewById<Button>(Resource.Id.ChildBasicsButton);
			Button measurementsButton = FindViewById<Button>(Resource.Id.MeasurementsButton);
			Button physicalDetailsButton = FindViewById<Button>(Resource.Id.PhysicalDetailsButton);
			Button doctorInfoButton = FindViewById<Button>(Resource.Id.DoctorInfoButton);
			Button dentalInfoButton = FindViewById<Button>(Resource.Id.DentalInfoButton);
			Button medicalAlertInfoButton = FindViewById<Button>(Resource.Id.MedicalAlertInfoButton);
			Button distinguishingFeaturesButton = FindViewById<Button>(Resource.Id.DistinguishingFeaturesButton);
			Button idChecklistButton = FindViewById<Button>(Resource.Id.IDChecklistButton);

			addPhotoButton.Click += HandleAddPhotoButton;
			childBasicsButton.Click += HandleChildBasicsButton;
			measurementsButton.Click += HandleMeasurementsButton;
			physicalDetailsButton.Click += HandlePhysicalDetailsButton;
			doctorInfoButton.Click += HandleDoctorInfoButton;
			dentalInfoButton.Click += HandleDentalInfoButton;
			medicalAlertInfoButton.Click += HandleMedicalAlertInfoButton;
			distinguishingFeaturesButton.Click += HandleDistinguishingFeaturesButton;
			idChecklistButton.Click += HandleIDChecklistButton;

            if (string.IsNullOrWhiteSpace(_child.Id))
            {
                addPhotoButton.Enabled = false;
                measurementsButton.Enabled = false;
                physicalDetailsButton.Enabled = false;
                doctorInfoButton.Enabled = false;
                dentalInfoButton.Enabled = false;
                medicalAlertInfoButton.Enabled = false;
                distinguishingFeaturesButton.Enabled = false;
                idChecklistButton.Enabled = false;
                HandleChildBasicsButton(this, new EventArgs());
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

