using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zamger2._0.Data;
using Zamger2._0.Models;

namespace Zamger2._0.Controllers
{
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exams
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var exams = await _context.Exams.Where(t => t.Subject.Profesor.Id == currentUserID).ToListAsync();
            if (exams != null)
            {
                foreach (Exam exam in exams)
                {
                    Subject s = new Subject();
                    s = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == exam.SubjectId);

                    exam.Subject = s;
                }
                
            }
            return View(exams);
        }
        [Authorize(Roles = "student")]
        public async Task<IActionResult> IndexStudent()
        {
            
            return View();
        }

        [Authorize(Roles = "profesor")]
        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.Id == id);

            var examSignUps = await _context.ExamSignUps
                .Where(m => m.Exam.Id == id).ToListAsync();

            if (examSignUps != null) {
                foreach (ExamSignUp signup in examSignUps)
                {
                    IdentityUser u = new IdentityUser();
                    u = await _context.Users.FirstOrDefaultAsync(m => m.Id.Equals(signup.StudentId));
                    signup.Student = u;
                }
                exam.ExamSignUps = examSignUps;
            }
            if (exam != null)
            {

                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Id==exam.SubjectId);

                exam.Subject = s;
            }

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        [Authorize(Roles = "profesor")]
        // GET: Exams/Create
        public async Task<IActionResult> CreateAsync()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Subject> results = new List<Subject>();
            results.AddRange(await _context.Subjects.Where(t => t.Profesor.Id == currentUserID).ToListAsync());

            var subjects = new List<string>();
            foreach (Subject s in results)
            {
                subjects.Add(s.Name);
            }
            var selectListItems = subjects.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            var model = new ExamCreateViewModel();
            model.Subjects = selectListItems;
            model.Subject = selectListItems.First().Value;
            return View(model);
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Create(ExamCreateViewModel exam)
        {
            if (ModelState.IsValid)
            {
                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Name == exam.Subject);
                _context.Exams.Add(new Exam()
                {

                    Name = exam.Name,
                    Deadline = exam.Deadline,
                    Time = exam.Time,
                    Subject = s
                });

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Edit/5
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Time,Deadline")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(exam);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
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
            return View(exam);
        }

        // GET: Exams/Delete/5
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exams/Delete/5
        [Authorize(Roles = "profesor")]
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
