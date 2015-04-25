using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Droid.Classic
{
    [Application]
    public class GlobalVars : Application
    {
        private ConfigHelper _configHelper = null;

        public ConfigHelper ConfigurationHelper { get { return _configHelper; } }

        public MobileServiceClient MobileServiceClient { get; set; }

        public string ApplicationURI { get; set; }
        public string ApplicationKey { get; set; }

        public string UserInfo { get; set; }
        public string AuthenticationToken { get; set; }

        public GlobalVars(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }
        public override void OnCreate()
        {
            base.OnCreate();
            _configHelper = new ConfigHelper(GetConfigSettings());
            ApplicationURI = ConfigurationHelper.AppSettings("applicationUri");
            ApplicationKey = ConfigurationHelper.AppSettings("applicationKey");
        }

        private string GetConfigSettings()
        {
            // Read the contents of our asset
            string content;
            using (StreamReader sr = new StreamReader(Application.Context.Resources.Assets.Open("MCMConfig.xml")))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }
    }
}