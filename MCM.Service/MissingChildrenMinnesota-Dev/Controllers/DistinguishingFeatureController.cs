﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MCM.Service.DataObjects;
using MCM.Service.Models;

namespace MCM_NormanGService.Controllers
{
    public class DistinguishingFeatureController : TableController<DistinguishingFeature>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<DistinguishingFeature>(context, Request, Services);
        }

        // GET tables/DistinguishingFeature
        public IQueryable<DistinguishingFeature> GetAllDistinguishingFeature()
        {
            return Query(); 
        }

        // GET tables/DistinguishingFeature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DistinguishingFeature> GetDistinguishingFeature(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/DistinguishingFeature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DistinguishingFeature> PatchDistinguishingFeature(string id, Delta<DistinguishingFeature> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/DistinguishingFeature
        public async Task<IHttpActionResult> PostDistinguishingFeature(DistinguishingFeature item)
        {
            DistinguishingFeature current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/DistinguishingFeature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDistinguishingFeature(string id)
        {
             return DeleteAsync(id);
        }

    }
}