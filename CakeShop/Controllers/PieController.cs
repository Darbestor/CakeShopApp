﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CakeShop.Models;
using CakeShop.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CakeShop.Controllers
{
	public class PieController : Controller
	{
		private readonly IPieRepository _pieRepository;
		private readonly ICategoryRepository _categoryRepository;

		public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
		{
			_pieRepository = pieRepository;
			_categoryRepository = categoryRepository;
		}

		// GET: /<controller>/
		public IActionResult List()
		{
			PiesListViewModel model = new PiesListViewModel();
			model.Pies = _pieRepository.AllPies;
			model.CurreuntCategory = "Cheese cakes";

			return View(model);
		}
	}
}