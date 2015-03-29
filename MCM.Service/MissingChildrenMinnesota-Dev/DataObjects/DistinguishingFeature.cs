using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class DistinguishingFeature : EntityData
    {
        public string ChildId { get; set; }
        public string Feature { get; set; }
        public int BodyChartNbr { get; set; }
    }
}
