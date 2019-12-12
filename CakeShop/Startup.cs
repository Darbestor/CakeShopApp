using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CakeShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CakeShop
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddScoped<IPieRepository, PieRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
			});
			services.AddHttpContextAccessor();
			services.AddSession();
			services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSession();

			app.UseRouting();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
