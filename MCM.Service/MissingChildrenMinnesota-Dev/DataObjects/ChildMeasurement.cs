using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class ChildMeasurement : EntityData
    {
        public string ChildId { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}
