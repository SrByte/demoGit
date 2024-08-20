using Api.MasterChef.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.MasterChef.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class IngredientsController : ControllerBase
	{
		private readonly IMemoryCache _cache;

		public IngredientsController(IMemoryCache cache)
		{
			_cache = cache;
			PopularIngredients();
		}
		private void PopularIngredients()
		{
			var cacheKey = "AllIngredients";

			if (!_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngredients))
			{
				allIngredients = new List<IngredientDto>();
				allIngredients.Add(new IngredientDto() { Id = 1, Name = "Leite" });
				allIngredients.Add(new IngredientDto() { Id = 2, Name = "Açucar" });
				allIngredients.Add(new IngredientDto() { Id = 3, Name = "Chocolate" });
				allIngredients.Add(new IngredientDto() { Id = 4, Name = "Sal" });


				var cacheEntryOptions = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
				};
				_cache.Set(cacheKey, allIngredients, cacheEntryOptions);
			}

		}

		[HttpPost]
		public async Task<ActionResult<IngredientDto>> CreateIngredient(IngredientDto ingredientDto)
		{
			var cacheKey = "AllIngredients";

			if (!_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngredients))
				allIngredients = new List<IngredientDto>();

			var maxId = allIngredients.DefaultIfEmpty().Max(r => r?.Id ?? 0);
			ingredientDto.Id = maxId + 1;

			allIngredients.Add(ingredientDto);

			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
			};

			_cache.Set(cacheKey, allIngredients, cacheEntryOptions);

			return CreatedAtAction(nameof(GetIngredient), new { id = ingredientDto.Id }, ingredientDto);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
		{
			var cacheKey = "AllIngredients";

			if (_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngredients))
			{
				var ingridient = allIngredients.FirstOrDefault(r => r.Id == id);

				if (ingridient != null)
					return ingridient;
			}

			return NotFound();
		}

		[HttpPut()]
		public async Task<IActionResult> UpdateIngredient(IngredientDto updatedIngredient)
		{
			var cacheKey = "AllIngredients";

			if (_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngredients))
			{
				var existingIngredient = allIngredients.FirstOrDefault(r => r.Id == updatedIngredient.Id);

				if (existingIngredient != null)
				{
					existingIngredient.Name = updatedIngredient.Name;
					existingIngredient.Name = updatedIngredient.Name;

					_cache.Set(cacheKey, allIngredients);
					return Ok(existingIngredient);

				}

				return NoContent();
			}

			return NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteIngredient(int id)
		{
			var cacheKey = "AllIngredients";

			if (_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngridients))
			{
				var engridientToRemove = allIngridients.FirstOrDefault(r => r.Id == id);

				if (engridientToRemove != null)
				{
					allIngridients.Remove(engridientToRemove);

					_cache.Set(cacheKey, allIngridients);
					return Ok(true);
				}

				return NoContent();
			}

			return NotFound();
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredients()
		{
			var cacheKey = "AllIngredients";

			if (_cache.TryGetValue(cacheKey, out List<IngredientDto> allIngredients))
				return allIngredients;

			return new List<IngredientDto>();
		}
	}
}