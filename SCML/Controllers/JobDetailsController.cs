using SCML.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using SCML.App_Code;

namespace SCML.Controllers
{
    public class JobDetailsController : Controller
    {
        private SCMLModel db = new SCMLModel();

        // GET: JobDetails
        [Authorize]
        public ActionResult Index()
        {
            return View(db.JobDetails.ToList().OrderByDescending(x => x.create_date));
        }

       



        // GET: Contents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailFormModel content = db.EmailFormModels.Where(x => x.id == id).SingleOrDefault();
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }



        [Authorize]
        public ActionResult Create()
        {
            JobDetail model = new JobDetail();
            model.deadline = DateTime.Today.Date;
            return View();
        }
        
        // POST: Types/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(JobDetail model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.create_date = DateTime.Now;
                    model.create_by = ClaimsPrincipal.Current.Identity.Name;
                    db.JobDetails.Add(model);
                    db.SaveChanges();
                    this.AddToastMessage("Success!", "Successfully Edited.", ToastType.Success);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("Error!", ex.InnerException.Message, ToastType.Error);
            }
            return View(model);
        }


        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDetail model = db.JobDetails.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(JobDetail model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.create_date = DateTime.Now;
                    model.create_by = ClaimsPrincipal.Current.Identity.Name;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    this.AddToastMessage("Success!", "Successfully Edited.", ToastType.Success);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("Error!", ex.InnerException.Message, ToastType.Error);
            }
            return View(model);
        }





        // POST: Contents/Delete/5

        [Authorize]
        public ActionResult Delete(int id)
        {
            JobDetail content = db.JobDetails.Find(id);
            db.JobDetails.Remove(content);
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