
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

using MCM.Core;

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
        private Button _cameraButton;
        private Button _pickImageButton;
        private DataObjects.Child _child;

        private ProgressDialog _progressDialog;
        private GlobalVars _globalVars;
        private string _srcPath = string.Empty;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _globalVars = ((GlobalVars)this.Application);

            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            SetContentView(Resource.Layout.AddPhoto);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            _cameraButton = FindViewById<Button>(Resource.Id.CameraButton);
            _pickImageButton = FindViewById<Button>(Resource.Id.PickImageButton);
            _pickImageButton.Click += ChooseImageButtonClick;

            _imageView.SetImageBitmap(null);
            if (!string.IsNullOrEmpty(_child.PictureUri))
            {
                Java.IO.File file = new Java.IO.File(this.ApplicationInfo.DataDir, _child.PictureUri);
                if (file.Exists())
                {
                    //string uri = file.CanonicalPath;
                    //_imageView.SetImageURI(Android.Net.Uri.Parse(uri));

                    int height = Resources.DisplayMetrics.HeightPixels;
                    int width = _imageView.Width;
                    Bitmap bm = file.Path.LoadAndResizeBitmap(width, height);

                    _imageView.SetImageBitmap(bm);
                }
            }

            if (IsThereAnAppToTakePictures())
            {
                _cameraButton.Enabled = true;
                CreateDirectoryForPictures();

                //if (CameraCapture.bitmap != null)
                //{
                //    _imageView.SetImageBitmap(CameraCapture.bitmap);
                //    CameraCapture.bitmap = null;
                //}
                _cameraButton.Click += TakeAPicture;
            }
            else
            {
                _cameraButton.Enabled = false;
            }
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
                    if (!string.IsNullOrWhiteSpace(_srcPath))
                    {
                        try
                        {
                            //var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                            string fileName = string.Format("{0}_{1}_{2}{3}", _child.FirstName, _child.Id, Enums.PictureTypes.Picture.ToString(), System.IO.Path.GetExtension(_srcPath) ?? ".jpg");
                            //var filePath = System.IO.Path.Combine(documentsPath, fileName);

                            Utility.SaveFileToInternalStorage(this, new Java.IO.File(_srcPath), fileName);
                            //System.IO.File.Copy(_srcPath, filePath, true);

                            _child.PictureUri = fileName;
                            Intent returnIntent = new Intent();
                            returnIntent.PutExtra("Child", JsonConvert.SerializeObject(_child));
                            this.SetResult(Result.Ok, returnIntent);

                            SaveChildPhoto();
                        }
                        catch (System.Exception ex)
                        {
                            string errMsg = ex.Message;
                        }
                    }
                    else
                    {
                        this.SetResult(Result.Canceled);
                    }

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

        private async void SaveChildPhoto()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Adding Photo");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();

            try
            {
                await _child.Save(this);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                _progressDialog.Dismiss();
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

            //the parameter, data, that is returned from the call to the camera is usually null
            if ((resultCode == Result.Ok))
            {
                Bitmap bm = null;
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _imageView.Width;

                if ((requestCode == (int)ActivityRequests.FromGallery))
                {
                    Android.Net.Uri uri = data.Data;
                    _srcPath = GetPathFromGalleryItem(uri);

                    bm = new File(_srcPath).Path.LoadAndResizeBitmap(width, height);
                }
                else
                {
                    //the camera capture file is deleted before calling the camera.
                    //this checks whether the camera creates the file when a pciture is taken.
                    if (CameraCapture._file.Exists())
                    {
                        // make it available in the gallery
                        Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                        Uri contentUri = Uri.FromFile(CameraCapture._file);
                        mediaScanIntent.SetData(contentUri);
                        SendBroadcast(mediaScanIntent);

                        _srcPath = CameraCapture._file.Path;
                        // Resize the bitmap to fit the display
                        // Loading the full sized image will consume to much memory 
                        // and cause the application to crash.
                        bm = CameraCapture._file.Path.LoadAndResizeBitmap(width, height);
                    }
                }
                if (bm != null)
                {
                    _imageView.SetImageBitmap(bm);
                }
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
            CameraCapture._dir = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures);
            //CameraCapture._dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "MNMissingChildren");
            //if (!CameraCapture._dir.Exists())
            //{
            //    CameraCapture._dir.Mkdirs();
            //}
        }

        private void TakeAPicture(object sender, System.EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            //CameraCapture._file = new File(CameraCapture._dir, "MCM_Camera_Capture.jpg");
            //if (!CameraCapture._file.Exists())
            //{
            //    CameraCapture._file.Delete();
            //}
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

