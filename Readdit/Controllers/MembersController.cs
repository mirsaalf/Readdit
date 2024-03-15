
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readdit.Areas.Identity.Data;
using Readdit.Models;

public class MembersController : Controller
{
    private readonly ReadditContext _context;

    public MembersController(ReadditContext context)
    {
        _context = context;
    }

    // GET: Members
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.ToListAsync();
        return View(books);
    }

    // GET: Members/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FirstOrDefaultAsync(m => m.book_id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
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

    [HttpPost]
    public async Task<IActionResult> AddToToBeReadList(int book_id)
    {
        try
        {
            // Find the book by its ID
            var book = await _context.Books.FindAsync(book_id);

            if (book == null)
            {
                // If the book with the given ID is not found, return a 404 Not Found response
                return NotFound();
            }

            // Add the book to the user's "To Be Read" list

            // Redirect to the ToBeRead page after adding the book to the list
            return RedirectToAction("ToBeRead", "Members");
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return a 500 Internal Server Error response
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

// GET: Members/Edit/5
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

    // POST: Members/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Pages,Genre")] Book book)
    {
        if (id != book.book_id)
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
                if (!BookExists(book.book_id))
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

    // GET: Members/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FirstOrDefaultAsync(m => m.book_id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Members/Delete/5
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
        return _context.Books.Any(e => e.book_id == id);
    }
}
