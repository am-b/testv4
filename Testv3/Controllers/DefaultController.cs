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
    public class DefaultController : Controller
    {
        private Testv3Entities db = new Testv3Entities();

        public void GetCurrentUserInViewBag()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentuser = manager.FindById(User.Identity.GetUserId());
            var currentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Student"))
            {
                var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

                if ((u.StudentFirstName != null) && (u.StudentLastName != null))
                {
                    ViewBag.CurrentUser = u.StudentFirstName.Trim() + " " + " " + u.StudentLastName.Trim();
                    ViewBag.CurrentUserStudentID = u.StudentID.Trim();
                }
                else
                {
                    ViewBag.CurrentUser = "Error: User has no name record.";
                    ViewBag.CurrentUserStudentID = "Error: User has no ID record.";
                }
            }
            else if (User.IsInRole("Counselor"))
            {
                var u = db.Counsellor.FirstOrDefault(d => d.UserID == currentUserId);

                if ((u.CounsellorFirstName != null) && (u.CounsellorLastName != null))
                {
                    ViewBag.CurrentUser = u.CounsellorFirstName.Trim() + " " + " " + u.CounsellorLastName.Trim();
                    ViewBag.CurrentUserStudentID = u.CounsellorID.ToString().Trim();
                }
                else
                {
                    ViewBag.CurrentUser = "Error: User has no name record.";
                    ViewBag.CurrentUserStudentID = "Error: User has no ID record.";
                }
            }
            else if (User.IsInRole("Administrator"))
            {
                    ViewBag.CurrentUser = "Administrator";                
            }


        }
    }
}