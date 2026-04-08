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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _productServce.AddProductAsync(model);
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("Index"); // redirect to your products list or dashboard
        }
    }
}
