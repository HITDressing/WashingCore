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
        [Authorize(Roles = "配送专员,客户")]
        public async Task<ActionResult> OrderCurrent()
        {
            var account = await _context.AccountModels.Include(x => x.PaybackTransport).Include(x => x.BorrowTransport).FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

            switch (User.FindFirst(ClaimTypes.Role).Value)
            {
                case "配送专员":
                    {
                        account.BorrowTransport = await _context.BorrowModels.Include(x => x.Account).Where(x => x.TranName == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
                        account.PaybackTransport = await _context.PaybackModels.Include(x => x.Account).Where(x => x.TranName == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
                        break;
                    }
                case "客户":
                    {
                        if (account.BorrowTransport != null)
                        {
                            account.BorrowTransport = account.BorrowTransport.Where(x => !x.IsCompleted && !x.IsCanceled).ToList();
                        }
                        if (account.PaybackTransport != null)
                        {
                            account.PaybackTransport = account.PaybackTransport.Where(x => !x.IsCompleted && !x.IsCanceled).ToList();
                        }
                        break;
                    }
                default: break;
            }

            return View(account);
        }
    }
}
