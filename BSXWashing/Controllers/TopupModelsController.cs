using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BSXWashing.Models.DBClass;

namespace BSXWashing.Controllers
{
    public class TopupModelsController : Controller
    {
        private readonly WashingContext _context;

        public TopupModelsController(WashingContext context) => _context = context;

        // GET: TopupModels
        public async Task<IActionResult> Index(string id)
        {
            var washingContext = _context.TopupModels.Include(t => t.Account);

            if (!String.IsNullOrEmpty(id))
            {
                ViewData["Flag"] = "OK";
                return View(await washingContext.Where(x => x.AccountName == id).ToListAsync());
            }
            else
            {
                ViewData["Flag"] = "NOTOK";
                return View(await washingContext.ToListAsync());
            }
        }

        // GET: TopupModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topupModel = await _context.TopupModels
                .Include(t => t.Account)
                .SingleOrDefaultAsync(m => m.TopupID == id);
            if (topupModel == null)
            {
                return NotFound();
            }

            return View(topupModel);
        }

        // GET: TopupModels/Create
        public IActionResult Create(string id)
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", id);
            return View();
        }

        // POST: TopupModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopupID,TopupValue,TopupNote,AccountName")] TopupModel topupModel)
        {
            topupModel.TopupTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                var account = await _context.AccountModels.FindAsync(topupModel.AccountName);
                account.Balance += topupModel.TopupValue;

                _context.Update(account);
                _context.Add(topupModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Balance", "AccountModels");
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", topupModel.AccountName);
            return View(topupModel);
        }

        // GET: TopupModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topupModel = await _context.TopupModels.SingleOrDefaultAsync(m => m.TopupID == id);
            if (topupModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", topupModel.AccountName);
            return View(topupModel);
        }

        // POST: TopupModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopupID,TopupValue,TopupTime,TopupNote,AccountName")] TopupModel topupModel)
        {
            if (id != topupModel.TopupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topupModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopupModelExists(topupModel.TopupID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", topupModel.AccountName);
            return View(topupModel);
        }

        // GET: TopupModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topupModel = await _context.TopupModels
                .Include(t => t.Account)
                .SingleOrDefaultAsync(m => m.TopupID == id);
            if (topupModel == null)
            {
                return NotFound();
            }

            return View(topupModel);
        }

        // POST: TopupModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topupModel = await _context.TopupModels.SingleOrDefaultAsync(m => m.TopupID == id);
            _context.TopupModels.Remove(topupModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopupModelExists(int id)
        {
            return _context.TopupModels.Any(e => e.TopupID == id);
        }
    }
}
