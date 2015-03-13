using System;

namespace MCM.Droid.Classic.DataObjects
{
    public class ChildMeasurement
    {
        public string Id { get; set; }
        public string ChildId { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}