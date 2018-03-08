﻿using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "Counselor")]
    public class CounsellingFormsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: CounsellingForms
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

        // GET: CounsellingForms/Student/5
        public ActionResult Student(string UserID)
        {
            GetCurrentUserInViewBag();

            List<CounsellingForm> StudentInventorylist = new List<CounsellingForm>();
            var datalist = db.CounsellingForm.Where(x => x.StudentUserID == UserID).ToList();
           
            if (datalist.Count() != 0)
            {
                foreach (var item in datalist)
                {
                    CounsellingForm pvm = new CounsellingForm();
                    pvm.Case = item.Case;
                    pvm.CounsellingFormID = item.CounsellingFormID;
                    pvm.CompletionDate = item.CompletionDate;

                    StudentInventorylist.Add(pvm);
                }
                return View(StudentInventorylist.ToList());

            }
            else
            {
                TempData["Error"] = "This user has no existing counselling records!";
                return RedirectToAction("Index");
            }
            
        }

        // GET: CounsellingForms/Search
        public ActionResult Search()
        {
            GetCurrentUserInViewBag();

            return View();
        }

        // POST: CounsellingForms/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string searchStringStudentID)
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();
            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();
            CounsellingForm form = new CounsellingForm();
            Student student = new Student();

            var check = db.Students
                        .Where(d => d.StudentID == searchStringStudentID)
                        .Count();

            if (check != 0)
            {

                var Student = db.Students.FirstOrDefault(x => x.StudentID == searchStringStudentID);

                vm.StudentUserID = Student.UserID;
                vm.StudentFirstName = Student.StudentFirstName;
                vm.StudentMiddleName = Student.StudentMiddleName;
                vm.StudentLastName = Student.StudentLastName;
                vm.Program = Student.Program;
                vm.YearLevel = Student.YearLevel;

                TempData["StudentUserID"] = Student.UserID;
                return RedirectToAction("Counsellor");

            }
            else
            {
                TempData["Error"] = "No student number: " + searchStringStudentID + " found!";
            }

            
            return View(vm);
        }

        // GET: CounsellingForms/Counsellor
        [HttpGet]
        public ActionResult Counsellor()
        {
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();
            CounsellingForm form = new CounsellingForm();

            string StudentUserID = Convert.ToString(TempData["StudentUserID"]);

            var studentUserID = StudentUserID;
            var Student = db.Students.FirstOrDefault(x => x.UserID == studentUserID);

            vm.StudentUserID = Student.UserID;
            vm.StudentFirstName = Student.StudentFirstName;
            vm.StudentMiddleName = Student.StudentMiddleName;
            vm.StudentLastName = Student.StudentLastName;
            vm.Program = Student.Program;
            vm.YearLevel = Student.YearLevel;
            vm.StudentID = Student.StudentID;


            var counsellor = db.Counsellor.FirstOrDefault(x => x.UserID == currentUserId);

            vm.CounsellorLastName = counsellor.CounsellorLastName;
            vm.CounsellorFirstName = counsellor.CounsellorFirstName;
            vm.CounsellorMiddleName = counsellor.CounsellorMiddleName;

            vm.CompletionDate = DateTime.Now;
            vm.Case = form.Case;
            vm.Session = form.Session;
            vm.PctionPlan = form.PctionPlan;
            vm.Recommendation = form.Recommendation;
            vm.Followup = form.Followup;

            return View(vm);
        }

        // POST: CounsellingForms/Counsellor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Counsellor(string searchStringStudentID, StudentCounsellingViewModel vm)
        {

            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();

            CounsellingForm form = new CounsellingForm();

            if (ModelState.IsValid)
            {
                form.UserID = currentUserId;
                form.StudentUserID = vm.StudentUserID;
                form.CompletionDate = DateTime.Now;
                form.Case = vm.Case;
                form.Session = vm.Session;
                form.PctionPlan = vm.PctionPlan;
                form.Recommendation = vm.Recommendation;
                form.Followup = vm.Followup;

                db.CounsellingForm.Add(form);
                db.SaveChanges();

                TempData["Message"] = "Counselling form " + form.CounsellingFormID + " successfully added!";
            }
            else
            {
                TempData["Error"] = "Error: Counselling form not added!";
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: CounsellingForms/Student/5
        public ActionResult Details(int CounsellingFormID)
        {
            GetCurrentUserInViewBag();

            var FormID = db.CounsellingForm.FirstOrDefault(x => x.CounsellingFormID == CounsellingFormID);

            var StudentUserID = FormID.StudentUserID;

            Student student = db.Students.Find(StudentUserID);

            if (student == null)
            {
                return HttpNotFound();
            }

            var form = db.CounsellingForm.FirstOrDefault(x => x.StudentUserID == StudentUserID);

            if (form == null)
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

            var counsellorUserID = form.UserID;
            Counsellor counsellor = db.Counsellor.Find(counsellorUserID);

            vm.CounsellorLastName = counsellor.CounsellorLastName;
            vm.CounsellorFirstName = counsellor.CounsellorFirstName;
            vm.CounsellorMiddleName = counsellor.CounsellorMiddleName;

            vm.CompletionDate = form.CompletionDate;
            vm.Case = form.Case;
            vm.Session = form.Session;
            vm.PctionPlan = form.PctionPlan;
            vm.Recommendation = form.Recommendation;
            vm.Followup = form.Followup;

            return View(vm);
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
