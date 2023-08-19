using Microsoft.AspNetCore.Mvc;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;
using Order.Web.Models;
using System.Diagnostics;

namespace Order.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll(includeProperties:"ProductImages").Take(10);
            return View(products);
        }

        public IActionResult Details(int Id)
        {
            Product product = _productRepository.Get(i => i.Id == Id,includeProperties:"ProductImages");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}