using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: Students/CreateEdit
        public ActionResult InventoryRecord()
        {
            var currentUserId = User.Identity.GetUserId();
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

            return View(student);
        }

        // POST: Students/CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryRecord(Student student)
        {

            var currentUserId = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (u == null)
            {
                u = db.Students.Create();
                u.UserID = currentUserId;
                db.Students.Add(u);

            }

            if (ModelState.IsValid)
            {
                u.PhoneNumber = student.PhoneNumber;
                u.Address = student.Address;
                u.DistanceFromSchool = student.DistanceFromSchool;
                u.Religion = student.Religion;
                u.Nationality = student.Nationality;
                u.Birthdate = student.Birthdate;
                u.Birthplace = student.Birthplace;
                u.BirthRank = student.BirthRank;
                u.Dialect = student.Dialect;
                u.Hobbies = student.Hobbies;
                u.Scholarship = student.Scholarship;

                // DateOfMarriage,PlaceOfMarriage,SpouseName,
                u.DateOfMarriage = student.DateOfMarriage;
                u.PlaceOfMarriage = student.PlaceOfMarriage;
                u.SpouseName = student.SpouseName;
                u.SpouseEducationalAttainment = student.SpouseEducationalAttainment;
                u.SpouseAge = student.SpouseAge;
                u.Occupation = student.Occupation;
                u.StudentEmployerAddress = student.StudentEmployerAddress;
                u.NumberOfChildren = student.NumberOfChildren;

                db.SaveChanges();

                TempData["Message"] = "User: " + userName + ", details successfully updated!";
            }

            //if (ModelState.IsValid)
            //{
            //    var civilStatus = GetAllCivilStatus();
            //    studentDropdown.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);
            //    var gender = GetAllGender();
            //    studentDropdown.Sexx = GetSelectListItems(gender);

            //    var CivilStatus = studentDropdown.Civil_Status__CivilStatus.Trim();
            //    var Gender = studentDropdown.Sex.Trim();

            //    if (CivilStatus == "")
            //    {
            //        throw new Exception("No Civil Status");
            //    }

            //    if (Gender == "")
            //    {
            //        throw new Exception("No Gender");
            //    }
            //}

            return View(student);
        }

        //
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
