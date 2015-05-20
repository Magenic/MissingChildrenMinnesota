
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
        
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.SafetyForChildren);

            _htmlWebView = (WebView)FindViewById(Resource.Id.SafetyForChildrenWebView);

            string content = string.Empty;

            DateTime updatedAt = GetStaticContentDate();
            content = GetStaticContentContent();

            //using (DataObjects.StaticContent staticContent = GetStaticContentItem())
            //{
            //};
            
            ////Read the contents of our asset
            //using (StreamReader sr = new StreamReader(Application.Context.Resources.Assets.Open("SafetyForChildren.txt")))
            //{
            //    content = sr.ReadToEnd();
            //}

            _htmlWebView.LoadData(content, _mimeType, _encoding);
        }

        private DateTime GetStaticContentDate()
        {
            DateTime updatedAt = DateTime.MinValue;

            //var t = Task.Run(() => (from sc in _globalVars.MobileServiceClient.GetTable<DataObjects.StaticContent>()
            //                        where sc.ContentTypeCode == "Page" && sc.ContentName == "SafetyForChildren"
            //                        select (sc.UpdatedAt)).ToListAsync());

            var t = Task.Run(() => _globalVars.MobileServiceClient.GetTable<DataObjects.StaticContent>()
                .Where(_ => _.ContentTypeCode == "Page" && _.ContentName == "SafetyForChildren")
                .Select(_ => _.__updatedAt)
                .ToListAsync()
                );

            t.Wait();

            if (t != null && t.Result != null && t.Result.Count > 0)
            {
                if (t.Result[0].HasValue)
                {
                    updatedAt = t.Result[0].GetValueOrDefault().DateTime;
                }                
            }

            return updatedAt;
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
    }
}

