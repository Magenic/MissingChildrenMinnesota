﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/childprofile_layout_label")]
    public class ChildProfileActivity : Activity
    {
        private DataObjects.Child _child;
        private TextView _pageTitleTextView;
        private Button _addPhotoButton;
        private Button _childBasicsButton;
        private Button _measurementsButton;
        private Button _physicalDetailsButton;
        private Button _doctorInfoButton;
        private Button _dentalInfoButton;
        private Button _medicalAlertInfoButton;
        private Button _distinguishingFeaturesButton;
        private Button _idChecklistButton;
        private ImageView _imageView;

        enum IntentCodes
        {
            Photo,
            Basics,
            Measurements,
            PhysicalDetails,
            DoctorInfo,
            DentalInfo,
            MedicalAlertInfo,
            DistinguishingFeatures,
            CheckList,
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            SetContentView(Resource.Layout.ChildProfile);

            _imageView = FindViewById<ImageView>(Resource.Id.ChildPhotoImageView);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (data != null &&
                    !string.IsNullOrEmpty(data.GetStringExtra("Child")))
                {
                    _child = JsonConvert.DeserializeObject<DataObjects.Child>(data.GetStringExtra("Child"));
                    InitializeDisplay();
                }
                else
                {
                    _child = new DataObjects.Child();
                    Finish();
                }

                switch (requestCode)
                {
                    case (int)IntentCodes.Measurements:
                        break;

                    case (int)IntentCodes.Basics:
                        break;
                    
                    case (int)IntentCodes.Photo:
                        
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Finish();
            }
        }//onActivityResult

        public override void Finish()
        {
            this.SetResult(Result.Ok);
            base.Finish();
        }

        private void InitializeDisplay()
        {
            if (string.IsNullOrWhiteSpace(_child.Id))
            {
                DisableChildInfoButtons();
                HandleChildBasicsButton(this, new EventArgs());
            }
            else
            {
                _pageTitleTextView.Text = string.Format("{0}'s Profile", _child.FirstName );
                _imageView.SetImageBitmap(null);
                if (!string.IsNullOrEmpty(_child.PictureUri))
                {
                    Java.IO.File file = new Java.IO.File(this.ApplicationInfo.DataDir, _child.PictureUri);
                    if (file.Exists())
                    {
                        int height = Resources.DisplayMetrics.HeightPixels;
                        int width = _imageView.Width;
                        Bitmap bm = file.Path.LoadAndResizeBitmap(width, height);
                        _imageView.SetImageBitmap(bm);
                    }
                }
                EnableChildInfoButtons();
            }

        }

        private void HandleAddPhotoButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(AddPhotoActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.Photo);
        }

        private void HandleChildBasicsButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(ChildBasicsActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.Basics);
        }

        private void HandleMeasurementsButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(MeasurementsActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.Measurements);
        }

        private void HandlePhysicalDetailsButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(PhysicalDetailsActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.PhysicalDetails);
        }

        private void HandleDoctorInfoButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(DoctorInfoActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.DoctorInfo);
        }

        private void HandleDentalInfoButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(DentalInfoActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.DentalInfo);
        }

        private void HandleMedicalAlertInfoButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(MedicalAlertInfoActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.MedicalAlertInfo);
        }

        private void HandleDistinguishingFeaturesButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(DistinguishingFeaturesActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.DistinguishingFeatures);
        }

        private void HandleIDChecklistButton(object sender, EventArgs ea)
        {
            var activity = new Intent(this, typeof(IDChecklistActivity));
            activity.PutExtra("Child", JsonConvert.SerializeObject(_child));
            StartActivityForResult(activity, (int)IntentCodes.CheckList);
        }

        private void DisableChildInfoButtons()
        {
            _addPhotoButton.Enabled = false;
            _measurementsButton.Enabled = false;
            _physicalDetailsButton.Enabled = false;
            _doctorInfoButton.Enabled = false;
            _dentalInfoButton.Enabled = false;
            _medicalAlertInfoButton.Enabled = false;
            _distinguishingFeaturesButton.Enabled = false;
            _idChecklistButton.Enabled = false;
        }

        private void EnableChildInfoButtons()
        {
            _addPhotoButton.Enabled = true;
            _measurementsButton.Enabled = true;
            _physicalDetailsButton.Enabled = true;
            _doctorInfoButton.Enabled = true;
            _dentalInfoButton.Enabled = true;
            _medicalAlertInfoButton.Enabled = true;
            _distinguishingFeaturesButton.Enabled = true;
            _idChecklistButton.Enabled = true;
        }
    }
}

