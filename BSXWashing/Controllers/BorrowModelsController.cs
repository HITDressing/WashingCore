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
using BSXWashing.Models.ViewModel;

namespace BSXWashing.Controllers
{
    public class BorrowModelsController : Controller
    {
        private readonly WashingContext _context;

        public BorrowModelsController(WashingContext context)
        {
            _context = context;
        }

        // GET: BorrowModels
        public async Task<IActionResult> Index(string start, string end, string name)
        {

            var washingContext = _context.BorrowModels.Include(b => b.Account).AsQueryable();
            //.Include(b => b.Account).ToListAsync();

            if (!string.IsNullOrEmpty(name))
            {
                washingContext = washingContext.Where(x => x.AccountName.Contains(name));
            }

            if (!string.IsNullOrEmpty(start))
            {
                washingContext = washingContext.Where(x => x.StartTime >= DateTime.Parse(start));
            }

            if (!string.IsNullOrEmpty(end))
            {
                washingContext = washingContext.Where(x => x.StartTime <= DateTime.Parse(end));
            }

            return View(await washingContext.ToListAsync());
        }

        // GET: BorrowModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowModel = await _context.BorrowModels
                .Include(b => b.Account)
                .Include(b => b.Account.Discounts)
                .SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            if (borrowModel == null)
            {
                return NotFound();
            }

            var discount = borrowModel.Account.Discounts.FirstOrDefault() == null
                ? 1.0 : borrowModel.Account.Discounts.FirstOrDefault().DiscountValue;

            ViewData["ItemModels"] = await MyGetRealItemsAsync(discount, borrowModel);
            return View(borrowModel);
        }

        // GET: BorrowModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowModel = await _context.BorrowModels.SingleOrDefaultAsync(m => m.BorrowOrderID == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("BorrowOrderID,TranName,AccountName,StartTime,TranTime,FinishTime,IsCanceled,IsCompleted,IsTraned,OrderMoney,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] BorrowModel borrowModel)
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
                return RedirectToAction("OrderPool", "Home");
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

            var borrowModel = await _context.BorrowModels
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
            var borrowModel = await _context.BorrowModels.SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            _context.BorrowModels.Remove(borrowModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowModelExists(int id)
        {
            return _context.BorrowModels.Any(e => e.BorrowOrderID == id);
        }

        private bool BorrowCheckZero(BorrowModel borrowModel)
        {
            return borrowModel.ItemNum1 == 0 && borrowModel.ItemNum2 == 0 && borrowModel.ItemNum3 == 0 && borrowModel.ItemNum4 == 0 &&
                borrowModel.ItemNum5 == 0 && borrowModel.ItemNum6 == 0 && borrowModel.ItemNum7 == 0 && borrowModel.ItemNum8 == 0 &&
                borrowModel.ItemNum9 == 0 && borrowModel.ItemNum10 == 0 && borrowModel.ItemNum11 == 0 && borrowModel.ItemNum12 == 0 &&
                borrowModel.ItemNum13 == 0 && borrowModel.ItemNum14 == 0 && borrowModel.ItemNum15 == 0 && borrowModel.ItemNum16 == 0 &&
                borrowModel.ItemNum17 == 0 && borrowModel.ItemNum18 == 0 && borrowModel.ItemNum19 == 0 && borrowModel.ItemNum20 == 0 &&
                borrowModel.ItemNum21 == 0 && borrowModel.ItemNum22 == 0 && borrowModel.ItemNum23 == 0 && borrowModel.ItemNum24 == 0 &&
                borrowModel.ItemNum25 == 0 && borrowModel.ItemNum26 == 0 && borrowModel.ItemNum27 == 0 && borrowModel.ItemNum28 == 0 &&
                borrowModel.ItemNum29 == 0 && borrowModel.ItemNum30 == 0 && borrowModel.ItemNum31 == 0 && borrowModel.ItemNum32 == 0 &&
                borrowModel.ItemNum33 == 0 && borrowModel.ItemNum34 == 0 && borrowModel.ItemNum35 == 0 && borrowModel.ItemNum36 == 0 &&
                borrowModel.ItemNum37 == 0 && borrowModel.ItemNum38 == 0 && borrowModel.ItemNum39 == 0 && borrowModel.ItemNum40 == 0;
        }

        //按照物品名字计算
        private async Task<double> ComputeTempBalance(BorrowModel borrowModel)
        {
            var item = _context.ItemModels;

            return borrowModel.ItemNum1 * (await item.FindAsync("床单 1.2M")).ItemValue 
                + borrowModel.ItemNum2 * (await item.FindAsync("床单 1.5M")).ItemValue
                + borrowModel.ItemNum3 * (await item.FindAsync("床单 1.8M")).ItemValue
                + borrowModel.ItemNum4 * (await item.FindAsync("被套 1.2M")).ItemValue
                + borrowModel.ItemNum5 * (await item.FindAsync("被套 1.5M")).ItemValue
                + borrowModel.ItemNum6 * (await item.FindAsync("被套 1.8M")).ItemValue
                + borrowModel.ItemNum7 * (await item.FindAsync("枕套")).ItemValue
                + borrowModel.ItemNum8 * (await item.FindAsync("浴巾")).ItemValue
                + borrowModel.ItemNum9 * (await item.FindAsync("地巾")).ItemValue
                + borrowModel.ItemNum10 * (await item.FindAsync("毛巾")).ItemValue

                + borrowModel.ItemNum11 * (await item.FindAsync("方巾")).ItemValue
                + borrowModel.ItemNum12 * (await item.FindAsync("大台布")).ItemValue
                + borrowModel.ItemNum13 *  (await item.FindAsync("香巾")).ItemValue
                + borrowModel.ItemNum14 *  (await item.FindAsync("小台布")).ItemValue
                + borrowModel.ItemNum15 *  (await item.FindAsync("口布")).ItemValue
                + borrowModel.ItemNum16 *  (await item.FindAsync("毛毯")).ItemValue
                + borrowModel.ItemNum17 *  (await item.FindAsync("桌围裙")).ItemValue
                + borrowModel.ItemNum18 *  (await item.FindAsync("厨衣")).ItemValue
                + borrowModel.ItemNum19 *  (await item.FindAsync("窗帘")).ItemValue
                + borrowModel.ItemNum20 *  (await item.FindAsync("窗帘内胆")).ItemValue

                + borrowModel.ItemNum21 *  (await item.FindAsync("浴帘")).ItemValue
                + borrowModel.ItemNum22 *  (await item.FindAsync("浴服")).ItemValue
                + borrowModel.ItemNum23 *  (await item.FindAsync("椅套")).ItemValue
                + borrowModel.ItemNum24 *  (await item.FindAsync("帽子")).ItemValue
                + borrowModel.ItemNum25 *  (await item.FindAsync("床裙")).ItemValue
                + borrowModel.ItemNum26 *  (await item.FindAsync("缎料工服")).ItemValue
                + borrowModel.ItemNum27 *  (await item.FindAsync("唐装")).ItemValue
                + borrowModel.ItemNum28 *  (await item.FindAsync("免烫工服")).ItemValue
                + borrowModel.ItemNum29 *  (await item.FindAsync("旗袍")).ItemValue
                + borrowModel.ItemNum30 *  (await item.FindAsync("西服")).ItemValue

                + borrowModel.ItemNum31 *  (await item.FindAsync("领带")).ItemValue
                + borrowModel.ItemNum32 *  (await item.FindAsync("已烫工服")).ItemValue
                + borrowModel.ItemNum33 *  (await item.FindAsync("沙发套")).ItemValue
                + borrowModel.ItemNum34 *  (await item.FindAsync("床罩")).ItemValue
                + borrowModel.ItemNum35 *  (await item.FindAsync("抹布")).ItemValue
                + borrowModel.ItemNum36 *  (await item.FindAsync("保护垫")).ItemValue
                + borrowModel.ItemNum37 *  (await item.FindAsync("地毯清洗")).ItemValue
                + borrowModel.ItemNum38 *  (await item.FindAsync("足浴窄床单")).ItemValue
                + borrowModel.ItemNum39 *  (await item.FindAsync("预留1")).ItemValue
                + borrowModel.ItemNum40 *  (await item.FindAsync("预留2")).ItemValue;
        }

        private void WareReduce(WarehouseModel ware, BorrowModel borrowModel)
        {
            //库存减少
            ware.ItemNum1 -= borrowModel.ItemNum1;
            ware.ItemNum2 -= borrowModel.ItemNum2;
            ware.ItemNum3 -= borrowModel.ItemNum3;
            ware.ItemNum4 -= borrowModel.ItemNum4;
            ware.ItemNum5 -= borrowModel.ItemNum5;
            ware.ItemNum6 -= borrowModel.ItemNum6;
            ware.ItemNum7 -= borrowModel.ItemNum7;
            ware.ItemNum8 -= borrowModel.ItemNum8;
            ware.ItemNum9 -= borrowModel.ItemNum9;
            ware.ItemNum10 -= borrowModel.ItemNum10;
            ware.ItemNum11 -= borrowModel.ItemNum11;
            ware.ItemNum12 -= borrowModel.ItemNum12;
            ware.ItemNum13 -= borrowModel.ItemNum13;
            ware.ItemNum14 -= borrowModel.ItemNum14;
            ware.ItemNum15 -= borrowModel.ItemNum15;
            ware.ItemNum16 -= borrowModel.ItemNum16;
            ware.ItemNum17 -= borrowModel.ItemNum17;
            ware.ItemNum18 -= borrowModel.ItemNum18;
            ware.ItemNum19 -= borrowModel.ItemNum19;
            ware.ItemNum20 -= borrowModel.ItemNum20;
            //------------------------------------//
            ware.ItemNum21 -= borrowModel.ItemNum21;
            ware.ItemNum22 -= borrowModel.ItemNum22;
            ware.ItemNum23 -= borrowModel.ItemNum23;
            ware.ItemNum24 -= borrowModel.ItemNum24;
            ware.ItemNum25 -= borrowModel.ItemNum25;
            ware.ItemNum26 -= borrowModel.ItemNum26;
            ware.ItemNum27 -= borrowModel.ItemNum27;
            ware.ItemNum28 -= borrowModel.ItemNum28;
            ware.ItemNum29 -= borrowModel.ItemNum29;
            ware.ItemNum30 -= borrowModel.ItemNum30;
            ware.ItemNum31 -= borrowModel.ItemNum31;
            ware.ItemNum32 -= borrowModel.ItemNum32;
            ware.ItemNum33 -= borrowModel.ItemNum33;
            ware.ItemNum34 -= borrowModel.ItemNum34;
            ware.ItemNum35 -= borrowModel.ItemNum35;
            ware.ItemNum36 -= borrowModel.ItemNum36;
            ware.ItemNum37 -= borrowModel.ItemNum37;
            ware.ItemNum38 -= borrowModel.ItemNum38;
            ware.ItemNum39 -= borrowModel.ItemNum39;
            ware.ItemNum40 -= borrowModel.ItemNum40;

            _context.Update(ware);
        }

        private void WareTopup(WarehouseModel ware, BorrowModel borrowModel)
        {
            //库存减少
            ware.ItemNum1 += borrowModel.ItemNum1;
            ware.ItemNum2 += borrowModel.ItemNum2;
            ware.ItemNum3 += borrowModel.ItemNum3;
            ware.ItemNum4 += borrowModel.ItemNum4;
            ware.ItemNum5 += borrowModel.ItemNum5;
            ware.ItemNum6 += borrowModel.ItemNum6;
            ware.ItemNum7 += borrowModel.ItemNum7;
            ware.ItemNum8 += borrowModel.ItemNum8;
            ware.ItemNum9 += borrowModel.ItemNum9;
            ware.ItemNum10 += borrowModel.ItemNum10;
            ware.ItemNum11 += borrowModel.ItemNum11;
            ware.ItemNum12 += borrowModel.ItemNum12;
            ware.ItemNum13 += borrowModel.ItemNum13;
            ware.ItemNum14 += borrowModel.ItemNum14;
            ware.ItemNum15 += borrowModel.ItemNum15;
            ware.ItemNum16 += borrowModel.ItemNum16;
            ware.ItemNum17 += borrowModel.ItemNum17;
            ware.ItemNum18 += borrowModel.ItemNum18;
            ware.ItemNum19 += borrowModel.ItemNum19;
            ware.ItemNum20 += borrowModel.ItemNum20;
            //------------------------------------//
            ware.ItemNum21 += borrowModel.ItemNum21;
            ware.ItemNum22 += borrowModel.ItemNum22;
            ware.ItemNum23 += borrowModel.ItemNum23;
            ware.ItemNum24 += borrowModel.ItemNum24;
            ware.ItemNum25 += borrowModel.ItemNum25;
            ware.ItemNum26 += borrowModel.ItemNum26;
            ware.ItemNum27 += borrowModel.ItemNum27;
            ware.ItemNum28 += borrowModel.ItemNum28;
            ware.ItemNum29 += borrowModel.ItemNum29;
            ware.ItemNum30 += borrowModel.ItemNum30;
            ware.ItemNum31 += borrowModel.ItemNum31;
            ware.ItemNum32 += borrowModel.ItemNum32;
            ware.ItemNum33 += borrowModel.ItemNum33;
            ware.ItemNum34 += borrowModel.ItemNum34;
            ware.ItemNum35 += borrowModel.ItemNum35;
            ware.ItemNum36 += borrowModel.ItemNum36;
            ware.ItemNum37 += borrowModel.ItemNum37;
            ware.ItemNum38 += borrowModel.ItemNum38;
            ware.ItemNum39 += borrowModel.ItemNum39;
            ware.ItemNum40 += borrowModel.ItemNum40;

            _context.Update(ware);
        }

        //客户填写借订单

        [Authorize(Roles = "客户")]
        // GET: BorrowModels/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ItemModels"] = await _context.ItemModels.ToListAsync();
            ViewData["Discount"] = await _context.DiscountModels.FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);
            //ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName");
            return View();
        }

        // POST: BorrowModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "客户")]
        public async Task<IActionResult> Create([Bind("BorrowOrderID,AccountName,StartTime,IsCanceled,IsCompleted,IsTraned,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] BorrowModel borrowModel)
        {
            if (BorrowCheckZero(borrowModel))
            {
                ModelState.AddModelError("BorrowNote", "订单不能全部为0");
            }

            if (ModelState.IsValid)
            {
                var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.Account.Type == Models.EnumClass.EnumAccountType.仓库保管员);
                var account = await _context.AccountModels.SingleOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);
                var discount = await _context.DiscountModels.SingleOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

                if (ware == null)
                {
                    ModelState.AddModelError("BorrowNote", "系统库存未初始化 请联系管理员");
                }
                else if (discount == null)
                {
                    //021错误是用户的折扣信息没有初始化 或者当前用户存在多个折扣信息
                    ModelState.AddModelError("BorrowNote", "用户信息初始化错误，请联系管理员，code：021");
                }
                else if (borrowModel.ItemNum1 > ware.ItemNum1)
                {
                    ModelState.AddModelError("ItemNum1", "库存不足");
                }
                else if (borrowModel.ItemNum2 > ware.ItemNum2)
                {
                    ModelState.AddModelError("ItemNum2", "库存不足");
                }
                else if (borrowModel.ItemNum3 > ware.ItemNum3)
                {
                    ModelState.AddModelError("ItemNum3", "库存不足");
                }
                else if (borrowModel.ItemNum4 > ware.ItemNum4)
                {
                    ModelState.AddModelError("ItemNum4", "库存不足");
                }
                else if (borrowModel.ItemNum5 > ware.ItemNum5)
                {
                    ModelState.AddModelError("ItemNum5", "库存不足");
                }
                else if (borrowModel.ItemNum6 > ware.ItemNum6)
                {
                    ModelState.AddModelError("ItemNum6", "库存不足");
                }
                else if (borrowModel.ItemNum7 > ware.ItemNum7)
                {
                    ModelState.AddModelError("ItemNum7", "库存不足");
                }
                else if (borrowModel.ItemNum8 > ware.ItemNum8)
                {
                    ModelState.AddModelError("ItemNum8", "库存不足");
                }
                else if (borrowModel.ItemNum9 > ware.ItemNum9)
                {
                    ModelState.AddModelError("ItemNum9", "库存不足");
                }
                else if (borrowModel.ItemNum10 > ware.ItemNum10)
                {
                    ModelState.AddModelError("ItemNum10", "库存不足");
                }
                //----------------------10----------------------------
                else if (borrowModel.ItemNum11 > ware.ItemNum11)
                {
                    ModelState.AddModelError("ItemNum11", "库存不足");
                }
                else if (borrowModel.ItemNum12 > ware.ItemNum12)
                {
                    ModelState.AddModelError("ItemNum12", "库存不足");
                }
                else if (borrowModel.ItemNum13 > ware.ItemNum13)
                {
                    ModelState.AddModelError("ItemNum13", "库存不足");
                }
                else if (borrowModel.ItemNum14 > ware.ItemNum14)
                {
                    ModelState.AddModelError("ItemNum14", "库存不足");
                }
                else if (borrowModel.ItemNum15 > ware.ItemNum15)
                {
                    ModelState.AddModelError("ItemNum15", "库存不足");
                }
                else if (borrowModel.ItemNum16 > ware.ItemNum16)
                {
                    ModelState.AddModelError("ItemNum16", "库存不足");
                }
                else if (borrowModel.ItemNum17 > ware.ItemNum17)
                {
                    ModelState.AddModelError("ItemNum17", "库存不足");
                }
                else if (borrowModel.ItemNum18 > ware.ItemNum18)
                {
                    ModelState.AddModelError("ItemNum18", "库存不足");
                }
                else if (borrowModel.ItemNum19 > ware.ItemNum19)
                {
                    ModelState.AddModelError("ItemNum19", "库存不足");
                }
                else if (borrowModel.ItemNum20 > ware.ItemNum20)
                {
                    ModelState.AddModelError("ItemNum20", "库存不足");
                }
                //---------------------------20------------------------
                else if (borrowModel.ItemNum21 > ware.ItemNum21)
                {
                    ModelState.AddModelError("ItemNum21", "库存不足");
                }
                else if (borrowModel.ItemNum22 > ware.ItemNum22)
                {
                    ModelState.AddModelError("ItemNum22", "库存不足");
                }
                else if (borrowModel.ItemNum23 > ware.ItemNum23)
                {
                    ModelState.AddModelError("ItemNum23", "库存不足");
                }
                else if (borrowModel.ItemNum24 > ware.ItemNum24)
                {
                    ModelState.AddModelError("ItemNum24", "库存不足");
                }
                else if (borrowModel.ItemNum25 > ware.ItemNum25)
                {
                    ModelState.AddModelError("ItemNum25", "库存不足");
                }
                else if (borrowModel.ItemNum26 > ware.ItemNum26)
                {
                    ModelState.AddModelError("ItemNum26", "库存不足");
                }
                else if (borrowModel.ItemNum27 > ware.ItemNum27)
                {
                    ModelState.AddModelError("ItemNum27", "库存不足");
                }
                else if (borrowModel.ItemNum28 > ware.ItemNum28)
                {
                    ModelState.AddModelError("ItemNum28", "库存不足");
                }
                else if (borrowModel.ItemNum29 > ware.ItemNum29)
                {
                    ModelState.AddModelError("ItemNum29", "库存不足");
                }
                else if (borrowModel.ItemNum30 > ware.ItemNum30)
                {
                    ModelState.AddModelError("ItemNum30", "库存不足");
                }
                //---------------------------30------------------------
                else if (borrowModel.ItemNum31 > ware.ItemNum31)
                {
                    ModelState.AddModelError("ItemNum31", "库存不足");
                }
                else if (borrowModel.ItemNum32 > ware.ItemNum32)
                {
                    ModelState.AddModelError("ItemNum32", "库存不足");
                }
                else if (borrowModel.ItemNum33 > ware.ItemNum33)
                {
                    ModelState.AddModelError("ItemNum33", "库存不足");
                }
                else if (borrowModel.ItemNum34 > ware.ItemNum34)
                {
                    ModelState.AddModelError("ItemNum34", "库存不足");
                }
                else if (borrowModel.ItemNum35 > ware.ItemNum35)
                {
                    ModelState.AddModelError("ItemNum35", "库存不足");
                }
                else if (borrowModel.ItemNum36 > ware.ItemNum36)
                {
                    ModelState.AddModelError("ItemNum36", "库存不足");
                }
                else if (borrowModel.ItemNum37 > ware.ItemNum37)
                {
                    ModelState.AddModelError("ItemNum37", "库存不足");
                }
                else if (borrowModel.ItemNum38 > ware.ItemNum38)
                {
                    ModelState.AddModelError("ItemNum38", "库存不足");
                }
                else if (borrowModel.ItemNum39 > ware.ItemNum39)
                {
                    ModelState.AddModelError("ItemNum39", "库存不足");
                }
                else if (borrowModel.ItemNum40 > ware.ItemNum40)
                {
                    ModelState.AddModelError("ItemNum40", "库存不足");
                }
                else if (_context.ItemModels.Count() < 40)
                {
                    ModelState.AddModelError("BorrowNote", "物品信息数量不足 请联系管理员");
                }
                else
                {
                    borrowModel.OrderMoney = await ComputeTempBalance(borrowModel) * discount.DiscountValue;

                    //余额非负判断
                    if (account.Balance >= borrowModel.OrderMoney)
                    {
                        account.Balance -= borrowModel.OrderMoney;//余额减少

                        WareReduce(ware, borrowModel);//库存减少

                        _context.Update(account);
                        _context.Add(borrowModel);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("OrderCurrent", "Home", new { id = borrowModel.AccountName });
                    }

                    ModelState.AddModelError("BorrowNote", "您的余额不足 无法提交订单");
                }
            }

            ViewData["Discount"] = await _context.DiscountModels.FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

            ViewData["ItemModels"] = await _context.ItemModels.ToListAsync();
            return View(borrowModel);
        }

        [Authorize(Roles = "超级管理员,客户,仓库保管员")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var borrowOrder = await _context.BorrowModels.FindAsync(id);
            var account = await _context.AccountModels.FindAsync(borrowOrder.AccountName);
            var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.Account.Type == Models.EnumClass.EnumAccountType.仓库保管员);
            if (String.IsNullOrEmpty(borrowOrder.TranName))
            {
                borrowOrder.IsCanceled = true;
                borrowOrder.FinishTime = DateTime.Now;

                account.Balance += borrowOrder.OrderMoney;

                WareTopup(ware, borrowOrder);

                try
                {
                    _context.Update(borrowOrder);
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
            }

            return RedirectToAction("OrderCurrent", "Home");
        }

        [Authorize(Roles = "超级管理员,客户,仓库保管员")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            if (!await _context.WarehouseModels.AnyAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value))
            {
                _context.Add(new WarehouseModel()
                {
                    AccountName = User.FindFirst(ClaimTypes.Sid).Value
                });
                await _context.SaveChangesAsync();
            }

            var borrowOrder = _context.BorrowModels.Find(id);

            borrowOrder.FinishTime = DateTime.Now;
            borrowOrder.IsCompleted = true;

            var ware = await _context.WarehouseModels.FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

            WareTopup(ware, borrowOrder);

            try
            {
                _context.Update(borrowOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            return RedirectToAction("OrderCurrent", "Home");

        }

        [Authorize(Roles = "超级管理员,配送专员,仓库保管员")]
        public async Task<IActionResult> TranedOrder(int id)
        {
            var borrowOrder = _context.BorrowModels.Find(id);
            borrowOrder.TranTime = DateTime.Now;
            borrowOrder.IsTraned = true;

            try
            {
                _context.Update(borrowOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return RedirectToAction("TranOrderCurrent", "Home");
        }

        private async Task<List<ItemViewModel>> MyGetRealItemsAsync(double discount, BorrowModel borrowModel)
        {
            var realItems = new List<ItemViewModel>();
            var item = _context.ItemModels;

            if (borrowModel.ItemNum1 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "床单 1.2M",
                    ItemQuantity = borrowModel.ItemNum1,
                    ItemTrueUnitValue = (await item.FindAsync("床单 1.2M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum2 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "床单 1.5M",
                    ItemQuantity = borrowModel.ItemNum2,
                    ItemTrueUnitValue = (await item.FindAsync("床单 1.5M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum3 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "床单 1.8M",
                    ItemQuantity = borrowModel.ItemNum3,
                    ItemTrueUnitValue = (await item.FindAsync("床单 1.8M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum4 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "被套 1.2M",
                    ItemQuantity = borrowModel.ItemNum4,
                    ItemTrueUnitValue = (await item.FindAsync("被套 1.2M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum5 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "被套 1.5M",
                    ItemQuantity = borrowModel.ItemNum5,
                    ItemTrueUnitValue = (await item.FindAsync("被套 1.5M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum6 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "被套 1.8M",
                    ItemQuantity = borrowModel.ItemNum6,
                    ItemTrueUnitValue = (await item.FindAsync("被套 1.8M")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum7 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "枕套",
                    ItemQuantity = borrowModel.ItemNum7,
                    ItemTrueUnitValue = (await item.FindAsync("枕套")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum8 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "浴巾",
                    ItemQuantity = borrowModel.ItemNum8,
                    ItemTrueUnitValue = (await item.FindAsync("浴巾")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum9 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "地巾",
                    ItemQuantity = borrowModel.ItemNum9,
                    ItemTrueUnitValue = (await item.FindAsync("地巾")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum10 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "毛巾",
                    ItemQuantity = borrowModel.ItemNum10,
                    ItemTrueUnitValue = (await item.FindAsync("毛巾")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum11 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "方巾",
                    ItemQuantity = borrowModel.ItemNum11,
                    ItemTrueUnitValue = (await item.FindAsync("方巾")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum12 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "大台布",
                    ItemQuantity = borrowModel.ItemNum12,
                    ItemTrueUnitValue = (await item.FindAsync("大台布")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum13 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "香巾",
                    ItemQuantity = borrowModel.ItemNum13,
                    ItemTrueUnitValue = (await item.FindAsync("香巾")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum14 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "小台布",
                    ItemQuantity = borrowModel.ItemNum14,
                    ItemTrueUnitValue = (await item.FindAsync("小台布")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum15 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "口布",
                    ItemQuantity = borrowModel.ItemNum15,
                    ItemTrueUnitValue = (await item.FindAsync("口布")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum16 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "毛毯",
                    ItemQuantity = borrowModel.ItemNum16,
                    ItemTrueUnitValue = (await item.FindAsync("毛毯")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum17 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "桌围裙",
                    ItemQuantity = borrowModel.ItemNum17,
                    ItemTrueUnitValue = (await item.FindAsync("桌围裙")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum18 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "厨衣",
                    ItemQuantity = borrowModel.ItemNum18,
                    ItemTrueUnitValue = (await item.FindAsync("厨衣")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum19 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "窗帘",
                    ItemQuantity = borrowModel.ItemNum19,
                    ItemTrueUnitValue = (await item.FindAsync("窗帘")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum20 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "窗帘内胆",
                    ItemQuantity = borrowModel.ItemNum20,
                    ItemTrueUnitValue = (await item.FindAsync("窗帘内胆")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum21 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "浴帘",
                    ItemQuantity = borrowModel.ItemNum21,
                    ItemTrueUnitValue = (await item.FindAsync("浴帘")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum22 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "浴服",
                    ItemQuantity = borrowModel.ItemNum22,
                    ItemTrueUnitValue = (await item.FindAsync("浴服")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum23 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "椅套",
                    ItemQuantity = borrowModel.ItemNum23,
                    ItemTrueUnitValue = (await item.FindAsync("椅套")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum24 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "帽子",
                    ItemQuantity = borrowModel.ItemNum24,
                    ItemTrueUnitValue = (await item.FindAsync("帽子")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum25 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "床裙",
                    ItemQuantity = borrowModel.ItemNum25,
                    ItemTrueUnitValue = (await item.FindAsync("床裙")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum26 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "缎料工服",
                    ItemQuantity = borrowModel.ItemNum26,
                    ItemTrueUnitValue = (await item.FindAsync("缎料工服")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum27 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "唐装",
                    ItemQuantity = borrowModel.ItemNum27,
                    ItemTrueUnitValue = (await item.FindAsync("唐装")).ItemValue * discount
                });
            }
            
            if (borrowModel.ItemNum28 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "免烫工服",
                    ItemQuantity = borrowModel.ItemNum28,
                    ItemTrueUnitValue = (await item.FindAsync("免烫工服")).ItemValue * discount
                });
            }   
            
            if (borrowModel.ItemNum29 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "旗袍",
                    ItemQuantity = borrowModel.ItemNum29,
                    ItemTrueUnitValue = (await item.FindAsync("旗袍")).ItemValue * discount
                });
            }     
            
            if (borrowModel.ItemNum30 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "西服",
                    ItemQuantity = borrowModel.ItemNum30,
                    ItemTrueUnitValue = (await item.FindAsync("西服")).ItemValue * discount
                });
            }   
            
            if (borrowModel.ItemNum31 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "领带",
                    ItemQuantity = borrowModel.ItemNum31,
                    ItemTrueUnitValue = (await item.FindAsync("领带")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum32 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "已烫工服",
                    ItemQuantity = borrowModel.ItemNum32,
                    ItemTrueUnitValue = (await item.FindAsync("已烫工服")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum33 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "沙发套",
                    ItemQuantity = borrowModel.ItemNum33,
                    ItemTrueUnitValue = (await item.FindAsync("沙发套")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum34 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "床罩",
                    ItemQuantity = borrowModel.ItemNum34,
                    ItemTrueUnitValue = (await item.FindAsync("床罩")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum35 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "抹布",
                    ItemQuantity = borrowModel.ItemNum35,
                    ItemTrueUnitValue = (await item.FindAsync("抹布")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum36 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "保护垫",
                    ItemQuantity = borrowModel.ItemNum36,
                    ItemTrueUnitValue = (await item.FindAsync("保护垫")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum37 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "地毯清洗",
                    ItemQuantity = borrowModel.ItemNum37,
                    ItemTrueUnitValue = (await item.FindAsync("地毯清洗")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum38 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "足浴窄床单",
                    ItemQuantity = borrowModel.ItemNum38,
                    ItemTrueUnitValue = (await item.FindAsync("足浴窄床单")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum39 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "预留1",
                    ItemQuantity = borrowModel.ItemNum39,
                    ItemTrueUnitValue = (await item.FindAsync("预留1")).ItemValue * discount
                });
            }

            if (borrowModel.ItemNum40 != 0)
            {
                realItems.Add(new ItemViewModel
                {
                    ItemName = "预留2",
                    ItemQuantity = borrowModel.ItemNum40,
                    ItemTrueUnitValue = (await item.FindAsync("预留2")).ItemValue * discount
                });
            }

            return realItems;
        }
    }
}