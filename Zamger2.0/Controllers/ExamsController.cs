using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zamger2._0.Data;
using Zamger2._0.Helpers;
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
            
            var exams = await _context.Exams.Where(t => t.Subject.Profesor.Id == User.GetLoggedInUserId<string>()).ToListAsync();
            if (exams != null)
            {
                foreach (Exam exam in exams)
                {
                    Subject s = new Subject();
                    s = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == exam.SubjectId);
                    if (s != null) {
                        exam.Subject = s;
                    }
                    
                }
                
            }
            return View(exams);
        }
        [Authorize(Roles = "student")]
        public async Task<IActionResult> IndexStudent()
        {
            
            var exams = await _context.Exams.Where(x => x.Deadline >= DateTime.Now).ToListAsync();
            if (exams != null)
            {
                foreach (Exam exam in exams)
                {
                    Subject s = new Subject();
                    s = _context.Subjects.FirstOrDefault(m => m.Id == exam.SubjectId);

                    var examSignUps = _context.ExamSignUps.Where(m => m.Exam.Id == exam.Id).ToList();
                    if (examSignUps != null)
                    {
                        foreach (ExamSignUp signup in examSignUps)
                        {
                            IdentityUser u = new IdentityUser();
                            u = await _context.Users.FirstOrDefaultAsync(m => m.Id.Equals(signup.StudentId));
                            if (u != null) {
                                signup.Student = u;
                            }
                            
                            
                        }
                        exam.ExamSignUps = examSignUps;
                    }

                    if (s != null) {
                        exam.Subject = s;
                    }
                   
                    
                    ViewBag.Current = User.GetLoggedInUserId<string>();

                    //var a = exam.ExamSignUps.FirstOrDefault(m => m.Student.Id.Equals(currentUserID));
                    
                    
                    
                    
                }

            }
            

            



            return View(exams);
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> SignUp(int id)
        {
            

           
            var exist = _context.ExamSignUps.FirstOrDefault(m => m.ExamId == id && m.StudentId == User.GetLoggedInUserId<string>());
            var exam = _context.Exams.FirstOrDefault(m => m.Id == id);
            if (exam!=null && exam.Deadline > DateTime.Now) {
                if (exist == null)
                {
                    _context.ExamSignUps.Add(new ExamSignUp()
                    {
                        ExamId = id,
                        StudentId = User.GetLoggedInUserId<string>(),
                        Time = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                }
            }
           
            

            return RedirectToAction(nameof(IndexStudent));
            
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> SignOut(int id)
        {

            
            var exist = _context.ExamSignUps.FirstOrDefault(m => m.ExamId == id && m.StudentId == User.GetLoggedInUserId<string>());
            var exam = _context.Exams.FirstOrDefault(m => m.Id == id);
            if (exam != null && exam.Deadline > DateTime.Now)
            {
                if (exist != null)
                {

                    var examSignUp = await _context.ExamSignUps.FindAsync(exist.Id);
                    _context.ExamSignUps.Remove(examSignUp);
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction(nameof(IndexStudent));

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
                    if (u != null) {
                        signup.Student = u;
                    }
                    
                }
                exam.ExamSignUps = examSignUps;
            }
            if (exam != null)
            {

                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Id==exam.SubjectId);
                if (s != null) {
                    exam.Subject = s;
                }
                
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
            
            List<Subject> results = new List<Subject>();
            results.AddRange(await _context.Subjects.Where(t => t.Profesor.Id == User.GetLoggedInUserId<string>()).ToListAsync());
            var subjects = new List<string>();
            if (results != null) {
                
                foreach (Subject s in results)
                {
                    subjects.Add(s.Name);
                }
                
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
            if (!ModelState.IsValid)
            {
                
                List<Subject> results = new List<Subject>();
                results.AddRange(await _context.Subjects.Where(t => t.Profesor.Id == User.GetLoggedInUserId<string>()).ToListAsync());
                var subjects = new List<string>();
                if (results != null) {
                    foreach (Subject s in results)
                    {
                        subjects.Add(s.Name);
                    }
                }
                
                
                var selectListItems = subjects.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                var model = new ExamCreateViewModel();
                model.Subjects = selectListItems;
                model.Subject = selectListItems.First().Value;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Name == exam.Subject);
                if (s != null) {
                    _context.Exams.Add(new Exam()
                    {

                        Name = exam.Name,
                        Deadline = exam.Deadline,
                        Time = exam.Time,
                        Subject = s
                    });
                }
                

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Edit/5
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            var exam = await _context.Exams
              .FirstOrDefaultAsync(h => h.Id == id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
            if (exam == null)
            {
                return NotFound();
            }
            List<Subject> results = new List<Subject>();
            results.AddRange(await _context.Subjects.Where(t => t.Profesor.Id == User.GetLoggedInUserId<string>()).ToListAsync());
            var subjects = new List<string>();
            if (results != null) {
                foreach (Subject s in results)
                {
                    subjects.Add(s.Name);
                }
            }
            
            
            var selectListItems = subjects.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            var model = new ExamCreateViewModel();
            model.Subjects = selectListItems;
            model.Subject = selectListItems.First().Value;
            model.Name = exam.Name;
            model.Deadline = exam.Deadline;
           
            return View(model);
           
      
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Time,Deadline,Subject")] ExamCreateViewModel e)
        {
            if (!ModelState.IsValid)
            {
                
                List<Subject> results = new List<Subject>();
                results.AddRange(await _context.Subjects.Where(t => t.Profesor.Id == User.GetLoggedInUserId<string>()).ToListAsync());

                var subjects = new List<string>();

                if (results != null) {
                    foreach (Subject su in results)
                    {
                        subjects.Add(su.Name);
                    }
                }
                
                var selectListItems = subjects.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                var model = new ExamCreateViewModel();
                model.Subjects = selectListItems;
                model.Subject = selectListItems.First().Value;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var exam = await _context.Exams
              .FirstOrDefaultAsync(h => h.Id == id && h.Subject.ProfesorId == User.GetLoggedInUserId<string>());
                if (exam == null)
                {
                    return NotFound();
                }
                

                exam.Name = e.Name;
                exam.Deadline = e.Deadline;
                exam.Time = e.Time;
                Subject s = new Subject();
                s = await _context.Subjects.FirstOrDefaultAsync(m => m.Name == e.Subject);
                if (s != null) {
                    exam.SubjectId = s.Id;
                }
               


                await _context.SaveChangesAsync();
               
            }

            return RedirectToAction(nameof(Index));
            
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
            if (exam != null) {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }


        
    }
}
