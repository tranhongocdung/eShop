using System.Collections.Generic;
using MVCWeb.Cores.Entities;

namespace MVCWeb.Cores.IServices
{
    public interface ICategoryService : IWebAppService
    {
        int Create(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int categoryId);
        Category RootCategory();
        List<Category> GetList(FilterParams fp, ref int totalCount);
        List<Category> GetAllWithTreeViewOrder();
        List<Category> GetAllWithPrefixOnChildren();
        Category GetWithChildren(int categoryId);
        IList<Category> GetCategories(IList<int> categoryIds);
        List<Category> GetParentListWithChildren();
    }
}
