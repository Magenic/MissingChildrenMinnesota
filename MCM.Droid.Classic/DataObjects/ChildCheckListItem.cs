using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;

using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Droid.Classic.DataObjects
{
    public class ChildCheckListItem
    {
        public string Id { get; set; }
        public string ChildId { get; set; }
        public int IDCheckListItemId { get; set; }
    }
}