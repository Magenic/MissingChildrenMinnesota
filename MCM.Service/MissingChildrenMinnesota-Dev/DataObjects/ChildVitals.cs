using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class ChildVitals : EntityData
    {
        public string ChildId { get; set; }

        public DateTime VitalsDate { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }
    }
}