using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    public class PsychTestController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: PsychTest
        [Authorize(Roles = "Counselor")]
        public ActionResult Index(string searchStringQuestion, string currentFilter, int? page)
        {
            GetCurrentUserInViewBag();

            try
            {
                int intPage = 1;
                int intPageSize = 10;
                int intTotalPageCount = 0;

                if (searchStringQuestion != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringQuestion = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringQuestion = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringQuestion;
                List<PsychTestViewModel> PsychTestList = new List<PsychTestViewModel>();
                int intSkip = (intPage - 1) * intPageSize;
                intTotalPageCount = db.Questions
                    .Where(x => x.Question.Contains(searchStringQuestion))
                    .Count();

                var datalist = db.Questions
                    .Where(x => x.Question.Contains(searchStringQuestion))
                    .OrderBy(x => x.QuestionID)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in datalist)
                {
                    PsychTestViewModel pvm = new PsychTestViewModel();
                    pvm.QuestionID = item.QuestionID;
                    pvm.Question = item.Question;
                    PsychTestList.Add(pvm);
                }

                // Set the number of pages
                var _UserAsIPagedList =
                    new StaticPagedList<PsychTestViewModel>
                    (
                        PsychTestList, intPage, intPageSize, intTotalPageCount
                        );

                return View(_UserAsIPagedList);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<PsychTestViewModel> PsychTestList = new List<PsychTestViewModel>();

                return View(PsychTestList.ToPagedList(1, 25));
            }

        }

        // GET: PsychologicalTest/Create
        [Authorize(Roles = "Counselor")]
        public ActionResult CreateTest()
        {
            GetCurrentUserInViewBag();

            Questions psychTest = new Questions();
            PsychTestViewModel pvm = new PsychTestViewModel();

            var questionTag = GetAllQuestionTags();
            pvm.QuestionTags = GetSelectListItems(questionTag);
            
            if (psychTest.QuestionTag != null)
            {
                pvm.QuestionTag = psychTest.QuestionTag.Trim();
            }

            pvm.QuestionID = psychTest.QuestionID;
            pvm.Question = psychTest.Question;

            return View(pvm);
        }

        // POST: PsychologicalTest/Create
        [Authorize(Roles = "Counselor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest([Bind(Include = "QuestionID,Question,QuestionTag")] PsychTestViewModel psychTestViewModel)
        {
            GetCurrentUserInViewBag();

            Questions Questions = new Questions();

            if (ModelState.IsValid)
            {
                //put question in Questions Table
                var questionTag = GetAllQuestionTags();
                psychTestViewModel.QuestionTags = GetSelectListItems(questionTag);

                if (psychTestViewModel.QuestionTag != null)
                {
                    Questions.QuestionTag = psychTestViewModel.QuestionTag;
                }

                Questions.QuestionID = psychTestViewModel.QuestionID;
                Questions.Question = psychTestViewModel.Question;

                db.Questions.Add(Questions);
                db.SaveChanges();

                TempData["Message"] = "Question " + Questions.QuestionID + " successfully added!";
            }
            else
            {
                TempData["Error"] = "Question " + Questions.QuestionID + " not added!";
            }

            return RedirectToAction("Index");
        }

        // GET: PsychologicalTest/Edit
        public ActionResult Edit(int? QuestionID)
        {
            GetCurrentUserInViewBag();

            if (QuestionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions Questions = db.Questions.Find(QuestionID);
            if (Questions == null)
            {
                return HttpNotFound();
            }

            PsychTestViewModel ptvm = new PsychTestViewModel();
            ptvm.Question = Questions.Question;

            return View(ptvm);
        }

        // POST: PsychologicalTest/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? QuestionID, PsychTestViewModel ptvm)
        {
            GetCurrentUserInViewBag();
            Questions Questions = db.Questions.Find(QuestionID);
            if (Questions == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Questions.Question = ptvm.Question;
                db.SaveChanges();

                TempData["Message"] = "Question " + Questions.QuestionID + " successfully updated!";
            }
            else
            {
                TempData["Error"] = "Question " + Questions.QuestionID + " not updated!";
            }

            return RedirectToAction("Index");

        }

        // DELETE: /PsychologicalTest/Delete
        [Authorize(Roles = "Counselor")]
        public ActionResult Delete(int QuestionID)
        {
            GetCurrentUserInViewBag();

            Questions Questions = db.Questions.Find(QuestionID);
            db.Questions.Remove(Questions);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: PsychologicalTest/Student
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            List<PsychTestViewModel> PsychTestList = new List<PsychTestViewModel>();

            //get lahat ng questions sa Questions table
            var datalist = db.Questions
                    .OrderBy(x => x.QuestionID)
                    .ToList();


            foreach (var item in datalist)
            {
                PsychTestViewModel pvm = new PsychTestViewModel();

                //lagay sa vm yung value sa db
                pvm.QuestionID = item.QuestionID;
                pvm.Question = item.Question;
                PsychTestList.Add(pvm);
            }

            //get lahat ng answers sa Answers table
            var answers = db.Answers
                    .OrderBy(x => x.QuestionID)
                    .ToList();


            foreach (var item in answers)
            {

                //check if may row na may laman ang userID, questionID, answerid, answer
                var check = db.Answers
                    .Where(x => x.AnswerID == item.AnswerID && x.QuestionID == item.QuestionID && x.UserID == currentUserId && x.Answer == item.Answer)
                    .ToList();

                if (check.Count() != 0)
                {
                    TempData["Message"] = "You have already completed this test!";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(PsychTestList);

        }

        // POST: PsychologicalTest/Student
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student(List<PsychTestViewModel> pvm)
        {
            GetCurrentUserInViewBag();

            var currentUserId = User.Identity.GetUserId();

            List<PsychTestViewModel> PsychTestList = new List<PsychTestViewModel>();

                foreach (PsychTestViewModel item in pvm)
                {

                    //check if may row na may laman ang userID, questionID, answerid, answer
                    var check = db.Answers
                        .Where(x => x.AnswerID == item.AnswerID && x.QuestionID == item.QuestionID && x.UserID == currentUserId && x.Answer == item.Answer)
                        .ToList(); 

                    if (check.Count() == 0)
                    {
                        //query ang row where answerid = answerid sa vm, syempre dapat wala
                        //var newTestId = db.Answers.FirstOrDefault(d => d.QuestionID == item.QuestionID);
                        var newTestId = db.Answers.FirstOrDefault(d => d.AnswerID == item.AnswerID);

                        //create ng bagong entry and lagyan ng laman the ff:
                        newTestId = db.Answers.Create();
                        newTestId.AnswerID = item.AnswerID;
                        newTestId.QuestionID = item.QuestionID;
                        newTestId.UserID = currentUserId;
                        newTestId.Answer = item.Answer;

                        db.Answers.Add(newTestId);
                        db.SaveChanges();
                        TempData["Message"] = "Survey successfully completed!";
                    }
                }

            return RedirectToAction("Index", "Home");
        }

        // GET: PsychologicalTest/Student
        [Authorize(Roles = "Counselor")]
        public ActionResult Responses()
        {


            return View();
        }

        private IEnumerable<string> GetAllQuestionTags()
        {
            return new List<string>
            {
                "Physical",
                "Emotional",
                "Social",
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