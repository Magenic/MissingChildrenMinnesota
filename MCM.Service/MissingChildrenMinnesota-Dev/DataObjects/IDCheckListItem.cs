using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class IDCheckListItem : EntityData
    {
        public int IDCheckListItemId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int DisplayOrder { get; set; }
    }
}
