using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using Microsoft.Practices.ObjectBuilder2;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;

namespace MVCWeb.Cores.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(
            IProductRepository productRepository,
            ICategoryService categoryService
            )
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public Product GetWithCategoriesById(int productId)
        {
            return _productRepository.Table.Include(o => o.Categories).FirstOrDefault(o => o.Id == productId);
        }

        public int Create(Product product)
        {
            if (!string.IsNullOrEmpty(product.MappedCategoryIds))
            {
                var categoryIds = product.MappedCategoryIds.Split(',').Select(int.Parse).ToList();
                var categories = _categoryService.GetCategories(categoryIds);
                product.Categories = categories;
            }
            _productRepository.Insert(product);
            return product.Id;
        }
        public bool UpdateProduct(Product product)
        {
            var currentProduct = GetWithCategoriesById(product.Id);
            if (currentProduct == null) return false;
            currentProduct.ProductName = product.ProductName;
            currentProduct.ShortDescription = product.ShortDescription;
            currentProduct.Categories.Clear();
            if (!string.IsNullOrEmpty(product.MappedCategoryIds))
            {
                var categoryIds = product.MappedCategoryIds.Split(',').Select(int.Parse).ToList();
                var categories = _categoryService.GetCategories(categoryIds);
                currentProduct.Categories = categories;
            }
            _productRepository.Update(product);
            return true;
        }
        public List<Product> GetList(FilterParams fp, ref int totalCount)
        {
            var list = _productRepository.TableNoTracking.Include(o => o.Categories);
            if (!string.IsNullOrEmpty(fp.Keyword))
            {
                list = list.Where(o => o.ProductName.Contains(fp.Keyword));
            }
            if (fp.CategoryId != 0)
            {
                var category = _categoryService.GetWithChildren(fp.CategoryId);
                var categoryIds = new List<int> { fp.CategoryId };
                if (category != null && category.ChildCategories.Any())
                {
                    categoryIds.AddRange(category.ChildCategories.Select(o => o.Id));
                }

                list = list.Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)));
            }
            totalCount = list.Count();
            list = list.OrderBy(fp.SortField + (fp.SortASC ? " ASC" : " DESC"));
            if (fp.PageNumber == 0) return list.ToList();
            var skip = (fp.PageNumber - 1) * fp.PageSize;
            var take = fp.PageSize;
            list = list.Skip(skip).Take(take);
            return list.ToList();
        }
    }
}