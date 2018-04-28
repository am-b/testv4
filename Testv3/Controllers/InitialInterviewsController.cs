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
using Rotativa;
using System.IO;

namespace Testv3.Controllers
{
    public class InitialInterviewsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: InitialInterviews
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
                        (from ans in db.InitialInterview
                         where ans.UserID == pvm.UserID
                         select new { CompletionDate = ans.CompletionDate })
                         .ToList();

                    InitialInterview initial = db.InitialInterview.FirstOrDefault(x => x.UserID == pvm.UserID);


                    if (completionDate.Count() != 0)
                    {
                        pvm.CompletionDate = initial.CompletionDate;
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

        // GET: InitialInterviews/Details
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Details(string UserID)
        {
            GetCurrentUserInViewBag();

            InitialInterview check = db.InitialInterview.FirstOrDefault(x => x.UserID == UserID);
            if (check == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");
            }
            else if (check.CompletionDate == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");

            }

            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(UserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            InitialInterview initial = db.InitialInterview.FirstOrDefault(user => user.UserID == UserID);

            if (initial == null)
            {
                return HttpNotFound();
            }
            if (initial == null)
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

            vm.CompletionDate = initial.CompletionDate;
            vm.ReasonForProgram = initial.ReasonForProgram;
            vm.ReasonForMMCC = initial.ReasonForMMCC;
            vm.CollegeLifeAdjustments = initial.CollegeLifeAdjustments;
            vm.ChoiceOfProgramAdjustments = initial.ChoiceOfProgramAdjustments;
            vm.PeersAdjustments = initial.PeersAdjustments;
            vm.MMCCStaffAdjustments = initial.MMCCStaffAdjustments;
            vm.FamilyAdjustments = initial.FamilyAdjustments;
            vm.CounselorNotes = initial.CounselorNotes;

            ViewBag.DateCompleted = vm.CompletionDate;

            return View(vm);
        }


        // GET: InitialInterviews/Details
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Report(string UserID)
        {
            GetCurrentUserInViewBag();

            InitialInterview check = db.InitialInterview.FirstOrDefault(x => x.UserID == UserID);
            if (check == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");
            }
            else if (check.CompletionDate == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");

            }

            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(UserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            InitialInterview initial = db.InitialInterview.FirstOrDefault(user => user.UserID == UserID);

            if (initial == null)
            {
                return HttpNotFound();
            }
            if (initial == null)
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

            vm.CompletionDate = initial.CompletionDate;
            vm.ReasonForProgram = initial.ReasonForProgram;
            vm.ReasonForMMCC = initial.ReasonForMMCC;
            vm.CollegeLifeAdjustments = initial.CollegeLifeAdjustments;
            vm.ChoiceOfProgramAdjustments = initial.ChoiceOfProgramAdjustments;
            vm.PeersAdjustments = initial.PeersAdjustments;
            vm.MMCCStaffAdjustments = initial.MMCCStaffAdjustments;
            vm.FamilyAdjustments = initial.FamilyAdjustments;
            vm.CounselorNotes = initial.CounselorNotes;

            ViewBag.DateCompleted = vm.CompletionDate;

            return View(vm);
        }


        // POST: InitialInterviews/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Details([Bind(Include = "CounselorNotes")] StudentInterviewViewModel vm, string UserID )
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

            var initialInterview = db.InitialInterview.FirstOrDefault(d => d.UserID == UserID);

            if (initialInterview == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InitialInterview initial = db.InitialInterview.FirstOrDefault(user => user.UserID == UserID);

            if (initial == null)
            {
                return HttpNotFound();
            }

                initial.CounselorNotes = vm.CounselorNotes;
                db.SaveChanges();

                TempData["Message"] = "Counselor notes successfully saved!";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;

            return RedirectToAction("Index", "Home");

        }


        public ActionResult Print(string UserID)
        {
            ////var a = new ActionAsPdf("Details", new { UserID = UserID }) { FileName = "Details.pdf" };
            ////a.Cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);
            ////a.FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            ////a.CustomSwitches = "--load-error-handling ignore";
            ////return a;

            //////Dictionary<string, string> cookieCollection = new Dictionary<string, string>();

            //////foreach (var key in Request.Cookies.AllKeys)
            //////{
            //////    cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            //////}

            //////return new ActionAsPdf("Details", new { UserID = UserID })
            //////{
            //////    FileName = "Details.pdf",
            //////    Cookies = cookieCollection
            //////};

            var name = db.Students.Find(UserID);
            var student = name.StudentLastName + ", " + name.StudentFirstName;

            InitialInterview check = db.InitialInterview.FirstOrDefault(x => x.UserID == UserID);
            if (check == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");
            }
            else if (check.CompletionDate == null)
            {
                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("Index", "InitialInterviews");

            }
            //var datalist =
            //            (from ans in db.InitialInterview
            //             where ans.UserID == UserID
            //             select new { CompletionDate = ans.CompletionDate });

            //if (datalist.Count() == 0)
            //{

            //    TempData["Error"] = "This user has not completed the test yet!";
            //    return RedirectToAction("Index", "InitialInterviews");
            //}

            return new ActionAsPdf(
                           "Report",
                           new { UserID = UserID })
            {
                FileName = string.Format("Initial_Interview_{0}.pdf", student)
            };


            //var a = new ViewAsPdf();
            //a.ViewName = "Details";

            //Student student = db.Students.Find(UserID);
            //StudentInterviewViewModel vm = new StudentInterviewViewModel();
            //vm.UserID = student.UserID;
            //a.Model = vm;
            //var pdfBytes = a.BuildFile(ControllerContext);

            //var fileName = string.Format("my_file_{0}.pdf", vm.UserID);
            //var path = Server.MapPath("~/App_Data/" + fileName);
            //System.IO.File.WriteAllBytes(path, pdfBytes);

            //MemoryStream ms = new MemoryStream(pdfBytes);
            //return new FileStreamResult(ms, "application/pdf");
        }

        // GET: InitialInterviews/Student
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            var check = db.InitialInterview
                .Where(x => x.UserID == currentUserId && x.CompletionDate!=null)
                .ToList();

            if (check.Count() != 0)
            {
                TempData["Message"] = "You have already completed the Initial Interview Form!";
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

            var initialInterview = db.InitialInterview.FirstOrDefault(d => d.UserID == currentUserId);

            if (initialInterview == null)
            {
                initialInterview = db.InitialInterview.Create();
                initialInterview.UserID = currentUserId;
                db.InitialInterview.Add(initialInterview);
                db.SaveChanges();
            }

            InitialInterview initial = db.InitialInterview.FirstOrDefault(user => user.UserID == currentUserId);

            if (initial == null)
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

            vm.CompletionDate = initial.CompletionDate;
            vm.ReasonForProgram = initial.ReasonForProgram;
            vm.ReasonForMMCC = initial.ReasonForMMCC;
            vm.CollegeLifeAdjustments = initial.CollegeLifeAdjustments;
            vm.ChoiceOfProgramAdjustments = initial.ChoiceOfProgramAdjustments;
            vm.PeersAdjustments = initial.PeersAdjustments;
            vm.MMCCStaffAdjustments = initial.MMCCStaffAdjustments;
            vm.FamilyAdjustments = initial.FamilyAdjustments;

            return View(vm);
        }

        // POST: InitialInterviews/Student
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Student(StudentInterviewViewModel vm)
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

            var initialInterview = db.InitialInterview.FirstOrDefault(d => d.UserID == currentUserId);

            if (initialInterview == null)
            {
                initialInterview = db.InitialInterview.Create();
                initialInterview.UserID = currentUserId;
                db.InitialInterview.Add(initialInterview);
                db.SaveChanges();
            }

            InitialInterview initial = db.InitialInterview.FirstOrDefault(user => user.UserID == currentUserId);

            if (initial == null)
            {
                return HttpNotFound();
            }


            if (ModelState.IsValid)
            {

                initial.CompletionDate = DateTime.Now;
                initial.ReasonForProgram = vm.ReasonForProgram;
                initial.ReasonForMMCC = vm.ReasonForMMCC;
                initial.CollegeLifeAdjustments = vm.CollegeLifeAdjustments;
                initial.ChoiceOfProgramAdjustments = vm.ChoiceOfProgramAdjustments;
                initial.PeersAdjustments = vm.PeersAdjustments;
                initial.MMCCStaffAdjustments = vm.MMCCStaffAdjustments;
                initial.FamilyAdjustments = vm.FamilyAdjustments;

                db.SaveChanges();

                TempData["Message"] = "User: " + userName + "'s, initial interview successfully saved!";
            }
            else
            {
                TempData["Error"] = "Error: Entry not saved!";
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: InitialInterviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InitialInterview initialInterview = db.InitialInterview.Find(id);
            if (initialInterview == null)
            {
                return HttpNotFound();
            }
            return View(initialInterview);
        }

        // POST: InitialInterviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InitialInterview initialInterview = db.InitialInterview.Find(id);
            db.InitialInterview.Remove(initialInterview);
            db.SaveChanges();
            return RedirectToAction("Index");
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
