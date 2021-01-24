using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Grecha.Parsing.ParserContext;
using Grecha.Parsing.Parsers;

namespace Grecha.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IParserContext _parserContext;

        public HomeController(AppDbContext context, IParserContext parserContext)
        {
            _context = context;
            _parserContext = parserContext;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            _parserContext.SetParsingStrategy(new AtbParser());
            var products = _parserContext.Parse("крупа", 20, null);
            var a = 1;
            return View(await _context.Products.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
