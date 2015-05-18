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
    public class ChildCheckListItemController : TableController<ChildCheckListItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ChildCheckListItem>(context, Request, Services);
        }

        // GET tables/ChildCheckListItem
        public IQueryable<ChildCheckListItem> GetAllChildCheckListItem()
        {
            return Query(); 
        }

        // GET tables/ChildCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ChildCheckListItem> GetChildCheckListItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ChildCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ChildCheckListItem> PatchChildCheckListItem(string id, Delta<ChildCheckListItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ChildCheckListItem
        public async Task<IHttpActionResult> PostChildCheckListItem(ChildCheckListItem item)
        {
            ChildCheckListItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ChildCheckListItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteChildCheckListItem(string id)
        {
             return DeleteAsync(id);
        }

    }
}