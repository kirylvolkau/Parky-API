using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccountRepository _account;
        private readonly ITrailRepository _trails;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository parks, 
            ITrailRepository trails, IAccountRepository account)
        {
            _logger = logger;
            _parks = parks;
            _trails = trails;
            _account = account;
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

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User userObj = await _account.LoginAsync(Globals.ApiUserUrl + "authenticate/", obj);
            if (userObj.Token is null)
            {
                return View();
            }
            HttpContext.Session.SetString("JWToken", userObj.Token);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _account.RegisterAsync(Globals.ApiUserUrl + "register/", obj);
            if (!result)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("JWToken",string.Empty);
            return RedirectToAction("Index");
        }
    }
}