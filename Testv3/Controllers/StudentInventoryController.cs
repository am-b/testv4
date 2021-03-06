﻿using Microsoft.AspNet.Identity;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    
    public class StudentInventoryController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        [Authorize(Roles = "Counselor")]
        // GET: ViewModel
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

        // GET: /Details
        //[Authorize(Roles = "Counselor")]
        [AllowAnonymous]
        public ActionResult View(string UserID)
        {
            GetCurrentUserInViewBag();


            if (UserID == null)
            {
                TempData["Error"] = "This user has no record yet!";
                return RedirectToAction("Index", "StudentInventory");
            }
            Student student = db.Students.Find(UserID);
            if (student == null)
            {
                return HttpNotFound();
            }

            IndividualInventoryRecord inventory = db.IndividualInventoryRecord.FirstOrDefault(user => user.UserID == UserID);

            if (inventory == null)
            {
                TempData["Error"] = "This user has no record yet!";
                return RedirectToAction("Index", "StudentInventory");
            }

            TestViewModel vm = new TestViewModel();

            vm.UserID = student.UserID;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;
            vm.StudentEmail = student.StudentEmail;
            vm.Address = student.Address;

            //
            var gender = GetAllGender();
            vm.Sexx = GetSelectListItems(gender);

            var civilstatus = GetAllCivilStatus();
            vm.Civil_Status__CivilStatuss = GetSelectListItems(civilstatus);

            var spouseEducAttainment = GetAllEducationalAttainment();
            vm.SpouseEducationalAttainments = GetSelectListItems(spouseEducAttainment);

            var motherEducAttainment = GetAllEducationalAttainment();
            vm.MothersEducationalAttainments = GetSelectListItems(motherEducAttainment);

            var fatherEducAttainment = GetAllEducationalAttainment();
            vm.FathersEducationalAttainments = GetSelectListItems(fatherEducAttainment);

            var familyDwelling = GetAllFamilyDwelling();
            vm.FamilyDwellings = GetSelectListItems(familyDwelling);

            var parentsStatus = GetAllParentsStatus();
            vm.ParentsStatuses = GetSelectListItems(parentsStatus);

            var economicStatus = GetAllEconomicStatus();
            vm.EconomicStatuses = GetSelectListItems(economicStatus);

            var livingPresentlyWith = GetAllLivingPresentlyWith();
            vm.PresentlyLivingWiths = GetSelectListItems(livingPresentlyWith);

            var presentlyStayingAt = GetAllPresentlyStaying();
            vm.PresentlyStayingAts = GetSelectListItems(presentlyStayingAt);

            var employmentStatus = GetAllEmploymentStatus();
            vm.EmploymentStatuses = GetSelectListItems(employmentStatus);

            //
            var whyMMCC = GetAllWhyMMCC();
            vm.WhyMMCC_Dropdown = GetSelectListItems(whyMMCC);

            var howYouKnowMMCC = GetAllHowUKnowMMCC();
            vm.ReferredToMMCCBy_Dropdown = GetSelectListItems(howYouKnowMMCC);

            var whoInfluencedYou = GetAllWhoInfluencedYou();
            vm.CourseChoiceInfluence_Dropdown = GetSelectListItems(whoInfluencedYou);

            if (student.Sex != null)
            {
                vm.Sex = student.Sex.Trim();
            }

            if (student.Civil_Status__CivilStatus != null)
            {
                vm.Civil_Status__CivilStatus = student.Civil_Status__CivilStatus.Trim();
            }

            vm.Religion = student.Religion;
            vm.Nationality = student.Nationality;

            if (student.Birthdate != null)
            {
                var date = student.Birthdate.Value.Month + "/" + student.Birthdate.Value.Day + "/" + student.Birthdate.Value.Year;
                vm.Birthdate = DateTime.Parse(date);
            }

            vm.PhoneNumber = student.PhoneNumber;
            vm.Birthplace = student.Birthplace;
            vm.Dialect = student.Dialect;
            vm.Hobbies = student.Hobbies;
            vm.BirthRank = student.BirthRank;
            vm.DistanceFromSchool = student.DistanceFromSchool;

            if (student.IsScholar != null)
            {
                vm.IsScholar = (bool)student.IsScholar;
            }

            vm.Scholarship = student.Scholarship;

            if (student.Civil_Status__CivilStatus != null && student.Civil_Status__CivilStatus.Trim() == "Married")
            {
                vm.DateOfMarriage = student.DateOfMarriage;
                vm.PlaceOfMarriage = student.PlaceOfMarriage;
                vm.SpouseName = student.SpouseName;
                vm.SpouseAge = student.SpouseAge;

                if (student.SpouseEducationalAttainment != null)
                {
                    vm.SpouseEducationalAttainment = student.SpouseEducationalAttainment.Trim();
                }

                vm.Occupation = student.Occupation;
                vm.StudentEmployerAddress = student.StudentEmployerAddress;
                vm.NumberOfChildren = student.NumberOfChildren;
            }

            vm.FathersName = inventory.FathersName;
            vm.FathersAddress = inventory.FathersAddress;
            vm.FathersAge = inventory.FathersAge;

            if (inventory.FathersEducationalAttainment != null)
            {
                vm.FathersEducationalAttainment = inventory.FathersEducationalAttainment.Trim();
            }

            vm.FathersOccupation = inventory.FathersOccupation;
            vm.FathersEmployerAddress = inventory.FathersEmployerAddress;
            vm.MothersName = inventory.MothersName;
            vm.MothersAddress = inventory.MothersAddress;
            vm.MothersAge = inventory.MothersAge;

            if (inventory.MothersEducationalAttainment != null)
            {
                vm.MothersEducationalAttainment = inventory.MothersEducationalAttainment.Trim();
            }

            vm.MothersOccupation = inventory.MothersOccupation;
            vm.MothersEmployerAddress = inventory.MothersEmployerAddress;
            vm.FamilyDwelling = inventory.FamilyDwelling;
            vm.EmergencyContactName = inventory.EmergencyContactName;
            vm.EmergencyContactNumber = inventory.EmergencyContactNumber;

            if (inventory.ParentsStatus != null)
            {
                vm.ParentsStatus = inventory.ParentsStatus.Trim();
            }

            if (inventory.EconomicStatus != null)
            {
                vm.EconomicStatus = inventory.EconomicStatus.Trim();
            }

            vm.NoOfSiblings = inventory.NoOfSiblings;

            if (inventory.PresentlyLivingWith != null)
            {
                vm.PresentlyLivingWith = inventory.PresentlyLivingWith.Trim();
            }

            if (inventory.PresentlyStayingAt != null)
            {
                vm.PresentlyStayingAt = inventory.PresentlyStayingAt.Trim();
            }

            vm.ElementarySchool = inventory.ElementarySchool;
            vm.ElementaryAddress = inventory.ElementaryAddress;
            vm.YearsAttendedElem = inventory.YearsAttendedElem;
            vm.HighSchool = inventory.HighSchool;
            vm.HighSchoolAddress = inventory.HighSchoolAddress;
            vm.YearsAttendedHS = inventory.YearsAttendedHS;
            vm.CollegeSchool = inventory.CollegeSchool;
            vm.CollegeAddress = inventory.CollegeAddress;
            vm.YearsAttendedCollege = inventory.YearsAttendedCollege;
            vm.Honors = inventory.Honors;
            vm.FaveSubject = inventory.FaveSubject;
            vm.LeastSubject = inventory.LeastSubject;
            vm.HowStudieIssFinanced = inventory.HowStudieIssFinanced;
            
            if (inventory.IsCoursePersonalChoice != null && inventory.IsCoursePersonalChoice == false)
            {
                vm.IsCoursePersonalChoice = (bool)inventory.IsCoursePersonalChoice;
                vm.CourseNotPersonalChoice = inventory.CourseNotPersonalChoice;
                vm.CoursePersonalChoice = inventory.CoursePersonalChoice;
            }
            else
            {
                vm.IsCoursePersonalChoice = (bool)inventory.IsCoursePersonalChoice;

            }

            if (inventory.CourseChoiceInfluence != null)
            {
                vm.CourseChoiceInfluence = inventory.CourseChoiceInfluence.Trim();
            }

            if (inventory.WhyMMCC != null)
            {
                vm.WhyMMCC = inventory.WhyMMCC.Trim();
            }

            if (inventory.ReferredToMMCCBy != null)
            {
                vm.ReferredToMMCCBy = inventory.ReferredToMMCCBy.Trim();
            }

            vm.Position = inventory.Position;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus.Trim();
            }

            vm.Salary = inventory.Salary;
            vm.Employer = inventory.Employer;
            vm.EmployerAddress = inventory.EmployerAddress;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus;

            }

            vm.MentalAbilityTestDate = inventory.MentalAbilityTestDate;
            vm.MentalAbilityTestScore = inventory.MentalAbilityTestScore;
            vm.MentalAbilityTestPercentile = inventory.MentalAbilityTestPercentile;
            vm.PersonalityTestDate = inventory.PersonalityTestDate;
            vm.PersonalityTestScore = inventory.PersonalityTestScore;
            vm.PersonalityTestPercentile = inventory.PersonalityTestPercentile;
            vm.VocationalTestDate = inventory.VocationalTestDate;
            vm.VocationalTestScore = inventory.VocationalTestScore;
            vm.VocationalTestPercentile = inventory.VocationalTestPercentile;
            vm.Disabilities = inventory.Disabilities;
            vm.ChronicIllness = inventory.ChronicIllness;
            vm.PreviousAccidents = inventory.PreviousAccidents;
            vm.PreviousSurgery = inventory.PreviousSurgery;
            vm.MaintenanceMedicines = inventory.MaintenanceMedicines;
            vm.Immunization = inventory.Immunization;
            vm.HaveTalkedWithACounselor = inventory.HaveTalkedWithACounselor;
            vm.HaveTalkedWithACounselorWhen = inventory.HaveTalkedWithACounselorWhen;
            vm.HaveTalkedWithACounselorWhy = inventory.HaveTalkedWithACounselorWhy;
            vm.HaveTalkedWithAPsychiatrist = inventory.HaveTalkedWithAPsychiatrist;
            vm.HaveTalkedWithAPsychiatristWhen = inventory.HaveTalkedWithAPsychiatristWhen;
            vm.HaveTalkedWithAPsychiatristWhy = inventory.HaveTalkedWithAPsychiatristWhy;
            vm.HaveTalkedWithAPsychologist = inventory.HaveTalkedWithAPsychologist;
            vm.HaveTalkedWithAPsychologistWhen = inventory.HaveTalkedWithAPsychologistWhen;
            vm.HaveTalkedWithAPsychologistWhy = inventory.HaveTalkedWithAPsychologistWhy;
            vm.AboutYourself = inventory.AboutYourself;


            return View(vm);


        }


        public ActionResult Report(string UserID)
        {
            GetCurrentUserInViewBag();


            if (UserID == null)
            {
                TempData["Error"] = "This user has no record yet!";
                return RedirectToAction("Index", "StudentInventory");
            }
            Student student = db.Students.Find(UserID);
            if (student == null)
            {
                return HttpNotFound();
            }

            IndividualInventoryRecord inventory = db.IndividualInventoryRecord.FirstOrDefault(user => user.UserID == UserID);

            if (inventory == null)
            {
                TempData["Error"] = "This user has no record yet!";
                return RedirectToAction("Index", "StudentInventory");
            }

            TestViewModel vm = new TestViewModel();

            vm.UserID = student.UserID;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;
            vm.StudentEmail = student.StudentEmail;
            vm.Address = student.Address;

            //
            var gender = GetAllGender();
            vm.Sexx = GetSelectListItems(gender);

            var civilstatus = GetAllCivilStatus();
            vm.Civil_Status__CivilStatuss = GetSelectListItems(civilstatus);

            var spouseEducAttainment = GetAllEducationalAttainment();
            vm.SpouseEducationalAttainments = GetSelectListItems(spouseEducAttainment);

            var motherEducAttainment = GetAllEducationalAttainment();
            vm.MothersEducationalAttainments = GetSelectListItems(motherEducAttainment);

            var fatherEducAttainment = GetAllEducationalAttainment();
            vm.FathersEducationalAttainments = GetSelectListItems(fatherEducAttainment);

            var familyDwelling = GetAllFamilyDwelling();
            vm.FamilyDwellings = GetSelectListItems(familyDwelling);

            var parentsStatus = GetAllParentsStatus();
            vm.ParentsStatuses = GetSelectListItems(parentsStatus);

            var economicStatus = GetAllEconomicStatus();
            vm.EconomicStatuses = GetSelectListItems(economicStatus);

            var livingPresentlyWith = GetAllLivingPresentlyWith();
            vm.PresentlyLivingWiths = GetSelectListItems(livingPresentlyWith);

            var presentlyStayingAt = GetAllPresentlyStaying();
            vm.PresentlyStayingAts = GetSelectListItems(presentlyStayingAt);

            var employmentStatus = GetAllEmploymentStatus();
            vm.EmploymentStatuses = GetSelectListItems(employmentStatus);

            if (student.Sex != null)
            {
                vm.Sex = student.Sex.Trim();
            }

            if (student.Civil_Status__CivilStatus != null)
            {
                vm.Civil_Status__CivilStatus = student.Civil_Status__CivilStatus.Trim();
            }

            vm.Religion = student.Religion;
            vm.Nationality = student.Nationality;
            vm.Birthdate = student.Birthdate;
            vm.PhoneNumber = student.PhoneNumber;
            vm.Birthplace = student.Birthplace;
            vm.Dialect = student.Dialect;
            vm.Hobbies = student.Hobbies;
            vm.BirthRank = student.BirthRank;
            vm.DistanceFromSchool = student.DistanceFromSchool;

            if (student.IsScholar != null)
            {
                vm.IsScholar = (bool)student.IsScholar;
            }

            vm.Scholarship = student.Scholarship;

            if (student.Civil_Status__CivilStatus != null && student.Civil_Status__CivilStatus.Trim() == "Married")
            {
                vm.DateOfMarriage = student.DateOfMarriage;
                vm.PlaceOfMarriage = student.PlaceOfMarriage;
                vm.SpouseName = student.SpouseName;
                vm.SpouseAge = student.SpouseAge;

                if (student.SpouseEducationalAttainment != null)
                {
                    vm.SpouseEducationalAttainment = student.SpouseEducationalAttainment.Trim();
                }

                vm.Occupation = student.Occupation;
                vm.StudentEmployerAddress = student.StudentEmployerAddress;
                vm.NumberOfChildren = student.NumberOfChildren;
            }

            vm.FathersName = inventory.FathersName;
            vm.FathersAddress = inventory.FathersAddress;
            vm.FathersAge = inventory.FathersAge;

            if (inventory.FathersEducationalAttainment != null)
            {
                vm.FathersEducationalAttainment = inventory.FathersEducationalAttainment.Trim();
            }

            vm.FathersOccupation = inventory.FathersOccupation;
            vm.FathersEmployerAddress = inventory.FathersEmployerAddress;
            vm.MothersName = inventory.MothersName;
            vm.MothersAddress = inventory.MothersAddress;
            vm.MothersAge = inventory.MothersAge;

            if (inventory.MothersEducationalAttainment != null)
            {
                vm.MothersEducationalAttainment = inventory.MothersEducationalAttainment.Trim();
            }

            vm.MothersOccupation = inventory.MothersOccupation;
            vm.MothersEmployerAddress = inventory.MothersEmployerAddress;
            vm.FamilyDwelling = inventory.FamilyDwelling;
            vm.EmergencyContactName = inventory.EmergencyContactName;
            vm.EmergencyContactNumber = inventory.EmergencyContactNumber;

            if (inventory.ParentsStatus != null)
            {
                vm.ParentsStatus = inventory.ParentsStatus.Trim();
            }

            if (inventory.EconomicStatus != null)
            {
                vm.EconomicStatus = inventory.EconomicStatus.Trim();
            }

            vm.NoOfSiblings = inventory.NoOfSiblings;

            if (inventory.PresentlyLivingWith != null)
            {
                vm.PresentlyLivingWith = inventory.PresentlyLivingWith.Trim();
            }

            if (inventory.PresentlyStayingAt != null)
            {
                vm.PresentlyStayingAt = inventory.PresentlyStayingAt.Trim();
            }

            vm.ElementarySchool = inventory.ElementarySchool;
            vm.ElementaryAddress = inventory.ElementaryAddress;
            vm.YearsAttendedElem = inventory.YearsAttendedElem;
            vm.HighSchool = inventory.HighSchool;
            vm.HighSchoolAddress = inventory.HighSchoolAddress;
            vm.YearsAttendedHS = inventory.YearsAttendedHS;
            vm.CollegeSchool = inventory.CollegeSchool;
            vm.CollegeAddress = inventory.CollegeAddress;
            vm.YearsAttendedCollege = inventory.YearsAttendedCollege;
            vm.Honors = inventory.Honors;
            vm.FaveSubject = inventory.FaveSubject;
            vm.LeastSubject = inventory.LeastSubject;
            vm.HowStudieIssFinanced = inventory.HowStudieIssFinanced;

            if (inventory.IsCoursePersonalChoice != null)
            {
                vm.IsCoursePersonalChoice = (bool)inventory.IsCoursePersonalChoice;
            }

            vm.CourseNotPersonalChoice = inventory.CourseNotPersonalChoice;
            vm.CourseChoiceInfluence = inventory.CourseChoiceInfluence;
            vm.CoursePersonalChoice = inventory.CoursePersonalChoice;
            vm.WhyMMCC = inventory.WhyMMCC;
            vm.ReferredToMMCCBy = inventory.ReferredToMMCCBy;
            vm.Position = inventory.Position;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus.Trim();
            }

            vm.Salary = inventory.Salary;
            vm.Employer = inventory.Employer;
            vm.EmployerAddress = inventory.EmployerAddress;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus;

            }

            vm.MentalAbilityTestDate = inventory.MentalAbilityTestDate;
            vm.MentalAbilityTestScore = inventory.MentalAbilityTestScore;
            vm.MentalAbilityTestPercentile = inventory.MentalAbilityTestPercentile;
            vm.PersonalityTestDate = inventory.PersonalityTestDate;
            vm.PersonalityTestScore = inventory.PersonalityTestScore;
            vm.PersonalityTestPercentile = inventory.PersonalityTestPercentile;
            vm.VocationalTestDate = inventory.VocationalTestDate;
            vm.VocationalTestScore = inventory.VocationalTestScore;
            vm.VocationalTestPercentile = inventory.VocationalTestPercentile;
            vm.Disabilities = inventory.Disabilities;
            vm.ChronicIllness = inventory.ChronicIllness;
            vm.PreviousAccidents = inventory.PreviousAccidents;
            vm.PreviousSurgery = inventory.PreviousSurgery;
            vm.MaintenanceMedicines = inventory.MaintenanceMedicines;
            vm.Immunization = inventory.Immunization;
            vm.HaveTalkedWithACounselor = inventory.HaveTalkedWithACounselor;
            vm.HaveTalkedWithACounselorWhen = inventory.HaveTalkedWithACounselorWhen;
            vm.HaveTalkedWithACounselorWhy = inventory.HaveTalkedWithACounselorWhy;
            vm.HaveTalkedWithAPsychiatrist = inventory.HaveTalkedWithAPsychiatrist;
            vm.HaveTalkedWithAPsychiatristWhen = inventory.HaveTalkedWithAPsychiatristWhen;
            vm.HaveTalkedWithAPsychiatristWhy = inventory.HaveTalkedWithAPsychiatristWhy;
            vm.HaveTalkedWithAPsychologist = inventory.HaveTalkedWithAPsychologist;
            vm.HaveTalkedWithAPsychologistWhen = inventory.HaveTalkedWithAPsychologistWhen;
            vm.HaveTalkedWithAPsychologistWhy = inventory.HaveTalkedWithAPsychologistWhy;
            vm.AboutYourself = inventory.AboutYourself;


            return View(vm);


        }

        public ActionResult Print(string UserID)
        {
            var name = db.Students.Find(UserID);
            var student = name.StudentLastName + ", " + name.StudentFirstName;

            IndividualInventoryRecord check = db.IndividualInventoryRecord.FirstOrDefault(x => x.UserID == UserID);
            if (check == null)
            {
                TempData["Error"] = "This user has no record yet!";
                return RedirectToAction("Index", "StudentInventory");
            }


            return new ActionAsPdf (
                           "Report",
                           new { UserID = UserID })
            {
                FileName = string.Format("Individual_Record_{0}.pdf", student)
            };


        }


        [Authorize(Roles = "Student")]
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

            var userInv = db.IndividualInventoryRecord.FirstOrDefault(d => d.UserID == currentUserId);

            if (userInv == null)
            {
                userInv = db.IndividualInventoryRecord.Create();
                userInv.UserID = currentUserId;
                db.IndividualInventoryRecord.Add(userInv);
                db.SaveChanges();
            }

            IndividualInventoryRecord inventory = db.IndividualInventoryRecord.FirstOrDefault(user => user.UserID == currentUserId);

            if (inventory == null)
            {
                return HttpNotFound();
            }

            TestViewModel vm = new TestViewModel();

            vm.UserID = student.UserID;
            vm.StudentFirstName = student.StudentFirstName;
            vm.StudentMiddleName = student.StudentMiddleName;
            vm.StudentLastName = student.StudentLastName;
            vm.StudentEmail = student.StudentEmail;
            vm.Address = student.Address;


            //
            var gender = GetAllGender();
            vm.Sexx = GetSelectListItems(gender);

            var civilstatus = GetAllCivilStatus();
            vm.Civil_Status__CivilStatuss = GetSelectListItems(civilstatus);

            var spouseEducAttainment = GetAllEducationalAttainment();
            vm.SpouseEducationalAttainments = GetSelectListItems(spouseEducAttainment);

            var motherEducAttainment = GetAllEducationalAttainment();
            vm.MothersEducationalAttainments = GetSelectListItems(motherEducAttainment);

            var fatherEducAttainment = GetAllEducationalAttainment();
            vm.FathersEducationalAttainments = GetSelectListItems(fatherEducAttainment);

            var familyDwelling = GetAllFamilyDwelling();
            vm.FamilyDwellings = GetSelectListItems(familyDwelling);

            var parentsStatus = GetAllParentsStatus();
            vm.ParentsStatuses = GetSelectListItems(parentsStatus);

            var economicStatus = GetAllEconomicStatus();
            vm.EconomicStatuses = GetSelectListItems(economicStatus);

            var livingPresentlyWith = GetAllLivingPresentlyWith();
            vm.PresentlyLivingWiths = GetSelectListItems(livingPresentlyWith);

            var presentlyStayingAt = GetAllPresentlyStaying();
            vm.PresentlyStayingAts = GetSelectListItems(presentlyStayingAt);

            var employmentStatus = GetAllEmploymentStatus();
            vm.EmploymentStatuses = GetSelectListItems(employmentStatus);

            //
            var whyMMCC = GetAllWhyMMCC();
            vm.WhyMMCC_Dropdown = GetSelectListItems(whyMMCC);

            var howYouKnowMMCC = GetAllHowUKnowMMCC();
            vm.ReferredToMMCCBy_Dropdown = GetSelectListItems(howYouKnowMMCC);

            var whoInfluencedYou = GetAllWhoInfluencedYou();
            vm.CourseChoiceInfluence_Dropdown = GetSelectListItems(whoInfluencedYou);

            if (student.Sex != null)
            {
                vm.Sex = student.Sex.Trim();
            }

            if (student.Civil_Status__CivilStatus != null)
            {
                vm.Civil_Status__CivilStatus = student.Civil_Status__CivilStatus.Trim();
            }
            
            vm.Religion = student.Religion;
            vm.Nationality = student.Nationality;
            vm.Birthdate = student.Birthdate;
            vm.PhoneNumber = student.PhoneNumber;
            vm.Birthplace = student.Birthplace;
            vm.Dialect = student.Dialect;
            vm.Hobbies = student.Hobbies;
            vm.BirthRank = student.BirthRank;
            vm.DistanceFromSchool = student.DistanceFromSchool;

            if (student.IsScholar != null)
            {
                vm.IsScholar = (bool) student.IsScholar;
            }

            vm.Scholarship = student.Scholarship;
            
            if (student.Civil_Status__CivilStatus!= null && student.Civil_Status__CivilStatus.Trim() == "Married")
            {
                vm.DateOfMarriage = student.DateOfMarriage;
                vm.PlaceOfMarriage = student.PlaceOfMarriage;
                vm.SpouseName = student.SpouseName;
                vm.SpouseAge = student.SpouseAge;

                if (student.SpouseEducationalAttainment != null)
                {
                    vm.SpouseEducationalAttainment = student.SpouseEducationalAttainment.Trim();
                }

                vm.Occupation = student.Occupation;
                vm.StudentEmployerAddress = student.StudentEmployerAddress;
                vm.NumberOfChildren = student.NumberOfChildren;
            }

            vm.FathersName = inventory.FathersName;
            vm.FathersAddress = inventory.FathersAddress;
            vm.FathersAge = inventory.FathersAge;

            if (inventory.FathersEducationalAttainment != null)
            {
                vm.FathersEducationalAttainment = inventory.FathersEducationalAttainment.Trim();
            }

            vm.FathersOccupation = inventory.FathersOccupation;
            vm.FathersEmployerAddress = inventory.FathersEmployerAddress;
            vm.MothersName = inventory.MothersName;
            vm.MothersAddress = inventory.MothersAddress;
            vm.MothersAge = inventory.MothersAge;

            if (inventory.MothersEducationalAttainment != null)
            {
                vm.MothersEducationalAttainment = inventory.MothersEducationalAttainment.Trim();
            }

            vm.MothersOccupation = inventory.MothersOccupation;
            vm.MothersEmployerAddress = inventory.MothersEmployerAddress;
            vm.FamilyDwelling = inventory.FamilyDwelling;
            vm.EmergencyContactName = inventory.EmergencyContactName;
            vm.EmergencyContactNumber = inventory.EmergencyContactNumber;

            if (inventory.ParentsStatus != null)
            {
                vm.ParentsStatus = inventory.ParentsStatus.Trim();
            }

            if (inventory.EconomicStatus != null)
            {
                vm.EconomicStatus = inventory.EconomicStatus.Trim();
            }

            vm.NoOfSiblings = inventory.NoOfSiblings;

            if (inventory.PresentlyLivingWith != null)
            {
                vm.PresentlyLivingWith = inventory.PresentlyLivingWith.Trim();
            }

            if (inventory.PresentlyStayingAt != null)
            {
                vm.PresentlyStayingAt = inventory.PresentlyStayingAt.Trim();
            }

            vm.ElementarySchool = inventory.ElementarySchool;
            vm.ElementaryAddress = inventory.ElementaryAddress;
            vm.YearsAttendedElem = inventory.YearsAttendedElem;
            vm.HighSchool = inventory.HighSchool;
            vm.HighSchoolAddress = inventory.HighSchoolAddress;
            vm.YearsAttendedHS = inventory.YearsAttendedHS;
            vm.CollegeSchool = inventory.CollegeSchool;
            vm.CollegeAddress = inventory.CollegeAddress;
            vm.YearsAttendedCollege = inventory.YearsAttendedCollege;
            vm.Honors = inventory.Honors;
            vm.FaveSubject = inventory.FaveSubject;
            vm.LeastSubject = inventory.LeastSubject;
            vm.HowStudieIssFinanced = inventory.HowStudieIssFinanced;

            if (inventory.IsCoursePersonalChoice != null && inventory.IsCoursePersonalChoice == false)
            {
                vm.IsCoursePersonalChoice = (bool) inventory.IsCoursePersonalChoice;
                vm.CourseNotPersonalChoice = inventory.CourseNotPersonalChoice;
                vm.CoursePersonalChoice = inventory.CoursePersonalChoice;
            }
            else if (inventory.IsCoursePersonalChoice != null && inventory.IsCoursePersonalChoice == true)
            {
                vm.IsCoursePersonalChoice = (bool) inventory.IsCoursePersonalChoice;

            }

            if (inventory.CourseChoiceInfluence != null)
            {
                vm.CourseChoiceInfluence = inventory.CourseChoiceInfluence.Trim();
            }

            if (inventory.WhyMMCC != null)
            {
                vm.WhyMMCC = inventory.WhyMMCC.Trim();
            }

            if (inventory.ReferredToMMCCBy != null)
            {
                vm.ReferredToMMCCBy = inventory.ReferredToMMCCBy.Trim();
            }

            vm.Position = inventory.Position;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus.Trim();
            }

            vm.Salary = inventory.Salary;
            vm.Employer = inventory.Employer;
            vm.EmployerAddress = inventory.EmployerAddress;

            if (vm.EmploymentStatus != null)
            {
                vm.EmploymentStatus = inventory.EmploymentStatus;

            }

            vm.MentalAbilityTestDate = inventory.MentalAbilityTestDate;
            vm.MentalAbilityTestScore = inventory.MentalAbilityTestScore;
            vm.MentalAbilityTestPercentile = inventory.MentalAbilityTestPercentile;
            vm.PersonalityTestDate = inventory.PersonalityTestDate;
            vm.PersonalityTestScore = inventory.PersonalityTestScore;
            vm.PersonalityTestPercentile = inventory.PersonalityTestPercentile;
            vm.VocationalTestDate = inventory.VocationalTestDate;
            vm.VocationalTestScore = inventory.VocationalTestScore;
            vm.VocationalTestPercentile = inventory.VocationalTestPercentile;
            vm.Disabilities = inventory.Disabilities;
            vm.ChronicIllness = inventory.ChronicIllness;
            vm.PreviousAccidents = inventory.PreviousAccidents;
            vm.PreviousSurgery = inventory.PreviousSurgery;
            vm.MaintenanceMedicines = inventory.MaintenanceMedicines;
            vm.Immunization = inventory.Immunization;
            vm.HaveTalkedWithACounselor = inventory.HaveTalkedWithACounselor;
            vm.HaveTalkedWithACounselorWhen = inventory.HaveTalkedWithACounselorWhen;
            vm.HaveTalkedWithACounselorWhy = inventory.HaveTalkedWithACounselorWhy;
            vm.HaveTalkedWithAPsychiatrist = inventory.HaveTalkedWithAPsychiatrist;
            vm.HaveTalkedWithAPsychiatristWhen = inventory.HaveTalkedWithAPsychiatristWhen;
            vm.HaveTalkedWithAPsychiatristWhy = inventory.HaveTalkedWithAPsychiatristWhy;
            vm.HaveTalkedWithAPsychologist = inventory.HaveTalkedWithAPsychologist;
            vm.HaveTalkedWithAPsychologistWhen = inventory.HaveTalkedWithAPsychologistWhen;
            vm.HaveTalkedWithAPsychologistWhy = inventory.HaveTalkedWithAPsychologistWhy;
            vm.AboutYourself = inventory.AboutYourself;


            return View(vm);
        }

        [Authorize(Roles = "Student")]
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

            var userInv = db.IndividualInventoryRecord.FirstOrDefault(d => d.UserID == currentUserId);

            if (userInv == null)
            {
                userInv = db.IndividualInventoryRecord.Create();
                userInv.UserID = currentUserId;
                db.IndividualInventoryRecord.Add(userInv);
            }



            var civilStatus = GetAllCivilStatus();
            vm.Civil_Status__CivilStatuss = GetSelectListItems(civilStatus);

            var gender = GetAllGender();
            vm.Sexx = GetSelectListItems(gender);

            var spouseEducAttainment = GetAllEducationalAttainment();
            vm.SpouseEducationalAttainments = GetSelectListItems(spouseEducAttainment);

            var motherEducAttainment = GetAllEducationalAttainment();
            vm.MothersEducationalAttainments = GetSelectListItems(motherEducAttainment);

            var fatherEducAttainment = GetAllEducationalAttainment();
            vm.FathersEducationalAttainments = GetSelectListItems(fatherEducAttainment);

            var familyDwelling = GetAllFamilyDwelling();
            vm.FamilyDwellings = GetSelectListItems(familyDwelling);

            var parentsStatus = GetAllParentsStatus();
            vm.ParentsStatuses = GetSelectListItems(parentsStatus);

            var economicStatus = GetAllEconomicStatus();
            vm.EconomicStatuses = GetSelectListItems(economicStatus);

            var livingPresentlyWith = GetAllLivingPresentlyWith();
            vm.PresentlyLivingWiths = GetSelectListItems(livingPresentlyWith);

            var presentlyStayingAt = GetAllPresentlyStaying();
            vm.PresentlyStayingAts = GetSelectListItems(presentlyStayingAt);

            var employmentStatus = GetAllEmploymentStatus();
            vm.EmploymentStatuses = GetSelectListItems(employmentStatus);

            //
            var whyMMCC = GetAllWhyMMCC();
            vm.WhyMMCC_Dropdown = GetSelectListItems(whyMMCC);

            var howYouKnowMMCC = GetAllHowUKnowMMCC();
            vm.ReferredToMMCCBy_Dropdown = GetSelectListItems(howYouKnowMMCC);

            var whoInfluencedYou = GetAllWhoInfluencedYou();
            vm.CourseChoiceInfluence_Dropdown = GetSelectListItems(whoInfluencedYou);

            if (ModelState.IsValid)
            {
                if (vm.Sex != null)
                {
                    u.Sex = vm.Sex;
                }

                if (vm.Civil_Status__CivilStatus != null)
                {
                    u.Civil_Status__CivilStatus = vm.Civil_Status__CivilStatus;
                }

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

                bool isScholar = false;
                if (vm.IsScholar == true)
                {
                    isScholar = true;
                }
                else
                {
                    isScholar = false;
                }

                u.IsScholar = isScholar;


                u.Scholarship = vm.Scholarship;

                if (vm.Civil_Status__CivilStatus != null && vm.Civil_Status__CivilStatus.Trim() == "Married")
                {
                    u.DateOfMarriage = vm.DateOfMarriage;
                    u.PlaceOfMarriage = vm.PlaceOfMarriage;
                    u.SpouseName = vm.SpouseName;
                    u.SpouseAge = vm.SpouseAge;

                    if (vm.SpouseEducationalAttainment != null)
                    {
                        u.SpouseEducationalAttainment = vm.SpouseEducationalAttainment;
                    }

                    u.Occupation = vm.Occupation;
                    u.StudentEmployerAddress = vm.StudentEmployerAddress;
                    u.NumberOfChildren = vm.NumberOfChildren;

                }else
                {
                    u.DateOfMarriage = null;
                    u.PlaceOfMarriage = null;
                    u.SpouseName = null;
                    u.SpouseAge = null;
                    u.SpouseEducationalAttainment = null;
                    u.Occupation = null;
                    u.StudentEmployerAddress = null;
                    u.NumberOfChildren = null;

                }

                

                //inventory
                userInv.FathersName = vm.FathersName;
                userInv.FathersAddress = vm.FathersAddress;
                userInv.FathersAge = vm.FathersAge;

                if (vm.FathersEducationalAttainment != null)
                {
                    userInv.FathersEducationalAttainment = vm.FathersEducationalAttainment;
                }

                userInv.FathersOccupation = vm.FathersOccupation;
                userInv.FathersEmployerAddress = vm.FathersEmployerAddress;
                userInv.MothersName = vm.MothersName;
                userInv.MothersAddress = vm.MothersAddress;
                userInv.MothersAge = vm.MothersAge;

                if (vm.MothersEducationalAttainment != null)
                {
                    userInv.MothersEducationalAttainment = vm.MothersEducationalAttainment;
                }

                userInv.MothersOccupation = vm.MothersOccupation;
                userInv.MothersEmployerAddress = vm.MothersEmployerAddress;

                if (vm.FamilyDwelling != null)
                {
                    userInv.FamilyDwelling = vm.FamilyDwelling;
                }

                userInv.EmergencyContactName = vm.EmergencyContactName;
                userInv.EmergencyContactNumber = vm.EmergencyContactNumber;

                if (vm.ParentsStatus != null)
                {
                    userInv.ParentsStatus = vm.ParentsStatus;
                }

                if (vm.EconomicStatus != null)
                {
                    userInv.EconomicStatus = vm.EconomicStatus;
                }

                userInv.NoOfSiblings = vm.NoOfSiblings;

                if (vm.PresentlyLivingWith != null)
                {
                    userInv.PresentlyLivingWith = vm.PresentlyLivingWith;
                }

                if (vm.PresentlyStayingAt != null)
                {
                    userInv.PresentlyStayingAt = vm.PresentlyStayingAt;

                }

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

                bool IsCoursePersonalChoice = false;
                if (vm.IsCoursePersonalChoice == true)
                {
                    IsCoursePersonalChoice = true;
                    userInv.CourseNotPersonalChoice = "N/A";
                    userInv.CoursePersonalChoice = "N/A";

                }
                else
                {
                    IsCoursePersonalChoice = false;
                    userInv.CourseNotPersonalChoice = vm.CourseNotPersonalChoice;
                    userInv.CoursePersonalChoice = vm.CoursePersonalChoice;
                }

                userInv.IsCoursePersonalChoice = IsCoursePersonalChoice;

                if (vm.CourseChoiceInfluence != null)
                {
                    userInv.CourseChoiceInfluence = vm.CourseChoiceInfluence.Trim();
                }

                if (vm.WhyMMCC != null)
                {
                    userInv.WhyMMCC = vm.WhyMMCC.Trim();
                }

                if (vm.ReferredToMMCCBy != null)
                {
                    userInv.ReferredToMMCCBy = vm.ReferredToMMCCBy.Trim();
                }

                userInv.Position = vm.Position;

                if (vm.EmploymentStatus != null)
                {
                    userInv.EmploymentStatus = vm.EmploymentStatus.Trim();
                }

                userInv.Salary = vm.Salary;
                userInv.Employer = vm.Employer;
                userInv.EmployerAddress = vm.EmployerAddress;

                if (vm.EmploymentStatus != null)
                {
                    userInv.EmploymentStatus = vm.EmploymentStatus;

                }

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
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["Error"] = "Error: Details not updated! Please answer all required fields.";
                
            }

            return View(vm);
        }

        [Authorize(Roles = "Counselor")]
        public ActionResult Charts()
        {

            return View();
        }

        //Get chart data
        public JsonResult GetCount()
        {
            List<ResultViewModel> list = new List<ResultViewModel>();

                ResultViewModel vm = new ResultViewModel();
                int TotalMale = db.Students
                        .Where(x => x.Sex == "Male")
                        .Count();

                int TotalFemale = db.Students
                            .Where(x => x.Sex == "Female")
                            .Count();

                int TotalScholar = db.Students
                            .Where(x => x.IsScholar == true)
                            .Count();

                int TotalNotScholar = db.Students
                            .Where(x => x.IsScholar == false)
                            .Count();

                int TotalPersonalChoice = db.IndividualInventoryRecord
                                .Where(x => x.IsCoursePersonalChoice == true)
                                .Count();

                int TotalNotPersonalChoice = db.IndividualInventoryRecord
                            .Where(x => x.IsCoursePersonalChoice == false)
                            .Count();

                if (TotalPersonalChoice == 0 && TotalNotPersonalChoice == 0)
                {
                    TempData["TotalGetCount"] = "0";
                }    

                vm.countMale = TotalMale;
                vm.countFemale = TotalFemale;
                vm.countScholar = TotalScholar;
                vm.countNotScholar = TotalNotScholar;
                vm.countTotalPersonalChoice = TotalPersonalChoice;
                vm.countTotalNotPersonalChoice = TotalNotPersonalChoice;

                list.Add(vm);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetWhyMMCCListCount()
        {
            var WhyMMCCList = GetAllWhyMMCC().ToList();
            List<ResultViewModel> list0 = new List<ResultViewModel>();

            //Why MMCC?
            foreach (var item in WhyMMCCList)
            {
                ResultViewModel vm = new ResultViewModel();

                var countWhyMMCC = (from tagList in db.IndividualInventoryRecord
                                 where tagList.WhyMMCC == item
                                 group tagList by new { tagList.WhyMMCC } into g
                                 orderby g.Count() descending
                                 select g.Count()
                                 ).SingleOrDefault();

                vm.WhyMMCC = item.ToString();
                vm.countWhyMMCC = countWhyMMCC;

                list0.Add(vm);

            }

            return Json(list0, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetHowMMCCListCount()
        {
            List<ResultViewModel> list0 = new List<ResultViewModel>();

            //How did you know about MMCC?
            var HowMMCCList = GetAllHowUKnowMMCC().ToList();
            foreach (var item in HowMMCCList)
            {
                ResultViewModel vm = new ResultViewModel();

                var countHowMMCC = (from tagList in db.IndividualInventoryRecord
                                    where tagList.ReferredToMMCCBy == item
                                    group tagList by new { tagList.ReferredToMMCCBy } into g
                                    orderby g.Count() descending
                                    select g.Count()
                                 ).SingleOrDefault();

                vm.HowMMCC = item.ToString();
                vm.countHowMMCC = countHowMMCC;

                list0.Add(vm);

            }

            return Json(list0, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetWhoInfluencedMMCCListCount()
        {
            List<ResultViewModel> list0 = new List<ResultViewModel>();

            //Who influenced you to take the course?
            var WhoInfluencedMMCCList = GetAllWhoInfluencedYou().ToList();
            foreach (var item in WhoInfluencedMMCCList)
            {
                ResultViewModel vm = new ResultViewModel();

                var countWhoInfluenced = (from tagList in db.IndividualInventoryRecord
                                          where tagList.CourseChoiceInfluence == item
                                          group tagList by new { tagList.CourseChoiceInfluence } into g
                                          orderby g.Count() descending
                                          select g.Count()
                                 ).SingleOrDefault();

                vm.WhoInfluenced = item.ToString();
                vm.countWhoInfluenced = countWhoInfluenced;

                list0.Add(vm);

            }


            return Json(list0, JsonRequestBehavior.AllowGet);

        }

        //Dropdown values
        private IEnumerable<string> GetAllWhoInfluencedYou()
        {
            return new List<string>
            {
                "Parents/Relatives",
                "Peers/Friends",
                "Teacher",
                "School/Particular Subject",
                "Books/Literature",
                "Film/TV/Radio",
                "Internet",
                "I don't know"
            };
        }

        private IEnumerable<string> GetAllHowUKnowMMCC()
        {
            return new List<string>
            {
                "Social Media",
                "School/Teachers",
                "Parent/Relatives",
                "Acquaintance/Peers",
                "Newspaper",
                "MMCC Website"
            };
        }

        private IEnumerable<string> GetAllWhyMMCC()
        {
            return new List<string>
            {
                "Reputation of the school",
                "People I know studied in MMCC",
                "Lower fees/cost of living",
                "Near home/convenience"
            };
        }

        private IEnumerable<string> GetAllEducationalAttainment()
        {
            return new List<string>
            {
                "Elementary Level",
                "Elementary Graduate",
                "High school Level",
                "High school Graduate",
                "College Level",
                "College Graduate",
                "Vocational/Associate"
            };
        }

        private IEnumerable<string> GetAllEmploymentStatus()
        {
            return new List<string>
            {
                "Regular",
                "Probationary",
                "Temporary",
                "Part-time",
            };
        }

        private IEnumerable<string> GetAllParentsStatus()
        {
            return new List<string>
            {
                "Married",
                "Living-in",
                "Seperated",
                "Annulled",
                "Divorced",
                "Widowed"
            };
        }

        private IEnumerable<string> GetAllEconomicStatus()
        {
            return new List<string>
            {
                "20,000 and below",
                "20,000 - 30,000",
                "30,000 - 40,000",
                "50,000 and above",
            };
        }

        private IEnumerable<string> GetAllFamilyDwelling()
        {
            return new List<string>
            {
                "House & Lot (Owned)",
                "Renting",
                "Boarding",
                "Living with relatives",
            };
        }

        private IEnumerable<string> GetAllLivingPresentlyWith()
        {
            return new List<string>
            {
                "Both Parents",
                "Mother only",
                "Father only",
                "Relatives",
            };
        }

        private IEnumerable<string> GetAllPresentlyStaying()
        {
            return new List<string>
            {
                "Renting",
                "Boarding",
                "Residing at home",
                "Residing with relatives",
            };
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
            var selectList = new List<SelectListItem>();

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

    }

}