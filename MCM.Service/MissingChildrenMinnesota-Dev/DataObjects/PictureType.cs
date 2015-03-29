using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class PictureType : EntityData
    {
        public string PictureTypeCode { get; set; }
        public string TypeDescription { get; set; }
    }
}
