using System;

namespace MCM.DataObjects
{
    public class ChildMeasurement
    {
        public string Id { get; set; }
        public int ChildId { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}