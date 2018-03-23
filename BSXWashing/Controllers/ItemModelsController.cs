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
    public class ItemModelsController : Controller
    {
        private readonly WashingContext _context;

        public ItemModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: ItemModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemModels.ToListAsync());
        }

        // GET: ItemModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels
                .SingleOrDefaultAsync(m => m.ItemName == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        // GET: ItemModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemName,ItemValue,ItemNote")] ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemModel);
        }

        // GET: ItemModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels.SingleOrDefaultAsync(m => m.ItemName == id);
            if (itemModel == null)
            {
                return NotFound();
            }
            return View(itemModel);
        }

        // POST: ItemModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ItemName,ItemValue,ItemNote")] ItemModel itemModel)
        {
            if (id != itemModel.ItemName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemModelExists(itemModel.ItemName))
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
            return View(itemModel);
        }

        // GET: ItemModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels
                .SingleOrDefaultAsync(m => m.ItemName == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        // POST: ItemModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var itemModel = await _context.ItemModels.SingleOrDefaultAsync(m => m.ItemName == id);
            _context.ItemModels.Remove(itemModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemModelExists(string id)
        {
            return _context.ItemModels.Any(e => e.ItemName == id);
        }
    }
}
