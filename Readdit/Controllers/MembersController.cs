
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readdit.Areas.Identity.Data;
using Readdit.Models;
using System.Configuration;

namespace Readdit.Controllers
{
    public class MembersController : Controller
    {
        private readonly ReadditContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersController(ReadditContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var books = await _context.UserBooks.ToListAsync();
            return View(books);
        }

        // GET: Members/Details
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userId = user.Id;

            var toBeReadBooks = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.ToBeRead)
                .Select(ub => ub.Book)
                .ToListAsync();

            var currentlyReadingBooks = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.CurrentlyReading)
                .Select(ub => ub.Book)
                .ToListAsync();

            var completedReadingBooks = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.Completed)
                .Select(ub => ub.Book)
                .ToListAsync();

            var viewModel = new MembersViewModel
            {
                ToBeReadBooks = toBeReadBooks,
                CurrentlyReadingBooks = currentlyReadingBooks,
                CompletedReadingBooks = completedReadingBooks
            };

            return View(viewModel);
        }


        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Pages,Genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Members/ToBeRead
        public async Task<IActionResult> ToBeRead()
        {
            // Retrieve the books marked as "To Be Read" for the current user
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id; // Implement the method to get the current user's ID
            var toBeReadBooks = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.ToBeRead)
                .Select(ub => ub.Book)
                .ToListAsync();

            return View(toBeReadBooks);
        }

        public async Task<IActionResult> CurrentlyReading()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var currentlyReadingBooks = await _context.UserBooks
            .Include(ub => ub.Book)
            .Where(ub => ub.UserId == userId && ub.Status == BookStatus.CurrentlyReading)
            .Select(ub => ub.Book) // Select the Book object directly
            .ToListAsync();

            var viewModel = new MembersViewModel
            {
                CurrentlyReadingBooks = currentlyReadingBooks
            };

            return View(viewModel);
        }


        // Action method to move a book to the Completed Reading list
        [HttpPost]
        public async Task<IActionResult> MoveToCompletedReading(int id)
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            // Find the user's book record with the given book ID
            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.BookId == id && ub.UserId == userId);

            if (userBook != null)
            {
                // Update the status of the book to CompletedReading
                userBook.Status = BookStatus.Completed;

                _context.UserBooks.Update(userBook);
                await _context.SaveChangesAsync();

                return Ok(); // Return a success response
            }

            return NotFound(); // Book not found for the user
        }

        public async Task<IActionResult> CompletedReading()
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            // Retrieve the books that the user has marked as completed reading
            var completedReadingBooks = await _context.UserBooks
                .Include(ub => ub.Book)
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.Completed)
                .Select(ub => ub.Book)
                .ToListAsync();

            return View(completedReadingBooks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromList(int bookId)
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            // Find the UserBook entry corresponding to the book and user
            var userBook = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.BookId == bookId)
                .FirstOrDefaultAsync();

            // If the UserBook entry exists, remove it from the database
            if (userBook != null)
            {
                _context.UserBooks.Remove(userBook);
                await _context.SaveChangesAsync();
            }

            // Redirect back to the same page or another appropriate page
            return RedirectToAction(nameof(CompletedReading));
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.book_id == id);
        }
    }
}
