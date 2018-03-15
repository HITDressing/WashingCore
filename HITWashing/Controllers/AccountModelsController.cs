using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HITWashing.Models.DBClass;

namespace HITWashing.Controllers
{
    public class AccountModelsController : Controller
    {
        private readonly WashingContext _context;

        public AccountModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: AccountModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountModels.ToListAsync());
        }

        // GET: AccountModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels
                .SingleOrDefaultAsync(m => m.AccountID == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // GET: AccountModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,MobileNumber,LocationX,LocationY,Type,Password,Salt,StoreName")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        // GET: AccountModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels.SingleOrDefaultAsync(m => m.AccountID == id);
            if (accountModel == null)
            {
                return NotFound();
            }
            return View(accountModel);
        }

        // POST: AccountModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountID,MobileNumber,LocationX,LocationY,Type,Password,Salt,StoreName")] AccountModel accountModel)
        {
            if (id != accountModel.AccountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountModelExists(accountModel.AccountID))
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
            return View(accountModel);
        }

        // GET: AccountModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels
                .SingleOrDefaultAsync(m => m.AccountID == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // POST: AccountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountModel = await _context.AccountModels.SingleOrDefaultAsync(m => m.AccountID == id);
            _context.AccountModels.Remove(accountModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountModelExists(int id)
        {
            return _context.AccountModels.Any(e => e.AccountID == id);
        }
    }
}
