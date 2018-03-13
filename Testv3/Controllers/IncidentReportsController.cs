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
    [Authorize]
    public class IncidentReportsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: IncidentReports
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

        // GET: IncidentReports/Add
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

            IncidentReport incident = new IncidentReport();
            var counsellor = db.Counsellor.FirstOrDefault(d => d.UserID == currentUserId);

            StudentInterviewViewModel vm = new StudentInterviewViewModel();

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

            vm.CompletionDate = incident.CompletionDate;
            vm.TypeOfIncident = incident.TypeOfIncident;
            vm.PlaceOfIncident = incident.PlaceOfIncident;
            vm.DateTimeOfIncident = incident.DateTimeOfIncident;
            vm.Witness = incident.Witness;
            vm.Details = incident.Details;
            vm.CounselorNotes = incident.CounsellorNotes;
            
            return View(vm);
        }

        // POST: IncidentReports/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Add(StudentInterviewViewModel vm, string UserID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            IncidentReport incident = new IncidentReport();
            var student = db.Students.FirstOrDefault(X => X.UserID == UserID);
            var counsellor = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);

            incident.EeportedBy = counsellor.UserID;
            incident.StudentUserID = student.UserID;

            incident.CompletionDate = DateTime.Now;
            incident.TypeOfIncident = vm.TypeOfIncident;
            incident.PlaceOfIncident = vm.PlaceOfIncident;
            incident.DateTimeOfIncident = vm.DateTimeOfIncident;
            incident.Witness = vm.Witness;
            incident.Details = vm.Details;
            incident.CounsellorNotes = vm.CounselorNotes;

            db.IncidentReport.Add(incident);
            
            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Message"] = "You have successfullly filed an Incident Report for student: " + student.StudentID + " !";
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: IncidentReports/Submit
        [Authorize(Roles = "Student")]
        public ActionResult Submit(string UserID)
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

            IncidentReport incident = new IncidentReport();
            StudentInterviewViewModel vm = new StudentInterviewViewModel();

            vm.UserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;


            vm.CompletionDate = incident.CompletionDate;
            vm.TypeOfIncident = incident.TypeOfIncident;
            vm.PlaceOfIncident = incident.PlaceOfIncident;
            vm.DateTimeOfIncident = incident.DateTimeOfIncident;
            vm.Witness = incident.Witness;
            vm.Details = incident.Details;
            vm.CounselorNotes = incident.CounsellorNotes;

            return View(vm);
        }

        // POST: IncidentReports/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Submit(StudentInterviewViewModel vm, string UserID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            IncidentReport incident = new IncidentReport();
            var student = db.Students.FirstOrDefault(X => X.UserID == UserID);
            var reportedBy = db.Students.FirstOrDefault(x => x.UserID == currentUserId);

            incident.EeportedBy = reportedBy.UserID;
            incident.StudentUserID = student.UserID;

            incident.CompletionDate = DateTime.Now;
            incident.TypeOfIncident = vm.TypeOfIncident;
            incident.PlaceOfIncident = vm.PlaceOfIncident;
            incident.DateTimeOfIncident = vm.DateTimeOfIncident;
            incident.Witness = vm.Witness;
            incident.Details = vm.Details;
            incident.CounsellorNotes = vm.CounselorNotes;

            db.IncidentReport.Add(incident);

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Message"] = "You have successfullly filed an Incident Report for student: " + student.StudentID + " !";
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = errors;
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: IncidentReports/Student/5
        [Authorize(Roles = "Counselor")]
        public ActionResult Student(string UserID)
        {
            GetCurrentUserInViewBag();

            List<IncidentReport> StudentInventorylist = new List<IncidentReport>();
            var datalist = db.IncidentReport.Where(x => x.StudentUserID == UserID).ToList();
            var student = db.Students.FirstOrDefault(x => x.UserID == UserID);

            if (datalist.Count() != 0)
            {
                foreach (var item in datalist)
                {
                    IncidentReport pvm = new IncidentReport();
                    pvm.TypeOfIncident = item.TypeOfIncident;
                    pvm.IncidentReportID = item.IncidentReportID;
                    pvm.CompletionDate = item.CompletionDate;

                    StudentInventorylist.Add(pvm);
                }
                return View(StudentInventorylist.ToList());

            }
            else
            {
                TempData["Error"] = "No Incident Reports under student: " + student.StudentLastName + ", " + student.StudentFirstName + "(" + student.StudentID + ")";
                return RedirectToAction("Index");
            }

        }

        // GET: IncidentReports/Details/5
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Details(int IncidentReportID)
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            var FormID = db.IncidentReport.FirstOrDefault(x => x.IncidentReportID == IncidentReportID);

            var StudentUserID = FormID.StudentUserID;

            Student student = db.Students.Find(StudentUserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            var form = db.IncidentReport.FirstOrDefault(x => x.StudentUserID == StudentUserID);

            if (form == null)
            {
                return HttpNotFound();
            }

            StudentInterviewViewModel vm = new StudentInterviewViewModel();

            vm.StudentUserID = student.UserID;
            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;

            var UserIDReporter = form.EeportedBy;
            Counsellor counsellor = db.Counsellor.Find(UserIDReporter);
            if (counsellor != null)
            {
                vm.ReportedByName = counsellor.CounsellorLastName + ", " + counsellor.CounsellorFirstName;
            }
            else
            {
                Student studentReporter = db.Students.Find(UserIDReporter);
                vm.ReportedByName = studentReporter.StudentLastName + ", " + studentReporter.StudentFirstName;
            }

            var counselloR = db.Counsellor.FirstOrDefault(x=>x.UserID == currentUserId);
            if (counselloR != null)
            {
                vm.CounsellorLastName = counselloR.CounsellorLastName;
                vm.CounsellorFirstName = counselloR.CounsellorFirstName;
                vm.CounsellorMiddleName = counselloR.CounsellorMiddleName;
            }
            

            vm.CompletionDate = form.CompletionDate;
            vm.TypeOfIncident = form.TypeOfIncident;
            vm.PlaceOfIncident = form.PlaceOfIncident;
            vm.DateTimeOfIncident = form.DateTimeOfIncident;
            vm.Witness = form.Witness;
            vm.Details = form.Details;
            vm.CounselorNotes = form.CounsellorNotes;

            return View(vm);
        }

        // POST: IncidentReports/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor")]
        public ActionResult Details(StudentInterviewViewModel vm, int IncidentReportID)
        {
            GetCurrentUserInViewBag();

            IncidentReport report = db.IncidentReport.FirstOrDefault(x => x.IncidentReportID == IncidentReportID);

            if (report == null)
            {
                return HttpNotFound();
            }

            report.CounsellorNotes = vm.CounselorNotes;

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

        public ActionResult Print(int IncidentReportID)
        {

            IncidentReport check = db.IncidentReport.FirstOrDefault(x => x.IncidentReportID == IncidentReportID);
            var student = db.Students.FirstOrDefault(x => x.UserID == check.StudentUserID);
            var name = student.StudentLastName + ", " + student.StudentFirstName + " ";


            if (check == null)
            {
                TempData["Error"] = "No record found!";
                return RedirectToAction("Student", "IncidentReports");
            }
            else if (check.CompletionDate == null)
            {
                TempData["Error"] = "No record found!";
                return RedirectToAction("Student", "IncidentReports");

            }


            return new ActionAsPdf(
                           "Details",
                           new { IncidentReportID = IncidentReportID })
            {
                FileName = string.Format("Incident_Report_{0}.pdf", name + check.CompletionDate)
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
