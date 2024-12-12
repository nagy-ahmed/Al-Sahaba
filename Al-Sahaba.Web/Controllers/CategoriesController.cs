using Al_Sahaba.Web.Core.Models;
using Al_Sahaba.Web.Core.ViewModels;
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
		[HttpGet]
		public IActionResult Create()
		{
            return View("Form");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var category = new Category
            {
                Name = model.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            var model = new CategoryFormViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View("Form",model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var category = _context.Categories.Find(model.Id);
            if (category is null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
