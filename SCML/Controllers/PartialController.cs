using SCML.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SCML.Controllers
{
    //[OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
   // [OutputCache(Duration = 3600, VaryByParam = "none")]
    public class PartialController : Controller
    {
        private SCMLModel db = new SCMLModel();

        //// GET: Partial
        public ActionResult Events()
        {
            EventsViewModel _model = new EventsViewModel();
            _model.Events = db.Contents.ToList().Where(x => x.Type.name == "Event").OrderByDescending(x => x.publish_date).Take(4);
            return PartialView(_model);
        }

        //// GET: Partial
        public ActionResult Price()
        {
            PriceViewModel _model = new PriceViewModel();
            _model.Prices= db.Contents.ToList().Where(x => x.Type.name == "Price").OrderBy(x => x.sort_order);
            return PartialView(_model);
        }

        // GET: Partial
        public ActionResult RecentPost()
        {
            RPostsViewModel _model = new RPostsViewModel();
            _model.RecentPosts = db.Contents.ToList().Where(x => x.Type.name == "Post").OrderByDescending(x => x.publish_date).Take(4);
            return PartialView(_model);
        }

       // [HttpPost]
        public ActionResult GetInTouch(String name, String email)
        {
            String s = name;
            if (!String.IsNullOrEmpty(name))
            {
                ViewBag.ErrorMsg = " Error";
                return PartialView();

            }
            else
                return PartialView();
        }
           
        ////[HttpPost]
        //public ActionResult GetInTouch(EmailFormModel _EmailFormModel)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    try
        //    //    {
        //    //        //Send Mail to SCML
        //    //        SendMail(_EmailFormModel);

        //    //        //Reply confirmation mail
        //    //        ReplyConfirmationMail(_EmailFormModel);


        //    //        TempData["IsMailSent"] = "true";
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        TempData["IsMailSent"] = "false";
        //    //        TempData["ResultMsg"] = ex.Message;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    TempData["IsMailSent"] = "false";
        //    //    TempData["ResultMsg"] = "Please input correctly.";
        //    //}
        //    return PartialView();
        //}

        //// GET: Partial
        //public ActionResult Messages()
        //{
        //    MessagesViewModel _model = new MessagesViewModel();
        //    _model.Messages = db.Contents.Where(x => x.Type.name == "Message").OrderByDescending(x => x.publish_date).SingleOrDefault();
        //    return PartialView(_model);
        //}

        //// GET: Partial
        //public ActionResult Services()
        //{
        //    ServicesViewModel _model = new ServicesViewModel();
        //    _model.Services = db.Contents.Where(x => x.Type.name == "Service").OrderByDescending(x => x.publish_date).SingleOrDefault();
        //    return PartialView(_model);
        //}

        //// GET: Partial
        //public ActionResult LNews()
        //{
        //    LNewsViewModel _model = new LNewsViewModel();

        //    _model.LNews = db.Contents.ToList().Where(x => x.Type.name == "LNews").OrderByDescending(x => x.publish_date).Take(2);
        //    return PartialView(_model);
        //}


        //// GET: Partial
        //public ActionResult MComments()
        //{
        //    MCommentsViewModel _model = new MCommentsViewModel();
        //    _model.MComments = db.Contents.ToList().Where(x => x.Type.name == "MComments").OrderByDescending(x => x.publish_date).Take(2);
        //    return PartialView(_model);
        //}


        //// GET: Partial
        //public ActionResult IPONotes()
        //{
        //    IPONotesViewModel _model = new IPONotesViewModel();
        //    _model.IPONotes = db.Contents.ToList().Where(x => x.Type.name == "IPONotes").OrderByDescending(x => x.publish_date).Take(2);
        //    return PartialView(_model);
        //}

        //// GET: Partial
        //public ActionResult Reports()
        //{
        //    ReportsViewModel _model = new ReportsViewModel();
        //    _model.Reports = db.Contents.ToList().Where(x => x.Type.name == "Reports").OrderByDescending(x => x.publish_date).Take(2);
        //    return PartialView(_model);
        //}

        // GET: Partial
        public ActionResult HomeTab()
        {
           // ReportsViewModel _model = new ReportsViewModel();
            //_model.Reports = db.Contents.ToList().Where(x => x.Type.name == "Reports").OrderByDescending(x => x.publish_date).Take(2);
            return PartialView();
        }

              
        public ActionResult ViewSendResume()
        {
            return PartialView("SendResume");
        }

        [HttpPost]
        public ActionResult SendResume(EmailFormModel _EmailFormModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Save as to dicrectory
                    SaveUploadedFile(_EmailFormModel);

                    //Save to database
                    SaveToDatabase(_EmailFormModel);

                    //Send Mail to SCML
                    SendMail(_EmailFormModel);

                    //Reply confirmation mail
                    ReplyConfirmationMail(_EmailFormModel);

                   
                    TempData["IsMailSent"] = "true";
                    TempData["ResultMsg"] = "Successfully Mail Sent!";
                }
                catch (Exception ex)
                {
                    TempData["IsMailSent"] = "false";
                    TempData["ResultMsg"] = ex.Message;
                }
            }
            else
            {
                TempData["IsMailSent"] = "false";
                TempData["ResultMsg"] = "Please input correctly.";
            }
            return RedirectToAction("index", "Career");
        }

        private void SendMail(EmailFormModel _EmailFormModel)
        {
            //Send mail
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_EmailFormModel.email);
                message.To.Add(new MailAddress(ConfigurationManager.AppSettings["SCMLEmailAddress"]));
                message.Subject = ConfigurationManager.AppSettings["EmailSubject"];
                message.Body = ConfigurationManager.AppSettings["EmailBody"];
                message.IsBodyHtml = true;
                if (_EmailFormModel.Upload != null && _EmailFormModel.Upload.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(_EmailFormModel.Upload.InputStream, Path.GetFileName(_EmailFormModel.Upload.FileName)));
                }
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
            }
            catch(Exception ex)
            {
                throw ex;
                }
        }

        private void ReplyConfirmationMail(EmailFormModel _EmailFormModel)
        {
            //Send mail
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["SCMLEmailAddress"]);
                message.To.Add(new MailAddress(_EmailFormModel.email));
                message.Subject = ConfigurationManager.AppSettings["ReplyEmailSubject"];
                message.Body = ConfigurationManager.AppSettings["ReplyEmailBody"];
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        private void SaveToDatabase(EmailFormModel _EmailFormModel)
        {

            _EmailFormModel.create_date = DateTime.Now;

            db.EmailFormModels.Add(_EmailFormModel);
            db.SaveChanges();
        }

        private void SaveUploadedFile(EmailFormModel _EmailFormModel)
        {
            HttpPostedFileBase file2 = Request.Files["upload"];
            if (file2 != null && file2.ContentLength > 0)
            {
                _EmailFormModel.file_path= SaveAsFile(file2, "RESUME_");
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

            return Path.Combine(_tmpFilePath, fileName);
        }
    }
}


