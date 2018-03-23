using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BSXWashing.Models.DBClass;
using BSXWashing.Models.EnumClass;
using Microsoft.AspNetCore.Authorization;

namespace BSXWashing.Controllers
{
    public class AccountModelsController : Controller
    {
        private readonly WashingContext _context;

        public AccountModelsController(WashingContext context) => _context = context;

        [Authorize(Roles = "超级管理员")]
        // GET: AccountModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountModels.ToListAsync());
        }

        [Authorize(Roles = "超级管理员,财务负责人")]
        // GET: AccountModels
        public async Task<IActionResult> Balance()
        {
            return View(await _context.AccountModels.Where(x=>x.Type == EnumAccountType.客户).ToListAsync());
        }

        // GET: AccountModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels
                .Include(x => x.Warehouses)
                .Include(x => x.Topups)
                .Include(x => x.BorrowTransport)
                .Include(x => x.PaybackTransport)
                .SingleOrDefaultAsync(m => m.AccountName == id);

            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // GET: AccountModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,MobileNumber,Address,Type,Level,Category,Password,Salt,StoreName,Balance")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

        // GET: AccountModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels.SingleOrDefaultAsync(m => m.AccountName == id);
            if (accountModel == null)
            {
                return NotFound();
            }
            return View(accountModel);
        }

        // POST: AccountModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountName,MobileNumber,Address,Type,Level,Category,Password,Salt,StoreName,Balance")] AccountModel accountModel)
        {
            if (id != accountModel.AccountName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountModelExists(accountModel.AccountName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id });
            }
            return View(accountModel);
        }

        // GET: AccountModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountModel = await _context.AccountModels
                .SingleOrDefaultAsync(m => m.AccountName == id);
            if (accountModel == null)
            {
                return NotFound();
            }

            return View(accountModel);
        }

        // POST: AccountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var accountModel = await _context.AccountModels.SingleOrDefaultAsync(m => m.AccountName == id);
            _context.AccountModels.Remove(accountModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountModelExists(string id)
        {
            return _context.AccountModels.Any(e => e.AccountName == id);
        }

        // GET: AccountModels/Regist
        public IActionResult Regist() => View();

        // POST: AccountModels/Regist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist([Bind("AccountName,MobileNumber,Address,Password,StoreName")] AccountModel accountModel)
        {
            accountModel.Type = EnumAccountType.客户;
            accountModel.Category = EnumAccountCategory.其他;
            accountModel.Level = EnumAccountLevel.E类;
            accountModel.Balance = 0;

            if (ModelState.IsValid)
            {
                _context.Add(accountModel);
                await _context.SaveChangesAsync();

                await new DiscountModelsController(_context).Create(new DiscountModel
                {
                    AccountName = accountModel.AccountName,
                    DiscountValue = 1,
                    DiscountNote = "初始化折扣为1"
                });

                return RedirectToAction("Login","Home");
            }
            return View(accountModel);
        }
    }
}
