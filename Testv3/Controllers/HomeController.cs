using Testv3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Testv3.Controllers
{

    [Authorize]
    public class HomeController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        //public void GetCurrentUserInViewBag()
        //{
        //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var currentuser = manager.FindById(User.Identity.GetUserId());
        //    var currentUserId = User.Identity.GetUserId();

        //    var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

        //    if (u != null)
        //    {
        //        ViewBag.CurrentUser = u.StudentFirstName.Trim() + " " + " " + u.StudentLastName.Trim();
        //        ViewBag.CurrentUserStudentID = u.StudentID.Trim();
        //    }
            

        //}

        public ActionResult Index(int page = 1, int recordsPerPage = 5)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentuser = manager.FindById(User.Identity.GetUserId());
            var currentUserId = User.Identity.GetUserId();

            GetCurrentUserInViewBag();

            var list = db.Announcements.ToList().ToPagedList(page, recordsPerPage);

            var students = db.IndividualInventoryRecord
                    .OrderBy(x => x.RecordID)
                    .ToList();


            foreach (var item in students)
            {

                var check = db.IndividualInventoryRecord
                    .Where(x => x.RecordID == item.RecordID && x.UserID == currentUserId && x.UserID == currentUserId)
                    .ToList();

                if (check.Count() == 0)
                {
                    TempData["NoIndividualRecord"] = "Please complete your Individual record!";
                    return RedirectToAction("IndividualRecord", "StudentInventory");
                }
            }

            return View(list);
        }

        public ActionResult ManageRecords()
        {
            ViewBag.Message = "Manage student records.";
            GetCurrentUserInViewBag();

            return View();
        }

        public ActionResult Appointments()
        {
            ViewBag.Message = "View and manage your appointments.";
            GetCurrentUserInViewBag();

            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.Message = "View and generate available reports.";
            GetCurrentUserInViewBag();

            return View();

        }

        public ActionResult ManageAccounts()
        {
            ViewBag.Message = "Manage user accounts.";
            GetCurrentUserInViewBag();

            return View();

        }
    }
}