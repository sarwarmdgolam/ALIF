﻿using SCML.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCML.Models
{
    [Serializable]
    public class Toastr
    {
        public bool ShowNewestOnTop { get; set; }
        public bool ShowCloseButton { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }

        public ToastMessage AddToastMessage1(string title, string message, ToastType toastType)
        {
            var toast = new ToastMessage()
            {
                Title = title,
                Message = message,
                ToastType = toastType
            };
            ToastMessages.Add(toast);
            return toast;
        }

        public Toastr()
        {
            ToastMessages = new List<ToastMessage>();
            ShowNewestOnTop = false;
            ShowCloseButton = false;
        }
    }
}