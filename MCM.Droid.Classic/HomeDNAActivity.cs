﻿
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

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/homedna_layout_label")]			
	public class HomeDNAActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

			SetContentView (Resource.Layout.HomeDNA);
        }
	}
}

