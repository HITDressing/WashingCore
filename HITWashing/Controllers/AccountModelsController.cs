using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HITWashing.Models.DBClass;
using HITWashing.Models.EnumClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HITWashing.Controllers
{
    public class AccountModelsController : Controller
    {
        private readonly WashingContext _context;

        public AccountModelsController(WashingContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "超级管理员")]
        // GET: AccountModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountModels.ToListAsync());
        }

        [Authorize(Roles = "超级管理员,客户")]
        // GET: AccountModels/Details/5
        public async Task<IActionResult> Details(string id)
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

            accountModel.BorrowTransport = await _context.Borrows.Where(x => x.AccountName == id && x.IsCompleted).ToListAsync();
            accountModel.PaybackTransport = await _context.Paybacks.Where(x => x.AccountName == id && x.IsCompleted).ToListAsync();

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
        public async Task<IActionResult> Create([Bind("AccountName,MobileNumber,Address,Type,Password,Salt,StoreName")] AccountModel accountModel)
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
        public async Task<IActionResult> Edit(string id, [Bind("AccountName,MobileNumber,Address,Type,Password,Salt,StoreName")] AccountModel accountModel)
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
                return RedirectToAction(nameof(Index));
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
        public IActionResult Regist()
        {
            return View();
        }

        // POST: AccountModels/Regist
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist([Bind("AccountName,MobileNumber,Address,Password,StoreName")] AccountModel accountModel)
        {
            accountModel.Type = EnumAccountType.客户;
            if (ModelState.IsValid)
            {
                _context.Add(accountModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountModel);
        }

    }
}
