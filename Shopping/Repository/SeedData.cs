using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel apple = new CategoryModel
				{
					Name = "Apple",
					Slug = "apple",
					Description = "Apple",
					Status = 1
				};
				BrandModel dell = new BrandModel
				{
					Name = "Dell",
					Slug = "dell",
					Description = "dell",
					Status = 1
				};
				_context.Products.AddRange(
					new ProductModel
					{
						Name = "Microsoft",
						Slug = "Microsoft",
						Description = "mac",
						Image = "1.jpg",
						Price = 1000,
						Category = apple,
						Brand = dell

					});
				_context.SaveChanges();
			}
		}
	}
}
