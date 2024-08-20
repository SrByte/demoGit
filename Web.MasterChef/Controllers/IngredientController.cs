using MasterChef.Web.Models;
using MasterChef.Web.Services.IServices;
using MasterChef.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterChef.Web.Controllers
{
	public class IngredientController : Controller
	{
		private readonly IIngredientService _ingredientService;

		public IngredientController(IIngredientService ingredientService)
		{
			_ingredientService = ingredientService ?? throw new ArgumentNullException(nameof(ingredientService));
		}
		public async Task<IActionResult> IngredientIndex()
		{
			var ingredients = await _ingredientService.FindAllIngredients();
			return View(ingredients);
		}

		public async Task<IActionResult> IngredientCreate()
		{
			return View();
		}

        //[Authorize]
        [HttpPost]
		public async Task<IActionResult> IngredientCreate(IngredientModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _ingredientService.CreateIngredient(model);
				if (response != null) return RedirectToAction(
					 nameof(IngredientIndex));
			}
			return View(model);
		}
        public async Task<IActionResult> IngredientUpdate(int id)
		{
			var model = await _ingredientService.FindIngredientById(id);
			if (model != null) return View(model);
			return NotFound();
		}

        //[Authorize]
        [HttpPost]
		public async Task<IActionResult> IngredientUpdate(IngredientModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _ingredientService.UpdateIngredient(model);
				if (response != null) return RedirectToAction(
					 nameof(IngredientIndex));
			}
			return View(model);
		}

        //[Authorize]
        public async Task<IActionResult> IngredientDelete(int id)
		{
			var model = await _ingredientService.FindIngredientById(id);
			if (model != null) return View(model);
			return NotFound();
		}

        [HttpPost]
		//[Authorize(Roles = Role.Admin)]
		public async Task<IActionResult> IngredientDelete(IngredientModel model)
		{
			var response = await _ingredientService.DeleteIngredientById(model.Id);
			if (response) return RedirectToAction(
					nameof(IngredientIndex));
			return View(model);
		}
	}
}