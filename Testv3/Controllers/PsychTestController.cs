using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using Rotativa;
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

            if (psychTest.IsQuestionPositive != null)
            {
                pvm.IsQuestionPositive = (bool)psychTest.IsQuestionPositive;
            }

            

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

                bool IsQuestionPositive = false;
                if (psychTestViewModel.IsQuestionPositive == true)
                {
                    IsQuestionPositive = true;
                }
                else
                {
                    IsQuestionPositive = false;
                }

                Questions.IsQuestionPositive = IsQuestionPositive;
                

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
            var questionTag = GetAllQuestionTags();
            ptvm.QuestionTags = GetSelectListItems(questionTag);

            if (Questions.QuestionTag != null)
            {
                ptvm.QuestionTag = Questions.QuestionTag.Trim();
            }

            if (Questions.IsQuestionPositive != null)
            {
                ptvm.IsQuestionPositive = (bool)Questions.IsQuestionPositive;
            }

            

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

                var questionTag = GetAllQuestionTags();
                ptvm.QuestionTags = GetSelectListItems(questionTag);

                if (ptvm.QuestionTag != null)
                {
                    Questions.QuestionTag = ptvm.QuestionTag;
                }

                bool IsQuestionPositive = false;
                if (ptvm.IsQuestionPositive == true)
                {
                    IsQuestionPositive = true;
                }
                else
                {
                    IsQuestionPositive = false;
                }

                Questions.IsQuestionPositive = IsQuestionPositive;
                

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
                        newTestId.TestCompletionDate = DateTime.Now;

                        db.Answers.Add(newTestId);
                        db.SaveChanges();
                        TempData["Message"] = "Survey successfully completed!";
                    }
                }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Counselor")]
        public ActionResult Charts()
        {
            GetCurrentUserInViewBag();

            List<TestViewModel> StudentInventorylist = new List<TestViewModel>();

            var datalist = db.Students.ToList();

            int noAnswer = 0;
            List<TestViewModel> StudentInventorylist3 = new List<TestViewModel>();

            foreach (var item in datalist)
            {
                TestViewModel pvm = new TestViewModel();
                pvm.StudentFirstName = item.StudentFirstName;
                pvm.StudentMiddleName = item.StudentMiddleName;
                pvm.StudentLastName = item.StudentLastName;
                pvm.UserID = item.UserID;
                pvm.StudentID = item.StudentID;
                StudentInventorylist.Add(pvm);

                //see how many students have no psych test
                var dlist =
                    (from ans in db.Answers
                     where ans.UserID == pvm.UserID
                     select new { Answer = ans.Answer });

                int countOfTotalStudents = datalist.Count();

                if (dlist.Count() == 0)
                {
                    noAnswer++;

                    ViewBag.ListOfStudents = "List of students who haven't taken the Psychological Test";

                    //write yung names ng student where nag dlist.count == 0
                    TestViewModel tvm = new TestViewModel();
                    tvm.StudentFirstName = pvm.StudentFirstName;
                    tvm.StudentMiddleName = pvm.StudentMiddleName;
                    tvm.StudentLastName = pvm.StudentLastName;
                    tvm.UserID = pvm.UserID;
                    tvm.StudentID = pvm.StudentID;
                    StudentInventorylist3.Add(tvm);

                    ViewBag.studentlist = StudentInventorylist3;

                    int countOfStudentsWithNoPsychTest = noAnswer;
                    ViewBag.CountOfTotalStudents = countOfTotalStudents;
                    ViewBag.CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;

                }
                else
                {
                    int countOfStudentsWithNoPsychTest = noAnswer;

                    ViewBag.CountOfTotalStudents = countOfTotalStudents;
                    ViewBag.CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;
                }

            }

            return View();
        }

        //public ContentResult GetData()
        public JsonResult GetData()
        {
            var datalistQuestions = db.Questions.ToList();
            List<ResultViewModel> questionlist = new List<ResultViewModel>();

            var datalist = db.Students.ToList();

            int noAnswer = 0;
            int countOfTotalStudents = 0;
            int countOfStudentsWithNoPsychTest = 0;
            int CountOfTotalStudents = 0;
            int CountOfStudentsWithNoPsychTest = 0;

            foreach (var item in datalist)
            {
                TestViewModel pvm = new TestViewModel();
                pvm.UserID = item.UserID;

                //see how many students have no psych test
                var dlist =
                    (from ans in db.Answers
                     where ans.UserID == pvm.UserID
                     select new { Answer = ans.Answer });

                 countOfTotalStudents = datalist.Count();

                if (dlist.Count() == 0)
                {
                    noAnswer++;

                     countOfStudentsWithNoPsychTest = noAnswer;
                     CountOfTotalStudents = countOfTotalStudents;
                     CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;

                }
                else
                {
                    countOfStudentsWithNoPsychTest = noAnswer;
                    CountOfTotalStudents = countOfTotalStudents;
                    CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;
                }

            }

            foreach (var question in datalistQuestions)
            {
                ResultViewModel ptvm = new ResultViewModel();

                var questionItem = (from qs in db.Questions
                                   where qs.QuestionID == question.QuestionID
                                   select new { Question = qs.Question }).Single();
                string str = questionItem.ToString();
                char[] MyChar = { '{', 'Q', 'u', 'e', 's','t','i','o','n','=', ' ' };
                string NewString = str.TrimStart(MyChar);

                char[] MyChar1 = { '}', ' '};
                string NewString1 = NewString.TrimEnd(MyChar1);

                var agree = from ans in db.Answers
                            where ans.Answer == 1 && ans.QuestionID == question.QuestionID
                            select new { Answer = ans.Answer };
                int agreeCount = agree.Count();
                int agreePercentage = (int)((double)agreeCount / CountOfStudentsWithNoPsychTest * 100);


                var somewhatAgree = from ans in db.Answers
                                    where ans.Answer == 2 && ans.QuestionID == question.QuestionID
                                    select new { Answer = ans.Answer };
                int somewhatAgreeCount = somewhatAgree.Count();
                int somewhatAgreePercentage = (int)((double)somewhatAgreeCount / CountOfStudentsWithNoPsychTest * 100);


                var disagree = from ans in db.Answers
                               where ans.Answer == 3 && ans.QuestionID == question.QuestionID
                               select new { Answer = ans.Answer };
                int disagreeCount = disagree.Count();
                int disagreePercentage = (int)((double)disagreeCount / CountOfStudentsWithNoPsychTest * 100);

                ptvm.questionItem = NewString1;
                ptvm.countAgree = agreePercentage;
                ptvm.countSomewhatAgree = somewhatAgreePercentage;
                ptvm.countDisagree = disagreePercentage;

                questionlist.Add(ptvm);
                
            }

            return Json(questionlist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDataAgree()
        {
            var datalistQuestions = db.Questions.ToList();
            List<ResultViewModel> questionlist = new List<ResultViewModel>();

            var datalist = db.Students.ToList();

            int noAnswer = 0;
            int countOfTotalStudents = 0;
            int countOfStudentsWithNoPsychTest = 0;
            int CountOfTotalStudents = 0;
            int CountOfStudentsWithNoPsychTest = 0;

            foreach (var item in datalist)
            {
                TestViewModel pvm = new TestViewModel();
                pvm.UserID = item.UserID;

                //see how many students have no psych test
                var dlist =
                    (from ans in db.Answers
                     where ans.UserID == pvm.UserID
                     select new { Answer = ans.Answer });

                countOfTotalStudents = datalist.Count();

                if (dlist.Count() == 0)
                {
                    noAnswer++;

                    countOfStudentsWithNoPsychTest = noAnswer;
                    CountOfTotalStudents = countOfTotalStudents;
                    CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;

                }
                else
                {
                    countOfStudentsWithNoPsychTest = noAnswer;
                    CountOfTotalStudents = countOfTotalStudents;
                    CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;
                }

            }

            foreach (var question in datalistQuestions)
            {
                ResultViewModel ptvm = new ResultViewModel();

                var questionItem = (from qs in db.Questions
                                    where qs.QuestionID == question.QuestionID
                                    select new { Question = qs.Question }).Single();
                string str = questionItem.ToString();
                char[] MyChar = { '{', 'Q', 'u', 'e', 's', 't', 'i', 'o', 'n', '=', ' ' };
                string NewString = str.TrimStart(MyChar);

                char[] MyChar1 = { '}', ' ' };
                string NewString1 = NewString.TrimEnd(MyChar1);

                var agree = from ans in db.Answers
                            where ans.Answer == 1 && ans.QuestionID == question.QuestionID
                            select new { Answer = ans.Answer };
                int agreeCount = agree.Count();
                int agreePercentage = (int)((double)agreeCount / CountOfStudentsWithNoPsychTest * 100);


                var somewhatAgree = from ans in db.Answers
                                    where ans.Answer == 2 && ans.QuestionID == question.QuestionID
                                    select new { Answer = ans.Answer };
                int somewhatAgreeCount = somewhatAgree.Count();
                int somewhatAgreePercentage = (int)((double)somewhatAgreeCount / CountOfStudentsWithNoPsychTest * 100);


                var disagree = from ans in db.Answers
                               where ans.Answer == 3 && ans.QuestionID == question.QuestionID
                               select new { Answer = ans.Answer };
                int disagreeCount = disagree.Count();
                int disagreePercentage = (int)((double)disagreeCount / CountOfStudentsWithNoPsychTest * 100);

                ptvm.questionItem = NewString1;
                ptvm.countAgree = agreePercentage;
                ptvm.countSomewhatAgree = somewhatAgreePercentage;
                ptvm.countDisagree = disagreePercentage;

                questionlist.Add(ptvm);

            }

            return Json(questionlist, JsonRequestBehavior.AllowGet);

        }



        [AllowAnonymous]
        // GET: PsychologicalTest/Responses
        //[Authorize(Roles = "Counselor")]
        public ActionResult Responses(string UserID)
        {
            GetCurrentUserInViewBag();

            List<PsychTestViewModel> PsychTestList = new List<PsychTestViewModel>();

            var datalist =
                        (from ans in db.Answers
                                 join question in db.Questions
                                 on ans.QuestionID equals question.QuestionID
                                 where ans.UserID == UserID
                                 select new { Answer = ans.Answer, QuestionID = question.QuestionID, Question = question.Question });

            if (datalist.Count() == 0){

                TempData["Error"] = "This user has not completed the test yet!";
            }
            else
            {
                var testCompletionDate = db.Answers
                        .Where(a => a.UserID == UserID)
                        .OrderByDescending(x => x.TestCompletionDate)
                        .Select(y => y.TestCompletionDate)
                        .First().ToString();

                ViewBag.TestCompletionDate = testCompletionDate;

                foreach (var item in datalist)
                {
                    PsychTestViewModel pvm = new PsychTestViewModel();

                    pvm.QuestionID = item.QuestionID;
                    pvm.Question = item.Question;
                    pvm.Answer = item.Answer;
                    PsychTestList.Add(pvm);
                }

                var answers = db.Answers
                        .OrderBy(x => x.QuestionID)
                        .ToList();

                var u = db.Students.FirstOrDefault(d => d.UserID == UserID);

                if ((u.StudentFirstName != null) && (u.StudentLastName != null))
                {
                    ViewBag.Student = u.StudentFirstName.Trim() + " " + " " + u.StudentLastName.Trim();
                }
                else
                {
                    ViewBag.Student = "Error: User has no name record.";
                }

                //loop through selectlist para dynamic si questiontag?
                //PHYSICAL
                var totalNumberOfNegativeQuestions = from question in db.Questions
                                                     where question.IsQuestionPositive == false
                                                     select new { Tag = question.IsQuestionPositive };

                //PHYSICAL
                var numberOfPhysicalQuestions = from question in db.Questions
                                                where question.QuestionTag == "Physical"
                                                select new { Tag = question.QuestionTag };


                var disagreeInPosPhysicalQuery =
                                    (from ans in db.Answers
                                     join question in db.Questions
                                     on ans.QuestionID equals question.QuestionID
                                     where ans.UserID == UserID && ans.Answer == 3 && question.QuestionTag == "Physical" && question.IsQuestionPositive == true
                                     select new { Answer = ans.Answer });


                //query Number of AGREE(Negative) in PHYSICAL
                var agreeInNegPhysicalQuery =
                                    (from ans in db.Answers
                                     join question in db.Questions on ans.QuestionID equals question.QuestionID
                                     where ans.UserID == UserID && ans.Answer == 1 && question.QuestionTag == "Physical" && question.IsQuestionPositive == false
                                     select new { Answer = ans.Answer });


                //EMOTIONAL
                var numberOfEmotionalQuestions = from question in db.Questions
                                                 where question.QuestionTag == "Emotional"
                                                 select new { Tag = question.QuestionTag };


                var disagreeInPosEmotionalQuery =
                                    (from ans in db.Answers
                                     join question in db.Questions
                                     on ans.QuestionID equals question.QuestionID
                                     where ans.UserID == UserID && ans.Answer == 3 && question.QuestionTag == "Emotional" && question.IsQuestionPositive == true
                                     select new { Answer = ans.Answer });

                //query Number of AGREE(Negative) in Emotional
                var agreeInNegEmotionalQuery = from ans in db.Answers
                                               join question in db.Questions on ans.QuestionID equals question.QuestionID
                                               where ans.UserID == UserID && ans.Answer == 1 && question.QuestionTag == "Emotional" && question.IsQuestionPositive == false
                                               select new { Answer = ans.Answer };


                //Social
                var numberOfSocialQuestions = from question in db.Questions
                                              where question.QuestionTag == "Social"
                                              select new { Tag = question.QuestionTag };

                //query Number of DISAGREE(Positive) in Social
                var disagreeInPosSocialQuery =
                                            (from ans in db.Answers
                                             join question in db.Questions
                                             on ans.QuestionID equals question.QuestionID
                                             where ans.UserID == UserID && ans.Answer == 3 && question.QuestionTag == "Social" && question.IsQuestionPositive == true
                                             select new { Answer = ans.Answer });

                //query Number of AGREE(Negative) in Social
                var agreeInNegSocialQuery = from ans in db.Answers
                                            join question in db.Questions on ans.QuestionID equals question.QuestionID
                                            where ans.UserID == UserID && ans.Answer == 1 && question.QuestionTag == "Social" && question.IsQuestionPositive == false
                                            select new { Answer = ans.Answer };

                int NumberOfPhysicalQuestions = numberOfPhysicalQuestions.Count();
                int TotalNumberOfNegativeQuestions = totalNumberOfNegativeQuestions.Count();
                int DiagreeInPosPhysicalQuery = disagreeInPosPhysicalQuery.Count();
                int AgreeInNegPhysicalQuery = agreeInNegPhysicalQuery.Count();
                int TotalNegativeAnswersInPhysical = DiagreeInPosPhysicalQuery + AgreeInNegPhysicalQuery;

                int NumberOfEmotionalQuestions = numberOfEmotionalQuestions.Count();
                int DisagreeInPosEmotionalQuery = disagreeInPosEmotionalQuery.Count();
                int AgreeInNegEmotionalQuery = agreeInNegEmotionalQuery.Count();
                int TotalNegativeAnswersInEmotional = DisagreeInPosEmotionalQuery + AgreeInNegEmotionalQuery;

                int NumberOfSocialQuestions = numberOfSocialQuestions.Count();
                int DisagreeInPosSocialQuery = disagreeInPosSocialQuery.Count();
                int AgreeInNegSocialQuery = agreeInNegSocialQuery.Count();
                int TotalNegativeAnswersInSocial = DisagreeInPosEmotionalQuery + AgreeInNegEmotionalQuery;

                int TotalNumberOfNegativeAnswers = TotalNegativeAnswersInPhysical + TotalNegativeAnswersInEmotional + TotalNegativeAnswersInSocial;

                //PHYSICAL
                int percentageOfPhysicalIssues = ((TotalNegativeAnswersInPhysical) * 200 + TotalNumberOfNegativeAnswers) / (TotalNumberOfNegativeAnswers * 2);
                //If positive ang question, 1 = good, 2= meh, 3 = bad
                //If negative ang question, 1 = bad, 2 = meh, 3 = good

                ViewBag.numOfPhysicalIssues = TotalNegativeAnswersInPhysical;
                ViewBag.countOfPhysicalIssues = NumberOfPhysicalQuestions;
                ViewBag.percentageOfPhysicalIssues = percentageOfPhysicalIssues;

                //EMOTIONAL
                int percentageOfEmotionalIssues = ((TotalNegativeAnswersInEmotional) * 200 + TotalNumberOfNegativeAnswers) / (TotalNumberOfNegativeAnswers * 2);
                ViewBag.numOfEmotionalIssues = TotalNegativeAnswersInEmotional;
                ViewBag.countOfEmotionalIssues = NumberOfEmotionalQuestions;
                ViewBag.percentageOfEmotionalIssues = percentageOfEmotionalIssues;

                //SOCIAL
                int percentageOfSocialIssues = ((TotalNegativeAnswersInSocial) * 200 + TotalNumberOfNegativeAnswers) / (TotalNumberOfNegativeAnswers * 2);
                ViewBag.numOfSocialIssues = TotalNegativeAnswersInSocial;
                ViewBag.countOfSocialIssues = NumberOfSocialQuestions;
                ViewBag.percentageOfSocialIssues = percentageOfSocialIssues;

                return View(PsychTestList);
            }

            return RedirectToAction("StudentList", "PsychTest");
        }

        public ContentResult GetData1()
        {
            var currentUserId = User.Identity.GetUserId();
            List<PsychTestViewModel> pvm = new List<PsychTestViewModel>();
            var results = db.Answers.ToList();

            foreach (Answers answers in results)
            {
                PsychTestViewModel viewmodel = new PsychTestViewModel();
                viewmodel.QuestionID = answers.QuestionID;
                viewmodel.Answer = answers.Answer;

                pvm.Add(viewmodel);
            }

            return Content(JsonConvert.SerializeObject(pvm), "application/json");
        }

        public ActionResult Print(string UserID)
        {
            var name = db.Students.Find(UserID);
            var student = name.StudentLastName + ", " + name.StudentFirstName;

            var datalist =
                        (from ans in db.Answers
                         join question in db.Questions
                         on ans.QuestionID equals question.QuestionID
                         where ans.UserID == UserID
                         select new { Answer = ans.Answer, QuestionID = question.QuestionID, Question = question.Question });

            if (datalist.Count() == 0)
            {

                TempData["Error"] = "This user has not completed the test yet!";
                return RedirectToAction("StudentList", "PsychTest");
            }


            return new ActionAsPdf(
                           "Responses",
                           new { UserID = UserID })
            {
                FileName = string.Format("Psych_Test_{0}.pdf", student),
                CustomSwitches = "--load-media-error-handling ignore --load-error-handling ignore"

            };

        }

        [Authorize(Roles = "Counselor")]
        // GET: PsychologicalTest/StudentList
        public ActionResult StudentList(string searchStringName, string currentFilter, int? page)
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
                    .Where(x => x.StudentLastName.Contains(searchStringName) || x.StudentFirstName.Contains(searchStringName))
                    .OrderBy(x => x.StudentLastName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                int noAnswer = 0;
                List<TestViewModel> StudentInventorylist3 = new List<TestViewModel>();
                
                foreach (var item in datalist)
                {
                    TestViewModel pvm = new TestViewModel();
                    pvm.StudentFirstName = item.StudentFirstName;
                    pvm.StudentMiddleName = item.StudentMiddleName;
                    pvm.StudentLastName = item.StudentLastName;
                    pvm.UserID = item.UserID;
                    pvm.StudentID = item.StudentID;
                    StudentInventorylist.Add(pvm);

                    //see how many students have no psych test
                    var dlist =
                        (from ans in db.Answers
                         where ans.UserID == pvm.UserID
                         select new {Answer = ans.Answer});

                    int countOfTotalStudents = datalist.Count();

                    if (dlist.Count() == 0)
                    {
                        noAnswer++;

                            ViewBag.ListOfStudents = "List of students who haven't taken the Psychological Test";

                            //write yung names ng student where nag dlist.count == 0
                            TestViewModel tvm = new TestViewModel();
                            tvm.StudentFirstName = pvm.StudentFirstName;
                            tvm.StudentMiddleName = pvm.StudentMiddleName;
                            tvm.StudentLastName = pvm.StudentLastName;
                            tvm.UserID = pvm.UserID;
                            tvm.StudentID = pvm.StudentID;
                            StudentInventorylist3.Add(tvm);

                            ViewBag.studentlist = StudentInventorylist3;

                            int countOfStudentsWithNoPsychTest = noAnswer;
                            ViewBag.CountOfTotalStudents = countOfTotalStudents;
                            ViewBag.CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;

                    }
                    else
                    {
                        int countOfStudentsWithNoPsychTest = noAnswer;

                        ViewBag.CountOfTotalStudents = countOfTotalStudents;
                        ViewBag.CountOfStudentsWithNoPsychTest = countOfTotalStudents - countOfStudentsWithNoPsychTest;
                    }

                }

                //display questions sa table
                var datalistQuestions = db.Questions.ToList();
                List<PsychTestViewModel> questionlist = new List<PsychTestViewModel>();

                foreach (var question in datalistQuestions)
                {
                    PsychTestViewModel ptvm = new PsychTestViewModel();
                    ptvm.QuestionID = question.QuestionID;
                    ptvm.Question = question.Question;                    

                    questionlist.Add(ptvm);
                }

                ViewBag.questionlist = questionlist;

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