using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspMVC.Data;
using AspMVC.Models;

namespace AspMVC.Areas.Project
{
    [Area("Project")]
    [Route("/project/[action]")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Project/Project
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProjectPage.Include(p => p.Genre);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Project/Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectPage == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPage
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        // GET: Project/Project/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }

        // POST: Project/Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Slug,GenreId")] ProjectPageModel projectPageModel)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(projectPageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            return View(projectPageModel);
        }

        // GET: Project/Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectPage == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPage.FindAsync(id);
            if (projectPageModel == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            return View(projectPageModel);
        }

        // POST: Project/Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,GenreId")] ProjectPageModel projectPageModel)
        {
            if (id != projectPageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectPageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectPageModelExists(projectPageModel.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", projectPageModel.GenreId);
            return View(projectPageModel);
        }

        // GET: Project/Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectPage == null)
            {
                return NotFound();
            }

            var projectPageModel = await _context.ProjectPage
                .Include(p => p.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPageModel == null)
            {
                return NotFound();
            }

            return View(projectPageModel);
        }

        // POST: Project/Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectPage == null)
            {
                return Problem("Entity set 'AppDbContext.ProjectPage'  is null.");
            }
            var projectPageModel = await _context.ProjectPage.FindAsync(id);
            if (projectPageModel != null)
            {
                _context.ProjectPage.Remove(projectPageModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectPageModelExists(int id)
        {
          return (_context.ProjectPage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
