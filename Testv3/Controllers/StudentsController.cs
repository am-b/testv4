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
        private Testv2Entities db = new Testv2Entities();

        // GET: Students/CreateEdit
        public ActionResult CreateEdit()
        {
            //ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            StudentDropdown objStudent = new StudentDropdown();

            var civilStatus = GetAllCivilStatus();
            objStudent.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);

            var gender = GetAllGender();
            objStudent.Sexx = GetSelectListItems(gender);
            return View(objStudent);
        }

        // POST: Students/CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit([Bind(Include = "StudentID,StudentLastName,StudentFirstName,StudentMiddleName,StudentEmail,CourseID,Address,Sex,Civil_Status__CivilStatus,Religion,Nationality,Birthdate,PhoneNumber,Birthplace,Dialect,Hobbies,BirthRank,UserID")] Student student, StudentDropdown studentDropdown)
        {
            var civilStatus = GetAllCivilStatus();
            studentDropdown.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);
            var gender = GetAllGender();
            studentDropdown.Sexx = GetSelectListItems(gender);

            var CivilStatus = studentDropdown.Civil_Status__CivilStatus.Trim();
            var Gender = studentDropdown.Sex.Trim();

            //ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", student.UserID);
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", student.CourseID);
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
