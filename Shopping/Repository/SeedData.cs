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
					name = "Apple",
					slug = "apple",
					description = "Apple",
					status = 1
				};
				BrandModel dell = new BrandModel
				{
					name = "Dell",
					slug = "dell",
					description = "dell",
					status = 1
				};
				_context.Products.AddRange(
					new ProductModel
					{
						name = "Microsoft",
						slug = "Microsoft",
						description = "mac",
						image = "1.jpg",
						price = 1000,
						category = apple,
						brand = dell

					});
				_context.SaveChanges();
			}
		}
	}
}
