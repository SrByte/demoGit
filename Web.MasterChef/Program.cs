using MasterChef.Web.Services.IServices;
using MasterChef.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS8604 // Possible null reference argument.
builder.Services.AddHttpClient<IRecipeService, RecipeService>(
	c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:MasterChefAPI"])
);
builder.Services.AddHttpClient<IIngredientService, IngredientService>(
	c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:MasterChefAPI"])
);
builder.Services.AddHttpClient<ICategoryService, CategoryService>(
	c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:MasterChefAPI"])
);
#pragma warning restore CS8604 // Possible null reference argument.


// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
