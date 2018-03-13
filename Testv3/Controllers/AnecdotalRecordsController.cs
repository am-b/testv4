using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    [Authorize(Roles = "Counselor")]
    public class AnecdotalRecordsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: AnecdotalRecords
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

        // GET: AnecdotalRecords/Add
        [Authorize(Roles = "Counselor")]
        public ActionResult Add(string UserID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            if (UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = db.Students.Find(UserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            AnecdotalRecord record = new AnecdotalRecord();
            var counsellor = db.Counsellor.FirstOrDefault(d => d.UserID == currentUserId);

            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();

            vm.UserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            vm.CounsellorFirstName = counsellor.CounsellorFirstName;
            vm.CounsellorMiddleName = counsellor.CounsellorMiddleName;
            vm.CounsellorLastName = counsellor.CounsellorLastName;

            vm.CompletionDate = record.CompletionDate;
            vm.DateTimeObserved = record.DateTimeObserved;
            vm.Place = record.Place;
            vm.Observer = record.Observer;
            vm.BehaviorObserved = record.BehaviorObserved;
            vm.Action = record.Action;
            vm.Summary = record.Summary;

            return View(vm);
        }

        // POST: AnecdotalRecords/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Add(StudentCounsellingViewModel vm, string UserID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            AnecdotalRecord record = new AnecdotalRecord();

            var student = db.Students.FirstOrDefault(X => X.UserID == UserID);
            var counsellor = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);

            record.UserID = counsellor.UserID;
            record.StudentUserID = student.UserID;

            record.CompletionDate = DateTime.Now;
            record.DateTimeObserved = vm.DateTimeObserved;
            record.Place = vm.Place;
            record.Observer = vm.Observer;
            record.BehaviorObserved = vm.BehaviorObserved;
            record.Action = vm.Action;
            record.Summary = vm.Summary;

            db.AnecdotalRecord.Add(record);

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Message"] = "You have successfullly added an Anecdotal Record for student: " + student.StudentID + ":" + student.StudentFirstName + " " + student.StudentLastName + " !";
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: AnecdotalRecords/Student/5
        [Authorize(Roles = "Counselor")]
        public ActionResult Student(string UserID)
        {
            GetCurrentUserInViewBag();

            List<AnecdotalRecord> StudentInventorylist = new List<AnecdotalRecord>();
            var datalist = db.AnecdotalRecord.Where(x => x.StudentUserID == UserID).ToList();
            var student = db.Students.FirstOrDefault(x => x.UserID == UserID);

            if (datalist.Count() != 0)
            {
                foreach (var item in datalist)
                {
                    AnecdotalRecord pvm = new AnecdotalRecord();
                    pvm.Place = item.Place;
                    pvm.AnecdotalRecordID = item.AnecdotalRecordID;
                    pvm.CompletionDate = item.CompletionDate;

                    StudentInventorylist.Add(pvm);
                }
                return View(StudentInventorylist.ToList());

            }
            else
            {
                TempData["Error"] = "No Anecdotal Reports under student: " + student.StudentLastName + ", " + student.StudentFirstName + "(" + student.StudentID + ")";
                return RedirectToAction("Index");
            }

        }

        // GET: AnecdotalRecords/Details/5
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Details(int AnecdotalRecordID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            var FormID = db.AnecdotalRecord.FirstOrDefault(x => x.AnecdotalRecordID == AnecdotalRecordID);

            var StudentUserID = FormID.StudentUserID;

            Student student = db.Students.Find(StudentUserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            var record = db.AnecdotalRecord.FirstOrDefault(x => x.StudentUserID == StudentUserID);

            if (record == null)
            {
                return HttpNotFound();
            }

            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();

            vm.StudentUserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            var counselloR = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);
            if(counselloR != null)
            {
                vm.CounsellorLastName = counselloR.CounsellorLastName;
                vm.CounsellorFirstName = counselloR.CounsellorFirstName;
                vm.CounsellorMiddleName = counselloR.CounsellorMiddleName;
            }
            

            vm.CompletionDate = record.CompletionDate;
            vm.Place = record.Place;
            vm.Observer = record.Observer;
            vm.BehaviorObserved = record.BehaviorObserved;
            vm.Action = record.Action;
            vm.Summary = record.Summary;

            return View(vm);
        }

        // GET: AnecdotalRecords/Details/5
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Details1(int AnecdotalRecordID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            var FormID = db.AnecdotalRecord.FirstOrDefault(x => x.AnecdotalRecordID == AnecdotalRecordID);

            var StudentUserID = FormID.StudentUserID;

            Student student = db.Students.Find(StudentUserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            var record = db.AnecdotalRecord.FirstOrDefault(x => x.StudentUserID == StudentUserID);

            if (record == null)
            {
                return HttpNotFound();
            }

            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();

            vm.StudentUserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            var counselloR = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);
            if (counselloR != null)
            {
                vm.CounsellorLastName = counselloR.CounsellorLastName;
                vm.CounsellorFirstName = counselloR.CounsellorFirstName;
                vm.CounsellorMiddleName = counselloR.CounsellorMiddleName;
            }


            vm.CompletionDate = record.CompletionDate;
            vm.Place = record.Place;
            vm.Observer = record.Observer;
            vm.BehaviorObserved = record.BehaviorObserved;
            vm.Action = record.Action;
            vm.Summary = record.Summary;

            return View(vm);
        }

        // POST: AnecdotalRecords/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Details(StudentCounsellingViewModel vm, int AnecdotalRecordID)
        {
            GetCurrentUserInViewBag();

            AnecdotalRecord report = db.AnecdotalRecord.FirstOrDefault(x => x.AnecdotalRecordID == AnecdotalRecordID);

            if (report == null)
            {
                return HttpNotFound();
            }

            report.Summary = vm.Summary;

            int result = db.SaveChanges();
            if (result > 0)
            {
                TempData["Message"] = "Counselor notes and summary successfully saved!";
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;
            }

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Print(int AnecdotalRecordID)
        {

            AnecdotalRecord check = db.AnecdotalRecord.FirstOrDefault(x => x.AnecdotalRecordID == AnecdotalRecordID);
            var student = db.Students.FirstOrDefault(x => x.UserID == check.StudentUserID);
            var name = student.StudentLastName + ", " + student.StudentFirstName + " ";


            if (check == null)
            {
                TempData["Error"] = "No record found!";
                return RedirectToAction("Student", "AnecdotalRecords");
            }
            else if (check.CompletionDate == null)
            {
                TempData["Error"] = "No record found!";
                return RedirectToAction("Student", "AnecdotalRecords");

            }


            return new ActionAsPdf(
                           "Details1",
                           new { AnecdotalRecordID = AnecdotalRecordID })
            {
                FileName = string.Format("Anecdotal_Record_{0}.pdf", name + check.CompletionDate)
            };
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
