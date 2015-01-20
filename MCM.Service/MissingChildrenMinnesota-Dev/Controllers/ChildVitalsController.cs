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
    public class ChildVitalsController : TableController<ChildVitals>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ChildVitals>(context, Request, Services);
        }

        // GET tables/ChildVitals
        public IQueryable<ChildVitals> GetAllChildVitals()
        {
            return Query(); 
        }

        // GET tables/ChildVitals/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ChildVitals> GetChildVitals(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ChildVitals/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ChildVitals> PatchChildVitals(string id, Delta<ChildVitals> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ChildVitals
        public async Task<IHttpActionResult> PostChildVitals(ChildVitals item)
        {
            ChildVitals current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ChildVitals/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteChildVitals(string id)
        {
             return DeleteAsync(id);
        }
    }
}