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
            var products = await _productServce.SearchProducts(SearchValue);
            var allproducts = await _productServce.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return View("Index", allproducts);
            }
            return View("Index", products);
        }
    }
}
