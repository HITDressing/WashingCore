using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BSXWashing.Models.DBClass;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BSXWashing.Controllers
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
            var washingContext = _context.PaybackModels.Include(p => p.Account);
            return View(await washingContext.ToListAsync());
        }

        // GET: PaybackModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.PaybackModels
                .Include(p => p.Account)
                .SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            if (paybackModel == null)
            {
                return NotFound();
            }

            return View(paybackModel);
        }

        // GET: PaybackModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.PaybackModels.SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            if (paybackModel == null)
            {
                return NotFound();
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x=>x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        // POST: PaybackModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaybackOrderID,TranName,AccountName,StartTime,TranTime,FinishTime,IsCanceled,IsCompleted,IsTraned,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] PaybackModel paybackModel)
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        // GET: PaybackModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paybackModel = await _context.PaybackModels
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
            var paybackModel = await _context.PaybackModels.SingleOrDefaultAsync(m => m.PaybackOrderID == id);
            _context.PaybackModels.Remove(paybackModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaybackModelExists(int id)
        {
            return _context.PaybackModels.Any(e => e.PaybackOrderID == id);
        }

        private bool PaybackCheckZero(PaybackModel paybackModel)
        {
            return paybackModel.ItemNum1 == 0 && paybackModel.ItemNum2 == 0 && paybackModel.ItemNum3 == 0 && paybackModel.ItemNum4 == 0 &&
                paybackModel.ItemNum5 == 0 && paybackModel.ItemNum6 == 0 && paybackModel.ItemNum7 == 0 && paybackModel.ItemNum8 == 0 &&
                paybackModel.ItemNum9 == 0 && paybackModel.ItemNum10 == 0 && paybackModel.ItemNum11 == 0 && paybackModel.ItemNum12 == 0 &&
                paybackModel.ItemNum13 == 0 && paybackModel.ItemNum14 == 0 && paybackModel.ItemNum15 == 0 && paybackModel.ItemNum16 == 0 &&
                paybackModel.ItemNum17 == 0 && paybackModel.ItemNum18 == 0 && paybackModel.ItemNum19 == 0 && paybackModel.ItemNum20 == 0 &&
                paybackModel.ItemNum21 == 0 && paybackModel.ItemNum22 == 0 && paybackModel.ItemNum23 == 0 && paybackModel.ItemNum24 == 0 &&
                paybackModel.ItemNum25 == 0 && paybackModel.ItemNum26 == 0 && paybackModel.ItemNum27 == 0 && paybackModel.ItemNum28 == 0 &&
                paybackModel.ItemNum29 == 0 && paybackModel.ItemNum30 == 0 && paybackModel.ItemNum31 == 0 && paybackModel.ItemNum32 == 0 &&
                paybackModel.ItemNum33 == 0 && paybackModel.ItemNum34 == 0 && paybackModel.ItemNum35 == 0 && paybackModel.ItemNum36 == 0 &&
                paybackModel.ItemNum37 == 0 && paybackModel.ItemNum38 == 0 && paybackModel.ItemNum39 == 0 && paybackModel.ItemNum40 == 0;
        }

        private void WareReduce(WarehouseModel ware, PaybackModel paybackModel)
        {
            //库存减少
            ware.ItemNum1 -= paybackModel.ItemNum1;
            ware.ItemNum2 -= paybackModel.ItemNum2;
            ware.ItemNum3 -= paybackModel.ItemNum3;
            ware.ItemNum4 -= paybackModel.ItemNum4;
            ware.ItemNum5 -= paybackModel.ItemNum5;
            ware.ItemNum6 -= paybackModel.ItemNum6;
            ware.ItemNum7 -= paybackModel.ItemNum7;
            ware.ItemNum8 -= paybackModel.ItemNum8;
            ware.ItemNum9 -= paybackModel.ItemNum9;
            ware.ItemNum10 -= paybackModel.ItemNum10;
            ware.ItemNum11 -= paybackModel.ItemNum11;
            ware.ItemNum12 -= paybackModel.ItemNum12;
            ware.ItemNum13 -= paybackModel.ItemNum13;
            ware.ItemNum14 -= paybackModel.ItemNum14;
            ware.ItemNum15 -= paybackModel.ItemNum15;
            ware.ItemNum16 -= paybackModel.ItemNum16;
            ware.ItemNum17 -= paybackModel.ItemNum17;
            ware.ItemNum18 -= paybackModel.ItemNum18;
            ware.ItemNum19 -= paybackModel.ItemNum19;
            ware.ItemNum20 -= paybackModel.ItemNum20;
            //------------------------------------//
            ware.ItemNum21 -= paybackModel.ItemNum21;
            ware.ItemNum22 -= paybackModel.ItemNum22;
            ware.ItemNum23 -= paybackModel.ItemNum23;
            ware.ItemNum24 -= paybackModel.ItemNum24;
            ware.ItemNum25 -= paybackModel.ItemNum25;
            ware.ItemNum26 -= paybackModel.ItemNum26;
            ware.ItemNum27 -= paybackModel.ItemNum27;
            ware.ItemNum28 -= paybackModel.ItemNum28;
            ware.ItemNum29 -= paybackModel.ItemNum29;
            ware.ItemNum30 -= paybackModel.ItemNum30;
            ware.ItemNum31 -= paybackModel.ItemNum31;
            ware.ItemNum32 -= paybackModel.ItemNum32;
            ware.ItemNum33 -= paybackModel.ItemNum33;
            ware.ItemNum34 -= paybackModel.ItemNum34;
            ware.ItemNum35 -= paybackModel.ItemNum35;
            ware.ItemNum36 -= paybackModel.ItemNum36;
            ware.ItemNum37 -= paybackModel.ItemNum37;
            ware.ItemNum38 -= paybackModel.ItemNum38;
            ware.ItemNum39 -= paybackModel.ItemNum39;
            ware.ItemNum40 -= paybackModel.ItemNum40;

            _context.Update(ware);
        }

        private void WareTopup(WarehouseModel ware, PaybackModel paybackModel)
        {
            //库存减少
            ware.ItemNum1 += paybackModel.ItemNum1;
            ware.ItemNum2 += paybackModel.ItemNum2;
            ware.ItemNum3 += paybackModel.ItemNum3;
            ware.ItemNum4 += paybackModel.ItemNum4;
            ware.ItemNum5 += paybackModel.ItemNum5;
            ware.ItemNum6 += paybackModel.ItemNum6;
            ware.ItemNum7 += paybackModel.ItemNum7;
            ware.ItemNum8 += paybackModel.ItemNum8;
            ware.ItemNum9 += paybackModel.ItemNum9;
            ware.ItemNum10 += paybackModel.ItemNum10;
            ware.ItemNum11 += paybackModel.ItemNum11;
            ware.ItemNum12 += paybackModel.ItemNum12;
            ware.ItemNum13 += paybackModel.ItemNum13;
            ware.ItemNum14 += paybackModel.ItemNum14;
            ware.ItemNum15 += paybackModel.ItemNum15;
            ware.ItemNum16 += paybackModel.ItemNum16;
            ware.ItemNum17 += paybackModel.ItemNum17;
            ware.ItemNum18 += paybackModel.ItemNum18;
            ware.ItemNum19 += paybackModel.ItemNum19;
            ware.ItemNum20 += paybackModel.ItemNum20;
            //------------------------------------//
            ware.ItemNum21 += paybackModel.ItemNum21;
            ware.ItemNum22 += paybackModel.ItemNum22;
            ware.ItemNum23 += paybackModel.ItemNum23;
            ware.ItemNum24 += paybackModel.ItemNum24;
            ware.ItemNum25 += paybackModel.ItemNum25;
            ware.ItemNum26 += paybackModel.ItemNum26;
            ware.ItemNum27 += paybackModel.ItemNum27;
            ware.ItemNum28 += paybackModel.ItemNum28;
            ware.ItemNum29 += paybackModel.ItemNum29;
            ware.ItemNum30 += paybackModel.ItemNum30;
            ware.ItemNum31 += paybackModel.ItemNum31;
            ware.ItemNum32 += paybackModel.ItemNum32;
            ware.ItemNum33 += paybackModel.ItemNum33;
            ware.ItemNum34 += paybackModel.ItemNum34;
            ware.ItemNum35 += paybackModel.ItemNum35;
            ware.ItemNum36 += paybackModel.ItemNum36;
            ware.ItemNum37 += paybackModel.ItemNum37;
            ware.ItemNum38 += paybackModel.ItemNum38;
            ware.ItemNum39 += paybackModel.ItemNum39;
            ware.ItemNum40 += paybackModel.ItemNum40;

            _context.Update(ware);
        }


        //--------------------------------------------------------------------------------------

        [Authorize(Roles = "配送专员")]
        // GET: PaybackModels/Create
        public IActionResult Create()
        {
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName");
            return View();
        }
        
        // POST: PaybackModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "配送专员")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaybackOrderID,TranName,AccountName,StartTime,TranTime,FinishTime,IsCanceled,IsCompleted,IsTraned,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] PaybackModel paybackModel)
        {
            if (PaybackCheckZero(paybackModel))
            {
                ModelState.AddModelError("BorrowNote", "订单不能全部为0");
            }

            if (ModelState.IsValid)
            {
                var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.AccountName == paybackModel.AccountName);

                if (ware == null)
                {
                    ModelState.AddModelError("BorrowNote", "该用户库存尚未初始化");
                }
                else if (paybackModel.ItemNum1 > ware.ItemNum1)
                {
                    ModelState.AddModelError("ItemNum1", "库存不足");
                }
                else if (paybackModel.ItemNum2 > ware.ItemNum2)
                {
                    ModelState.AddModelError("ItemNum2", "库存不足");
                }
                else if (paybackModel.ItemNum3 > ware.ItemNum3)
                {
                    ModelState.AddModelError("ItemNum3", "库存不足");
                }
                else if (paybackModel.ItemNum4 > ware.ItemNum4)
                {
                    ModelState.AddModelError("ItemNum4", "库存不足");
                }
                else if (paybackModel.ItemNum5 > ware.ItemNum5)
                {
                    ModelState.AddModelError("ItemNum5", "库存不足");
                }
                else if (paybackModel.ItemNum6 > ware.ItemNum6)
                {
                    ModelState.AddModelError("ItemNum6", "库存不足");
                }
                else if (paybackModel.ItemNum7 > ware.ItemNum7)
                {
                    ModelState.AddModelError("ItemNum7", "库存不足");
                }
                else if (paybackModel.ItemNum8 > ware.ItemNum8)
                {
                    ModelState.AddModelError("ItemNum8", "库存不足");
                }
                else if (paybackModel.ItemNum9 > ware.ItemNum9)
                {
                    ModelState.AddModelError("ItemNum9", "库存不足");
                }
                else if (paybackModel.ItemNum10 > ware.ItemNum10)
                {
                    ModelState.AddModelError("ItemNum10", "库存不足");
                }
                //----------------------10----------------------------
                else if (paybackModel.ItemNum11 > ware.ItemNum11)
                {
                    ModelState.AddModelError("ItemNum11", "库存不足");
                }
                else if (paybackModel.ItemNum12 > ware.ItemNum12)
                {
                    ModelState.AddModelError("ItemNum12", "库存不足");
                }
                else if (paybackModel.ItemNum13 > ware.ItemNum13)
                {
                    ModelState.AddModelError("ItemNum13", "库存不足");
                }
                else if (paybackModel.ItemNum14 > ware.ItemNum14)
                {
                    ModelState.AddModelError("ItemNum14", "库存不足");
                }
                else if (paybackModel.ItemNum15 > ware.ItemNum15)
                {
                    ModelState.AddModelError("ItemNum15", "库存不足");
                }
                else if (paybackModel.ItemNum16 > ware.ItemNum16)
                {
                    ModelState.AddModelError("ItemNum16", "库存不足");
                }
                else if (paybackModel.ItemNum17 > ware.ItemNum17)
                {
                    ModelState.AddModelError("ItemNum17", "库存不足");
                }
                else if (paybackModel.ItemNum18 > ware.ItemNum18)
                {
                    ModelState.AddModelError("ItemNum18", "库存不足");
                }
                else if (paybackModel.ItemNum19 > ware.ItemNum19)
                {
                    ModelState.AddModelError("ItemNum19", "库存不足");
                }
                else if (paybackModel.ItemNum20 > ware.ItemNum20)
                {
                    ModelState.AddModelError("ItemNum20", "库存不足");
                }
                //---------------------------20------------------------
                else if (paybackModel.ItemNum21 > ware.ItemNum21)
                {
                    ModelState.AddModelError("ItemNum21", "库存不足");
                }
                else if (paybackModel.ItemNum22 > ware.ItemNum22)
                {
                    ModelState.AddModelError("ItemNum22", "库存不足");
                }
                else if (paybackModel.ItemNum23 > ware.ItemNum23)
                {
                    ModelState.AddModelError("ItemNum23", "库存不足");
                }
                else if (paybackModel.ItemNum24 > ware.ItemNum24)
                {
                    ModelState.AddModelError("ItemNum24", "库存不足");
                }
                else if (paybackModel.ItemNum25 > ware.ItemNum25)
                {
                    ModelState.AddModelError("ItemNum25", "库存不足");
                }
                else if (paybackModel.ItemNum26 > ware.ItemNum26)
                {
                    ModelState.AddModelError("ItemNum26", "库存不足");
                }
                else if (paybackModel.ItemNum27 > ware.ItemNum27)
                {
                    ModelState.AddModelError("ItemNum27", "库存不足");
                }
                else if (paybackModel.ItemNum28 > ware.ItemNum28)
                {
                    ModelState.AddModelError("ItemNum28", "库存不足");
                }
                else if (paybackModel.ItemNum29 > ware.ItemNum29)
                {
                    ModelState.AddModelError("ItemNum29", "库存不足");
                }
                else if (paybackModel.ItemNum30 > ware.ItemNum30)
                {
                    ModelState.AddModelError("ItemNum30", "库存不足");
                }
                //---------------------------30------------------------
                else if (paybackModel.ItemNum31 > ware.ItemNum31)
                {
                    ModelState.AddModelError("ItemNum31", "库存不足");
                }
                else if (paybackModel.ItemNum32 > ware.ItemNum32)
                {
                    ModelState.AddModelError("ItemNum32", "库存不足");
                }
                else if (paybackModel.ItemNum33 > ware.ItemNum33)
                {
                    ModelState.AddModelError("ItemNum33", "库存不足");
                }
                else if (paybackModel.ItemNum34 > ware.ItemNum34)
                {
                    ModelState.AddModelError("ItemNum34", "库存不足");
                }
                else if (paybackModel.ItemNum35 > ware.ItemNum35)
                {
                    ModelState.AddModelError("ItemNum35", "库存不足");
                }
                else if (paybackModel.ItemNum36 > ware.ItemNum36)
                {
                    ModelState.AddModelError("ItemNum36", "库存不足");
                }
                else if (paybackModel.ItemNum37 > ware.ItemNum37)
                {
                    ModelState.AddModelError("ItemNum37", "库存不足");
                }
                else if (paybackModel.ItemNum38 > ware.ItemNum38)
                {
                    ModelState.AddModelError("ItemNum38", "库存不足");
                }
                else if (paybackModel.ItemNum39 > ware.ItemNum39)
                {
                    ModelState.AddModelError("ItemNum39", "库存不足");
                }
                else if (paybackModel.ItemNum40 > ware.ItemNum40)
                {
                    ModelState.AddModelError("ItemNum40", "库存不足");
                }
                else
                {
                    WareReduce(ware, paybackModel);

                    _context.Add(paybackModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TranOrderCurrent", "Home", new { id = paybackModel.AccountName });
                }
            }
            ViewData["AccountName"] = new SelectList(_context.AccountModels.Where(x => x.Type == Models.EnumClass.EnumAccountType.客户), "AccountName", "AccountName", paybackModel.AccountName);
            return View(paybackModel);
        }

        [Authorize(Roles = "超级管理员,仓库保管员")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var paybackOrder = await _context.PaybackModels.FindAsync(id);
            var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.AccountName == paybackOrder.AccountName);

            paybackOrder.IsCanceled = true;
            paybackOrder.FinishTime = DateTime.Now;

            WareTopup(ware, paybackOrder);

            try
            {
                _context.Update(paybackOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return RedirectToAction("Order", "Home");
        }

        [Authorize(Roles = "超级管理员,客户,仓库保管员")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var paybackOrder = _context.PaybackModels.Find(id);

            paybackOrder.FinishTime = DateTime.Now;
            paybackOrder.IsCompleted = true;

            var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.Account.Type == Models.EnumClass.EnumAccountType.仓库保管员);

            WareTopup(ware, paybackOrder);

            try
            {
                _context.Update(paybackOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            return RedirectToAction("Order", "Home");

        }

        [Authorize(Roles = "超级管理员,配送专员,仓库保管员")]
        public async Task<IActionResult> TranedOrder(int id)
        {
            var paybackOrder = _context.PaybackModels.Find(id);
            paybackOrder.TranTime = DateTime.Now;
            paybackOrder.IsTraned = true;

            try
            {
                _context.Update(paybackOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return RedirectToAction("TranOrderCurrent", "Home");
        }
    }
}
