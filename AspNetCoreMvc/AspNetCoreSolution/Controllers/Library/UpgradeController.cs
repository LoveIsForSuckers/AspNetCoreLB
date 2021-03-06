﻿using AspNetCoreSolution.Extensions;
using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Controllers.Library
{
    [Authorize(Policy = CustomClaimTypes.CanEditLibrary)]
    [Route("Upgrade/[action]")]
    public class UpgradeController : Controller
    {
        private ILibraryRepository _repo;

        public UpgradeController(ILibraryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _repo.GetUpgrades();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Upgrade();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]Upgrade newUpgrade)
        {
            if (ModelState.IsValid)
            {
                newUpgrade.Id = await _repo.GetNextUpgradeId();

                await _repo.AddUpgrade(newUpgrade);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newUpgrade);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _repo.GetUpgrade((int)id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm]Upgrade newUpgrade)
        {
            if (ModelState.IsValid)
            {
                newUpgrade.Version++;

                await _repo.ReplaceUpgrade(newUpgrade.Id, newUpgrade);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newUpgrade);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _repo.GetUpgrade((int)id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Upgrade deletedUpgrade)
        {
            await _repo.RemoveUpgrade(deletedUpgrade.Id);
            await _repo.IncreaseDeletedItemsVersionCount(deletedUpgrade.Version);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [ResponseCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public PartialViewResult NewUpgradeLevel(int index)
        {
            var upgrade = new Upgrade();

            var partial = PartialView("UpgradeLevel", new UpgradeLevel());
            var htmlPrefix = HtmlHelperExtensions.GetItemHtmlPrefix<Upgrade, UpgradeLevel>(null, m => m.Levels[index]);

            partial.ViewData.TemplateInfo.HtmlFieldPrefix = htmlPrefix;

            return partial as PartialViewResult;
        }

        protected override void Dispose(bool disposing)
        {
            _repo = null;

            base.Dispose(disposing);
        }
    }
}
