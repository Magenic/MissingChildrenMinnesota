using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class ChildCheckListItem : EntityData
    {
        public string ChildId { get; set; }
        public int IDCheckListItemId { get; set; }
    }
}
