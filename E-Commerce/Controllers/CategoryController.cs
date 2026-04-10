using E_Commerce.Interfaces;
using E_Commerce.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admindashboard/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("AddCategory", new CategoryCreateVM());
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.AddCategoryAsync(model);
            TempData["SuccessMessage"] = "Category added successfully.";
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            var model = new CategoryEditVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View("EditCategory", model);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = await _categoryService.CategoryExistsAsync(model.Id);
            if (!exists)
                return NotFound();

            await _categoryService.UpdateCategoryAsync(model);
            TempData["SuccessMessage"] = "Category updated successfully.";
            return RedirectToAction("Categories", "Admin");
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _categoryService.CategoryExistsAsync(id);
            if (!exists)
                return Json(new { success = false, message = "Category not found." });

            await _categoryService.DeleteCategoryAsync(id);
            return Json(new { success = true });
        }

        public async Task<IActionResult> IsNameUnique(string name)
        {
            var exists = await _categoryService.IsNameExistsAsync(name);
            return Json(!exists);
        }

    }
}
