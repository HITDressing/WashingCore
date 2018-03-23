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
    public class DiscountModelsController : Controller
    {
        private readonly WashingContext _context;

        public DiscountModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: DiscountModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.DiscountModels.Include(d => d.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: DiscountModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountModel = await _context.DiscountModels
                .Include(d => d.Account)
                .SingleOrDefaultAsync(m => m.DiscountID == id);
            if (discountModel == null)
            {
                return NotFound();
            }

            return View(discountModel);
        }

        // GET: DiscountModels/Create
        public IActionResult Create(string id)
        {
            var list = _context.DiscountModels.Select(x => x.AccountName);
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x=>x.Type == Models.EnumClass.EnumAccountType.客户 && !list.Contains(x.AccountName)), "AccountName", "AccountName", id);
            return View();
        }

        // POST: DiscountModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountID,DiscountValue,AccountName,DiscountNote")] DiscountModel discountModel)
        {
            if (ModelState.IsValid)
            {
                if(await _context.DiscountModels.AnyAsync(x=>x.AccountName == discountModel.AccountName))
                {
                    ModelState.AddModelError("AccountName", "该用户已经填写过折扣");
                }
                else
                {
                    _context.Add(discountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x=>x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", discountModel.AccountName);
            return View(discountModel);
        }

        // GET: DiscountModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountModel = await _context.DiscountModels.SingleOrDefaultAsync(m => m.DiscountID == id);
            if (discountModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", discountModel.AccountName);
            return View(discountModel);
        }

        // POST: DiscountModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountID,DiscountValue,AccountName,DiscountNote")] DiscountModel discountModel)
        {
            if (id != discountModel.DiscountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountModelExists(discountModel.DiscountID))
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", discountModel.AccountName);
            return View(discountModel);
        }

        // GET: DiscountModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountModel = await _context.DiscountModels
                .Include(d => d.Account)
                .SingleOrDefaultAsync(m => m.DiscountID == id);
            if (discountModel == null)
            {
                return NotFound();
            }

            return View(discountModel);
        }

        // POST: DiscountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountModel = await _context.DiscountModels.SingleOrDefaultAsync(m => m.DiscountID == id);
            _context.DiscountModels.Remove(discountModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountModelExists(int id)
        {
            return _context.DiscountModels.Any(e => e.DiscountID == id);
        }
    }
}
