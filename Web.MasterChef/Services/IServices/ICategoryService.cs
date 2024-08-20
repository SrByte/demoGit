using MasterChef.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterChef.Web.Services.IServices
{
    public interface ICategoryService
	{
        Task<IEnumerable<CategoryModel>> FindAllCategories();
        Task<CategoryModel> FindCategoryById(long id);
        Task<CategoryModel> CreateCategory(CategoryModel model);
        Task<CategoryModel> UpdateCategory(CategoryModel model);
        Task<bool> DeleteCategoryById(long id);
    }
} 