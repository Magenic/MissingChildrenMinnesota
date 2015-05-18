using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MCM.Service.DataObjects;
using MCM.Service.Models;

namespace MCM.Service.Controllers
{
    public class IDCheckListItemController : TableController<IDCheckListItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<IDCheckListItem>(context, Request, Services);
        }

        // GET tables/IDCheckListItem
        public IQueryable<IDCheckListItem> GetAllIDCheckListItem()
        {
            return Query(); 
        }

        // GET tables/IDCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<IDCheckListItem> GetIDCheckListItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/IDCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<IDCheckListItem> PatchIDCheckListItem(string id, Delta<IDCheckListItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/IDCheckListItem
        public async Task<IHttpActionResult> PostIDCheckListItem(IDCheckListItem item)
        {
            IDCheckListItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/IDCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteIDCheckListItem(string id)
        {
             return DeleteAsync(id);
        }

    }
}