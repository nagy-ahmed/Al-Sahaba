namespace Al_Sahaba.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            return View(_mapper.Map<IEnumerable<CategoryViewModel>>(categories));
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(model);

            _context.Categories.Add(category);
            _context.SaveChanges();

            var viewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            var formModel = _mapper.Map<CategoryFormViewModel>(category);

            return PartialView("_Form", formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = _context.Categories.Find(model.Id);
            if (category is null)
            {
                return NotFound();
            }

            category = _mapper.Map(model, category);
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            var viewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            category.IsDeleted = !category.IsDeleted;
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            return Ok(category.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var category = _context.Categories.SingleOrDefault(m => m.Name == model.Name);
            var isValid = category is null || model.Id == category.Id;
            return Json(isValid);
        }
    }
}
