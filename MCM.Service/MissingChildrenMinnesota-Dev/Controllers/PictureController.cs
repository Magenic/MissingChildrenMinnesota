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
    public class PictureController : TableController<Picture>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Picture>(context, Request, Services);
        }

        // GET tables/Picture
        public IQueryable<Picture> GetAllPicture()
        {
            return Query(); 
        }

        // GET tables/Picture/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Picture> GetPicture(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Picture/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Picture> PatchPicture(string id, Delta<Picture> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Picture
        public async Task<IHttpActionResult> PostPicture(Picture item)
        {
            Picture current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Picture/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePicture(string id)
        {
             return DeleteAsync(id);
        }

    }
}