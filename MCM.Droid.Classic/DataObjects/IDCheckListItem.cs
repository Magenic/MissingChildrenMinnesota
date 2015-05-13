using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;

using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Droid.Classic.DataObjects
{
    public class IDCheckListItem
    {
        private static GlobalVars _globalVars;

        public string Id { get; set; }
        public int IDCheckListItemId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int DisplayOrder { get; set; }

    }
}