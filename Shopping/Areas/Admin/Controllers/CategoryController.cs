using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories.OrderByDescending(p => p.id).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_dataContext.Categories, "id", "name");
            ViewBag.Brand = new SelectList(_dataContext.Brands, "id", "name");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.slug = category.name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(s => s.slug == category.slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại");
                    return View(category);
                }

                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Lỗi";
                List<string> errors = new List<string>();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await _dataContext.Categories.FirstOrDefaultAsync(p => p.id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel category)
        {
            var existed_category = _dataContext.Categories.Find(category.id);
            if (ModelState.IsValid)
            {
                category.slug = category.name.Replace(" ", "-");
                //var slug = await _dataContext.Categories.FirstOrDefaultAsync(s => s.slug == category.slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "Danh mục đã tồn tại");
                //    return View(category);
                //}
                existed_category.name = category.name;
                existed_category.slug = category.slug;
                existed_category.description = category.description;
                existed_category.status = category.status;

                _dataContext.Update(existed_category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Lỗi";
                List<string> errors = new List<string>();
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel category = await _dataContext.Categories.FirstOrDefaultAsync(p => p.id == id);

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa danh mục thành công";
            return RedirectToAction("Index");
        }
    }
}
