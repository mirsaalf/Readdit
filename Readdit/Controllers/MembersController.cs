
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
            // Retrieve the list of books from the database
            var books = await _context.Books.ToListAsync();

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




        // GET: Members/ToBeRead
        public async Task<IActionResult> ToBeRead()
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

            var viewModel = new MembersViewModel
            {
                ToBeReadBooks = toBeReadBooks
            };

            return View(viewModel);
        }


        // POST: Members/AddToToBeReadList
        [HttpPost]
        public async Task<IActionResult> AddToToBeReadList([FromBody] Book book)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Where(ub => ub.UserId == user.Id && ub.BookId == book.book_id)
                .FirstOrDefaultAsync();

            if (userBook == null)
            {
                userBook = new UserBook
                {
                    UserId = user.Id,
                    BookId = book.book_id,
                    Status = BookStatus.ToBeRead
                };
                _context.UserBooks.Add(userBook);
            }
            else
            {
                userBook.Status = BookStatus.ToBeRead;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }




        // GET: Members/CurrentlyReading
        public async Task<IActionResult> CurrentlyReading()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userId = user.Id;
            var currentlyReadingBooks = await _context.UserBooks
                .Include(ub => ub.Book)
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.CurrentlyReading)
                .Select(ub => ub.Book)
                .ToListAsync();

            var viewModel = new MembersViewModel
            {
                CurrentlyReadingBooks = currentlyReadingBooks
            };

            return View(viewModel);
        }

        // POST: Members/MoveToCurrentlyReading
        [HttpPost]
        public async Task<IActionResult> MoveToCurrentlyReading(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.BookId == id && ub.UserId == userId);

            if (userBook != null)
            {
                userBook.Status = BookStatus.CurrentlyReading;
                _context.UserBooks.Update(userBook);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }


        // Action method to move a book to the Completed Reading list
        // POST: Members/MoveToCompletedReading
        [HttpPost]
        public async Task<IActionResult> MoveToCompletedReading(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.BookId == id && ub.UserId == userId);

            if (userBook != null)
            {
                userBook.Status = BookStatus.Completed;
                _context.UserBooks.Update(userBook);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }


        // GET: Members/CompletedReading
        public async Task<IActionResult> CompletedReading()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userId = user.Id;
            var completedReadingBooks = await _context.UserBooks
                .Include(ub => ub.Book)
                .Where(ub => ub.UserId == userId && ub.Status == BookStatus.Completed)
                .Select(ub => ub.Book)
                .ToListAsync();

            var viewModel = new MembersViewModel
            {
                CompletedReadingBooks = completedReadingBooks
            };

            return View(viewModel);
        }



        // POST: Members/RemoveFromList
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromList(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userId = user.Id;
            var userBook = await _context.UserBooks
                .Where(ub => ub.UserId == userId && ub.BookId == bookId)
                .FirstOrDefaultAsync();

            if (userBook != null)
            {
                _context.UserBooks.Remove(userBook);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.book_id == id);
        }
    }
}
