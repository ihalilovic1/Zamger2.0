using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zamger2._0.Data;
using Zamger2._0.Helpers;
using Zamger2._0.Models;

namespace Zamger2._0.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeworkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Homework
        public IActionResult Index()
        {
            var homeworks = _context.Homeworks
                .Include(h => h.Subject)
                .Where(h => h.Deadline > DateTime.Now);
            if (User.IsInRole("profesor"))
            {
                return View(homeworks.Where(h => h.Subject.ProfesorId == User.GetLoggedInUserId<string>()));
            }

            return View(homeworks);
        }

        // GET: Homework/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var homework = await _context.Homeworks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (homework == null || (User.IsInRole("profesor") && homework.Subject.ProfesorId != User.GetLoggedInUserId<string>()))
            {
                return NotFound();
            }

            return View(homework);
        }

        // GET: Homework/Create
        [Authorize(Roles = "profesor")]
        public IActionResult Create()
        {
            var model = new HomeworkViewModel
            {
                Subjects = new SelectList(_context.Subjects.Where(s => s.ProfesorId == User.GetLoggedInUserId<string>()), "Id", "Name"),
                Deadline = DateTime.Now
            };

            return View(model);
        }

        // POST: Homework/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "profesor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Deadline,SubjectId")] HomeworkViewModel homeworkViewModel)
        {
            if (!ModelState.IsValid)
            {
                homeworkViewModel.Subjects = new SelectList(_context.Subjects.Where(s => s.ProfesorId == User.GetLoggedInUserId<string>()), "Id", "Name");
                return View(homeworkViewModel);
            }

            var subject = await _context.Subjects.FindAsync(homeworkViewModel.SubjectId);

            if (subject == null || subject.ProfesorId != User.GetLoggedInUserId<string>())
            {
                ModelState.AddModelError("Subject", "Subject not found");
                homeworkViewModel.Subjects = new SelectList(_context.Subjects.Where(s => s.ProfesorId == User.GetLoggedInUserId<string>()), "Id", "Name");
                return View(homeworkViewModel);
            }

            var homework = new Homework
            {
                Name = homeworkViewModel.Name,
                Deadline = homeworkViewModel.Deadline,
                SubjectId = homeworkViewModel.SubjectId
            };

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = homework.Id });

        }

        // GET: Homework/Edit/5
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int id)
        {
            var homework = await _context.Homeworks
               .Include(h => h.Subject)
               .FirstOrDefaultAsync(h => h.Id == id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
            if (homework == null)
            {
                return NotFound();
            }

            var homeworkViewModel = new HomeworkViewModel
            {
                Id = homework.Id,
                Name = homework.Name,
                Deadline = homework.Deadline,
                SubjectId = homework.SubjectId,
                Subjects = new SelectList(_context.Subjects, "Id", "Name", homework.SubjectId)
            };
     
            return View(homeworkViewModel);
        }

        // POST: Homework/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Deadline,SubjectId")] HomeworkViewModel homeworkViewModel)
        {

            if (!ModelState.IsValid)
            {
                homeworkViewModel.Subjects = new SelectList(_context.Subjects.Where(s => s.ProfesorId == User.GetLoggedInUserId<string>()), "Id", "Name");
                return View(homeworkViewModel);
            }
            var homework = await _context.Homeworks
               .Include(h => h.Subject)
               .FirstOrDefaultAsync(h => h.Id == homeworkViewModel.Id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
            if (homework == null)
            {
                return NotFound();
            }
            homework.Name = homeworkViewModel.Name;
            homework.SubjectId = homeworkViewModel.SubjectId;
            homework.Deadline = homeworkViewModel.Deadline;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = homeworkViewModel.Id });
        }

        // GET: Homework/Delete/5
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Delete(int id)
        {
            var homework = await _context.Homeworks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(h => h.Id == id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        // POST: Homework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homework = await _context.Homeworks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(h => h.Id == id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
            if (homework == null)
            {
                return NotFound();
            }
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
