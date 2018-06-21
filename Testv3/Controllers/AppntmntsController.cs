using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Testv3.Models;

namespace Testv3.Controllers
{
    public class AppntmntsController : DefaultController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: Appntmnts
        public ActionResult Index()
        {
            var appntmnt = db.Appntmnt.Include(a => a.Student);
            return View(appntmnt.ToList());
        }

        // GET: Appntmnts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appntmnt appntmnt = db.Appntmnt.Find(id);
            if (appntmnt == null)
            {
                return HttpNotFound();
            }
            return View(appntmnt);
        }

        // GET: Appntmnts/Create
        public ActionResult Create()
        {
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            GetCurrentUserInViewBag();
            var currentUserId = User.Identity.GetUserId();
            return View();
        }

        // POST: Appntmnts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Appntmnt_ID,StudentUserID,Appntmnt_Date,Appntmnt_Time")] Appntmnt appntmnt)
        {
            GetCurrentUserInViewBag();
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            var currentUserId = User.Identity.GetUserId();
            var app = db.Appntmnt.FirstOrDefault(d => d.StudentUserID == currentUserId);
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (ModelState.IsValid)
            {
                db.Appntmnt.Add(appntmnt);
                db.SaveChanges();
                TempData["Message"] = "User: " + u.StudentID + ", has set a date for Counselling";
                return RedirectToAction("Index","Home");
            }

            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID", appntmnt.StudentUserID);
            return View(appntmnt);
        }

        // GET: Appntmnts/Edit/5
        public ActionResult Edit(int? id)
        {
            GetCurrentUserInViewBag();
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            var currentUserId = User.Identity.GetUserId();
            var app = db.Appntmnt.FirstOrDefault(d => d.StudentUserID == currentUserId);
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appntmnt appntmnt = db.Appntmnt.Find(id);
            if (appntmnt == null)
            {
                return HttpNotFound();
            }

            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID", appntmnt.StudentUserID);
            return View(appntmnt);
        }

        // POST: Appntmnts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Appntmnt_ID,StudentUserID,Appntmnt_Date,Appntmnt_Time")] Appntmnt appntmnt)
        {
            GetCurrentUserInViewBag();
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            var currentUserId = User.Identity.GetUserId();
            var app = db.Appntmnt.FirstOrDefault(d => d.StudentUserID == currentUserId);
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (ModelState.IsValid)
            {
                db.Entry(appntmnt).State = EntityState.Modified;
                db.Appntmnt.Add(appntmnt);
                try
                {
                    db.SaveChanges();
                }

                catch (Exception)
                {
                    ViewBag.Status = "Problem while rescheduling, Please check details.";
                }
                return RedirectToAction("Index");
            }

            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID", appntmnt.StudentUserID);
            return View(appntmnt);
        }

        // GET: Appntmnts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appntmnt appntmnt = db.Appntmnt.Find(id);
            if (appntmnt == null)
            {
                return HttpNotFound();
            }
            return View(appntmnt);
        }

        // POST: Appntmnts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appntmnt appntmnt = db.Appntmnt.Find(id);
            db.Appntmnt.Remove(appntmnt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmailRequest()
        {
            Student student = new Student();

            var items = student.StudentEmail.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EmailRequest()
        //{
        //    return View();
        //}

        public ActionResult ProcessRequest()
        {
            return View();
        }

        public ActionResult Submit(int? id)
        {
            GetCurrentUserInViewBag();
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            var currentUserId = User.Identity.GetUserId();
            var app = db.Appntmnt.FirstOrDefault(d => d.StudentUserID == currentUserId);
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appntmnt appntmnt = db.Appntmnt.Find(id);
            if (appntmnt == null)
            {
                return HttpNotFound();
            }
            return View(appntmnt);
        }

        [HttpPost, ActionName("Submit")]
        [ValidateAntiForgeryToken]
        public ActionResult Submit()
        {
            GetCurrentUserInViewBag();
            ViewBag.StudentUserID = new SelectList(db.Students, "UserID", "StudentID");
            var currentUserId = User.Identity.GetUserId();
            var app = db.Appntmnt.FirstOrDefault(d => d.StudentUserID == currentUserId);
            var u = db.Students.FirstOrDefault(d => d.UserID == currentUserId);

            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 25;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "teambbl_mmccis@gmail.com";
                WebMail.Password = "M@pua2013";

                //Sender email address.  
                WebMail.From = "teambbl_mmccis@gmail.com";

                //Send email  
                WebMail.Send(to: u.StudentEmail , subject: "Message from MMCIS Guidance", body: "Date: " + app.Appntmnt_Date + "Time: " + app.Appntmnt_Time, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";
            }

            return RedirectToAction("Index");
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
