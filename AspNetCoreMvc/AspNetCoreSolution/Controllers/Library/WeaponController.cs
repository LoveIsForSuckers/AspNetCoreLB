using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.Library;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSolution.Controllers.Library
{
    [Authorize(Policy = CustomClaimTypes.CanEditLibrary)]
    [Route("Weapon/[action]")]
    public class WeaponController : Controller
    {
        private ILibraryRepository _repo;

        public WeaponController(ILibraryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _repo.GetWeapons();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Weapon();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]Weapon newWeapon)
        {
            if (ModelState.IsValid)
            {
                newWeapon.Id = await _repo.GetNextWeaponId();

                await _repo.AddWeapon(newWeapon);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newWeapon);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _repo.GetWeapon((int)id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm]Weapon newWeapon)
        {
            if (ModelState.IsValid)
            {
                newWeapon.Version++;

                await _repo.ReplaceWeapon(newWeapon.Id, newWeapon);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newWeapon);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _repo.GetWeapon((int)id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Weapon deletedWeapon)
        {
            await _repo.RemoveWeapon(deletedWeapon.Id);
            await _repo.IncreaseDeletedItemsVersionCount(deletedWeapon.Version);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _repo = null;

            base.Dispose(disposing);
        }
    }
}
