using MasterChef.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterChef.Web.Services.IServices
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeModel>> FindAllRecipes();
        Task<RecipeModel> FindRecipeById(long id);
        Task<RecipeModel> CreateRecipe(RecipeModel model);
        Task<RecipeModel> UpdateRecipe(RecipeModel model);
        Task<bool> DeleteRecipeById(long id);
    }
} 