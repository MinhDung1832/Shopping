using Microsoft.AspNetCore.Mvc;
using Shopping.Repository;

namespace Shopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Details(int id)
		{
            if (id == null) return RedirectToAction("Index");

            var productById = _dataContext.Products.Where(s => s.id == id).FirstOrDefault();

			return View(productById);
		}
	}
}
