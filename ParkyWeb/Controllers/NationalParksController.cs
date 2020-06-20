using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _repository;

        public NationalParksController(INationalParkRepository repository)
        {
            _repository = repository;
        }
        // GET
        public IActionResult Index()
        {
            var model = new NationalPark();
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Upsert(int? id)
        {
           NationalPark park = new NationalPark();
           if (id is null)
           {
               //true for insert/create
               return View(park);
           }
           // for update
           park = await _repository.GetAsync(Globals.ApiNpUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
           if (park is null)
           {
               return NotFound();
           }

           return View(park);
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new {data = await _repository.GetAllAsync(Globals.ApiNpUrl, HttpContext.Session.GetString("JWToken"))});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark park)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                byte[] pi = null;
                if (files.Count > 0)
                {
                    using (var fsi = files[0].OpenReadStream())
                    {
                        using (var msi = new MemoryStream())
                        {
                            fsi.CopyTo(msi);
                            pi = msi.ToArray();
                        }
                    }
                    park.Image = pi;
                }
                else
                {
                    var parkFromDb = await _repository.GetAsync(Globals.ApiNpUrl, park.Id, HttpContext.Session.GetString("JWToken"));
                    if (parkFromDb != null)
                    {
                        park.Image = parkFromDb.Image;
                    }
                }
                if (park.Id == 0)
                {
                    await _repository.CreateAsync(Globals.ApiNpUrl, park, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _repository.UpdateAsync(Globals.ApiNpUrl, park, park.Id, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(park);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repository.DeleteAsync(Globals.ApiNpUrl,id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new {success = true, message = "Successfully deleted!"});
            }
            return Json(new {success = false, message = "Not deleted!"});
        }
    }
}