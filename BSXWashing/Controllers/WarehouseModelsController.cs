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
    public class WarehouseModelsController : Controller
    {
        private readonly WashingContext _context;

        public WarehouseModelsController(WashingContext context) => _context = context;

        // GET: WarehouseModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.WarehouseModels.Include(w => w.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: WarehouseModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseModel = await _context.WarehouseModels
                .Include(w => w.Account)
                .SingleOrDefaultAsync(m => m.WarehouseID == id);
            if (warehouseModel == null)
            {
                return NotFound();
            }

            return View(warehouseModel);
        }

        // GET: WarehouseModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName");
            return View();
        }

        // POST: WarehouseModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarehouseID,AccountName,WareNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] WarehouseModel warehouseModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", warehouseModel.AccountName);
            return View(warehouseModel);
        }

        // GET: WarehouseModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseModel = await _context.WarehouseModels.SingleOrDefaultAsync(m => m.WarehouseID == id);
            if (warehouseModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", warehouseModel.AccountName);
            return View(warehouseModel);
        }

        // POST: WarehouseModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WarehouseID,AccountName,WareNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] WarehouseModel warehouseModel)
        {
            if (id != warehouseModel.WarehouseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseModelExists(warehouseModel.WarehouseID))
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", warehouseModel.AccountName);
            return View(warehouseModel);
        }

        // GET: WarehouseModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseModel = await _context.WarehouseModels
                .Include(w => w.Account)
                .SingleOrDefaultAsync(m => m.WarehouseID == id);
            if (warehouseModel == null)
            {
                return NotFound();
            }

            return View(warehouseModel);
        }

        // POST: WarehouseModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouseModel = await _context.WarehouseModels.SingleOrDefaultAsync(m => m.WarehouseID == id);
            _context.WarehouseModels.Remove(warehouseModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseModelExists(int id)
        {
            return _context.WarehouseModels.Any(e => e.WarehouseID == id);
        }
    }
}
