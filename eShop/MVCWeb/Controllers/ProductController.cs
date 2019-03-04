using System.Web.Mvc;
using MVCWeb.Cores;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;
using MVCWeb.Cores.Security;
using MVCWeb.Libraries;
using MVCWeb.Models;

namespace MVCWeb.Controllers
{
    [WhitespaceFilter]
    [CustomAuthorize(Roles = "Admin")]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IProductRepository productRepository
            )
        {
            _productService = productService;
            _categoryService = categoryService;
            _productRepository = productRepository;
        }

        public ActionResult Manage()
        {
            var categories = _categoryService.GetAllWithPrefixOnChildren();
            categories.Insert(0, new Category {Id = 0, CategoryName = "-- All categories --"});
            var model = new ProductManageViewModel()
            {
                CurrentPage = 1,
                PageSize = 10,
            };
            var totalCount = 0;
            model.Categories = categories;
            model.Products = _productService.GetList(new FilterParams(), ref totalCount);
            model.ItemCount = totalCount;
            return View(model);
        }

        [HttpPost]
        public ActionResult Manage(ProductManageViewModel model, int page)
        {
            model.CurrentPage = page;
            model.PageSize = 10;
            var totalCount = 0;
            model.Products = _productService.GetList(new FilterParams
            {
                PageNumber = page,
                Keyword = model.Keyword,
                CategoryId = model.CategoryId
            }, ref totalCount);
            model.ItemCount = totalCount;
            return View("_ProductTable", model);
        }

        [HttpPost]
        public ActionResult ProductListForOrder(int categoryId)
        {
            var count = 0;
            var model = _productService.GetList(new FilterParams
            {
                CategoryId = categoryId,
                PageNumber = 0
            }, ref count);
            return View("_ProductListForOrder", model);
        }
    }
}