using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.DATA.EF.Models;

namespace BookStore.UI.MVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;

        public BooksController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookStoreContext = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Condition)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.Type)
                .Include(b => b.OrderBooks);
            return View(await bookStoreContext.ToListAsync());
        }
        // Tiled Products Action
        public async Task<IActionResult> TiledBooks(int bookId = 0)
        {
            var bookStoreContext = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Condition)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.Type)
                .Include(b => b.OrderBooks);
            return View(await bookStoreContext.ToListAsync());

            //#region Optional Search Filter
            //if (searchTerm != null)
            //{
            //    searchTerm = searchTerm.ToLower();
            //    products = products.Where(p =>
            //             p.ProductName.ToLower().Contains(searchTerm) ||
            //             p.Supplier.SupplierName.ToLower().Contains(searchTerm) ||
            //             p.Category.CategoryName.ToLower().Contains(searchTerm) ||
            //             p.ProductDescription.ToLower().Contains(searchTerm)).ToList();
            //    ViewBag.NbrResults = products.Count;
            //    ViewBag.SearchTerm = searchTerm;
            //}



            //#endregion

            //#region Optional Category Filter
            ////create a ViewBag/Data to send a list of categories to the view
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
            //if (categoryId != 0)
            //{
            //    products = products.Where(p => p.CategoryId == categoryId).ToList();
            //    ViewBag.NbrResults = products.Count;
            //    ViewBag.SearchTerm = searchTerm;
            //}

            ////paged list:
            //int pageSize = 6;

            //#endregion

            //return View(bookStoreContext);
            //return View(products.ToPagedList(page, pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Condition)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.Type)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorFname");
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "ConditionId", "ConditionDescription");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeDescription");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,AuthorId,PublicationDate,GenreId,IsFiction,TypeId,Pages,PublisherId,ConditionId,BookPrice,Isbn,Image,UnitsInStock")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorFname", book.AuthorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "ConditionId", "ConditionDescription", book.ConditionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City", book.PublisherId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeDescription", book.TypeId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorFname", book.AuthorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "ConditionId", "ConditionDescription", book.ConditionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City", book.PublisherId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeDescription", book.TypeId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,AuthorId,PublicationDate,GenreId,IsFiction,TypeId,Pages,PublisherId,ConditionId,BookPrice,Isbn,Image,UnitsInStock")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorFname", book.AuthorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "ConditionId", "ConditionDescription", book.ConditionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City", book.PublisherId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeDescription", book.TypeId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Condition)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.Type)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
