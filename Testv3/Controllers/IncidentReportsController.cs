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
using System.Web.Script.Serialization;
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
        public ActionResult Add(string[] name, StudentInterviewViewModel vm, string UserID, string date, string time)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            IncidentReport incident = new IncidentReport();
            var student = db.Students.FirstOrDefault(X => X.UserID == UserID);
            var counsellor = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);

            incident.EeportedBy = counsellor.UserID;
            incident.StudentUserID = student.UserID;

            incident.CompletionDate = DateTime.Now;
            incident.PlaceOfIncident = vm.PlaceOfIncident;
            incident.DateTimeOfIncident = DateTime.Parse(date + " " + time);

            incident.Witness = vm.Witness;
            incident.Details = vm.Details;
            incident.CounsellorNotes = vm.CounselorNotes;

            db.IncidentReport.Add(incident);

            var selectedTags = name.ToList();

            foreach (var item in selectedTags)
            {
                var dlist =
                    (from ans in db.IncidentReportIncidents
                     where ans.Type == item
                     select new { TypeID = ans.TypeID }).Single();


                //check if may row na may laman ang IncidentReportID, TypeID
                var check = db.IncidentReportTags
                    .Where(x => x.IncidentReportID == incident.IncidentReportID && x.TypeID == dlist.TypeID)
                    .ToList();

                if (check.Count() == 0)
                {
                    //create ng bagong entry and lagyan ng laman the ff:
                    var newTagId = db.IncidentReportTags.Create();

                    newTagId.IncidentReportID = incident.IncidentReportID;
                    newTagId.TypeID = dlist.TypeID;


                    db.IncidentReportTags.Add(newTagId);
                }

            }

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Message"] = "You have successfullly filed an Incident Report for student: " + student.StudentID + "!";
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
                vm.PlaceOfIncident = incident.PlaceOfIncident;
                vm.DateTimeOfIncident = incident.DateTimeOfIncident;
                vm.Witness = incident.Witness;
                vm.Details = incident.Details;
                vm.CounselorNotes = incident.CounsellorNotes;

            return View(vm);
        }


        [HttpPost]
        public string [][] ProcessData(string name)
        {
            string[][] selectedTag;
            selectedTag = JsonConvert.DeserializeObject<string[][]>(name);
            return (selectedTag);
        }

        public ActionResult GetValue(string[] name)
        {
            return Json(name);
        }

        // POST: IncidentReports/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Submit(string[] name, StudentInterviewViewModel vm, string UserID, string date, string time)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            //if (ModelState.IsValid)
            //{

                IncidentReport incident = new IncidentReport();
                var student = db.Students.FirstOrDefault(X => X.UserID == UserID); //yung nirereport
                var reportedBy = db.Students.FirstOrDefault(x => x.UserID == currentUserId); //nagrereport

                incident.EeportedBy = reportedBy.UserID;
                incident.StudentUserID = UserID.TrimEnd().TrimStart();

                incident.CompletionDate = DateTime.Now;
                incident.PlaceOfIncident = vm.PlaceOfIncident;
                incident.DateTimeOfIncident = DateTime.Parse(date + " " + time);

                incident.Witness = vm.Witness;
                incident.Details = vm.Details;
                incident.CounsellorNotes = vm.CounselorNotes;

                db.IncidentReport.Add(incident);

                var selectedTags = name.ToList();

                foreach (var item in selectedTags)
                {
                    var dlist =
                        (from ans in db.IncidentReportIncidents
                         where ans.Type == item
                         select new { TypeID = ans.TypeID }).Single();


                    //check if may row na may laman ang IncidentReportID, TypeID
                    var check = db.IncidentReportTags
                        .Where(x => x.IncidentReportID == incident.IncidentReportID && x.TypeID == dlist.TypeID)
                        .ToList();

                    if (check.Count() == 0)
                    {
                        //create ng bagong entry and lagyan ng laman the ff:
                        var newTagId = db.IncidentReportTags.Create();

                        newTagId.IncidentReportID = incident.IncidentReportID;
                        newTagId.TypeID = dlist.TypeID;


                        db.IncidentReportTags.Add(newTagId);
                    }

                }


                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["Message"] = "You have successfullly filed an Incident Report for student: " + student.StudentID + "!";
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    TempData["Error"] = errors;
                }
            //}
            //else
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    TempData["Error"] = errors;

            //}

            return RedirectToAction("Index", "Home");
            //return Json(Url.Action("Index", "Home"));
            //return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        public JsonResult GetTypeOfIncidents()
        {
            var currentUserId = User.Identity.GetUserId();
            List<IRViewModel> pvm = new List<IRViewModel>();
            var results = db.IncidentReportIncidents.ToList();

            foreach (IncidentReportIncidents incident in results)
            {
                IRViewModel viewmodel = new IRViewModel();
                viewmodel.name = incident.Type;

                pvm.Add(viewmodel);
            }

            return Json(pvm, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Counselor")]
        public ActionResult Charts()
        {
            GetCurrentUserInViewBag();

            return View();
        }

        public JsonResult GetIncidentTypesSummary()
        {
            //--Student w / highest number of records

            //    SELECT TOP 5 Student,COUNT(*) AS TOTAL
            //    FROM IncidentReport
            //    GROUP BY Student
            //    ORDER BY TOTAL DESC;


            var datalistIRs = db.IncidentReportIncidents.ToList();
            List<IRChartsViewModel> taglist = new List<IRChartsViewModel>();
            foreach(var item in datalistIRs)
            {
                try
                {
                    IRChartsViewModel vm = new IRChartsViewModel();

                    var type = (from tagList in db.IncidentReportIncidents
                                join selectedTags in db.IncidentReportTags 
                                on tagList.TypeID equals selectedTags.TypeID    //if wala si tag nag nnull so try catch
                                where tagList.TypeID == item.TypeID
                                group tagList by new { tagList.Type } into g
                                orderby g.Count() descending
                                select g.Key.Type
                                ).SingleOrDefault();

                    var typeCount = (from tagList in db.IncidentReportIncidents
                                     join selectedTags in db.IncidentReportTags on tagList.TypeID equals selectedTags.TypeID
                                     where tagList.TypeID == item.TypeID
                                     group tagList by new { tagList.Type } into g
                                     orderby g.Count() descending
                                     select g.Count()
                                     ).SingleOrDefault();

                    vm.Type = type.ToString();
                    vm.count = typeCount;

                    taglist.Add(vm);

                }
                catch
                {
                    //do nothing
                }

            }

            return Json(taglist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetReportersSummary()
        {

            var datalistIRs = db.IncidentReport.ToList();
            List<IRChartsViewModel> taglist = new List<IRChartsViewModel>();

                IRChartsViewModel vm = new IRChartsViewModel();
               
               //--Reported by: Counsellor v. Students
               //SELECT COUNT(*) AS[Total Reports]
               //FROM IncidentReport A
               //INNER JOIN Counsellor S ON S.UserID = A.EeportedBy
               //GROUP BY A.EeportedBy
               //ORDER BY[Total Reports] DESC;

                var counsellorCount = (from A in db.IncidentReport
                                         join B in db.Counsellor on A.EeportedBy equals B.UserID
                                         group A by new { A.EeportedBy } into g
                                         orderby g.Count() descending
                                         select g.Count()
                                         ).FirstOrDefault();

                var studCount = (from A in db.IncidentReport
                                       join B in db.Students on A.EeportedBy equals B.UserID
                                       group A by new { A.EeportedBy } into g
                                       orderby g.Count() descending
                                       select g.Count()
                                 ).FirstOrDefault();


                vm.counsellorCount = counsellorCount;
                vm.studentCount = studCount;

                taglist.Add(vm);

            return Json(taglist, JsonRequestBehavior.AllowGet);

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

            var form = db.IncidentReport.FirstOrDefault(x => x.StudentUserID == StudentUserID); //change this

            if (FormID == null)
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
            

            vm.CompletionDate = FormID.CompletionDate;
            vm.PlaceOfIncident = FormID.PlaceOfIncident;
            vm.DateTimeOfIncident = FormID.DateTimeOfIncident;
            vm.Witness = FormID.Witness;
            vm.Details = FormID.Details;
            vm.CounselorNotes = FormID.CounsellorNotes;

            return View(vm);
        }

        // GET: IncidentReports/Report/5
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult Report(int IncidentReportID)
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

            var counselloR = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);
            if (counselloR != null)
            {
                vm.CounsellorLastName = counselloR.CounsellorLastName;
                vm.CounsellorFirstName = counselloR.CounsellorFirstName;
                vm.CounsellorMiddleName = counselloR.CounsellorMiddleName;
            }


            vm.CompletionDate = form.CompletionDate;
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
                           "Report",
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
