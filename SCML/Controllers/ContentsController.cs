using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCML.Models;
using System.Security.Claims;
using System.IO;
using SCML.App_Code;
using System.Drawing;
using System.Drawing.Imaging;


namespace SCML.Controllers
{
    public class ContentsController : Controller
    {
        private SCMLModel db = new SCMLModel();

        //[Authorize]
        //public ActionResult Index()
        //{


        //    ViewBag.types = new SelectList(db.Types.OrderBy(i => i.name), "id", "name");
        //    var contents = db.Contents.Include(c => c.Type);

        //    return View(db.Contents.ToList());
        //    //return View(contents.ToList().OrderBy(i => i.Type.name));
        //}




        [Authorize]
        public ActionResult Index(String typeId)
        {
            ViewBag.types = new SelectList(db.Types.OrderBy(i => i.name), "id", "name");
            var contents = db.Contents.Include(c => c.Type);

            if (!String.IsNullOrEmpty(typeId))
            {
                contents = contents.Where(x => x.type_id.ToString() == typeId);
            }
            return View(contents.ToList().OrderBy(i => i.Type.name));
        }


        // GET: Contents
        [Authorize]
        public ActionResult ContentsByTypeID(String typeId)
        {
            ViewBag.types = new SelectList(db.Types.OrderBy(i => i.name), "id", "name");
            var contents = db.Contents.Include(c => c.Type);

            if (!String.IsNullOrEmpty(typeId))
            {
                contents = contents.Where(x => x.type_id.ToString() == typeId);
            }
            return View(contents.ToList().OrderBy(i => i.Type.name));
        }


        // GET: Contents
        //public ActionResult MoreList(int id)
        //{
        //    var contents = db.Contents.Include(c => c.Type);

        //    if (id != null)
        //    {
        //        contents = contents.Where(x => x.Type.id == id).OrderByDescending(o => o.publish_date);
        //    }
        //    if (contents.Any())
        //    {
        //        ViewBag.Title = contents.FirstOrDefault().Type.name;
        //    }
        //    else
        //    {
        //        ViewBag.Title = "List";
        //    }
        //    return View(contents.ToList());
        //}


        // GET: Contents/Details/5
        public ActionResult Details(String typename, String contenttitle, String id)
        {
           
            if (typename == null && id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             int _id=Convert.ToInt32(id);
            Content content = new Models.Content();

            if (!String.IsNullOrEmpty(id))
                content = db.Contents.Where(x => x.id == _id).FirstOrDefault();
            else
                content = db.Contents.Where(x => x.Type.name == typename && x.title == contenttitle).FirstOrDefault();

            if (content == null)
            {
                content = new Content()
                {
                    title = "Page Not Found",
                    contents = ""
                };
            }

            return View(content);
        }



        public ActionResult MoreList(String typename, String contenttitle)
        {
            if (typename == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contents = db.Contents.Where(x => x.Type.name == typename && (x.title == contenttitle || contenttitle == null)).OrderByDescending(o => o.publish_date).Take(5);
            if (contents.Any())
            {
                ViewBag.Title = contents.FirstOrDefault().Type.name;
            }
            else
            {
                ViewBag.Title = "List";
            }
            return View(contents.ToList());
        }


        // GET: Contents/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.type_id = new SelectList(db.Types, "id", "name");
            Content _Content = new Content();
            _Content.publish_date = DateTime.Today.Date;
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Content content, String upload)
        {
            try
            {
                ViewBag.type_id = new SelectList(db.Types, "id", "name", content.type_id);
                switch (upload)
                {
                    case "imageUpload":
                        UploadImage(content);
                        return View(content);
                    case "contentUpload":
                        UploadContent(content);
                        return View(content);
                    case "Create":
                        if (ModelState.IsValid)
                        {
                            Insert(content);
                             this.AddToastMessage("Success!", "Successfully Created.", ToastType.Success);
                            return RedirectToAction("Index");
                        }
                        else
                            return View(content);
                    default:
                        return View(content);
                }
            }
            catch (Exception ex)
            {
                 this.AddToastMessage("Error!", ex.InnerException.Message, ToastType.Error);
                return View(content);
            }
        }
        private ActionResult Insert(Content content)
        {
            content.publish_by = ClaimsPrincipal.Current.Identity.Name;
            db.Contents.Add(content);

            db.SaveChanges();
            return View(content);
        }

        private ActionResult UploadImage(Content content)
        {
            if (((HttpPostedFileBase)Request.Files["file1"]).ContentLength > 0)
            {
                content.large_image_path = null;
                content.thambnail_image_path = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file1 = Request.Files[i];
                    if (file1 != null && file1.ContentLength > 0)
                    {
                        SaveAsFile(content, file1, "Image");
                    }
                }
            }
            return View(content);
        }

        private ActionResult UploadContent(Content content)
        {
            if (((HttpPostedFileBase)Request.Files["file2"]).ContentLength > 0)
            {
                content.content_file_path = null;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file1 = Request.Files[i];
                    if (file1 != null && file1.ContentLength > 0)
                    {
                        SaveAsFile(content, file1, "Content");
                    }
                }
            }
            return View(content);
        }

        private void SaveAsFile(Content content, HttpPostedFileBase file, String FileType)
        {
            var _tmpFileMapPath = Server.MapPath("~/Documents/");
            var _tmpFilePath = ("/Documents/");
            var fileName = FileType + "_" + Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);


            //thambnail_image_path
            if (String.Equals(FileType, "Image"))
            {
                //large_image_path
                var large_image_path = Path.Combine(_tmpFileMapPath, fileName);
                var large_image_path_for_DB = Path.Combine(_tmpFilePath, fileName);
                if (System.IO.File.Exists(large_image_path)) System.IO.File.Delete(large_image_path);
                file.SaveAs(large_image_path);
                if (String.IsNullOrEmpty(content.large_image_path))
                    content.large_image_path = large_image_path_for_DB;
                else
                    content.large_image_path = content.large_image_path + ";" + large_image_path_for_DB;


                //thambnail image path
                fileName = "thambnail" + fileName;
                var thambnail_image_path = Path.Combine(_tmpFileMapPath, fileName);
                var thambnail_image_path_for_DB = Path.Combine(_tmpFilePath, fileName);
                if (System.IO.File.Exists(thambnail_image_path)) System.IO.File.Delete(thambnail_image_path);

                //save image into directory
                byte[] bitmap = PhotoManager.ResizeImage(file, 124, 110);
                using (Image image = Image.FromStream(new MemoryStream(bitmap)))
                {
                    image.Save(thambnail_image_path, ImageFormat.Png);  // Or Png
                }

                if (String.IsNullOrEmpty(content.thambnail_image_path))
                    content.thambnail_image_path = thambnail_image_path_for_DB;
                else
                    content.thambnail_image_path = content.thambnail_image_path + ";" + thambnail_image_path_for_DB;

            }
            else if (String.Equals(FileType, "Content"))
            {
                //content_file_path
                var content_file_path = Path.Combine(_tmpFileMapPath, fileName);
                var content_file_path_for_DB = Path.Combine(_tmpFilePath, fileName);
                if (System.IO.File.Exists(content_file_path)) System.IO.File.Delete(content_file_path);
                file.SaveAs(content_file_path);

                if (String.IsNullOrEmpty(content.content_file_path))
                    content.content_file_path = content_file_path_for_DB;
                else
                    content.content_file_path = content.content_file_path + ";" + content_file_path_for_DB;
            }
        }

        private String SaveAsFile(HttpPostedFileBase file, String FileType)
        {
            var _tmpFileMapPath = Server.MapPath("~/Documents/");
            var _tmpFilePath = ("/Documents/");
            var fileName = FileType + Path.GetFileName(file.FileName);

            //large_image_path
            var large_image_path = Path.Combine(_tmpFileMapPath, fileName);
            if (System.IO.File.Exists(large_image_path)) System.IO.File.Delete(large_image_path);
            file.SaveAs(large_image_path);

            //thambnail_image_path
            if (String.Equals(FileType, "Image"))
            {
                var thambnail_image_path = Path.Combine(_tmpFileMapPath, "thambnail" + fileName);
                if (System.IO.File.Exists(thambnail_image_path)) System.IO.File.Delete(thambnail_image_path);

                byte[] bitmap = PhotoManager.ResizeImage(file, 124, 110);
                using (Image image = Image.FromStream(new MemoryStream(bitmap)))
                {
                    image.Save(thambnail_image_path, ImageFormat.Png);  // Or Png
                }
            }

            return Path.Combine(_tmpFilePath, fileName);
        }

        // GET: Contents/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_id = new SelectList(db.Types, "id", "name", content.type_id);
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Content content, String upload)
        {
            try
            {
                ViewBag.type_id = new SelectList(db.Types, "id", "name", content.type_id);
                switch (upload)
                {
                    case "imageUpload":
                        UploadImage(content);
                        ModelState.Clear();
                        return View(content);
                    case "contentUpload":
                        UploadContent(content);
                        ModelState.Clear();
                        return View(content);
                    case "Edit":
                        if (ModelState.IsValid)
                        {
                            Edit(content);
                             this.AddToastMessage("Success!", "Successfully Edited.", ToastType.Success);
                            return RedirectToAction("Index");
                        }
                        else
                            return View(content);
                    default:
                        return View(content);
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("Error!", ex.InnerException.Message, ToastType.Error);
                return View(content);
            }
        }


        private ActionResult Edit(Content content)
        {
            content.publish_by = ClaimsPrincipal.Current.Identity.Name;
            db.Entry(content).State = EntityState.Modified;
            db.SaveChanges();
            return View(content);
        }

        // GET: Contents/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost, ActionName("DeleteFile")]
        public JsonResult DeleteFile(int? id, string filetype)
        {
            //string filetype=Request.QueryString["filetype"];
            var path = string.Empty;
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Content content = db.Contents.Find(id);
                if (content == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                if (filetype == "image")
                {



                    //Delete file from the file system
                    path = Server.MapPath(content.large_image_path);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    path = Server.MapPath(content.thambnail_image_path);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    content.large_image_path = null;
                    content.thambnail_image_path = null;
                    db.Entry(content).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (filetype == "content")
                {


                    //Delete file from the file system
                    path = Server.MapPath(content.content_file_path);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    content.content_file_path = null;
                    db.Entry(content).State = EntityState.Modified;
                    db.SaveChanges();

                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public FileResult Download(String path)
        {
            return File(Server.MapPath(path), System.Net.Mime.MediaTypeNames.Application.Octet, "");
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

