using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;

namespace MVCWeb.Cores.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(
            ICategoryRepository categoryRepository
            )
        {
            _categoryRepository = categoryRepository;
        }


        public int Create(Category category)
        {
            if (category.ParentId == 0) category.ParentId = null;
            _categoryRepository.Insert(category);
            return category.Id;
        }

        public bool UpdateCategory(Category category)
        {
            var currentCategory = _categoryRepository.GetById(category.Id);
            if (currentCategory == null) return false;
            currentCategory.CategoryName = category.CategoryName;
            currentCategory.ParentId = category.ParentId == 0 ? null : category.ParentId;
            _categoryRepository.Update(category);
            return true;
        }

        public bool DeleteCategory(int categoryId)
        {
            var category = GetWithChildren(categoryId);
            if (category == null) return false;
            if (category.ChildCategories.Any()) return false;
            _categoryRepository.Delete(category);
            return true;
        }

        public Category RootCategory()
        {
            return new Category
            {
                Id = 0,
                CategoryName = "--"
            };
        }

        public List<Category> GetList(FilterParams fp, ref int totalCount)
        {
            var list = _categoryRepository.TableNoTracking;
            if (!string.IsNullOrEmpty(fp.Keyword))
            {
                list = list.Where(o => o.CategoryName.Contains(fp.Keyword));
            }
            totalCount = list.Count();
            list = list.OrderBy(fp.SortField + (fp.SortASC ? " ASC" : " DESC"));
            if (fp.PageNumber == 0) return list.ToList();
            var skip = (fp.PageNumber - 1) * fp.PageSize;
            var take = fp.PageSize;
            list = list.Skip(skip).Take(take);
            return list.ToList();
        }

        public List<Category> GetAllWithTreeViewOrder()
        {
            var list = GetParentListWithChildren();
            var sortedList = new List<Category>();
            list.ForEach(category =>
            {
                sortedList.Add(category);
                sortedList.AddRange(category.ChildCategories);
            });
            return sortedList;
        }

        public List<Category> GetAllWithPrefixOnChildren()
        {
            var list = GetAllWithTreeViewOrder();
            list.ForEach(category =>
            {
                if (category.ParentId != null)
                {
                    category.CategoryName = "-- " + category.CategoryName;
                }
            });
            return list;
        }
        public IList<Category> GetCategories(IList<int> categoryIds)
        {
            return _categoryRepository.Table.Where(o => categoryIds.Contains(o.Id)).ToList();
        }
        public Category GetWithChildren(int categoryId)
        {
            return _categoryRepository.Table.Include(o => o.ChildCategories).FirstOrDefault(o => o.Id == categoryId);
        }

        public List<Category> GetParentListWithChildren()
        {
            return
                _categoryRepository.TableNoTracking.Include(o => o.ChildCategories)
                    .Where(o => o.ParentId == null)
                    .ToList();
        }
    }
}