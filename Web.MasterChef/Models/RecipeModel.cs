namespace MasterChef.Web.Models
{
	public class RecipeModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public string Tags { get; set; }
		public string URL { get; set; }
		public List<int> IngredientIds { get; set; }
		public string Description { get; set; }
	}
}
