using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakeShop.ViewModels
{
	// View model that encapsulates Pie repository with Category name
	public class PiesListViewModel
	{
		public IEnumerable<Pie> Pies { get; set; }
		public string CurreuntCategory { get; set; }
	}
}
