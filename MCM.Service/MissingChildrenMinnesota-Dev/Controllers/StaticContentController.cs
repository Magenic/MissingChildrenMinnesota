using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MCM.Service.DataObjects;
using MCM.Service.Models;

using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace MCM.Service.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class StaticContentController : TableController<StaticContent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<StaticContent>(context, Request, Services);
        }

        // GET tables/StaticContent
        public IQueryable<StaticContent> GetAllStaticContent()
        {
            return Query(); 
        }

        // GET tables/StaticContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<StaticContent> GetStaticContent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StaticContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<StaticContent> PatchStaticContent(string id, Delta<StaticContent> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/StaticContent
        public async Task<IHttpActionResult> PostStaticContent(StaticContent item)
        {
            StaticContent current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/StaticContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStaticContent(string id)
        {
             return DeleteAsync(id);
        }

    }
}