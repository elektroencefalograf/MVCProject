using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YourBooks.Areas.Identity.Data;
using YourBooks.Models;

namespace YourBooks.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _user = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _user.GetUserAsync(HttpContext.User);
            Book book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            BookReviewViewModel viewModel = await GetBookDetails(book);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("BookId,ReviewBody")] BookReviewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review();


                review.ReviewBody = viewModel.ReviewBody;
                ApplicationUser user = await _user.GetUserAsync(HttpContext.User);
                Book book = await _context.Books
                    .SingleOrDefaultAsync(m => m.Id == viewModel.BookId);

                if (book == null)
                {
                    return NotFound();
                }

                review.Book = book;
                review.Author = user;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                viewModel = await GetBookDetails(book);
            }


            return View(viewModel);
        }
        private async Task<BookReviewViewModel> GetBookDetails(Book book)
        {
            BookReviewViewModel viewModel = new BookReviewViewModel();
            ApplicationUser user = await _user.GetUserAsync(HttpContext.User);
            viewModel.Book = book;
            viewModel.Author = user;
            List<Review> reviews = await _context.Reviews
                .Where(m => m.Book == book).ToListAsync();

            viewModel.Reviews = reviews;
            return viewModel;
        }
        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,DateOfPublication,Publisher,FileName")] BookCoverModel Cbook)
        {
            string FileName = UploadFile(Cbook);
            Book book = new Book
            {
                Id = Cbook.Id,
                Title = Cbook.Title,
                Author = Cbook.Author,
                DateOfPublication = Cbook.DateOfPublication,
                Publisher = Cbook.Publisher,
                CoverImage = FileName
            };

            if (ModelState.IsValid)
            { 
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index");
        }
        private string UploadFile(BookCoverModel bookCoverModel)
        {
            string fileName = null;
            if (bookCoverModel.FileName != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + bookCoverModel.FileName.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    bookCoverModel.FileName.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,DateOfPublication,Publisher")] Book book)
        {
            if (id != book.Id)
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
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
