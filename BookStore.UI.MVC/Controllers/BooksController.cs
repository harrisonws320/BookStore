using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;//added for file upload
using BookStore.UI.MVC.Utilities;//added for image resize utility

namespace BookStore.UI.MVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(BookStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        //    public async Task<IActionResult> TiledBooks(string searchTerm, int bookId = 0)
        //    {
        //        var bookStoreContext = _context.Books
        //            .Include(b => b.Author)
        //            .Include(b => b.Condition)
        //            .Include(b => b.Genre)
        //            .Include(b => b.Publisher)
        //            .Include(b => b.Type)
        //            .Include(b => b.OrderBooks);
        //        return View(await bookStoreContext.ToListAsync());


        //}

        // GET: Books/Details/5

        public async Task<IActionResult> TiledBooks(string searchTerm, int bookId = 0)
        {
            IQueryable<Book> bookQuery = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Condition)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.Type)
                .Include(b => b.OrderBooks);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                bookQuery = bookQuery.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    (b.Author.AuthorFname + " " + b.Author.AuthorLname).ToLower().Contains(searchTerm));
            }

            var bookStoreContext = await bookQuery.ToListAsync();
            return View(bookStoreContext);
        }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,AuthorId,PublicationDate,GenreId,IsFiction,TypeId,Pages,PublisherId,ConditionId,BookPrice,Isbn,Image,ImageFile,UnitsInStock")] Book book)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - CREATE
                if (book.ImageFile == null)
                {
                    book.Image = "noimage.png";
                }
                else
                {
                    string ext = Path.GetExtension(book.ImageFile.FileName);
                    List<string> validExt = new() { ".jpeg", ".jpg", ".gif", ".png" };
                    if (validExt.Contains(ext.ToLower()) && book.ImageFile.Length < 4_194_303)
                    {
                        book.Image = Guid.NewGuid() + ext;
                        string webrootPath = _webHostEnvironment.WebRootPath;
                        string fullImagePath = webrootPath + "/img/";
                        using var memoryStream = new MemoryStream();
                        await book.ImageFile.CopyToAsync(memoryStream);
                        using var img = Image.FromStream(memoryStream);

                        int maxImage = 300;
                        int maxThumbSize = 100;
                        ImageUtilities.ResizeImage(fullImagePath, book.Image, img, maxImage, maxThumbSize);
                    }
                }
                #endregion
                //end:
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,AuthorId,PublicationDate,GenreId,IsFiction,TypeId,Pages,PublisherId,ConditionId,BookPrice,Isbn,Image,ImageFile,UnitsInStock")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region File Upload - Edit
                string? oldImageName = book.Image;
                if (book.ImageFile != null)
                {
                    string ext = Path.GetExtension(book.ImageFile.FileName);
                    List<string> validExts = new() { ".jpeg", ".jpg", ".png", ".gif" };
                    if (validExts.Contains(ext.ToLower()) && book.ImageFile.Length < 4_194_303)
                    {
                        book.Image = Guid.NewGuid() + ext;
                        string fullPath = _webHostEnvironment.WebRootPath + "/img/";
                        if (oldImageName != null && !oldImageName.ToLower().StartsWith("noimage"))
                        {
                            ImageUtilities.Delete(fullPath, oldImageName);
                        }
                        using var memoryStream = new MemoryStream();
                        await book.ImageFile.CopyToAsync(memoryStream);
                        using var img = Image.FromStream(memoryStream);
                        int maxImgSize = 500;
                        int maxThumbSize = 100;
                        ImageUtilities.ResizeImage(fullPath, book.Image, img, maxImgSize, maxThumbSize);
                    }
                }
                #endregion
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                string? oldImageName = book.Image;
                string fullPath = _webHostEnvironment.WebRootPath + "/img/";
                if (oldImageName != null && !oldImageName.ToLower().Contains("noimage"))
                {
                    ImageUtilities.Delete(fullPath, oldImageName);
                }
                
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
