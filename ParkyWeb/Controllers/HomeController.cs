using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _parks;
        private readonly ITrailRepository _trails;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository parks, ITrailRepository trails)
        {
            _logger = logger;
            _parks = parks;
            _trails = trails;
        }

        public async Task<IActionResult> Index()
        {
            var index = new IndexViewModel()
            {
                Parks = await _parks.GetAllAsync(Globals.ApiNpUrl),
                Trails = await _trails.GetAllAsync(Globals.ApiTrialUrl)
            };
            return View(index);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}