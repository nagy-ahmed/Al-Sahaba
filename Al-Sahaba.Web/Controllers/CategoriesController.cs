using Microsoft.AspNetCore.Mvc;

namespace Al_Sahaba.Web.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			//todo : use viewModel
			var categories = _context.Categories.ToList();
			return View(categories);
		}
	}
}
