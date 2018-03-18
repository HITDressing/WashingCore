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
    public class BalanceModelsController : Controller
    {
        private readonly WashingContext _context;

        public BalanceModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: BalanceModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.Balances.Include(b => b.Account);
            return View(await washingContext.ToListAsync());
        }

        public IActionResult Topup(string id)
        {
            var name = _context.Balances.Select(x => x.Account.AccountName);
            var list = _context.AccountModels.Where(x => name.Contains(x.AccountName));
            ViewData["AccountName"] = new SelectList(list, "AccountName", "AccountName", id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Topup([Bind("Balance,AccountName")] BalanceModel balanceModel)
        {
            if (balanceModel.Balance > 0 && ModelState.IsValid)
            {
                try
                {
                    var find = await _context.Balances.FirstOrDefaultAsync(x => x.AccountName == balanceModel.AccountName);
                    find.Balance += balanceModel.Balance;
                    _context.Update(find);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BalanceModelExists(balanceModel.BalanceID))
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
            ModelState.AddModelError("Balance", "充值金额不能为负");
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", balanceModel.AccountName);
            return View(balanceModel);
        }

        // GET: BalanceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var balanceModel = await _context.Balances
                .Include(b => b.Account)
                .SingleOrDefaultAsync(m => m.BalanceID == id);
            if (balanceModel == null)
            {
                return NotFound();
            }

            return View(balanceModel);
        }

        // GET: BalanceModels/Create
        public IActionResult Create()
        {
            var name = _context.Balances.Select(x => x.AccountName);
            var list = _context.AccountModels.Where(x => !name.Contains(x.AccountName));
            ViewData["AccountName"] = new SelectList(list, "AccountName", "AccountName");
            return View();
        }

        // POST: BalanceModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BalanceID,Balance,AccountName")] BalanceModel balanceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(balanceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", balanceModel.AccountName);
            return View(balanceModel);
        }

        // GET: BalanceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var balanceModel = await _context.Balances.SingleOrDefaultAsync(m => m.BalanceID == id);
            if (balanceModel == null)
            {
                return NotFound();
            }
            var name = _context.Balances.Select(x => x.AccountName);
            var list = _context.AccountModels.Where(x => name.Contains(x.AccountName));
            ViewData["AccountName"] = new SelectList(list, "AccountName", "AccountName", balanceModel.AccountName);
            return View(balanceModel);
        }

        // POST: BalanceModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BalanceID,Balance,AccountName")] BalanceModel balanceModel)
        {
            if (id != balanceModel.BalanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(balanceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BalanceModelExists(balanceModel.BalanceID))
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
            var name = _context.Balances.Select(x => x.AccountName);
            var list = _context.AccountModels.Where(x => name.Contains(x.AccountName));
            ViewData["AccountName"] = new SelectList(list, "AccountName", "AccountName", balanceModel.AccountName);
            return View(balanceModel);
        }

        // GET: BalanceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var balanceModel = await _context.Balances
                .Include(b => b.Account)
                .SingleOrDefaultAsync(m => m.BalanceID == id);
            if (balanceModel == null)
            {
                return NotFound();
            }

            return View(balanceModel);
        }

        // POST: BalanceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var balanceModel = await _context.Balances.SingleOrDefaultAsync(m => m.BalanceID == id);
            _context.Balances.Remove(balanceModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BalanceModelExists(int id)
        {
            return _context.Balances.Any(e => e.BalanceID == id);
        }
    }
}
