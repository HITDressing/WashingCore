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
    public class PaybackModelsController : Controller
    {
        private readonly WashingContext _context;

        public PaybackModelsController(WashingContext context) => _context = context;

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

        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        // GET: PaybackModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName");
            return View();
        }

        // POST: PaybackModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        public async Task<IActionResult> Create([Bind("PaybackOrderID,ItemNum_1,ItemNum_2,ItemNum_3")] PaybackModel paybackModel)
        {
            paybackModel.IsCanceled = false;
            paybackModel.IsCompleted = false;
            paybackModel.AccountName = User.FindFirst(ClaimTypes.Sid).Value;

            var ware = _context.Warehouses.FirstOrDefault(x => x.AccountName == paybackModel.AccountName);

            if (ModelState.IsValid)
            {
                if (ware==null)
                {
                    ModelState.AddModelError("ItemNum_1", "该用户未有任何库存信息");
                }
                else if (paybackModel.ItemNum_1 <= ware.ItemNum_1 && paybackModel.ItemNum_2 <= ware.ItemNum_2 && paybackModel.ItemNum_3 <= ware.ItemNum_3)
                {
                    _context.Add(paybackModel);

                    ware.ItemNum_1 -= paybackModel.ItemNum_1;
                    ware.ItemNum_2 -= paybackModel.ItemNum_2;
                    ware.ItemNum_3 -= paybackModel.ItemNum_3;

                    _context.Update(ware);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details","AccountModels", paybackModel.AccountName);
                }
                else
                {
                    if (paybackModel.ItemNum_1 > ware.ItemNum_1) ModelState.AddModelError("ItemNum_1", "库存不足");
                    if (paybackModel.ItemNum_2 > ware.ItemNum_2) ModelState.AddModelError("ItemNum_2", "库存不足");
                    if (paybackModel.ItemNum_3 > ware.ItemNum_3) ModelState.AddModelError("ItemNum_3", "库存不足");
                }
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.配送专员), "AccountName", "AccountName", paybackModel.AccountName);
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
        public async Task<IActionResult> Edit(int id, [Bind("PaybackOrderID,UserName,AccountName,ItemNum_1,ItemNum_2,ItemNum_3,IsCanceled,IsCompleted")] PaybackModel paybackModel)
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

        [Authorize(Roles = "配送专员")]
        public async Task<ActionResult> PickOrder(int? id)
        {
            var find = await _context.Paybacks.FindAsync(id);

            if (String.IsNullOrEmpty(find.UserName))
            {
                find.UserName = User.FindFirst(ClaimTypes.Sid).Value;

                try
                {
                    _context.Update(find);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaybackModelExists(find.PaybackOrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("OrderPool", "Home");
        }

        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        public async Task<ActionResult> ConfirmOrder(int? id)
        {
            var find = await _context.Paybacks.FindAsync(id);

            if (!find.IsCanceled && !find.IsCompleted)
            {
                //标记订单状态

                find.IsCompleted = true;

                //总库存增加

                var allWare = await _context.Warehouses.FirstOrDefaultAsync(x => x.Account.Type == Models.EnumClass.EnumAccountType.超级管理员);
                if (allWare == null)
                {
                    return RedirectToAction("OrderCurrent", "Home");
                }
                else
                {
                    allWare.ItemNum_1 += find.ItemNum_1;
                    allWare.ItemNum_2 += find.ItemNum_2;
                    allWare.ItemNum_3 += find.ItemNum_3;
                    _context.Update(allWare);
                }

                //单体库存减少

                var accountWare = await _context.Warehouses.FirstOrDefaultAsync(x => x.AccountName == find.AccountName);

                if (accountWare == null)
                {
                    _context.Add(new WarehouseModel()
                    {
                        AccountName = find.AccountName,
                        ItemNum_1 = -find.ItemNum_1,
                        ItemNum_2 = -find.ItemNum_2,
                        ItemNum_3 = -find.ItemNum_3
                    });
                    //await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                }
                else
                {
                    accountWare.ItemNum_1 -= find.ItemNum_1;
                    accountWare.ItemNum_2 -= find.ItemNum_2;
                    accountWare.ItemNum_3 -= find.ItemNum_3;
                    _context.Update(accountWare);
                }

                ////扣除单体资金

                //var accountBalance = await _context.Balances.FirstOrDefaultAsync(x => x.AccountName == find.AccountName);

                //var item = await _context.Items.ToListAsync();

                //var tempBalance = find.ItemNum_1 * item[0].ItemValue + find.ItemNum_2 * item[1].ItemValue + find.ItemNum_3 * item[2].ItemValue;

                //if (accountBalance == null)
                //{
                //    _context.Add(new BalanceModel()
                //    {
                //        AccountName = find.AccountName,
                //        Balance = -tempBalance
                //    });
                //}
                //else
                //{
                //    accountBalance.Balance -= tempBalance;
                //}

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return RedirectToAction("OrderCurrent", "Home");
        }

        [Authorize(Roles = "超级管理员,仓库保管员,客户")]
        public async Task<ActionResult> CancelOrder(int? id)
        {
            var find = await _context.Paybacks.FindAsync(id);
            find.IsCanceled = true;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(find);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("OrderCurrent", "Home");
        }
    }
}
