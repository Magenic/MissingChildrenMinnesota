using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class Picture : EntityData
    {
        public string ChildId { get; set; }
        public string PictureType { get; set; }
        public string PictureUri { get; set; }
        public string PictureUrl { get; set; }
    }
}
