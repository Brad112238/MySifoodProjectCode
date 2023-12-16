﻿using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Models;
using sifoodproject.Areas.Admin.Models;

namespace sifoodproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreManageController : Controller
    {
        private readonly Sifood3Context _context;

        public StoreManageController(Sifood3Context context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/StoreManage/SendSetPasswordEmail")]
        public string SendSetPasswordEmail([FromBody] SendMail model)
        {
            SmtpClient client = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("brad881112@gmail.com", "")
            };
            MailMessage mail = new()
            {
                Subject = "Sifood商家驗證信",
                From = new MailAddress("brad881112@gmail.com", "Sifood官方帳號")
            };
            mail.To.Add($"{model?.StoreEmail}");
            string MailHtmlBody = $"<div id=\"body\" class=\"sc-jlZhew fuwAaz body\" data-name=\"body\" data-draggable=\"false\" data-empty=\"false\" style=\"background-color: rgb(237, 237, 237);\">\r\n    <div id=\"nsDEeKWBv5AT25uLHham4W\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 60px 30px 100px; border-radius: 0px; background-color: rgb(26, 33, 32); margin-bottom: unset; --width: 680px;\">\r\n        <div id=\"nC73z7rDaZtb1DnPoZmesl\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 0px; border-radius: 0px; background-color: unset; --width: 475px;\">\r\n            <div id=\"nWyg-VBoKGfIpL9Xj0qwoq\" src=\"https://uploads.tabular.email/u/88f987f4-4b2f-49a3-bfd1-56a8c4319a80/51fea2b2-7c41-41de-86af-dd2b48532c62.png\" alt=\"\" class=\"sc-jlZhew fuwAaz image\" data-name=\"image\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: flex-start; text-align: left; margin-top: 0px; margin-bottom: 90px; padding: 0px; --width: 40px;\">\r\n                     </div>\r\n            <div id=\"nzk_5G7mxamMm4foEnLJHm\" class=\"sc-jlZhew fuwAaz heading-1\" data-name=\"heading-1\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: center; margin-top: 0px; margin-bottom: 16px; font-family: &quot;Albert Sans&quot;; font-weight: 700; font-style: normal; color: rgb(255, 255, 255); font-size: 35px; letter-spacing: 0px; word-spacing: 0px; line-height: 39px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 475px; padding-bottom: 0px;\">\r\n                <span id=\"nCayFSnnkGGRlSTMgTwG7k\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">商家密碼設定</span>\r\n            </div>\r\n            <div id=\"n8eO6sS8lPQIrh69cr9M49\" class=\"sc-jlZhew fuwAaz paragraph\" data-name=\"paragraph\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; margin-top: 0px; margin-bottom: 56px; font-family: &quot;Albert Sans&quot;; font-weight: 400; font-style: normal; color: rgb(255, 255, 255); font-size: 21px; letter-spacing: 0px; word-spacing: 0px; line-height: 32px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 430px;\">\r\n                <span id=\"nkqntWTTVhUjDKRCzOLEKf\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">為了設定您的密碼，請幫我點選以下連結(帳號為您的信箱)   </span>\r\n            </div>\r\n            <div id=\"nCyOjg661zdZ7IUK7vV47T\" href=\"https://tabular.email\" class=\"sc-jlZhew fuwAaz button\" data-name=\"button\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; font-family: &quot;Inter Tight&quot;; font-weight: 800; font-style: normal; text-align: center; text-decoration: none; font-size: 14px; line-height: 48px; color: rgb(255, 255, 255); background-color: rgb(17, 152, 114); margin-top: 0px; margin-bottom: 0px; padding: 0px; letter-spacing: 0.65px; word-spacing: 0px; direction: ltr; border-radius: 4px; text-transform: uppercase; --width: 246px;\">\r\n             <span  id=\"nONbX9vLZTyWRQAlCXgYjO\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\"><a href=\"https://sifood103.azurewebsites.net/Stores/Account/SetAccount\">密碼設定</a></span>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
            mail.Body = MailHtmlBody;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            client.Send(mail);
            var store = _context.Stores.Where(x => x.Email == model.StoreEmail).FirstOrDefault();
            store.StoreIsAuthenticated = 1;
            _context.SaveChanges();
            return "已寄送驗證信";            
        }
    }
}
