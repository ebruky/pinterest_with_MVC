using SunucuMvcPinterest.Models.DataContext;
using SunucuMvcPinterest.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static SunucuMvcPinterest.Models.Model.Uye;

namespace SunucuMvcPinterest.Controllers
{
    public class HomeController : Controller
    {
        Db_Context db = new Db_Context();

        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Giris(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (Db_Context db = new Db_Context())
                {
                    var obj = db.Uye.Where(a => a.k_adi.Equals(model.k_adi) && a.sifre.Equals(model.sifre)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["ad"] = obj.adSoyad;
                        Session["id"] = obj.id;
                        if (obj.id == 1)
                            return RedirectToAction("Index", "Admin");
                        else
                            return RedirectToAction("Index", "User");

                    }
                }
            }
            return View(model);
        }

        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Giris");
        }
        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(string Mail)
        {
            var kayit = db.Uye.Where(a => a.Mail.Equals(Mail)).SingleOrDefault();
            if (kayit != null)
            {
                Guid randomkey = Guid.NewGuid();
                string sifre=randomkey.ToString().Substring(0, 5);
                kayit.sifre = sifre;
                db.SaveChanges();
                var fromAddress = new MailAddress("proje.rfid.22@gmail.com");
                var toAddress = new MailAddress(Mail);
                const string subject = "Yeni Şifre";
                using (var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, "raspberry.12")
                })
                {
                    using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = sifre })
                    {
                        smtp.Send(message);
                        return RedirectToAction("Giris");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Uye_Kayit()
        {
            Session["uyarı"] = "";
            return View();
        }
        [HttpPost]

        public ActionResult Uye_Kayit([Bind(Include = "id,k_adi,sifre,adSoyad,telefon,Mail")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                int counter = 0;
                List<string> patterns = new List<string>();
                patterns.Add(@"[a-z]");
                patterns.Add(@"[A-Z]");
                patterns.Add(@"[0-9]");
                patterns.Add(@"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]");


                foreach (string p in patterns)
                {
                    if (Regex.IsMatch(uye.sifre, p))
                    {
                        counter++;
                    }
                }
                if (counter > 3)
                {
                    db.Uye.Add(uye);
                    db.SaveChanges();
                    Session["id"] = uye.id;
                    Session["ad"] = uye.adSoyad;
                    Session.Remove("uyarı");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    Session["uyarı"] = "Lütfen şifre belirlerken en az bir büyük harf,bir küçük harf,bir rakam ve bir özel karakter kullanınız!";
                }

            }

            return View(uye);
        }
    }
}