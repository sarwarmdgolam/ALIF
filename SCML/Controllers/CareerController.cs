using SCML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCML.App_Code;

namespace SCML.Controllers
{
    public static class ControllerExtensions
    {
        public static ToastMessage AddToastMessage(this Controller controller, string title, string message, ToastType toastType = ToastType.Info)
        {
            Toastr toastr = controller.TempData["Toastr"] as Toastr;
            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage1(title, message, toastType);
            controller.TempData["Toastr"] = toastr;
            return toastMessage;
        }
    }
    public class CareerController : Controller
    {
        // GET: Career
        public ActionResult Index()
        {
            if (TempData["IsMailSent"] != null && String.Equals(TempData["IsMailSent"].ToString(), "true"))
            {
                this.AddToastMessage("Congratulations", TempData["ResultMsg"].ToString(), ToastType.Success);
                return View();
            }
            else if (TempData["IsMailSent"] != null && String.Equals(TempData["IsMailSent"].ToString(), "false"))
            {
                this.AddToastMessage("Sorry", TempData["ResultMsg"].ToString(), ToastType.Error);
                return View();
            }
            else
                return View();
          
        }

        


        public ActionResult CurrentOpenings()
        {
            SCMLModel db = new SCMLModel();
            return View(db.JobDetails.ToList().Where(x => x.deadline >= DateTime.Now.Date).OrderByDescending(x => x.deadline));
        }

    }
}