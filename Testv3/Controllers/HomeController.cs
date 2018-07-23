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

        public ActionResult Index(int page = 1, int recordsPerPage = 5)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentuser = manager.FindById(User.Identity.GetUserId());
            var currentUserId = User.Identity.GetUserId();

            GetCurrentUserInViewBag();

            var list = db.Announcements.ToList().ToPagedList(page, recordsPerPage);

            var check = db.IndividualInventoryRecord
                .Where(x => x.UserID == currentUserId && x.HowStudieIssFinanced != null && x.CourseChoiceInfluence != null)
                .ToList();

            if (check.Count() == 0)
            {
                TempData["NoIndividualRecord"] = "Please complete your Individual record!";
                ViewBag.NoRecord = "Please complete your Individual record!";
            }


            return View(list);
        }

        public ActionResult History()
        {
            GetCurrentUserInViewBag();

            return View();
        }
    }

}