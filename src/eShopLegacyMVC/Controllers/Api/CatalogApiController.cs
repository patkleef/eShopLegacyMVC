using System.Web.Http;
using eShopLegacyMVC.Models;
using eShopLegacyMVC.Services;

namespace eShopLegacyMVC.Controllers.Api
{
    public class CatalogApiController : ApiController
    {
        private ICatalogService service;

        public CatalogApiController(ICatalogService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("api/catalog/brands")]
        public virtual IHttpActionResult GetBrands()
        {
            return Ok(service.GetCatalogBrands());
        }

        [HttpGet]
        [Route("api/catalog")]
        public virtual IHttpActionResult Get()
        {
            return Ok(service.GetCatalogItemsPaginated(100, 0));
        }

        [HttpPost]
        [Route("api/catalog")]
        public virtual IHttpActionResult Create([FromBody] CatalogItem catalogItem)
        {
            if (ModelState.IsValid)
            {
                service.CreateCatalogItem(catalogItem);
                return Ok(catalogItem);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("api/catalog/{id}")]
        public virtual IHttpActionResult Update([FromBody] CatalogItem catalogItem, int id)
        {
            if (ModelState.IsValid)
            {
                var item = service.FindCatalogItem(id);
                if (item == null)
                {
                    return NotFound();
                }
                service.UpdateCatalogItem(catalogItem);
                return Ok(catalogItem);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("api/catalog/{id}")]
        public virtual IHttpActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var item = service.FindCatalogItem(id);
                if (item == null)
                {
                    return NotFound();
                }

                service.RemoveCatalogItem(item);

                return Ok();
            }
            return BadRequest();
        }
    }
}
