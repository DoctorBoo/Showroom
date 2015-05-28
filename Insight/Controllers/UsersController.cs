using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Contexts;

namespace Insight.Controllers
{
    public class UsersController : Controller
    {
        private UserCtx db = new UserCtx();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserGroup,LastName,FirstName,MidleName,Initial,Password,AccessLevel,Role,Lang,Func,Notes,NotifTimerInterval,DueDateDay,DueDateHrs,DueDateMin,ReminderDay,ReminderHrs,ReminderMin,SnoozeDay,SnoozeHrs,SnoozeMin,LoginTime,LogoutTime,LogCount,LogFlag,LogErrCount,LogRelCount,PictureFile,LocalDir,wavFile,wallpaper,VChar1,VChar2,VChar3,VChar4,VChar5,Int1,Int2,Int3,Int4,Int5,Float1,Float2,Text1,Text2,Text3,Text4,Text5,isDriver,isWorker,isDeliveryMan")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserGroup,LastName,FirstName,MidleName,Initial,Password,AccessLevel,Role,Lang,Func,Notes,NotifTimerInterval,DueDateDay,DueDateHrs,DueDateMin,ReminderDay,ReminderHrs,ReminderMin,SnoozeDay,SnoozeHrs,SnoozeMin,LoginTime,LogoutTime,LogCount,LogFlag,LogErrCount,LogRelCount,PictureFile,LocalDir,wavFile,wallpaper,VChar1,VChar2,VChar3,VChar4,VChar5,Int1,Int2,Int3,Int4,Int5,Float1,Float2,Text1,Text2,Text3,Text4,Text5,isDriver,isWorker,isDeliveryMan")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
