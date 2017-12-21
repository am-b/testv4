using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Testv3.Models;
using System.Threading.Tasks;

namespace Testv3.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentDropdownsController : Controller
    {
        private Testv3Entities db = new Testv3Entities();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: StudentDropdowns/Create
        public ActionResult InventoryRecord()
        {
            //get current user
            //var currentUserId = User.Identity.GetUserId();
            //var newid = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            //populate dropdowns
            StudentDropdown objStudent = new StudentDropdown();

            var civilStatus = GetAllCivilStatus();
            ViewBag.Roles = GetSelectListItems(civilStatus);
            objStudent.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);

            var gender = GetAllGender();
            //ViewBag.Gender = GetSelectListItems(gender);
            objStudent.Sexx = GetSelectListItems(gender);

            //get details of current user
            //Student student = db.Students.Find(newid.UserID);

            return View(objStudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryRecord([Bind(Include = "StudentEmail,StudentLastName,StudentFirstName,StudentMiddleName,Address,Sex,Civil_Status__CivilStatus,Religion,Nationality,Birthdate,PhoneNumber,Birthplace,Dialect,Hobbies,BirthRank,DistanceFromSchool,Scholarship")] StudentDropdown studentDropdown)
        {
            var civilStatus = GetAllCivilStatus();
            studentDropdown.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);
            var gender = GetAllGender();
            studentDropdown.Sexx = GetSelectListItems(gender);

            var CivilStatus = studentDropdown.Civil_Status__CivilStatus.Trim();
            var Gender = studentDropdown.Sex.Trim();

            if (CivilStatus == "")
            {
                throw new Exception("No Civil Status");
            }

            if (Gender == "")
            {
                throw new Exception("No Gender");
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }

                
            return View(studentDropdown);
        }


        private IEnumerable<string> GetAllCivilStatus()
        {
            return new List<string>
            {
                "Single",
                "Married",
            };
        }

        private IEnumerable<string> GetAllGender()
        {
            return new List<string>
            {
                "Male",
                "Female",
            };
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
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
