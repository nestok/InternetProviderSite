using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperInternet.Models;
using System.Data.Entity;
using Microsoft;
namespace SuperInternet.Controllers
{
    public class ServicesController : Controller
    {
        ServicesContext db = new ServicesContext();
        public ActionResult Services()
        {
            IEnumerable<Service> services = db.AllServices;
            ViewBag.Services = services;
            User user = (User)Session["User"];
            ViewBag.SelectedUser = user;
            return View(db.AllServices);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult ServiceInfo(int? id)
        {
            if (id == null)
                return HttpNotFound();
            User user = (User)Session["User"];
            ViewBag.SelectedUser = user;
            Service service = db.AllServices.Find(id);
            ViewBag.Service = service;
            IEnumerable<Comment> comments = db.Comments.Where(c => (c.ServiceId == service.Id)).Include(c => c.Sender);
            ViewBag.Comments = comments;

            ViewBag.User = Session["User"];
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(int? serviceId)
        {
            if (serviceId == null)
                return HttpNotFound();
            Comment comment = new Comment();
            comment.Text = Request.Form["commentText"];
            comment.ServiceId = (int)serviceId;
            comment.SenderId = ((User)Session["User"]).Id;

            db.Comments.Add(comment);
            db.SaveChanges();
            return Redirect("ServiceInfo/" + serviceId);
        }

        [HttpPost]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            int serviceId = comment.ServiceId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Redirect("ServiceInfo/" + serviceId);
        }

        [HttpGet]
        public ActionResult AddService()
        {
            User user = (User)Session["User"];
            if ((user == null) || (user.Role != UserRole.ADMIN))
                return HttpNotFound();
            return View();
        }

        [HttpPost]
        public ActionResult AddService(Service service)
        {
            if ((service.Tarif != null) && (service.ConnectionType != null) && (service.Payment != null) && (service.Speed != null) &&
                (service.Term != null) && (service.Traffic != null) && (service.SubscrCash != null) && (service.Agreement != null))
            {
                db.AllServices.Add(service);
                db.SaveChanges();
                return RedirectToAction("Services");
            }
            else
                return View();
           
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            User user = (User)Session["User"];
            if ((user == null) || (user.Role != UserRole.ADMIN))
                return HttpNotFound();
            Service serv = db.AllServices.Find(id);
            if (serv == null)
            {
                return HttpNotFound();
            }
            return View(serv);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Service serv = db.AllServices.Find(id);
            if (serv == null)
            {
                return HttpNotFound();
            }
            db.AllServices.Remove(serv);
            db.SaveChanges();
            return RedirectToAction("Services");
        }

        [HttpGet]
        public ActionResult EditService(int? id)
        {
            User user = (User)Session["User"];
            if ((user == null) || (user.Role != UserRole.ADMIN))
                return HttpNotFound();
            if (id == null)
            {
                return HttpNotFound();
            }
            Service serv = db.AllServices.Find(id);
            if (serv != null)
            {
                ViewBag.Message = serv;
                return View();
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditService(Service service)
        {
            User user = (User)Session["User"];
            if ((user == null) || (user.Role != UserRole.ADMIN))
                return HttpNotFound();
            if ((service.Tarif != null) && (service.ConnectionType != null) && (service.Payment != null) && (service.Speed != null) &&
            (service.Term != null) && (service.Traffic != null) && (service.SubscrCash != null) && (service.Agreement != null))
            {
                if (service.Id != 0)
                {
                    db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                    db.AllServices.Add(service);
                return RedirectToAction("Services");
            }
            else return View();
        }
    }
}
