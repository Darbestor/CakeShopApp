using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakeShop.Models
{
	public class ShoppingCart
	{
		private readonly AppDbContext _appDbContext;

		public string ShoppingCartId { get; set; }
		public List<ShoppingCartItem> ShoppingCartItems { get; set; }

		private ShoppingCart(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		// Gets the current Cart is it was created in the current session
		// Or creates new one
		public static ShoppingCart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?.
				HttpContext.Session;
			var context = services.GetService<AppDbContext>();
			string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

			session.SetString("CartId", cartId);

			return new ShoppingCart(context) { ShoppingCartId = cartId };
		}

		// Adds pie object to current cart
		public void AddToCart(Pie pie, int amount)
		{
			var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
				s => s.Pie.PieId == pie.PieId && s.ShoppingCard == ShoppingCartId);

			if (shoppingCartItem == null)
			{
				shoppingCartItem = new ShoppingCartItem
				{
					ShoppingCard = ShoppingCartId,
					Pie = pie,
					Amount = 1
				};

				_appDbContext.ShoppingCartItems.Add(shoppingCartItem);
			}
			else
			{
				shoppingCartItem.Amount++;
			}
			_appDbContext.SaveChanges();
		}

		// Removes 1 amount if there are > 1 pies or removes pie item from cart
		// And returns Amount of pies left
		public int RemoveFromCart(Pie pie)
		{
			var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
				s => s.Pie.PieId == pie.PieId && s.ShoppingCard == ShoppingCartId);

			var localAmount = 0;

			if (shoppingCartItem != null)
			{
				if (shoppingCartItem.Amount > 1)
				{
					shoppingCartItem.Amount--;
					localAmount = shoppingCartItem.Amount;
				}
				else
				{
					_appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
				}
			}
			_appDbContext.SaveChanges();
			return localAmount;
		}

		// Gets whole shopping cart
		public List<ShoppingCartItem> GetShoppingCartItems()
		{
			return ShoppingCartItems ??
				(ShoppingCartItems = _appDbContext.ShoppingCartItems.Where(
					c => c.ShoppingCard == ShoppingCartId).Include(s => s.Pie).ToList());
		}

		// Removes shopping cart
		public void ClearCart()
		{
			var cartItems = _appDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCard == ShoppingCartId);

			_appDbContext.ShoppingCartItems.RemoveRange(cartItems);
			_appDbContext.SaveChanges();
		}

		// Gets total price (price * amount) of cart
		public decimal GetShoppingCartTotal()
		{
			var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCard == ShoppingCartId).Select(c => c.Pie.Price * c.Amount).Sum();

			return total;
		}
	}
}
