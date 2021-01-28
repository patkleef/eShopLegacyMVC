using System.Linq;
using System.Web.Mvc;
using eShopLegacyMVC.Services;
using eShopLegacyMVC.ViewModel;

namespace eShopLegacyMVC.Controllers
{
    public class CatalogStatisticsController : Controller
    {
        private ICatalogService service;

        public CatalogStatisticsController(ICatalogService service)
        {
            this.service = service;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var viewModel = new CatalogStatisticsViewModel
            {
                NumberOfBrands = service.GetCatalogBrands().Count(),
                NumberOfCategories = service.GetCatalogTypes().Count(),
                NumberOfItems = (int)service.GetCatalogItemsPaginated(1000, 0).TotalItems
            };
            return View(viewModel);
        }
    }
}