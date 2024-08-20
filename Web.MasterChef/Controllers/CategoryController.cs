using MasterChef.Web.Models;
using MasterChef.Web.Services.IServices;
using MasterChef.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterChef.Web.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
		}
		public async Task<IActionResult> CategoryIndex()
		{
			var categories = await _categoryService.FindAllCategories();
			return View(categories);
		}

		public async Task<IActionResult> CategoryCreate()
		{
			return View();
		}

        //[Authorize]
        [HttpPost]
		public async Task<IActionResult> CategoryCreate(CategoryModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _categoryService.CreateCategory(model);
				if (response != null) return RedirectToAction(
					 nameof(CategoryIndex));
			}
			return View(model);
		}
        public async Task<IActionResult> CategoryUpdate(int id)
		{
			var model = await _categoryService.FindCategoryById(id);
			if (model != null) return View(model);
			return NotFound();
		}

        //[Authorize]
        [HttpPost]
		public async Task<IActionResult> CategoryUpdate(CategoryModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _categoryService.UpdateCategory(model);
				if (response != null) return RedirectToAction(
					 nameof(CategoryIndex));
			}
			return View(model);
		}

        //[Authorize]
        public async Task<IActionResult> CategoryDelete(int id)
		{
			var model = await _categoryService.FindCategoryById(id);
			if (model != null) return View(model);
			return NotFound();
		}

        [HttpPost]
		//[Authorize(Roles = Role.Admin)]
		public async Task<IActionResult> CategoryDelete(CategoryModel model)
		{
			var response = await _categoryService.DeleteCategoryById(model.Id);
			if (response) return RedirectToAction(
					nameof(CategoryIndex));
			return View(model);
		}
	}
}