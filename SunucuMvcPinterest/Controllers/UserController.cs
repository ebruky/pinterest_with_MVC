using SunucuMvcPinterest.Models;
using SunucuMvcPinterest.Models.DataContext;
using SunucuMvcPinterest.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SunucuMvcPinterest.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private Db_Context db = new Db_Context();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult kisiler(string aranan)
        {
            var Kisiler = db.Uye.Where(x => x.k_adi.Contains(aranan) || aranan == null).ToList();
            return View(Kisiler);
        }
        
           public ActionResult uyeGonderileri(int? id)
        {
            var yayin = db.Yayin.Where(x => x.uye_Id == id).ToList();
            if (yayin != null)
                return View(yayin);
            return View();
        }
        public ActionResult Profil()
        {
            
            string id = Session["id"].ToString();
            int Id = int.Parse(id);
            Uye uye = db.Uye.Find(Id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpGet]
        public ActionResult Profil_Guncelle(int? id)
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
        public ActionResult Profil_Guncelle([Bind(Include = "id,k_adi,sifre,adSoyad,telefon,Mail")] Uye uye)
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
                    return RedirectToAction("Profil");
                }
            }
            return View(uye);

        }

        public ActionResult Gonderiler()
        {
            string id = Session["id"].ToString();
            int Id = int.Parse(id);
            var yayin = db.Yayin.Where(x => x.uye_Id == Id).ToList();
            if (yayin != null)
                return View(yayin);
            return View();
        }
        [HttpGet]
        public ActionResult Gonderi_Güncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yayin yayin = db.Yayin.Find(id);
            if (yayin == null)
            {
                return HttpNotFound();
            }
            return View(yayin);

        }
        [HttpPost]
        public ActionResult Gonderi_Güncelle(int id, Yayin yayin, HttpPostedFileBase resim_yol)
        {
            if (ModelState.IsValid)
            {
                var yayin1 = db.Yayin.Where(x => x.İd == id).SingleOrDefault();
                if (resim_yol != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(yayin1.resim_yol)))
                    {
                        System.IO.File.Delete(Server.MapPath(yayin1.resim_yol));
                    }
                    WebImage img = new WebImage(resim_yol.InputStream);
                    System.IO.FileInfo imgInfo = new System.IO.FileInfo(resim_yol.FileName);
                    string ImageName = $"kullanici_{yayin.uye_Id}" + Guid.NewGuid().ToString() + imgInfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Yayin/" + ImageName);
                    yayin1.resim_yol = "/Uploads/Yayin/" + ImageName;

                }
                yayin1.icerik = yayin.icerik;


                db.SaveChanges();
                return RedirectToAction("Gonderiler");
            }
            return View(yayin);
        }
        [HttpGet]

        public ActionResult Gonderi_Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yayin yayin = db.Yayin.Find(id);
            if (yayin == null)
            {
                return HttpNotFound();
            }
            return View(yayin);
        }
        [HttpPost]

        public ActionResult Gonderi_Sil(int id)
        {
            Yayin yayin = db.Yayin.Find(id);
            db.Yayin.Remove(yayin);
            db.SaveChanges();
            return RedirectToAction("Gonderiler");

        }
        [HttpGet]
        public ActionResult Gonderi_Ekle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Gonderi_Ekle(Yayin yayin, HttpPostedFileBase resim_yol)
        {
            if (ModelState.IsValid)
            {

                if (resim_yol != null)
                {
                    WebImage img = new WebImage(resim_yol.InputStream);
                    System.IO.FileInfo imgInfo = new System.IO.FileInfo(resim_yol.FileName);
                    string ImageName = $"kullanici_{yayin.uye_Id}" + Guid.NewGuid().ToString() + imgInfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Yayin/" + ImageName);
                    yayin.resim_yol = "/Uploads/Yayin/" + ImageName;

                }
                string id = Session["id"].ToString();
                int Id = int.Parse(id);
                yayin.uye_Id = Id;
                yayin.tarih = DateTime.Now;
                db.Yayin.Add(yayin);
                db.SaveChanges();
                return RedirectToAction("Gonderiler");
            }
            return View();


        }
    }
}