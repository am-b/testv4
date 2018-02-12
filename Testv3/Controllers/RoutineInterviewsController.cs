using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    public class RoutineInterviewsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: RoutineInterviews
        [Authorize(Roles = "Counselor")]
        public ActionResult Index(string searchStringName, string currentFilter, int? page)
        {
            GetCurrentUserInViewBag();

            try
            {
                int intPage = 1;
                int intPageSize = 10;
                int intTotalPageCount = 0;

                if (searchStringName != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringName = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringName = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringName;
                List<TestViewModel> StudentInventorylist = new List<TestViewModel>();
                int intSkip = (intPage - 1) * intPageSize;
                intTotalPageCount = db.Students
                    .Where(x => x.StudentFirstName.Contains(searchStringName))
                    .Count();

                var datalist = db.Students
                    .Where(x => x.StudentLastName.Contains(searchStringName) || x.StudentFirstName.Contains(searchStringName))
                    .OrderBy(x => x.StudentLastName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in datalist)
                {
                    TestViewModel pvm = new TestViewModel();
                    pvm.StudentFirstName = item.StudentFirstName;
                    pvm.StudentMiddleName = item.StudentMiddleName;
                    pvm.StudentLastName = item.StudentLastName;
                    pvm.UserID = item.UserID;

                    var completionDate =
                        (from ans in db.RoutineInterview
                         where ans.UserID == pvm.UserID
                         select new { CompletionDateInitialInterview = ans.CompletionDate })
                         .ToList();

                    RoutineInterview routine = db.RoutineInterview.FirstOrDefault(x => x.UserID == pvm.UserID);


                    if (completionDate.Count() != 0)
                    {
                        pvm.CompletionDateInitialInterview = routine.CompletionDate;
                    }


                    StudentInventorylist.Add(pvm);
                }

                // Set the number of pages
                var _UserAsIPagedList =
                    new StaticPagedList<TestViewModel>
                    (
                        StudentInventorylist, intPage, intPageSize, intTotalPageCount
                        );

                return View(_UserAsIPagedList);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<TestViewModel> StudentInventorylist = new List<TestViewModel>();

                return View(StudentInventorylist.ToPagedList(1, 25));
            }

        }

        // GET: RoutineInterviews/Details/5
        [Authorize(Roles = "Counselor")]
        public ActionResult Details(string UserID)
        {
            GetCurrentUserInViewBag();
            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(UserID);
            if (student == null)
            {
                return HttpNotFound();
            }

            RoutineInterview routine = db.RoutineInterview.FirstOrDefault(user => user.UserID == UserID);

            if (routine == null)
            {
                return HttpNotFound();
            }
            if (routine == null)
            {
                return HttpNotFound();
            }

            StudentInterviewViewModel vm = new StudentInterviewViewModel();

            vm.UserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            vm.CompletionDate = routine.CompletionDate;
            vm.Q1 = routine.Q1;
            vm.Q2 = routine.Q2;
            vm.Q3 = routine.Q3;
            vm.Q4 = routine.Q4;
            vm.Q5 = routine.Q5;
            vm.OtherMatters = routine.OtherMatters;


            ViewBag.DateCompleted = vm.CompletionDate;

            return View(vm);
        }

        // GET: RoutineInterviews/Create
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            var check = db.RoutineInterview
                .Where(x => x.UserID == currentUserId && x.CompletionDate != null)
                .ToList();

            if (check.Count() != 0)
            {
                TempData["Message"] = "You have already completed the Routine Interview!";
                return RedirectToAction("Index", "Home");
            }


            var newid = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (newid == null)
            {
                newid = db.Students.Create();
                newid.UserID = currentUserId;
                db.Students.Add(newid);

            }

            Student student = db.Students.Find(newid.UserID);
            if (student == null)
            {
                return HttpNotFound();
            }

            var routineInterview = db.RoutineInterview.FirstOrDefault(d => d.UserID == currentUserId);

            if (routineInterview == null)
            {
                routineInterview = db.RoutineInterview.Create();
                routineInterview.UserID = currentUserId;
                db.RoutineInterview.Add(routineInterview);
                db.SaveChanges();
            }

            RoutineInterview routine = db.RoutineInterview.FirstOrDefault(user => user.UserID == currentUserId);

            if (routine == null)
            {
                return HttpNotFound();
            }

            StudentInterviewViewModel vm = new StudentInterviewViewModel();

            vm.UserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            vm.CompletionDate = routine.CompletionDate;
            vm.Q1 = routine.Q1;
            vm.Q2 = routine.Q2;
            vm.Q3 = routine.Q3;
            vm.Q4 = routine.Q4;
            vm.Q5 = routine.Q5;
            vm.OtherMatters = routine.OtherMatters;

            return View(vm);
        }

        // POST: RoutineInterviews/Create
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student(RoutineInterview vm)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (u == null)
            {
                u = db.Students.Create();
                u.UserID = currentUserId;
                db.Students.Add(u);

            }

            Student student = db.Students.Find(u.UserID);
            if (student == null)
            {
                return HttpNotFound();
            }

            var routineInterview = db.RoutineInterview.FirstOrDefault(d => d.UserID == currentUserId);

            if (routineInterview == null)
            {
                routineInterview = db.RoutineInterview.Create();
                routineInterview.UserID = currentUserId;
                db.RoutineInterview.Add(routineInterview);
                db.SaveChanges();
            }

            RoutineInterview routine = db.RoutineInterview.FirstOrDefault(user => user.UserID == currentUserId);

            if (routine == null)
            {
                return HttpNotFound();
            }


            if (ModelState.IsValid)
            {

                routine.CompletionDate = DateTime.Now;
                routine.Q1 = vm.Q1;
                routine.Q2 = vm.Q2;
                routine.Q3 = vm.Q3;
                routine.Q4 = vm.Q4;
                routine.Q5 = vm.Q5;
                routine.OtherMatters = vm.OtherMatters;

                db.SaveChanges();

                TempData["Message"] = "User: " + userName + "'s, routine interview successfully saved!";
            }
            else
            {
                TempData["Error"] = "Error: Entry not saved!";
            }

            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
