﻿using AngularBlog.Core.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AngularBlog.Core.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendContactEmail(Contact contact)
        {
            System.Threading.Thread.Sleep(5000);
            try {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("mail.alanadi.com");
                mailMessage.From = new MailAddress("info@alanadi.com");
                mailMessage.To.Add("alpaslan@gmail.com");

                mailMessage.Subject = contact.Subject;
                mailMessage.Body = contact.Message;
                mailMessage.IsBodyHtml = true;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("info@alanadi.com", "*****!");

                smtpClient.Send(mailMessage);
                return Ok();
            } 
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }

        }
    }
}
