using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    [Authorize(Roles = "Student")]
    public class ViewModelController : HomeController
    {
        private Testv3Entities db = new Testv3Entities();
        

        // GET: ViewModel
        public ActionResult Index()
        {
            Testv3Entities ctx = new Testv3Entities();
            
            GetCurrentUserInViewBag();

            List<TestViewModel> StudentInventorylist = new List<TestViewModel>();


            var datalist = (from student in ctx.Students
                            join inventory in ctx.IndividualInventoryRecords on student.UserID equals inventory.UserID
                            select new { student.StudentFirstName, student.StudentMiddleName, student.StudentLastName, inventory.FathersName }).ToList();


            foreach (var item in datalist)
            {
                TestViewModel pvm = new TestViewModel();
                pvm.StudentFirstName = item.StudentFirstName;
                pvm.StudentMiddleName = item.StudentMiddleName;
                pvm.StudentLastName = item.StudentLastName;
                pvm.FathersName = item.FathersName;
                StudentInventorylist.Add(pvm);
            }

            return View(StudentInventorylist);
        }

        //GET: IndividualRecord
        public ActionResult IndividualRecord()
        {
            GetCurrentUserInViewBag();
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

            var userInv = db.IndividualInventoryRecords.FirstOrDefault(d => d.UserID == currentUserId);

            if (userInv == null)
            {
                userInv = db.IndividualInventoryRecords.Create();
                userInv.UserID = currentUserId;
                db.IndividualInventoryRecords.Add(userInv);
                db.SaveChanges();
            }

            // IndividualInventoryRecord inventory = db.IndividualInventoryRecords.Find(userInv.UserID);
            //var inventory = db.IndividualInventoryRecords.SqlQuery("SELECT * FROM dbo.IndividualInventoryRecord WHERE UserID = @currentUserId", new System.Data.SqlClient.SqlParameter("@currentUserId", currentUserId));

            IndividualInventoryRecord inventory = db.IndividualInventoryRecords.FirstOrDefault(user => user.UserID == currentUserId);

            if (inventory == null)
            {
                return HttpNotFound();
            }

            TestViewModel vm = new TestViewModel()
            {
                UserID = student.UserID,
                StudentFirstName = student.StudentFirstName,
                StudentMiddleName = student.StudentMiddleName,
                StudentLastName = student.StudentLastName,
                StudentEmail = student.StudentEmail,
                Address = student.Address,
                Sex = student.Sex,
                Civil_Status__CivilStatus = student.Civil_Status__CivilStatus,
                Religion = student.Religion,
                Nationality = student.Nationality,
                Birthdate = student.Birthdate,
                PhoneNumber = student.PhoneNumber,
                Birthplace = student.Birthplace,
                Dialect = student.Dialect,
                Hobbies = student.Hobbies,
                BirthRank = student.BirthRank,
                DistanceFromSchool = student.DistanceFromSchool,
                Scholarship = student.Scholarship,
                DateOfMarriage = student.DateOfMarriage,
                PlaceOfMarriage = student.PlaceOfMarriage,
                SpouseName = student.SpouseName,
                SpouseAge = student.SpouseAge,
                SpouseEducationalAttainment = student.SpouseEducationalAttainment,
                Occupation = student.Occupation,
                StudentEmployerAddress = student.StudentEmployerAddress,
                NumberOfChildren = student.NumberOfChildren,

                FathersName = inventory.FathersName,
                FathersAddress = inventory.FathersAddress,
                FathersAge = inventory.FathersAge,
                FathersEducationalAttainment = inventory.FathersEducationalAttainment,
                FathersOccupation = inventory.FathersOccupation,
                FathersEmployerAddress = inventory.FathersEmployerAddress,
                MothersName = inventory.MothersName,
                MothersAddress = inventory.MothersAddress,
                MothersAge = inventory.MothersAge,
                MothersEducationalAttainment = inventory.MothersEducationalAttainment,
                MothersOccupation = inventory.MothersOccupation,
                MothersEmployerAddress = inventory.MothersEmployerAddress,
                FamilyDwelling = inventory.FamilyDwelling,
                EmergencyContactName = inventory.EmergencyContactName,
                EmergencyContactNumber = inventory.EmergencyContactNumber,
                ParentsStatus = inventory.ParentsStatus,
                EconomicStatus = inventory.EconomicStatus,
                NoOfSiblings = inventory.NoOfSiblings,
                PresentlyLivingWith = inventory.PresentlyLivingWith,
                PresentlyStayingAt = inventory.PresentlyStayingAt,

            };

            return View(vm);
        }


        // POST: Students/IndividualRecord
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndividualRecord(TestViewModel vm)
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

            var userInv = db.IndividualInventoryRecords.FirstOrDefault(d => d.UserID == currentUserId);

            if (userInv == null)
            {
                userInv = db.IndividualInventoryRecords.Create();
                userInv.UserID = currentUserId;
                db.IndividualInventoryRecords.Add(userInv);
            }

            if (ModelState.IsValid)
            {
                u.PhoneNumber = vm.PhoneNumber;
                u.Address = vm.Address;
                u.DistanceFromSchool = vm.DistanceFromSchool;
                u.Religion = vm.Religion;
                u.Nationality = vm.Nationality;
                u.Birthdate = vm.Birthdate;
                u.Birthplace = vm.Birthplace;
                u.BirthRank = vm.BirthRank;
                u.Dialect = vm.Dialect;
                u.Hobbies = vm.Hobbies;
                u.Scholarship = vm.Scholarship;
                u.DateOfMarriage = vm.DateOfMarriage;
                u.PlaceOfMarriage = vm.PlaceOfMarriage;
                u.SpouseName = vm.SpouseName;
                u.SpouseAge = vm.SpouseAge;
                u.SpouseEducationalAttainment = vm.SpouseEducationalAttainment;
                u.Occupation = vm.Occupation;
                u.StudentEmployerAddress = vm.StudentEmployerAddress;
                u.NumberOfChildren = vm.NumberOfChildren;

                //inventory
                userInv.FathersName = vm.FathersName;
                userInv.FathersAddress = vm.FathersAddress;
                userInv.FathersAge = vm.FathersAge;
                userInv.FathersEducationalAttainment = vm.FathersEducationalAttainment;
                userInv.FathersOccupation = vm.FathersOccupation;
                userInv.FathersEmployerAddress = vm.FathersEmployerAddress;
                userInv.MothersName = vm.MothersName;
                userInv.MothersAddress = vm.MothersAddress;
                userInv.MothersAge = vm.MothersAge;
                userInv.MothersEducationalAttainment = vm.MothersEducationalAttainment;
                userInv.MothersOccupation = vm.MothersOccupation;
                userInv.MothersEmployerAddress = vm.MothersEmployerAddress;
                userInv.FamilyDwelling = vm.FamilyDwelling;
                userInv.EmergencyContactName = vm.EmergencyContactName;
                userInv.EmergencyContactNumber = vm.EmergencyContactNumber;
                userInv.ParentsStatus = vm.ParentsStatus;
                userInv.EconomicStatus = vm.EconomicStatus;
                userInv.NoOfSiblings = vm.NoOfSiblings;
                userInv.PresentlyLivingWith = vm.PresentlyLivingWith;
                userInv.PresentlyStayingAt = vm.PresentlyStayingAt;
                userInv.ElementarySchool = vm.ElementarySchool;
                userInv.ElementaryAddress = vm.ElementaryAddress;
                userInv.YearsAttendedElem = vm.YearsAttendedElem;
                userInv.HighSchool = vm.HighSchool;
                userInv.HighSchoolAddress = vm.HighSchoolAddress;
                userInv.YearsAttendedHS = vm.YearsAttendedHS;
                userInv.CollegeSchool = vm.CollegeSchool;
                userInv.CollegeAddress = vm.CollegeAddress;
                userInv.YearsAttendedCollege = vm.YearsAttendedCollege;
                userInv.Honors = vm.Honors;
                userInv.FaveSubject = vm.FaveSubject;
                userInv.LeastSubject = vm.LeastSubject;
                userInv.HowStudieIssFinanced = vm.HowStudieIssFinanced;
                userInv.IsCoursePersonalChoice = vm.IsCoursePersonalChoice;
                userInv.CourseNotPersonalChoice = vm.CourseNotPersonalChoice;
                userInv.CourseChoiceInfluence = vm.CourseChoiceInfluence;
                userInv.CoursePersonalChoice = vm.CoursePersonalChoice;
                userInv.WhyMMCC = vm.WhyMMCC;
                userInv.ReferredToMMCCBy = vm.ReferredToMMCCBy;
                userInv.Position = vm.Position;
                userInv.Salary = vm.Salary;
                userInv.Employer = vm.Employer;
                userInv.EmployerAddress = vm.EmployerAddress;
                userInv.EmploymentStatus = vm.EmploymentStatus;
                userInv.MentalAbilityTestDate = vm.MentalAbilityTestDate;
                userInv.MentalAbilityTestScore = vm.MentalAbilityTestScore;
                userInv.MentalAbilityTestPercentile = vm.MentalAbilityTestPercentile;
                userInv.PersonalityTestDate = vm.PersonalityTestDate;
                userInv.PersonalityTestScore = vm.PersonalityTestScore;
                userInv.PersonalityTestPercentile = vm.PersonalityTestPercentile;
                userInv.VocationalTestDate = vm.VocationalTestDate;
                userInv.VocationalTestScore = vm.VocationalTestScore;
                userInv.VocationalTestPercentile = vm.VocationalTestPercentile;
                userInv.Disabilities = vm.Disabilities;
                userInv.ChronicIllness = vm.ChronicIllness;
                userInv.PreviousAccidents = vm.PreviousAccidents;
                userInv.PreviousSurgery = vm.PreviousSurgery;
                userInv.MaintenanceMedicines = vm.MaintenanceMedicines;
                userInv.Immunization = vm.Immunization;
                userInv.HaveTalkedWithACounselor = vm.HaveTalkedWithACounselor;
                userInv.HaveTalkedWithACounselorWhen = vm.HaveTalkedWithACounselorWhen;
                userInv.HaveTalkedWithACounselorWhy = vm.HaveTalkedWithACounselorWhy;
                userInv.HaveTalkedWithAPsychiatrist = vm.HaveTalkedWithAPsychiatrist;
                userInv.HaveTalkedWithAPsychiatristWhen = vm.HaveTalkedWithAPsychiatristWhen;
                userInv.HaveTalkedWithAPsychiatristWhy = vm.HaveTalkedWithAPsychiatristWhy;
                userInv.HaveTalkedWithAPsychologist = vm.HaveTalkedWithAPsychologist;
                userInv.HaveTalkedWithAPsychologistWhen = vm.HaveTalkedWithAPsychologistWhen;
                userInv.HaveTalkedWithAPsychologistWhy = vm.HaveTalkedWithAPsychologistWhy;
                userInv.AboutYourself = vm.AboutYourself;

                db.SaveChanges();

                TempData["Message"] = "User: " + userName + ", details successfully updated!";
            }
            else
            {
                TempData["Error"] = "Error: Details not updated!";
            }



            return View(vm);
        }
    }

}