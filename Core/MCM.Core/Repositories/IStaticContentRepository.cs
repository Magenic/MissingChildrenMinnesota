using System;
using System.Collections.Generic;
using System.Text;

namespace MCM.Core.Repositories
{
    public interface IStaticContentRepository
    {
        DateTime GetStaticContentDate(Enums.ContentTypes contentType, Enums.StaticPageTypes staticPageType);
        string GetStaticContentContent(Enums.ContentTypes contentType, Enums.StaticPageTypes staticPageType);
    }
}
