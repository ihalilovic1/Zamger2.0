using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zamger2._0.Data;
using Zamger2._0.Models;

namespace Zamger2._0.Controllers
{
    public class ExamController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Exam
        public ActionResult IndexProfesor()
        {

            return View();
        }
        // GET: Exam
        public ActionResult IndexStudent()
        {

            return View();
        }

        // GET: Exam/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Exam/Create
        [Authorize(Roles = "profesor")]
        public async Task<ActionResult> CreateAsync()
        {
            List < Subject > results = new List<Subject>();
            results.AddRange(await _context.Subjects.ToListAsync());
            
            var subjects = new List<string>();
            foreach (Subject s in results) {
                subjects.Add(s.Name);
            }
            var selectListItems = subjects.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            var model = new ExamViewModel();
            model.Subjects = selectListItems;
            model.Subject = selectListItems.First().Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ExamViewModel exam)
        {
            if (ModelState.IsValid)
            {
                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Name == exam.Subject);
                _context.Exams.Add(new Exam()
                {

                    Name = exam.Name,
                    Deadline = exam.Deadline,
                    Time = DateTime.Now,
                    Subject = s
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exam/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Exam/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exam/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
