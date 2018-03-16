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
    public class PaybackModelsController : Controller
    {
        private readonly WashingContext _context;

        public PaybackModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: PaybackModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.Paybacks.Include(p => p.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: PaybackModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.Paybacks
                .Include(p => p.Account)
                .SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            if (paybackModel == null)
            {
                return NotFound();
            }

            return View(paybackModel);
        }

        // GET: PaybackModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName");
            return View();
        }

        // POST: PaybackModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaybackOrderID,UserID,AccountName,IsCanceled,IsCompleted")] PaybackModel paybackModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paybackModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        // GET: PaybackModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.Paybacks.SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            if (paybackModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        // POST: PaybackModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaybackOrderID,UserID,AccountName,IsCanceled,IsCompleted")] PaybackModel paybackModel)
        {
            if (id != paybackModel.PaybackOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paybackModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaybackModelExists(paybackModel.PaybackOrderID))
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        // GET: PaybackModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.Paybacks
                .Include(p => p.Account)
                .SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            if (paybackModel == null)
            {
                return NotFound();
            }

            return View(paybackModel);
        }

        // POST: PaybackModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paybackModel = await _context.Paybacks.SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            _context.Paybacks.Remove(paybackModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaybackModelExists(int id)
        {
            return _context.Paybacks.Any(e => e.PaybackOrderID == id);
        }
    }
}
