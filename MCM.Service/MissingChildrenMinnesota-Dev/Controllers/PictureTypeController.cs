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
    public class PictureTypeController : TableController<PictureType>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<PictureType>(context, Request, Services);
        }

        // GET tables/PictureType
        public IQueryable<PictureType> GetAllPictureType()
        {
            return Query(); 
        }

        // GET tables/PictureType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PictureType> GetPictureType(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PictureType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PictureType> PatchPictureType(string id, Delta<PictureType> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/PictureType
        public async Task<IHttpActionResult> PostPictureType(PictureType item)
        {
            PictureType current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/PictureType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePictureType(string id)
        {
             return DeleteAsync(id);
        }

    }
}