using E_Commerce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductService _productServce;
        public ProductDetailController(IProductService service)
        {
            _productServce = service;
        }
        public IActionResult Index(int id)
        {
            var ProductDetail = _productServce.GetProductByIdAsync(id).Result;
            return View(ProductDetail);
        }
    }
}
