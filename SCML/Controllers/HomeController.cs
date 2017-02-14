using SCML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCML.App_Code;
using System.Net.Mail;
using System.Configuration;

namespace SCML.Controllers
{
    public class HomeController : Controller
    {

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(String name, String email, String subject, String message)
        {
            String s = name;
            if (!String.IsNullOrEmpty(name))
            {
                String returnmsg = SendMail(name, email, subject, message);

                ViewBag.MailReturnMessage = returnmsg;
                String url = "Index";// +Uri.EscapeUriString("#Cotact");
                return RedirectToAction(url);
            }
            else
                return View();
        }

        private String SendMail(String name, String email, String subject, String body)
        {
            //Send mail
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(email);
                message.To.Add(new MailAddress(ConfigurationManager.AppSettings["SCMLEmailAddress"]));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //public ActionResult GetInTouch(EmailFormModel _EmailFormModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //Send Mail to SCML
        //            SendMail(_EmailFormModel);

        //            //Reply confirmation mail
        //            ReplyConfirmationMail(_EmailFormModel);


        //            TempData["IsMailSent"] = "true";
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["IsMailSent"] = "false";
        //            TempData["ResultMsg"] = ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        TempData["IsMailSent"] = "false";
        //        TempData["ResultMsg"] = "Please input correctly.";
        //    }
        //    return PartialView();
        //}


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Why()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Boards()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Profile()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}

