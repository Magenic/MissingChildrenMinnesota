
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net;
using Android.Database;
using Java.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;
using Android.Util;
using Android.Graphics.Drawables;
using Newtonsoft.Json;

namespace MCM
{
    [Activity(Label = "@string/addphoto_layout_label")]			
	public class AddPhotoActivity : Activity
	{
        enum ActivityRequests
        {
            FromGallery,
            FromCamera
        };

        private ImageView _imageView;
        private DataObjects.Child _child;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            SetContentView(Resource.Layout.AddPhoto);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += ChooseImageButtonClick;


            Button cameraButton = FindViewById<Button>(Resource.Id.myButton);
            if (CameraCapture.bitmap != null)
            {
                _imageView.SetImageBitmap(CameraCapture.bitmap);
                CameraCapture.bitmap = null;
            }
            cameraButton.Click += TakeAPicture;
			
		}

        private void ChooseImageButtonClick(object sender, System.EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), (int)ActivityRequests.FromGallery);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == (int)ActivityRequests.FromGallery) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                _imageView.SetImageURI(uri);

                //Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.GetContentResolver(), uri);

                _imageView.BuildDrawingCache(true);
                Bitmap bitmap = _imageView.GetDrawingCache(true);

                SaveImageToChild(bitmap);

                string path = GetPathToImage(uri);
                Toast.MakeText(this, path, ToastLength.Long);
            }
            else
            {
                // make it available in the gallery
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(CameraCapture._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // display in ImageView. We will resize the bitmap to fit the display
                // Loading the full sized image will consume to much memory 
                // and cause the application to crash.
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _imageView.Width;
                CameraCapture.bitmap = CameraCapture._file.Path.LoadAndResizeBitmap(width, height);

                _imageView.SetImageBitmap(CameraCapture.bitmap);
                SaveImageToChild(CameraCapture.bitmap);

            }
        }

        private void SaveImageToChild(Bitmap bitmap)
        {
            var baos = new System.IO.MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, baos); //bm is the bitmap object   
            byte[] b = baos.ToArray();
            string encodedImage = Base64.EncodeToString(b, Base64.Default);
            _child.Picture = encodedImage;
        }

        private string GetPathToImage(Uri uri)
        {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
            using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateDirectoryForPictures()
        {
            CameraCapture._dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "MNMissingChildren");
            if (!CameraCapture._dir.Exists())
            {
                CameraCapture._dir.Mkdirs();
            }
        }

        private void TakeAPicture(object sender, System.EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            CameraCapture._file = new File(CameraCapture._dir, string.Format("MCM_{0}.jpg", System.Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(CameraCapture._file));

            StartActivityForResult(intent, (int)ActivityRequests.FromCamera);
        }

        public static class CameraCapture
        {
            public static File _file;
            public static File _dir;
            public static Bitmap bitmap;
        }
	}
}

