using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class CategoryController : Controller
    {
		private readonly DataContext _dataContext;

		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string slug = "")
        {
			CategoryModel category = _dataContext.Categories.Where(s => s.slug == slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			var productByCat = _dataContext.Products.Where(s => s.categoryId == category.id);
            return View(await productByCat.OrderByDescending(s => s.id).ToListAsync());
        }
    }
}
