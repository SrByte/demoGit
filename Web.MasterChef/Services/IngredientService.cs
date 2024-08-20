using MasterChef.Web.Models;
using MasterChef.Web.Services.IServices;
using MasterChef.Web.Utils;

namespace MasterChef.Web.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/ingredients";

        public IngredientService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<IngredientModel>> FindAllIngredients()
        {
            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<IngredientModel>>();
        }

        public async Task<IngredientModel> FindIngredientById(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<IngredientModel>();
        }

        public async Task<IngredientModel> CreateIngredient(IngredientModel model)
        {
            var response = await _client.PostAsJson(BasePath, model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<IngredientModel>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }
        public async Task<IngredientModel> UpdateIngredient(IngredientModel model)
        {
            var response = await _client.PutAsJson(BasePath, model);
			return await response.ReadContentAs<IngredientModel>();

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<IngredientModel>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }

        public async Task<bool> DeleteIngredientById(long id)
        {
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else throw new Exception("Deu algum ruim na chamada da API.");
        }
    }
}