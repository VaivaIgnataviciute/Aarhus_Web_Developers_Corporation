using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface,refering our partial view and creating new instance for contact for model
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        // Handling user input and where message should be sent to 
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

            TempData["success"] = true;


            MailMessage message = new MailMessage();
            message.To.Add("vaiva.ignataviciute17@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            //Handling how to recieve message 
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("vaiva.ignataviciute17@gmail.com", "cbrsggwrcebfholf");

                //send mail
                smtp.Send(message);

                IContent messages = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

                messages.SetValue("messagename", model.Name);
                messages.SetValue("email", model.Email);
                messages.SetValue("subject", model.Subject);
                messages.SetValue("messagecontent", model.Message);

                //save
                Services.ContentService.Save(messages);
                
            }

                return RedirectToCurrentUmbracoPage();


        }
    }

}