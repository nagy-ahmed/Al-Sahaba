namespace Al_Sahaba.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var authors = _context.Authors.AsNoTracking().ToList();
            return View(_mapper.Map<IEnumerable<AuthorViewModel>>(authors));
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(model);

            _context.Authors.Add(author);
            _context.SaveChanges();

            var viewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }

            var formModel = _mapper.Map<AuthorFormViewModel>(author);

            return PartialView("_Form", formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = _context.Authors.Find(model.Id);
            if (author is null)
            {
                return NotFound();
            }

            author = _mapper.Map(model, author);
            author.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            var viewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }

            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            return Ok(author.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(AuthorFormViewModel model)
        {
            var author = _context.Authors.SingleOrDefault(m => m.Name == model.Name);
            var isValid = author is null || model.Id == author.Id;
            return Json(isValid);
        }
    }
}
