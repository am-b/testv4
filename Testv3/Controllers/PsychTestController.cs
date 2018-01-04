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
                List<PsychTestViewModel>PsychTestList = new List<PsychTestViewModel>();
                int intSkip = (intPage - 1) * intPageSize;
                intTotalPageCount = db.PsychTestQuestions
                    .Where(x => x.Question.Contains(searchStringQuestion))
                    .Count();

                var datalist = db.PsychTestQuestions
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

            PsychTestQuestion psychTest = new PsychTestQuestion();
            PsychTestViewModel pvm = new PsychTestViewModel();
            pvm.QuestionID = psychTest.QuestionID;
            pvm.Question = psychTest.Question;

            return View(pvm);
        }

        // POST: PsychologicalTest/Create
        [Authorize(Roles = "Counselor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest([Bind(Include = "QuestionID,Question")] PsychTestViewModel psychTestViewModel)
        {
            GetCurrentUserInViewBag();
            PsychTestQuestion psychTest = new PsychTestQuestion();
            

            if (ModelState.IsValid)
            {

                psychTest.QuestionID = psychTestViewModel.QuestionID;
                psychTest.Question = psychTestViewModel.Question;

                db.PsychTestQuestions.Add(psychTest);
                db.SaveChanges();
                TempData["Message"] = "Question " + psychTest.QuestionID + " successfully added!";
            }
            else
            {
                TempData["Error"] = "Question " + psychTest.QuestionID + " not added!";
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
            PsychTestQuestion psychTestQuestion = db.PsychTestQuestions.Find(QuestionID);
            if (psychTestQuestion == null)
            {
                return HttpNotFound();
            }

            PsychTestViewModel ptvm = new PsychTestViewModel();
            ptvm.Question = psychTestQuestion.Question;

            return View(ptvm);
        }

        // POST: PsychologicalTest/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? QuestionID, PsychTestViewModel ptvm)
        {
            GetCurrentUserInViewBag();
            PsychTestQuestion psychTestQuestion = db.PsychTestQuestions.Find(QuestionID);
            if (psychTestQuestion == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                psychTestQuestion.Question = ptvm.Question;
                db.SaveChanges();

                TempData["Message"] = "Question " + psychTestQuestion.QuestionID + " successfully updated!";
            }
            else
            {
                TempData["Error"] = "Question " + psychTestQuestion.QuestionID + " not updated!";
            }

            return RedirectToAction("Index");

        }
        // DELETE: /PsychologicalTest/Delete
        [Authorize(Roles = "Counselor")]
        public ActionResult Delete(int QuestionID)
        {
            GetCurrentUserInViewBag();

            PsychTestQuestion psychTestQuestion = db.PsychTestQuestions.Find(QuestionID);
            db.PsychTestQuestions.Remove(psychTestQuestion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}