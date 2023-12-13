﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.DATA.EF.Models;

namespace BookStore.UI.MVC.Controllers
{
    public class BuyersController : Controller
    {
        private readonly BookStoreContext _context;

        public BuyersController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: Buyers
        public async Task<IActionResult> Index()
        {
              return _context.Buyers != null ? 
                          View(await _context.Buyers.ToListAsync()) :
                          Problem("Entity set 'BookStoreContext.Buyers'  is null.");
        }

        // GET: Buyers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Buyers == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers
                .FirstOrDefaultAsync(m => m.BuyerId == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // GET: Buyers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyerId,BuyerFname,BuyerLname,Address,City,State,PostalCode,Country,Phone,Email")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }

        // GET: Buyers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Buyers == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return View(buyer);
        }

        // POST: Buyers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BuyerId,BuyerFname,BuyerLname,Address,City,State,PostalCode,Country,Phone,Email")] Buyer buyer)
        {
            if (id != buyer.BuyerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyerExists(buyer.BuyerId))
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
            return View(buyer);
        }

        // GET: Buyers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Buyers == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers
                .FirstOrDefaultAsync(m => m.BuyerId == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Buyers == null)
            {
                return Problem("Entity set 'BookStoreContext.Buyers'  is null.");
            }
            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer != null)
            {
                _context.Buyers.Remove(buyer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyerExists(string id)
        {
          return (_context.Buyers?.Any(e => e.BuyerId == id)).GetValueOrDefault();
        }
    }
}
