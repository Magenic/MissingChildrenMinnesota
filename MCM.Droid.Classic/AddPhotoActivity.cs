
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
using System.Threading.Tasks;

namespace MCM.Droid.Classic
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

        private ProgressDialog _progressDialog;
        private System.Threading.Timer _timer;
        private int _timerCount = 0;
        private GlobalVars _globalVars;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _globalVars = ((GlobalVars)this.Application);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            SetContentView(Resource.Layout.AddPhoto);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += ChooseImageButtonClick;


            Button cameraButton = FindViewById<Button>(Resource.Id.myButton);
            if (!string.IsNullOrEmpty(_child.PictureUri))
            {
                _imageView.SetImageURI(Android.Net.Uri.Parse(_child.PictureUri));
            }
            else if (CameraCapture.bitmap != null)
            {
                _imageView.SetImageBitmap(CameraCapture.bitmap);
                CameraCapture.bitmap = null;
            }
            cameraButton.Click += TakeAPicture;

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
                    Intent returnIntent = new Intent();
                    returnIntent.PutExtra("Child", JsonConvert.SerializeObject(_child));
                    this.SetResult(Result.Ok, returnIntent);

                    Task task = null;
                    _progressDialog = new ProgressDialog(this);
                    _progressDialog.SetTitle("Adding Child Information");
                    _progressDialog.SetMessage("Please Wait...");
                    _progressDialog.Show();

                    var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
                    task = Task.Factory.StartNew(() => childTable.UpdateAsync(_child));

                    //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
                    //to retrieve the returned Id from the mobile service.
                    _timerCount = 0;
                    _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);

                    Finish();
                    return true;

                case Resource.Id.menu_cancel_info:
                    this.SetResult(Result.Canceled);
                    Finish();
                    return true;


                default:
                    Finish();
                    return true;
            }
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


                //_imageView.BuildDrawingCache(true);
                // Bitmap bitmap = _imageView.GetDrawingCache(true);
                //SaveImageToChild(bitmap);


                var srcPath = GetPathFromGalleryItem(uri);

                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = System.IO.Path.Combine(documentsPath, string.Format("{0}_{1}_{2}{3}", _child.FirstName, _child.Id, "P", System.IO.Path.GetExtension(srcPath) ?? ".jpg"));
                _child.PictureUri = filePath;
                System.Diagnostics.Debug.WriteLine(filePath);


                System.IO.File.Copy(srcPath, filePath, true);

                Toast.MakeText(this, srcPath, ToastLength.Long);
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
                //SaveImageToChild(CameraCapture.bitmap);

            }
        }

        private string GetPathFromGalleryItem(Uri uri)
        {
            string path = GetPathToImage(uri);
            if (string.IsNullOrEmpty(path))
            {
                bool isdoc = DocumentsContract.IsDocumentUri(this, uri);
                if (isdoc)
                {
                    if (IsExternalStorageDocument(uri))
                    {

                        //Actually Here i don t know how to handle all possibility.......
                        string docId = DocumentsContract.GetDocumentId(uri);
                        string[] split = docId.Split(':');
                        string type = split[0];

                        if ("primary".Equals(type))
                        {
                            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures) + "/" + split[1];
                        }

                    }
                    else if (IsDownloadsDocument(uri))
                    {

                        string id = DocumentsContract.GetDocumentId(uri);
                        Uri contentUri = ContentUris.WithAppendedId(Uri.Parse("content://downloads/public_downloads"), System.Convert.ToInt64(id));

                        path = GetDataColumn(this, contentUri, null, null);

                    }
                    else if (IsMediaDocument(uri))
                    {

                        string docId = DocumentsContract.GetDocumentId(uri);
                        string[] split = docId.Split(':');

                        string type = split[0];

                        Uri contentUri = null;
                        if ("image".Equals(type))
                        {
                            contentUri = MediaStore.Images.Media.ExternalContentUri;
                        }
                        else if ("video".Equals(type))
                        {
                            contentUri = MediaStore.Video.Media.ExternalContentUri;
                        }
                        else if ("audio".Equals(type))
                        {
                            contentUri = MediaStore.Audio.Media.ExternalContentUri;
                        }

                        string selection = "_id=?";
                        string[] selectionArgs = new string[] {
                            split[1]
                        };

                        path = GetDataColumn(this, contentUri, selection, selectionArgs);

                    }

                }
            }
            return path;
        }

        private string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
        {

            ICursor cursor = null;
            string column = "_data";
            string[] projection = {
                column
            };

            try
            {

                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return null;
        }

        private bool IsExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        private bool IsDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        private bool IsMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }

        private bool IsGooglePhotosUri(Uri uri)
        {
            return "com.google.android.apps.photos.content".Equals(uri.Authority);
        }

        private void SaveImageToChild(Bitmap bitmap)
        {
            var baos = new System.IO.MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, baos); //bm is the bitmap object   
            byte[] b = baos.ToArray();
            string encodedImage = Base64.EncodeToString(b, Base64Flags.Default);
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

        private void TimerDelegate(object state)
        {
            if (!string.IsNullOrWhiteSpace(_child.Id))
            {
                _timer.Dispose();
                _progressDialog.Dismiss();
            }
            else
            {
                //if more than 3 seconds has elapsed, consider error
                if (_timerCount > 12)
                {
                    //final check to see if there is an Id
                    if (string.IsNullOrWhiteSpace(_child.Id))
                    {
                        _timer.Dispose();
                        _progressDialog.Dismiss();
                    }
                }
            }

            _timerCount++;
        }
    }
}

