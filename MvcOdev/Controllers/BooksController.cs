using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcOdev.Data;
using MvcOdev.Models;

namespace MvcOdev.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string aramaMetni, int? kategoriId, string siralama)
        {
            ViewBag.CurrentFilter = aramaMetni;
            ViewBag.CurrentCategory = kategoriId;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(siralama) ? "price_desc" : "";

            var kitaplarSorgusu = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                kitaplarSorgusu = kitaplarSorgusu.Where(b => b.Title.Contains(aramaMetni) || b.Author.Contains(aramaMetni));
            }

            if (kategoriId.HasValue)
            {
                kitaplarSorgusu = kitaplarSorgusu.Where(b => b.CategoryId == kategoriId);
            }

            switch (siralama)
            {
                case "price_desc":
                    kitaplarSorgusu = kitaplarSorgusu.OrderByDescending(b => b.Price);
                    break;
                default:
                    kitaplarSorgusu = kitaplarSorgusu.OrderBy(b => b.Price);
                    break;
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(await kitaplarSorgusu.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kitap = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kitap == null)
            {
                return NotFound();
            }

            return View(kitap);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Book yeniKitap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yeniKitap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", yeniKitap.CategoryId);
            return View(yeniKitap);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kitap = await _context.Books.FindAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", kitap.CategoryId);
            return View(kitap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Book guncelKitap)
        {
            if (ModelState.IsValid)
            {
                _context.Update(guncelKitap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", guncelKitap.CategoryId);
            return View(guncelKitap);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kitap = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kitap == null)
            {
                return NotFound();
            }

            return View(kitap);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kitap = await _context.Books.FindAsync(id);
            if (kitap != null)
            {
                _context.Books.Remove(kitap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}