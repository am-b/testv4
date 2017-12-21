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
    [Authorize(Roles = "Administrator")]
    public class AnnouncementsController : HomeController
    {
        private Testv3Entities db = new Testv3Entities();

        // GET: Announcements
        public ActionResult Index()
        {
            GetCurrentUserInViewBag();
            return View(db.Announcements.ToList());
        }

        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
        {
            GetCurrentUserInViewBag();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: Announcements/Create
        public ActionResult Create()
        {
            GetCurrentUserInViewBag();
            return View();
        }

        // POST: Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnouncementID,AnnouncementDate,AnnouncementTitle,AnnouncementBody")] Announcement announcement)
        {
            GetCurrentUserInViewBag();
            if (ModelState.IsValid)
            {
                //var datePosted = db.Announcements.Create();
                //datePosted.AnnouncementDate = DateTime.Today;
                ////var datePosted = DateTime.Today;
                announcement.AnnouncementDate = DateTime.Now;

                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        // GET: Announcements/Edit/5
        public ActionResult Edit(int? id)
        {
            GetCurrentUserInViewBag();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnouncementID,AnnouncementDate,AnnouncementTitle,AnnouncementBody")] Announcement announcement)
        {
            GetCurrentUserInViewBag();
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        public ActionResult Delete(int? id)
        {
            GetCurrentUserInViewBag();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GetCurrentUserInViewBag();
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
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
