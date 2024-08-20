using MasterChef.Web.Models;
using MasterChef.Web.Services.IServices;
using MasterChef.Web.Utils;
using System.Net.Http.Headers;

namespace MasterChef.Web.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/recipes";

        public RecipeService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<RecipeModel>> FindAllRecipes()
        {
            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<RecipeModel>>();
        }

        public async Task<RecipeModel> FindRecipeById(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<RecipeModel>();
        }

        public async Task<RecipeModel> CreateRecipe(RecipeModel model)
        {
            var response = await _client.PostAsJson(BasePath, model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<RecipeModel>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }
        public async Task<RecipeModel> UpdateRecipe(RecipeModel model)
        {
            var response = await _client.PutAsJson(BasePath, model);
			return await response.ReadContentAs<RecipeModel>();

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<RecipeModel>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }

        public async Task<bool> DeleteRecipeById(long id)
        {
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }
    }
}