using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Zamger2._0.Controllers
{
    public class HomeworkController : Controller
    {
        // GET: HomeworkController
        public ActionResult IndexProfesor()
        {
            return View();
        }

        // GET: HomeworkController
        public ActionResult IndexStudent()
        {
            return View();
        }

        // GET: HomeworkController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeworkController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeworkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: HomeworkController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeworkController/Edit/5
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

        // GET: HomeworkController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeworkController/Delete/5
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
