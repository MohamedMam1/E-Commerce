using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productServce;
        public ProductController(IProductService service)
        {
            _productServce = service;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServce.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Search(string SearchValue)
        {
            if (string.IsNullOrWhiteSpace(SearchValue))
            {
                return RedirectToAction(nameof(Index));
            }

            var products = await _productServce.SearchProducts(SearchValue);

            if (products == null || !products.Any())
            {
                return View("Index", new List<ProductListVM>());
            }

            return View("Index", products);
        }

        public async Task<IActionResult> Filter(ProductFilterVM filter)
        {
            var products = await _productServce.FilterProducts(filter);

            return View("Index", products);
        }
    }
}
