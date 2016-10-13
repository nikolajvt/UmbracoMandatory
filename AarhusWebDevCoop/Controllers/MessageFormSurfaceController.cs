using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using Umbraco.Web.Mvc;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class MessageFormSurfaceController : SurfaceController
    {
        // GET: MessageFormSurface
        public ActionResult Index()
        {
            return View("MessageForm", new MessageForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(MessageForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Save sent mail as post

            IContent message = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

            message.SetValue("fullName", model.FullName);
            message.SetValue("email", model.Email);
            message.SetValue("subject", model.Subject);
            message.SetValue("body", model.Body);

            //Save
            Services.ContentService.SaveAndPublishWithStatus(message);
            // Services.ContentService.Save(comment);




            return RedirectToCurrentUmbracoPage();
        }
    }
}