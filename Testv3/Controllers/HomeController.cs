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
    public class HomeController : Controller
    {
        private Testv2Entities db = new Testv2Entities();

        public ActionResult Index(int page = 1, int recordsPerPage = 5)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentuser = manager.FindById(User.Identity.GetUserId());

            

            //ViewBag.Message = " Welcome " + currentuser.FirstName + " " + currentuser.LastName + "!";
            //relationship between aspnetusers table and student/counselor table
            ViewBag.Message = " Welcome " + currentuser.Email + "!";

            var list = db.Announcements.ToList().ToPagedList(page, recordsPerPage);
            return View(list);
        }

        public ActionResult ManageRecords()
        {
            ViewBag.Message = "Manage student records.";

            return View();
        }

        public ActionResult Appointments()
        {
            ViewBag.Message = "View and manage your appointments.";

            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.Message = "View and generate available reports.";

            return View();

        }

        public ActionResult ManageAccounts()
        {
            ViewBag.Message = "Manage user accounts.";

            return View();

        }
    }
}