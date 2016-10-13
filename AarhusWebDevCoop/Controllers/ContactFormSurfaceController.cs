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
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return View("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) {
                return CurrentUmbracoPage();
            }
            // Define the layout of the sent message below:
            MailMessage message = new MailMessage();
            message.To.Add("nikolaj.vt@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message + "\n my email is: " + model.Email;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                //smtp.Port = 465; //SSL Port
                smtp.Credentials = new System.Net.NetworkCredential("nikkofar14@gmail.com", "DetteErEnAdgangskode");
                smtp.EnableSsl = true;

                smtp.Send(message);

            }
            TempData["success"] = true;

            // Save sent mail as post

            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Comment");

            comment.SetValue("fullName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            //Save
            Services.ContentService.SaveAndPublishWithStatus(comment);
            // Services.ContentService.Save(comment);




            return RedirectToCurrentUmbracoPage();
        }
    }
}