using MasterChef.Web.Models;
using MasterChef.Web.Services.IServices;
using MasterChef.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterChef.Web.Controllers
{
	public class RecipeController : Controller
	{
		private readonly IRecipeService _recipeService;
		private readonly ICategoryService _categoryService;
		private readonly IIngredientService _ingredientService;
		private readonly IMemoryCache _cache;



		public RecipeController(IRecipeService recipeService, ICategoryService categoryService, IIngredientService ingredientService)
		{
			_recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
			_categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
			_ingredientService = ingredientService ?? throw new ArgumentNullException(nameof(ingredientService));
		}
		public async Task<IActionResult> RecipeIndex()
		{

			var recipes = await _recipeService.FindAllRecipes();
			return View(recipes);
		}
		private async Task<ActionResult> popularCategoriaAsync()
		{

			ViewBag.Lista = await _categoryService.FindAllCategories();

			return View();
		}
		public async Task<IActionResult> RecipeCreate()
		{
			ViewBag.CategoryId = new SelectList(	await _categoryService.FindAllCategories(),
				"Id",
				"Name"
				);

			ViewBag.Ingredients = await _ingredientService.FindAllIngredients();

			return View();
		}

		//[Authorize]
		[HttpPost]
		public async Task<IActionResult> RecipeCreate(RecipeModel model, string CategoryId)
		{
			model.CategoryId = int.Parse(CategoryId);

			if (ModelState.IsValid)
			{
				var response = await _recipeService.CreateRecipe(model);
				if (response != null) return RedirectToAction(
					 nameof(RecipeIndex));
			}
			return View(model);
		}
		public async Task<IActionResult> RecipeUpdate(int id)
		{

			var model = await _recipeService.FindRecipeById(id);
			if (model == null) return NotFound();
			
			var lista = await _categoryService.FindAllCategories();

			List<SelectListItem> categoryList = (from p in lista.AsEnumerable()
												 select new SelectListItem
												 {
													 Text = p.Name,
													 Value = p.Id.ToString()
												 }).ToList();

			categoryList.Find(c => c.Value == model.CategoryId.ToString()).Selected = true;

			ViewBag.CategoryId = categoryList;

			ViewBag.Ingredients = await _ingredientService.FindAllIngredients();


			return View(model);

		}

		//[Authorize]
		[HttpPost]
		public async Task<IActionResult> RecipeUpdate(RecipeModel model, string ddlCategories)
		{
			model.CategoryId = int.Parse(ddlCategories);

			if (ModelState.IsValid)
			{
				var response = await _recipeService.UpdateRecipe(model);
				if (response != null) return RedirectToAction(
					 nameof(RecipeIndex));
			}
			return View(model);
		}

		//[Authorize]
		public async Task<IActionResult> RecipeDelete(int id)
		{
			 var model = await _recipeService.FindRecipeById(id);
            if (model == null) return NotFound();

            var lista = await _categoryService.FindAllCategories();

            List<SelectListItem> categoryList = (from p in lista.AsEnumerable()
                                                 select new SelectListItem
                                                 {
                                                     Text = p.Name,
                                                     Value = p.Id.ToString()
                                                 }).ToList();

            categoryList.Find(c => c.Value == model.CategoryId.ToString()).Selected = true;

            ViewBag.CategoryId = categoryList;
            ViewBag.Url = model.URL;

            ViewBag.Ingredients = await _ingredientService.FindAllIngredients();



            return View(model);
        }

		[HttpPost]
		//[Authorize(Roles = Role.Admin)]
		public async Task<IActionResult> RecipeDelete(RecipeModel model)
		{
			var response = await _recipeService.DeleteRecipeById(model.Id);
			if (response) return RedirectToAction(
					nameof(RecipeIndex));
			return View(model);
		}
	}
}