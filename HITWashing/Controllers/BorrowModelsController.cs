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
    public class BorrowModelsController : Controller
    {
        private readonly WashingContext _context;

        public BorrowModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: BorrowModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.Borrows.Include(b => b.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: BorrowModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowModel = await _context.Borrows
                .Include(b => b.Account)
                .SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            if (borrowModel == null)
            {
                return NotFound();
            }

            return View(borrowModel);
        }

        // GET: BorrowModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName");
            return View();
        }

        // POST: BorrowModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowOrderID,UserID,AccountName,IsCanceled,IsCompleted")] BorrowModel borrowModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        // GET: BorrowModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowModel = await _context.Borrows.SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            if (borrowModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        // POST: BorrowModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowOrderID,UserID,AccountName,IsCanceled,IsCompleted")] BorrowModel borrowModel)
        {
            if (id != borrowModel.BorrowOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowModelExists(borrowModel.BorrowOrderID))
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        // GET: BorrowModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowModel = await _context.Borrows
                .Include(b => b.Account)
                .SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            if (borrowModel == null)
            {
                return NotFound();
            }

            return View(borrowModel);
        }

        // POST: BorrowModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowModel = await _context.Borrows.SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            _context.Borrows.Remove(borrowModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowModelExists(int id)
        {
            return _context.Borrows.Any(e => e.BorrowOrderID == id);
        }
    }
}
