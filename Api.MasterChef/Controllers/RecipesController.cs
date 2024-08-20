using Api.MasterChef.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.MasterChef.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class RecipesController : ControllerBase
	{
		private readonly IMemoryCache _cache;

		public RecipesController(IMemoryCache cache)
		{
			_cache = cache;
		}

		[HttpPost]
		public async Task<ActionResult<RecipeDto>> CreateRecipe(RecipeDto recipeDto)
		{
			var cacheKey = "AllRecipes";

			if (!_cache.TryGetValue(cacheKey, out List<RecipeDto> allRecipes))
				allRecipes = new List<RecipeDto>();

			var maxId = allRecipes.DefaultIfEmpty().Max(r => r?.Id ?? 0);
			recipeDto.Id = maxId + 1;

			allRecipes.Add(recipeDto);

			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
			};

			_cache.Set(cacheKey, allRecipes, cacheEntryOptions);

			return CreatedAtAction(nameof(GetRecipe), new { id = recipeDto.Id }, recipeDto);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
		{
			var cacheKey = "AllRecipes";

			if (_cache.TryGetValue(cacheKey, out List<RecipeDto> allRecipes))
			{
				var recipe = allRecipes.FirstOrDefault(r => r.Id == id);

				if (recipe != null)
					return recipe;
			}

			return NotFound();
		}

		[HttpPut()]
		public async Task<IActionResult> UpdateRecipe(RecipeDto updatedRecipe)
		{
			var cacheKey = "AllRecipes";

			if (_cache.TryGetValue(cacheKey, out List<RecipeDto> allRecipes))
			{
				var existingRecipe = allRecipes.FirstOrDefault(r => r.Id == updatedRecipe.Id);

				if (existingRecipe != null)
				{
					existingRecipe.Name = updatedRecipe.Name;
					existingRecipe.Description = updatedRecipe.Description;
					existingRecipe.CategoryId = updatedRecipe.CategoryId;
                    existingRecipe.URL = updatedRecipe.URL;
					existingRecipe.IngredientIds = updatedRecipe.IngredientIds;
					existingRecipe.Tags = updatedRecipe.Tags;



                    _cache.Set(cacheKey, allRecipes);
					return Ok(existingRecipe);

				}

				return NoContent();
			}

			return NotFound();
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRecipe(int id)
		{
			var cacheKey = "AllRecipes";

			if (_cache.TryGetValue(cacheKey, out List<RecipeDto> allRecipes))
			{
				var recipeToRemove = allRecipes.FirstOrDefault(r => r.Id == id);

				if (recipeToRemove != null)
				{
					allRecipes.Remove(recipeToRemove);

					_cache.Set(cacheKey, allRecipes);
					return Ok(true);
				}

				return NoContent();
			}

			return NotFound();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipes()
		{
			var cacheKey = "AllRecipes";

			if (_cache.TryGetValue(cacheKey, out List<RecipeDto> allRecipes))
				return allRecipes;

			return new List<RecipeDto>();
		}
	}
}