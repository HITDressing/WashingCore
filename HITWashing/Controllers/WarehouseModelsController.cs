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
    public class WarehouseModelsController : Controller
    {
        private readonly WashingContext _context;

        public WarehouseModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: WarehouseModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.Warehouses.Include(w => w.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: WarehouseModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseModel = await _context.Warehouses
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
        public async Task<IActionResult> Create([Bind("WarehouseID,ItemNum_1,ItemNum_2,ItemNum_3,AccountName")] WarehouseModel warehouseModel)
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

            var warehouseModel = await _context.Warehouses.SingleOrDefaultAsync(m => m.WarehouseID == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("WarehouseID,ItemNum_1,ItemNum_2,ItemNum_3,AccountName")] WarehouseModel warehouseModel)
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

            var warehouseModel = await _context.Warehouses
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
            var warehouseModel = await _context.Warehouses.SingleOrDefaultAsync(m => m.WarehouseID == id);
            _context.Warehouses.Remove(warehouseModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseModelExists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseID == id);
        }
    }
}
