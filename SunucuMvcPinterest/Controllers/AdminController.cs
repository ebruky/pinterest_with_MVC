using SunucuMvcPinterest.Models;
using SunucuMvcPinterest.Models.DataContext;
using SunucuMvcPinterest.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SunucuMvcPinterest.Controllers
{
    [adminAuthorize]
    public class AdminController : Controller
    {
        private Db_Context db = new Db_Context();
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Uyeler()
        {
            return View(db.Uye.ToList());
        }
        [HttpGet]
        public ActionResult Uye_Güncelle(int? id)
        {
            Session["uyarı"] = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpPost]
        public ActionResult Uye_Güncelle([Bind(Include = "id,k_adi,sifre,adSoyad,telefon,Mail")] Uye uye)
        {
            bool sorunYok = true;
            if (ModelState.IsValid)
            {
                Uye u = db.Uye.Find(uye.id);
                if (uye.sifre != u.sifre)
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
                        sorunYok = true;

                    }
                    else
                    {
                        Session["uyarı"] = "Lütfen şifre belirlerken en az bir büyük harf,bir küçük harf,bir rakam ve bir özel karakter kullanınız!";
                        sorunYok = false;
                    }
                }
                if (sorunYok)
                {
                    u.k_adi = uye.k_adi;
                    u.adSoyad = u.telefon;
                    u.sifre = uye.sifre;
                    u.Mail = u.Mail;
                    db.SaveChanges();
                    Session.Remove("uyarı");
                    return RedirectToAction("Uyeler");
                }
            }
            return View(uye);
        }
        [HttpGet]
        public ActionResult Uye_Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpPost]

        public ActionResult Uye_Sil(int id)
        {
            Uye uye = db.Uye.Find(id);
            db.Uye.Remove(uye);
            db.SaveChanges();
            return RedirectToAction("Uyeler");
        }
        [HttpGet]
        public ActionResult Uye_Ekle()
        {
            Session["uyarı"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Uye_Ekle([Bind(Include = "id,k_adi,sifre,adSoyad,telefon,Mail")] Uye uye)
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
                    Session.Remove("uyarı");
                    return RedirectToAction("Uyeler", "Admin");
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