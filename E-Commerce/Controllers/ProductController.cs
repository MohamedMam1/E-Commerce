using E_Commerce.Interfaces;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
