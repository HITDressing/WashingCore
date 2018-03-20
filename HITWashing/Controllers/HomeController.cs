using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HITWashing.Models;
using HITWashing.Models.DBClass;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace HITWashing.Controllers
{
    public class HomeController : Controller
    {
        protected readonly WashingContext _context;

        public HomeController(WashingContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
            //if (ModelState.IsValid)
            //{

            //}

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

            ModelState.AddModelError("", "用户名或密码错误！");

            return View(accountModel);
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "配送专员")]
        public async Task<ActionResult> OrderPool()
        {
            var borrow = _context.Borrows.Where(x => String.IsNullOrEmpty(x.UserID)).Include(w => w.Account);
            var payback = _context.Paybacks.Where(x => String.IsNullOrEmpty(x.UserName)).Include(w => w.Account);

            ViewBag.Payback = await payback.ToListAsync();
            return View(await borrow.ToListAsync());
        }

        [Authorize(Roles = "配送专员,客户")]
        public async Task<ActionResult> OrderHistory()
        {
            //User.FindFirst(ClaimTypes.Sid).Value
            //var data = await _context.AccountModels.FindAsync(User.FindFirst(ClaimTypes.Sid).Value);
            //.Where(x => x.AccountName == );
            //    .Include(a => a.BorrowTransport)
            //    .Include(a => a.PaybackTransport).FirstOrDefaultAsync();

            //data.BorrowTransport = data.BorrowTransport.Where(x => x.IsCanceled || x.IsCompleted).ToList();
            //data.PaybackTransport = data.PaybackTransport.Where(x => x.IsCanceled || x.IsCompleted).ToList();
            var name = User.FindFirst(ClaimTypes.Sid).Value;
            var data = await _context.AccountModels.FindAsync(name);
            data.BorrowTransport = await _context.Borrows.Where(x => x.UserID == name && (x.IsCompleted || x.IsCanceled)).ToListAsync();
            data.PaybackTransport = await _context.Paybacks.Where(x => x.UserName == name && (x.IsCompleted || x.IsCanceled)).ToListAsync();

            return View(data);
        }

        [Authorize(Roles = "配送专员,客户")]
        public async Task<ActionResult> OrderCurrent()
        {
            var account = await _context.AccountModels.Include(x => x.PaybackTransport).Include(x => x.BorrowTransport).FirstOrDefaultAsync(x => x.AccountName == User.FindFirst(ClaimTypes.Sid).Value);

            switch (User.FindFirst(ClaimTypes.Role).Value)
            {
                case "配送专员":
                    {
                        account.BorrowTransport = await _context.Borrows.Include(x=>x.Account).Where(x => x.UserID == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
                        account.PaybackTransport = await _context.Paybacks.Include(x => x.Account).Where(x => x.UserName == User.FindFirst(ClaimTypes.Sid).Value && !x.IsCompleted && !x.IsCanceled).ToListAsync();
                        break;
                    }
                case "客户":
                    {
                        if (account.BorrowTransport != null)
                        {
                            account.BorrowTransport = account.BorrowTransport.Where(x => !String.IsNullOrEmpty(x.UserID) && !x.IsCompleted && !x.IsCanceled).ToList();
                        }
                        if (account.PaybackTransport != null)
                        {
                            account.PaybackTransport = account.PaybackTransport.Where(x => !String.IsNullOrEmpty(x.UserName) && !x.IsCompleted && !x.IsCanceled).ToList();
                        }
                        break;
                    }
                default:break;
            }

            return View(account);
        }
    }
}
