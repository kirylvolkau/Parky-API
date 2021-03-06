using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private ITrailRepository _trails;
        private INationalParkRepository _parks;

        public TrailsController(ITrailRepository trails, INationalParkRepository parks)
        {
            _trails = trails;
            _parks = parks;
        }
        
        // GET
        public IActionResult Index()
        {
            return View(new TrailsViewModel());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _parks.GetAllAsync(Globals.ApiNpUrl,HttpContext.Session.GetString("JWToken"));
            TrailsViewModel trailVM = new TrailsViewModel()
            {
                NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(trailVM);
            }

            trailVM.Trail = await _trails.GetAsync(Globals.ApiTrialUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (trailVM.Trail is null)
            {
                return NotFound();
            }

            return View(trailVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel trailVM)
        {
            if (ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)
                {
                    await _trails.CreateAsync(Globals.ApiTrialUrl, trailVM.Trail,HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _trails.UpdateAsync(Globals.ApiTrialUrl, trailVM.Trail, trailVM.Trail.Id,HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trailVM);
            }
        }
        
        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new
            {
                data = await _trails.GetAllAsync(Globals.ApiTrialUrl,
                    HttpContext.Session.GetString("JWToken"))
            });
        }
        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trails.DeleteAsync(url:Globals.ApiTrialUrl,id:id,HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new {success = true, message = "Successfully deleted!"});
            }
            return Json(new {success = false, message = "Not deleted!"});
        }
    }
}