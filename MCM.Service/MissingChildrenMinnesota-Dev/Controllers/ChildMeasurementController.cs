using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MCM.Service.DataObjects;
using MCM.Service.Models;

namespace MCM_NormanGService.Controllers
{
    public class ChildMeasurementController : TableController<ChildMeasurement>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ChildMeasurement>(context, Request, Services);
        }

        // GET tables/ChildMeasurement
        public IQueryable<ChildMeasurement> GetAllChildMeasurement()
        {
            return Query(); 
        }

        // GET tables/ChildMeasurement/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ChildMeasurement> GetChildMeasurement(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ChildMeasurement/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ChildMeasurement> PatchChildMeasurement(string id, Delta<ChildMeasurement> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ChildMeasurement
        public async Task<IHttpActionResult> PostChildMeasurement(ChildMeasurement item)
        {
            ChildMeasurement current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ChildMeasurement/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteChildMeasurement(string id)
        {
             return DeleteAsync(id);
        }

    }
}