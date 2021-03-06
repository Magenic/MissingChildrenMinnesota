﻿
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
using Android.Webkit;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/missingchild_layout_label")]			
	public class MissingChildActivity : Activity
	{
        private string _mimeType = "text/html";
        private string _encoding = "utf-8";
        private WebView _htmlWebView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.MissingChild);

            _htmlWebView = (WebView)FindViewById(Resource.Id.MissingChildWebView);

            // Read the contents of our asset
            string content;
            using (StreamReader sr = new StreamReader(Application.Context.Resources.Assets.Open("MissingChild.txt")))
            {
                content = sr.ReadToEnd();
            }

            _htmlWebView.LoadData(content, _mimeType, _encoding);
        }
	}
}

