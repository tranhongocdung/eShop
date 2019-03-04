using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCWeb.Cores;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;
using MVCWeb.Cores.Security;
using MVCWeb.Libraries;
using MVCWeb.Models;

namespace MVCWeb.Controllers
{
    [WhitespaceFilter]
    [CustomAuthorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryService categoryService,
            ICategoryRepository categoryRepository)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
        }
        public ActionResult Manage()
        {
            var categoryEditViewModel = new CategoryEditViewModel
            {
                Category = new Category
                {
                    Id = 0
                },
                ParentCategories = _categoryService.GetParentListWithChildren()
            };
            categoryEditViewModel.ParentCategories.Insert(0, _categoryService.RootCategory());
            var model = new CategoryManageViewModel()
            {
                Categories = _categoryService.GetAllWithTreeViewOrder(),
                CategoryEditViewModel = categoryEditViewModel
            };
            
            return View("_Manage", model);
        }

        public ActionResult Edit(int id = 0)
        {
            var parentCategories = _categoryService.GetParentListWithChildren().Where(o => o.Id != id).ToList();
            parentCategories.Insert(0, _categoryService.RootCategory());
            var model = new CategoryEditViewModel();
            if (id != 0)
            {
                var category = _categoryService.GetWithChildren(id);
                if (category != null)
                {
                    model.Category = category;
                    if (category.ParentId == null && category.ChildCategories.Any())
                    {
                        model.IsDeletable = false;
                        model.ParentCategories = new List<Category> { _categoryService.RootCategory() };
                    }
                    else
                    {
                        model.IsDeletable = true;
                        model.ParentCategories = parentCategories;
                    }
                }
            }
            else
            {
                model.Category = new Category
                {
                    Id = 0
                };
                model.ParentCategories = parentCategories;
            }
            return View("_Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CategoryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message;
                var obj = _categoryRepository.GetById(model.Category.Id);
                if (obj == null)
                {
                    obj = new Category();
                    obj.Id = _categoryService.Create(model.Category);
                    message = "Đã thêm thành công!";
                }
                else
                {
                    _categoryService.UpdateCategory(model.Category);
                    message = "Đã cập nhật thành công!";
                }
                var categories = _categoryService.GetAllWithTreeViewOrder();
                return Json(new ReturnData { Success = true, Message = message, Data = RenderRazorViewToString("_CategoryListTreeView", categories) });
            }
            return Json(new ReturnData { Success = false, Message = "Lỗi!" });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _categoryService.DeleteCategory(id);
            if (result)
            {
                var message = "Đã xóa thành công!";
                var categories = _categoryService.GetAllWithTreeViewOrder();
                return Json(new ReturnData { Success = true, Message = message, Data = RenderRazorViewToString("_CategoryListTreeView", categories) });
            }
            return Json(new ReturnData { Success = false, Message = "Không thể xóa, nhóm chứa nhóm con hoặc không tồn tại!" });
        }
    }
}