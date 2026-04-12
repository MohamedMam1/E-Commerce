using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productServce;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductService service,
            ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productServce = service;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productServce.GetFilteredProductsForCustomerAsync(
                searchTerm: null, categoryName: null, categoryId: null,
                isAvailable: null, minPrice: null, maxPrice: null,
                sortBy: null, pageNumber: 1, pageSize: 12);

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new ProductCreateVM
            {
                Categories = await GetCategorySelectList()
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectList();
                return View(model);
            }

            await _productServce.AddProductAsync(model);
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("Products", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productServce.GetProductEditVMAsync(id);
            if (model == null) return NotFound();

            model.Categories = await GetCategorySelectList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectList();
                return View(model);
            }

            var exists = await _productServce.ProductExistsAsync(model.Id);
            if (!exists) return NotFound();

            await _productServce.UpdateProductAsync(model);
            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction("Products", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admindashboard/products/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _productServce.ProductExistsAsync(id);
            if (!exists)
                return Json(new { success = false, message = "Product not found!" });

            await _productServce.DeleteProductAsync(id);
            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        private async Task<List<SelectListItem>> GetCategorySelectList()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts(string searchTerm, string categoryName, int? categoryId, bool? isAvailable,decimal? minPrice, decimal? maxPrice, string sortBy,int pageNumber = 1, int pageSize = 12)
        {
            var result = await _productServce.GetFilteredProductsForCustomerAsync(searchTerm,categoryName,categoryId,isAvailable,minPrice,maxPrice,sortBy,pageNumber,pageSize);

            return Json(new
            {
                products = result.Data,
                totalCount = result.TotalCount,
                pageNumber = result.PageNumber,
                pageSize = result.PageSize,
                totalPages = result.TotalPages,
                hasPrevious = result.HasPrevious,
                hasNext = result.HasNext
            });
        }
    }
}