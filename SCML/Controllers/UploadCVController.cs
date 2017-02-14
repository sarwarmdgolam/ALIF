using SCML.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SCML.Controllers
{
    public class UploadCVController : Controller
    {
        private SCMLModel db = new SCMLModel();

        // GET: UploadCV
         [Authorize]
        public ActionResult Index()
        {
            return View(db.EmailFormModels.ToList().OrderByDescending(x =>x.create_date));
        }

         // GET: Types/Create
         [Authorize]
         public ActionResult Create()
         {
             return View();
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

         // POST: Types/Create
         // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         [Authorize]
         public ActionResult Create(EmailFormModel model)
         {
             if (ModelState.IsValid)
             {
                // db.EmailFormModels.Add(model);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             return View(model);
         }


         [HttpPost]
         [ValidateAntiForgeryToken]
         [Authorize]
         public ActionResult Edit(EmailFormModel model)
         {
             if (ModelState.IsValid)
             {
                 db.Entry(model).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(model);
         }

      



         // POST: Contents/Delete/5
        
         [Authorize]
         public ActionResult Delete(int id)
         {
             EmailFormModel content = db.EmailFormModels.Find(id);
             db.EmailFormModels.Remove(content);
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