using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readdit.Areas.Identity.Data;
using Readdit.Models;
using SQLitePCL;
using System.Linq;
using System.Threading.Tasks;

namespace Readdit.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ReadditContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ReadditContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            ViewBag.Books = _context.Books.ToList();

        }

        // GET: Admin
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("book_id,book_name,book_genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Welcome to the Create page!";
                return View();
            }
            return View(book);
        }

        // GET: Admin/Delete
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.book_id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ReadditContext.Books' is null");

            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{book.book_name} was removed successfully";
            }
            else
            {
                TempData["Message"] = $"{book.book_name} has already been removed";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Update
        public async Task<IActionResult> Update(int? id)
        {
            // Retrieve the list of users and books from database
            var users = _context.Users.ToList();
            var books = _context.Books.ToList();

            // Create a view model to pass both lists to the view
            var viewModel = new UpdateViewModel
            {
                Users = users,
                Books = books
            };

            // Pass the view model to the view
            return View(viewModel);
        }

        // POST: Admin/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("book_id,book_name,author_name,book_genre")] Book book)
        {
            if (id != book.book_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing book from the database
                    var existingBook = await _context.Books.FindAsync(id);

                    // Update the properties of the existing book with values from the updated book
                    existingBook.book_name = book.book_name;
                    existingBook.author_name = book.author_name;
                    existingBook.book_genre = book.book_genre;

                    // Update the book in the database
                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.book_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(book);
        }


        // GET: Admin/Details
        public async Task<IActionResult> Details()
        {
            // Retrieve user details with associated books
            var usersWithBooks = await _context.Users
                .Include(u => u.UserBooks)
                .ThenInclude(ub => ub.Book)
                .ToListAsync();

            // Create a list of UserDetailsViewModel objects
            var userDetailsViewModels = new List<UserDetailsViewModel>();

            // Populate the view models
            foreach (var user in usersWithBooks)
            {
                var viewModel = new UserDetailsViewModel
                {
                    User = user,
                    UserBooks = user.UserBooks.ToList()
                };
                userDetailsViewModels.Add(viewModel);
            }

            return View(userDetailsViewModels);
        }



        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.book_id == id);
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
                {
                    var users = await _userManager.Users.ToListAsync();
                    return View(users);
                }

        // GET: Admin/DeleteUser/5
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/Roles
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // GET: Admin/CreateRole
        public IActionResult CreateRole()
        {
            return View();
        }

        // POST: Admin/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole([Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Roles));
            }
            return View(role);
        }
    }
}
