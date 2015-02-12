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

namespace MCM
{
    [Application]
    public class GlobalVars : Application
    {
        public string UserInfo { get; set; }
        public string AuthenticationToken { get; set; }

        public GlobalVars(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }
        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}