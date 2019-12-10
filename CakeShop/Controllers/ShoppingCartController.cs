using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeShop.Models;
using CakeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CakeShop.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly IPieRepository _pieRepository;
		private readonly ShoppingCart _shoppingCart;

		public ShoppingCartController(IPieRepository pieRepository, ShoppingCart shoppingCart)
		{
			_pieRepository = pieRepository;
			_shoppingCart = shoppingCart;
		}

		// GET: /<controller>/
		public IActionResult Index()
		{
			var items = _shoppingCart.GetShoppingCartItems();
			// Sets cart property to current items
			_shoppingCart.ShoppingCartItems = items;

			var shoppingCartViewModel = new ShoppingCartViewModel
			{
				ShoppingCart = _shoppingCart,
				ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
			};
			return View(shoppingCartViewModel);
		}

		public IActionResult AddToShoppingCart(int pieId)
		{
			Pie selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

			if (selectedPie != null)
			{
				_shoppingCart.AddToCart(selectedPie, 1);
			}
			return RedirectToAction("Index");
		}

		public IActionResult RemoveFromShoppingCart(int pieId)
		{
			Pie selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

			if (selectedPie != null)
			{
				_shoppingCart.RemoveFromCart(selectedPie);
			}
			return RedirectToAction("Index");
		}
	}
}
