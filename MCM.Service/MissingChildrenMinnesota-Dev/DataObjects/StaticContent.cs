using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MCM.Service.DataObjects
{
    public class StaticContent : EntityData
    {
        public int StaticContentId { get; set; }
        public string ContentTypeCode { get; set; }
        public string ContentName { get; set; }
        public string Content { get; set; }
    }
}
