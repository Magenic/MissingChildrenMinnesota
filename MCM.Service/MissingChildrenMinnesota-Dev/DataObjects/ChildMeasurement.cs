using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class ChildMeasurement : EntityData
    {
        public string ChildId { get; set; }
        public int Feet { get; set; }
        public int Inches { get; set; }
        public int Pounds { get; set; }
        public int Ounces { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}
