using MasterChef.Web.Models;

namespace MasterChef.Web.Services.IServices
{
	public interface IIngredientService
	{
        Task<IEnumerable<IngredientModel>> FindAllIngredients();
        Task<IngredientModel> FindIngredientById(long id);
        Task<IngredientModel> CreateIngredient(IngredientModel model);
        Task<IngredientModel> UpdateIngredient(IngredientModel model);
        Task<bool> DeleteIngredientById(long id);
    }
} 