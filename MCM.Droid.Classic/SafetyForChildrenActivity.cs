
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/safetyforchildren_layout_label")]			
	public class SafetyForChildrenActivity : Activity
	{
        private GlobalVars _globalVars;

        private string _mimeType = "text/html";
        private string _encoding = "utf-8";
        private WebView _htmlWebView;
        private ProgressDialog _progressDialog;
        private string _pageContent = string.Empty;
        private bool _saveContentToFile = false;
        
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.SafetyForChildren);

            _htmlWebView = (WebView)FindViewById(Resource.Id.SafetyForChildrenWebView);

            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Retreiving page content");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();

            GetPageContent();

        }

        private async void GetPageContent()
        {
            await DisplayRetrievedContent();
            
            _htmlWebView.LoadData(_pageContent, _mimeType, _encoding);

            if (_saveContentToFile)
            {
                SaveContentToInternalStorage(_pageContent);
            }

            _progressDialog.Dismiss();
        }

        private Task DisplayRetrievedContent()
        {
            DateTime fileDt = DateTime.MinValue;
            Java.IO.File file = this.GetFileStreamPath("SafetyForChildren.txt");
            if (file.Exists())
            {
                Java.Text.SimpleDateFormat sdf = new Java.Text.SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
                fileDt = DateTime.Parse(sdf.Format(file.LastModified()));
            }

            DateTime databaseDt = GetStaticContentDate();

            Task t = Task.Run(() => 
                {
                    DetermineStaticContentSource(databaseDt, fileDt);
                });

            t.Wait();

            return t;
        }

        private void DetermineStaticContentSource(DateTime databaseDate, DateTime fileDate)
        {
            try
            {
                if (fileDate == DateTime.MinValue || databaseDate > fileDate)
                {
                    //Read the contents of file from database
                    _pageContent = GetStaticContentContent();
                    _saveContentToFile = true;
                }
                else
                {
                    //Read the contents of file from internal storage
                    _pageContent = GetContentFromInternalStorage();
                }
            }
            catch (Exception ex)
            {
                _pageContent = string.Format("Unable to retrieve content: {0}", ex.Message);
            }
        }

        private DateTime GetStaticContentDate()
        {
            //var t = Task.Run(() => (from sc in _globalVars.MobileServiceClient.GetTable<DataObjects.StaticContent>()
            //                        where sc.ContentTypeCode == "Page" && sc.ContentName == "SafetyForChildren"
            //                        select (sc.UpdatedAt)).ToListAsync());
            DateTime updatedDt = DateTime.MinValue;

            var t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.StaticContent>()
                .Where(_ => _.ContentTypeCode == "Page" && _.ContentName == "SafetyForChildren")
                .Select(_ => _.__updatedAt)
                .ToListAsync());

            t.Wait();

            if (t != null && t.Result != null && t.Result.Count > 0)
            {
                updatedDt = t.Result[0].GetValueOrDefault().DateTime;
            }
            return updatedDt;
        }

        private string GetStaticContentContent()
        {
            string content = string.Empty;

            var t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.StaticContent>()
                .Where(_ => _.ContentTypeCode == "Page" && _.ContentName == "SafetyForChildren")
                .Select(_ => _.Content)
                .ToListAsync()
                );

            t.Wait();

            if (t != null && t.Result != null && t.Result.Count > 0)
            {
                content = t.Result[0];
            }

            return content;
        }

        private async void SaveContentToInternalStorage(string content)
        {
            using (var output = new StreamWriter(OpenFileOutput("SafetyForChildren.txt", FileCreationMode.Private)))
            {
                await output.WriteAsync(content).ConfigureAwait(false);
            }
        }

        private string GetContentFromInternalStorage()
        {
            string content = string.Empty;

            //Read the contents of file from internal storage
            using (StreamReader sr = new StreamReader(this.ApplicationContext.OpenFileInput("SafetyForChildren.txt")))
            {
                content = sr.ReadToEnd();
            }

            ////Read the contents of file under assets folder
            //using (StreamReader sr = new StreamReader(Application.Context.Resources.Assets.Open("SafetyForChildren.txt")))
            //{
            //    content = sr.ReadToEnd();
            //}

            return content;
        }
    }
}

