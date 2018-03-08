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
    public class CounsellingContractsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: CounsellingContracts
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
                        (from ans in db.CounsellingContract
                         where ans.StudentUserID == pvm.UserID
                         select new { CompletionDate = ans.CompletionDate })
                         .ToList();

                    CounsellingContract contract = db.CounsellingContract.FirstOrDefault(x => x.StudentUserID == pvm.UserID);


                    if (completionDate.Count() != 0)
                    {
                        pvm.CompletionDate = contract.CompletionDate;
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


        // GET: CounsellingContracts/Create
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {

            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            var check = db.CounsellingContract
                .Where(x => x.StudentUserID == currentUserId && x.CompletionDate != null && x.StudentAgrees == true)
                .ToList();

            if (check.Count() != 0)
            {
                TempData["Message"] = "You have already completed the Counselling Contract!";
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

            var contract = db.CounsellingContract.FirstOrDefault(d => d.StudentUserID == currentUserId);

            if (contract == null)
            {
                contract = db.CounsellingContract.Create();
                contract.StudentUserID = currentUserId;
                db.CounsellingContract.Add(contract);
                db.SaveChanges();
            }

            CounsellingContract Contract = db.CounsellingContract.FirstOrDefault(user => user.StudentUserID == currentUserId);

            if (Contract == null)
            {
                return HttpNotFound();
            }

            StudentCounsellingViewModel vm = new StudentCounsellingViewModel();

            vm.StudentID = student.StudentID;
            vm.Program = student.Program;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;
            vm.Program = student.Program;
            vm.YearLevel = student.YearLevel;

            if (contract.StudentAgrees != null)
            {
                vm.StudentAgrees = (bool)contract.StudentAgrees;
            }


            return View(vm);
        }

        // POST: CounsellingContracts/Create
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student(CounsellingContract vm)
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

            var counsellingContract = db.CounsellingContract.FirstOrDefault(d => d.StudentUserID == currentUserId);

            if (counsellingContract == null)
            {
                counsellingContract = db.CounsellingContract.Create();
                counsellingContract.StudentUserID = currentUserId;
                db.CounsellingContract.Add(counsellingContract);
                db.SaveChanges();
            }

            CounsellingContract contract = db.CounsellingContract.FirstOrDefault(user => user.StudentUserID == currentUserId);

            if (contract == null)
            {
                return HttpNotFound();
            }


            if (ModelState.IsValid)
            {

                contract.CompletionDate = DateTime.Now;

                bool studentAgrees = false;
                if (vm.StudentAgrees == true)
                {
                    studentAgrees = true;
                    TempData["Message"] = "You successsfully agreed to the Guidance and Counselling Terms!";
                }
                else
                {
                    studentAgrees = false;
                    TempData["Message"] = "You did not agree to the Guidance and Counselling Terms! Note that you have to agree prior any consultation with the Guidance Office.";
                }

                contract.StudentAgrees = studentAgrees;


                db.SaveChanges();

                
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
