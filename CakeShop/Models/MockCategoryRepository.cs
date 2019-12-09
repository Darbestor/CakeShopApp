﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakeShop.Models
{
	public class MockCategoryRepository : ICategoryRepository
	{
		public IEnumerable<Category> AllCategories => 
			new List<Category>
			{
				new Category{CategoryId=1, CategoryName="Fruit pies", Description="All-fruit-pies" },
				new Category{CategoryId=2, CategoryName="Cheese pies", Description="Chees all the way" },
				new Category{CategoryId=3, CategoryName="Seasonal pies", Description="get in the mood for a seasonal pie" }
			};
	}
}
