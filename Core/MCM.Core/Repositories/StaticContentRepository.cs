using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MCM.Core.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Core.Repositories
{
    public class StaticContentRepository : IStaticContentRepository
    {
        protected IMobileServiceTable<StaticContent> _dataTable; 

        public StaticContentRepository(MobileServiceClient mobileServiceClient)
        {
            _dataTable = mobileServiceClient.GetTable<StaticContent>(); 
        }

        public DateTime GetStaticContentDate(Enums.ContentTypes contentType, Enums.StaticPageTypes staticPageType)
        {
            //var t1 = Task.Run(() => (from sc in _dataTable
            //                         where sc.ContentTypeCode == "Page" && sc.ContentName == "SafetyForChildren"
            //                         select (sc.__updatedAt)).ToListAsync());

            DateTime updatedDt = DateTime.MinValue;

            //var t = Task.Run(() => 
            var t = _dataTable
                .Where(_ => _.ContentTypeCode == contentType.ToString() && _.ContentName == staticPageType.ToString())
                .Select(_ => _.__updatedAt)
                .ToListAsync();

            if (t != null && t.Result != null && t.Result.Count > 0)
            {
                updatedDt = t.Result[0].GetValueOrDefault().LocalDateTime;
            }

            return updatedDt;
        }

        public string GetStaticContentContent(Enums.ContentTypes contentType, Enums.StaticPageTypes staticPageType)
        {
            string content = string.Empty;

            var t = _dataTable
                .Where(_ => _.ContentTypeCode == contentType.ToString() && _.ContentName == staticPageType.ToString())
                .Select(_ => _.Content)
                .ToListAsync();

            if (t != null && t.Result != null && t.Result.Count > 0)
            {
                content = t.Result[0];
            }

            return content;
        }
    }
}
