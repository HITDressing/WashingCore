namespace BSXWashing.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using BSXWashing.Models;
    using BSXWashing.Models.DBClass;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;

    public class HomeController : Controller
    {
        protected readonly WashingContext _context;

        public HomeController(WashingContext context) => _context = context;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "应用程序说明界面";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "联系人页面";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("AccountName,Password")] AccountModel accountModel)
        {
            var find = await _context.AccountModels.FirstOrDefaultAsync(x =>
            x.AccountName == accountModel.AccountName && x.Password == accountModel.Password);

            if (find != null)
            {
                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, find.AccountName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, find.Type.ToString()));

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(30)
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("AccountName", "用户名或密码错误！");

            return View(accountModel);
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "超级管理员,仓库保管员")]
        public async Task<ActionResult> OrderPool()
        {
            var borrow = _context.BorrowModels.Where(x => String.IsNullOrEmpty(x.TranName)).Include(w => w.Account);
            var payback = _context.PaybackModels.Where(x => String.IsNullOrEmpty(x.TranName)).Include(w => w.Account);

            ViewBag.Payback = await payback.ToListAsync();
            return View(await borrow.ToListAsync());
        }

        [Authorize(Roles = "配送专员,客户")]
        public async Task<ActionResult> OrderHistory()
        {
            var name = User.FindFirst(ClaimTypes.Sid).Value;
            var data = await _context.AccountModels.FindAsync(name);
            data.BorrowTransport = await _context.BorrowModels.Where(x => x.TranName == name && (x.IsCompleted || x.IsCanceled)).ToListAsync();
            data.PaybackTransport = await _context.PaybackModels.Where(x => x.TranName == name && (x.IsCompleted || x.IsCanceled)).ToListAsync();

            return View(data);
        }

        //未完成的订单
        [Authorize(Roles = "客户")]
        public async Task<ActionResult> OrderCurrent()
        {
            var account = await _context.AccountModels.Include(x => x.PaybackTransport).Include(x => x.BorrowTransport).FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

            if (account.BorrowTransport != null)
            {
                account.BorrowTransport = account.BorrowTransport.Where(x => !x.IsCompleted && !x.IsCanceled && !x.IsTraned).ToList();
            }
            if (account.PaybackTransport != null)
            {
                account.PaybackTransport = account.PaybackTransport.Where(x => !x.IsCompleted && !x.IsCanceled && !x.IsTraned).ToList();
            }

            //switch (User.FindFirst(ClaimTypes.Role).Value)
            //{
            //    case "配送专员":
            //        {
            //            account.BorrowTransport = await _context.BorrowModels.Include(x => x.Account).Where(x => x.TranName == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
            //            account.PaybackTransport = await _context.PaybackModels.Include(x => x.Account).Where(x => x.TranName == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
            //            break;
            //        }
            //    case "客户":
            //        {

            //            break;
            //        }
            //    default: break;
            //}

            return View(account);
        }
    }
}

/*

*/
/*
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
    public class BorrowModelsController : Controller
    {
        private readonly WashingContext _context;

        public BorrowModelsController(WashingContext context) => _context = context;

        // GET: BorrowModels
        public async Task<IActionResult> Index()
        {
            var washingContext = _context.BorrowModels.Include(b => b.Account);
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
                .SingleOrDefaultAsync(m => m.BorrowOrderID == id);
            if (borrowModel == null)
            {
                return NotFound();
            }

            return View(borrowModel);
        }

        [Authorize(Roles = "客户")]
        // GET: BorrowModels/Create
        public IActionResult Create()
        {
            //ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName");
            return View();
        }

        [Authorize(Roles = "客户")]
        // POST: BorrowModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowOrderID,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] BorrowModel borrowModel)
        {
            if (borrowModel.ItemNum1 == 0 && borrowModel.ItemNum2 == 0 && borrowModel.ItemNum3 == 0 && borrowModel.ItemNum4 == 0 &&
                borrowModel.ItemNum5 == 0 && borrowModel.ItemNum6 == 0 && borrowModel.ItemNum7 == 0 && borrowModel.ItemNum8 == 0 &&
                borrowModel.ItemNum9 == 0 && borrowModel.ItemNum10 == 0 && borrowModel.ItemNum11 == 0 && borrowModel.ItemNum12 == 0 &&
                borrowModel.ItemNum13 == 0 && borrowModel.ItemNum14 == 0 && borrowModel.ItemNum15 == 0 && borrowModel.ItemNum16 == 0 &&
                borrowModel.ItemNum17 == 0 && borrowModel.ItemNum18 == 0 && borrowModel.ItemNum19 == 0 && borrowModel.ItemNum20 == 0 &&
                borrowModel.ItemNum21 == 0 && borrowModel.ItemNum22 == 0 && borrowModel.ItemNum23 == 0 && borrowModel.ItemNum24 == 0 &&
                borrowModel.ItemNum25 == 0 && borrowModel.ItemNum26 == 0 && borrowModel.ItemNum27 == 0 && borrowModel.ItemNum28 == 0 &&
                borrowModel.ItemNum29 == 0 && borrowModel.ItemNum30 == 0 && borrowModel.ItemNum31 == 0 && borrowModel.ItemNum32 == 0 &&
                borrowModel.ItemNum33 == 0 && borrowModel.ItemNum34 == 0 && borrowModel.ItemNum35 == 0 && borrowModel.ItemNum36 == 0 &&
                borrowModel.ItemNum37 == 0 && borrowModel.ItemNum38 == 0 && borrowModel.ItemNum39 == 0 && borrowModel.ItemNum40 == 0)
            {
                ModelState.AddModelError("BorrowNote", "订单不能全部为0");
            }

            borrowModel.AccountName = User.FindFirst(ClaimTypes.Sid).Value;
            borrowModel.IsCanceled = false;
            borrowModel.IsCompleted = false;
            borrowModel.IsTraned = false;
            borrowModel.StartTime = DateTime.Now;

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
                    var tempBalance = account.Balance - await ComputeTempBalance(borrowModel) * discount.DiscountValue;

                    //余额非负判断
                    if (tempBalance > 0)
                    {
                        account.Balance = tempBalance;//余额减少

                        WareReduce(ware, borrowModel);

                        _context.Update(account);
                        _context.Add(borrowModel);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", "AccountModels", new { id = borrowModel.AccountName });
                    }

                    ModelState.AddModelError("BorrowNote", "您的余额不足 无法提交订单");
                }
            }
            //ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        private async Task<double> ComputeTempBalance(BorrowModel borrowModel)
        {
            var item = await _context.ItemModels.ToListAsync();
            return borrowModel.ItemNum1 * item[0].ItemValue + borrowModel.ItemNum2 * item[1].ItemValue + borrowModel.ItemNum3 * item[2].ItemValue + borrowModel.ItemNum4 * item[3].ItemValue +
                       borrowModel.ItemNum5 * item[4].ItemValue + borrowModel.ItemNum6 * item[5].ItemValue + borrowModel.ItemNum7 * item[6].ItemValue + borrowModel.ItemNum8 * item[7].ItemValue +
                       borrowModel.ItemNum9 * item[8].ItemValue + borrowModel.ItemNum10 * item[9].ItemValue + borrowModel.ItemNum11 * item[10].ItemValue + borrowModel.ItemNum12 * item[11].ItemValue +
                       borrowModel.ItemNum13 * item[12].ItemValue + borrowModel.ItemNum14 * item[13].ItemValue + borrowModel.ItemNum15 * item[14].ItemValue + borrowModel.ItemNum16 * item[15].ItemValue +
                       borrowModel.ItemNum17 * item[16].ItemValue + borrowModel.ItemNum18 * item[17].ItemValue + borrowModel.ItemNum19 * item[18].ItemValue + borrowModel.ItemNum20 * item[19].ItemValue +
                       borrowModel.ItemNum21 * item[20].ItemValue + borrowModel.ItemNum22 * item[21].ItemValue + borrowModel.ItemNum23 * item[22].ItemValue + borrowModel.ItemNum24 * item[23].ItemValue +
                       borrowModel.ItemNum25 * item[24].ItemValue + borrowModel.ItemNum26 * item[25].ItemValue + borrowModel.ItemNum27 * item[26].ItemValue + borrowModel.ItemNum28 * item[27].ItemValue +
                       borrowModel.ItemNum29 * item[28].ItemValue + borrowModel.ItemNum30 * item[29].ItemValue + borrowModel.ItemNum31 * item[30].ItemValue + borrowModel.ItemNum32 * item[31].ItemValue +
                       borrowModel.ItemNum33 * item[32].ItemValue + borrowModel.ItemNum34 * item[33].ItemValue + borrowModel.ItemNum35 * item[34].ItemValue + borrowModel.ItemNum36 * item[35].ItemValue +
                       borrowModel.ItemNum37 * item[36].ItemValue + borrowModel.ItemNum38 * item[37].ItemValue + borrowModel.ItemNum39 * item[38].ItemValue + borrowModel.ItemNum40 * item[39].ItemValue;
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
            ViewData["AccountName"] = new SelectList(_context.AccountModels, "AccountName", "AccountName", borrowModel.AccountName);
            return View(borrowModel);
        }

        // POST: BorrowModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowOrderID,TranName,AccountName,StartTime,TranTime,FinishTime,IsCanceled,IsCompleted,IsTraned,BorrowNote,ItemNum1,ItemNum2,ItemNum3,ItemNum4,ItemNum5,ItemNum6,ItemNum7,ItemNum8,ItemNum9,ItemNum10,ItemNum11,ItemNum12,ItemNum13,ItemNum14,ItemNum15,ItemNum16,ItemNum17,ItemNum18,ItemNum19,ItemNum20,ItemNum21,ItemNum22,ItemNum23,ItemNum24,ItemNum25,ItemNum26,ItemNum27,ItemNum28,ItemNum29,ItemNum30,ItemNum31,ItemNum32,ItemNum33,ItemNum34,ItemNum35,ItemNum36,ItemNum37,ItemNum38,ItemNum39,ItemNum40")] BorrowModel borrowModel)
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

        [HttpPost]
        [Authorize(Roles = "超级管理员,客户,仓库保管员")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var item = await _context.ItemModels.ToListAsync();
            var borrowOrder = await _context.BorrowModels.FindAsync(id);
            var account = await _context.AccountModels.FindAsync(borrowOrder.AccountName);
            if (String.IsNullOrEmpty(borrowOrder.TranName))
            {
                var tempBalance = borrowOrder.ItemNum1 * item[0].ItemValue + borrowOrder.ItemNum2 * item[1].ItemValue + borrowOrder.ItemNum3 * item[2].ItemValue + borrowOrder.ItemNum4 * item[3].ItemValue +
    borrowOrder.ItemNum5 * item[4].ItemValue + borrowOrder.ItemNum6 * item[5].ItemValue + borrowOrder.ItemNum7 * item[6].ItemValue + borrowOrder.ItemNum8 * item[7].ItemValue +
    borrowOrder.ItemNum9 * item[8].ItemValue + borrowOrder.ItemNum10 * item[9].ItemValue + borrowOrder.ItemNum11 * item[10].ItemValue + borrowOrder.ItemNum12 * item[11].ItemValue +
    borrowOrder.ItemNum13 * item[12].ItemValue + borrowOrder.ItemNum14 * item[13].ItemValue + borrowOrder.ItemNum15 * item[14].ItemValue + borrowOrder.ItemNum16 * item[15].ItemValue +
    borrowOrder.ItemNum17 * item[16].ItemValue + borrowOrder.ItemNum18 * item[17].ItemValue + borrowOrder.ItemNum19 * item[18].ItemValue + borrowOrder.ItemNum20 * item[19].ItemValue +
    borrowOrder.ItemNum21 * item[20].ItemValue + borrowOrder.ItemNum22 * item[21].ItemValue + borrowOrder.ItemNum23 * item[22].ItemValue + borrowOrder.ItemNum24 * item[23].ItemValue +
    borrowOrder.ItemNum25 * item[24].ItemValue + borrowOrder.ItemNum26 * item[25].ItemValue + borrowOrder.ItemNum27 * item[26].ItemValue + borrowOrder.ItemNum28 * item[27].ItemValue +
    borrowOrder.ItemNum29 * item[28].ItemValue + borrowOrder.ItemNum30 * item[29].ItemValue + borrowOrder.ItemNum31 * item[30].ItemValue + borrowOrder.ItemNum32 * item[31].ItemValue +
    borrowOrder.ItemNum33 * item[32].ItemValue + borrowOrder.ItemNum34 * item[33].ItemValue + borrowOrder.ItemNum35 * item[34].ItemValue + borrowOrder.ItemNum36 * item[35].ItemValue +
    borrowOrder.ItemNum37 * item[36].ItemValue + borrowOrder.ItemNum38 * item[37].ItemValue + borrowOrder.ItemNum39 * item[38].ItemValue + borrowOrder.ItemNum40 * item[39].ItemValue;
                borrowOrder.IsCanceled = true;

                account.

                try
                {
                    _context.Update(borrowOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
            }

            return RedirectToAction("OrderCurrent", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "超级管理员,客户,仓库保管员")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var borrowOrder = _context.BorrowModels.Find(id);
            if (String.IsNullOrEmpty(borrowOrder.TranName))
            {
                borrowOrder.IsCompleted = true;

                try
                {
                    _context.Update(borrowOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
            }

            return RedirectToAction("OrderCurrent", "Home");

        }

        [HttpPost]
        [Authorize(Roles = "超级管理员,配送专员,仓库保管员")]
        public async Task<IActionResult> TranedOrder(int id)
        {
            var borrowOrder = _context.BorrowModels.Find(id);
            if (String.IsNullOrEmpty(borrowOrder.TranName))
            {
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
            }

            return RedirectToAction("OrderCurrent", "Home");

        }
    }
}
*/