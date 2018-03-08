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
    public class ExitInterviewsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: ExitInterviews
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
                    .Where(x => x.StudentLastName.Contains(searchStringName) || x.StudentFirstName.Contains(searchStringName) || x.StudentID.Contains(searchStringName))
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
                        (from ans in db.ExitInterview
                         where ans.StudentUserID == pvm.UserID
                         select new { CompletionDate = ans.CompletionDate })
                         .ToList();

                    ExitInterview exit = db.ExitInterview.FirstOrDefault(x => x.StudentUserID == pvm.UserID);


                    if (completionDate.Count() != 0)
                    {
                        pvm.CompletionDate = exit.CompletionDate;
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

        // GET: ExitInterviews/Details/5
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

            ExitInterview exit = db.ExitInterview.FirstOrDefault(user => user.StudentUserID == UserID);

            if (exit == null)
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

            vm.CompletionDate = exit.CompletionDate;
            vm.MMCCLikes = exit.MMCCLikes;
            vm.MMCCDislikes = exit.MMCCDislikes;
            vm.MMCCMoments = exit.MMCCMoments;
            vm.Professors = exit.Professors;
            vm.Staff = exit.Staff;
            vm.Future = exit.Future;
            vm.Others = exit.Others;
            vm.GuidanceNotes = exit.GuidanceNotes;

            ViewBag.DateCompleted = vm.CompletionDate;

            return View(vm);
        }

        // POST: ExitInterviews/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Details([Bind(Include = "GuidanceNotes")] StudentInterviewViewModel vm, string UserID)
        {
            GetCurrentUserInViewBag();

            var u = db.Students.FirstOrDefault(d => d.UserID == UserID);

            if (u == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Student student = db.Students.Find(u.UserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            var exitInterview = db.ExitInterview.FirstOrDefault(d => d.StudentUserID == UserID);

            if (exitInterview == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExitInterview exit = db.ExitInterview.FirstOrDefault(user => user.StudentUserID == UserID);

            if (exit == null)
            {
                return HttpNotFound();
            }

            exit.GuidanceNotes = vm.GuidanceNotes;
            int result = db.SaveChanges();
            if (result > 0)
            {
                TempData["Message"] = "Counselor notes successfully saved!";
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;
            }

            return RedirectToAction("Index", "Home");

        }


        // GET: InitialInterviews/Student
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            var check = db.ExitInterview
                .Where(x => x.StudentUserID == currentUserId && x.CompletionDate != null)
                .ToList();

            if (check.Count() != 0)
            {
                TempData["Message"] = "You have already completed the Exit Interview!";
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

            var exitInterview = db.ExitInterview.FirstOrDefault(d => d.StudentUserID == currentUserId);

            if (exitInterview == null)
            {
                exitInterview = db.ExitInterview.Create();
                exitInterview.StudentUserID = currentUserId;
                db.ExitInterview.Add(exitInterview);
                db.SaveChanges();
            }

            ExitInterview exit = db.ExitInterview.FirstOrDefault(user => user.StudentUserID == currentUserId);

            if (exit == null)
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

            vm.CompletionDate = exit.CompletionDate;
            vm.MMCCLikes = exit.MMCCLikes;
            vm.MMCCDislikes = exit.MMCCDislikes;
            vm.MMCCMoments = exit.MMCCMoments;
            vm.Professors = exit.Professors;
            vm.Staff = exit.Staff;
            vm.Future = exit.Future;
            vm.Others = exit.Others;

            return View(vm);
        }

        // POST: InitialInterviews/Student
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Student([Bind(Include = "MMCCLikes,MMCCDislikes,MMCCMoments,Professors,Staff,Future,Others,CompletionDate")]StudentInterviewViewModel vm)
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

            var exitInterview = db.ExitInterview.FirstOrDefault(d => d.StudentUserID == currentUserId);

            if (exitInterview == null)
            {
                exitInterview = db.ExitInterview.Create();
                exitInterview.StudentUserID = currentUserId;
                db.ExitInterview.Add(exitInterview);
                db.SaveChanges();
            }

            ExitInterview exit = db.ExitInterview.FirstOrDefault(user => user.StudentUserID == currentUserId);

            if (exit == null)
            {
                return HttpNotFound();
            }

                exit.CompletionDate = DateTime.Now;
                exit.MMCCLikes = vm.MMCCLikes;
                exit.MMCCDislikes = vm.MMCCDislikes;
                exit.MMCCMoments = vm.MMCCMoments;
                exit.Professors = vm.Professors;
                exit.Staff = vm.Staff;
                exit.Future = vm.Future;
                exit.Others = vm.Others;

                int result = db.SaveChanges();
                
                if (result > 0)
                {
                    TempData["Message"] = "User: " + userName + "'s, exit interview successfully saved!";
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    TempData["Error"] = errors;
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
