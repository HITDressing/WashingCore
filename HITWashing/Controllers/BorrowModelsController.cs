using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HITWashing.Models.DBClass;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HITWashing.Controllers
{
    //[Authorize(Roles = "超级管理员,仓库保管员,财务负责人,配送专员,客户")]
    //[Authorize(Roles = "超级管理员,仓库保管员")]
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

        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        // GET: BorrowModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x=>x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName");
            return View();
        }

        // POST: BorrowModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        public async Task<IActionResult> Create([Bind("BorrowOrderID,ItemNum_1,ItemNum_2,ItemNum_3")] BorrowModel borrowModel)
        {
            borrowModel.IsCanceled = false;
            borrowModel.IsCompleted = false;
            borrowModel.AccountName = User.FindFirst(ClaimTypes.Sid).Value;

            var ware = _context.Warehouses.FirstOrDefault(x => x.Account.Type == Models.EnumClass.EnumAccountType.超级管理员);
            if (ModelState.IsValid)
            {
                if (ware == null)
                {
                    ModelState.AddModelError("ItemNum_1", "库存中未有任何物品信息 请联系管理员");

                }
                else if (borrowModel.ItemNum_1 <= ware.ItemNum_1 && borrowModel.ItemNum_2 <= ware.ItemNum_2 && borrowModel.ItemNum_3 <= ware.ItemNum_3)
                {
                    _context.Add(borrowModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (borrowModel.ItemNum_1 > ware.ItemNum_1) ModelState.AddModelError("ItemNum_1", "库存不足");
                    if (borrowModel.ItemNum_2 > ware.ItemNum_2) ModelState.AddModelError("ItemNum_2", "库存不足");
                    if (borrowModel.ItemNum_3 > ware.ItemNum_3) ModelState.AddModelError("ItemNum_3", "库存不足");
                }
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName", borrowModel.AccountName);
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        // POST: BorrowModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowOrderID,UserID,ItemNum_1,ItemNum_2,ItemNum_3,AccountName,IsCanceled,IsCompleted")] BorrowModel borrowModel)
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName", borrowModel.AccountName);
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
