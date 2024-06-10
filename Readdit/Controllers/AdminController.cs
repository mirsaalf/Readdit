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
        public async Task<IActionResult> Create([Bind("book_name,author_name,book_genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.date_added = DateTime.Now;  // Ensure date_added is set
                _context.Add(book);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Book successfully added!";
                return View();
            }

            return View(book);
        }






        // GET: Admin/Delete
        public async Task<IActionResult> Delete()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // POST: Admin/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{book.book_name} was removed successfully";

            return RedirectToAction(nameof(Delete));
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

        // POST: Admin/UpdateUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUsers(List<ApplicationUser> users)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var user in users)
                    {
                        var existingUser = await _context.Users.FindAsync(user.Id);
                        if (existingUser != null)
                        {
                            _context.Entry(existingUser).CurrentValues.SetValues(user);
                        }
                    }
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Users updated successfully";
                    return RedirectToAction(nameof(Update));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating users: {ex.Message}");
                }
            }
            var viewModel = new UpdateViewModel
            {
                Users = users,
                Books = _context.Books.ToList()
            };
            return View("Update", viewModel);
        }


        // POST: Admin/UpdateBooks
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBooks(List<Book> books)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var book in books)
                    {
                        var existingBook = await _context.Books.FindAsync(book.book_id);
                        if (existingBook != null)
                        {
                            _context.Entry(existingBook).CurrentValues.SetValues(book);
                        }
                    }
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Books updated successfully";
                    return RedirectToAction(nameof(Update));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Concurrency error occurred while updating books.");
                }
            }
            var viewModel = new UpdateViewModel
            {
                Users = _context.Users.ToList(),
                Books = _context.Books.ToList()
            };
            return View("Update", viewModel);
        }


        // Method to check if a book exists
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.book_id == id);
        }


        // GET: Admin/Details
        public async Task<IActionResult> Details()
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


        // GET: Admin/Users
        public async Task<IActionResult> Users()
                {
                    var users = await _userManager.Users.ToListAsync();
                    return View(users);
                }

        // GET: Admin/DeleteUser
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

        // POST: Admin/DeleteUser
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


