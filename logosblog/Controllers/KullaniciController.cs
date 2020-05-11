using logosblog.App_Classes;
using logosblog.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace logosblog.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        logosblogEntities context = new logosblogEntities();
        public ActionResult Index()
        {
            return View();
        }
        //[Route]
        public ActionResult GirisYap()
        {
            if (User.Identity.IsAuthenticated == true || Request.UrlReferrer.ToString() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Session.Add("rdurl", Request.UrlReferrer.ToString());
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Kullanici kl, string hatirla)
        {
            string ka = ValidateUser(kl.KullaniciAdi, kl.Parola);
            if (!string.IsNullOrEmpty(ka))
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, kl.KullaniciAdi, DateTime.Now, DateTime.Now.AddMinutes(120), true, ka, FormsAuthentication.FormsCookiePath);

                if (hatirla == "on")
                {
                    ticket.Expiration.AddDays(30);
                }

                HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName);

                if (ticket.IsPersistent)
                {
                    ck.Expires = ticket.Expiration;
                }
                Response.Cookies.Add(ck);

                FormsAuthentication.RedirectFromLoginPage(kl.KullaniciAdi, true);
                return Redirect(Session["rdurl"].ToString());
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı ya da Parola yanlış!";
                return View();
            }
        }

        string ValidateUser(string ka, string pwd)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == ka && x.Parola == pwd);

            if (kl != null)
            {
                return kl.KullaniciAdi;
            }
            else
            {
                return "";
            }

        }

        public ActionResult ParolamiUnuttum()
        {
            return View();
        }

        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult YazarOl()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin") == false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        public ActionResult YazarOl(Kullanici kl)
        {

            if (context.Kullanicis.Any(x => x.KullaniciAdi == kl.KullaniciAdi || x.MailAdres == kl.MailAdres))
            {
                return View();
            }
            kl.Yazar = true;
            kl.Onaylandi = false;
            kl.Aktif = true;
            if (kl.DogumTarihi != null)
            {
                kl.DogumTarihi = kl.DogumTarihi.Value.Date;
            }
            kl.KayitTarihi = DateTime.Now;
            context.Kullanicis.Add(kl);
            context.SaveChanges();
            Rol yazar = context.Rols.FirstOrDefault(x => x.RolAdi == "Yazar");
            KullaniciRol kr = new KullaniciRol();
            kr.RolID = yazar.RolId;
            kr.KullaniciID = kl.KullaniciId;
            context.KullaniciRols.Add(kr);
            context.SaveChanges();

            return RedirectToAction("GirisYap");
        }


        public string KullaniciKontrol(string id)
        {
            if (context.Kullanicis.Any(x => x.KullaniciAdi == id || x.MailAdres == id))
            {
                return "0";
            }
            else
            {
                return "1";
            }

        }

        public ActionResult Hesabim()
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            return View(kl);
        }

        [HttpPost]
        public ActionResult Hesabim(Kullanici kl, HttpPostedFileBase profoto)
        {
            Kullanici klc = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == kl.KullaniciAdi);
            klc.Adi = kl.Adi;
            klc.Soyadi = kl.Soyadi;
            klc.MailAdres = kl.MailAdres;
            klc.Parola = kl.Parola;
            klc.Aciklama = kl.Aciklama;
            klc.DogumTarihi = kl.DogumTarihi;

            if (profoto != null)
            {
                Random rnd = new Random();
                string imgrnd = "-" + rnd.Next(0, 999999).ToString() + "-";

                Image img = Image.FromStream(profoto.InputStream);
                Bitmap ortaResim = new Bitmap(img, Ayarlar.ResimOrtaBoyut);
                ortaResim.Save(Server.MapPath("/Content/images/ProfilResim/" + User.Identity.Name + imgrnd + profoto.FileName));

                Resim rsm = new Resim();
                rsm.OrtaBoyut = "/Content/images/ProfilResim/" + User.Identity.Name + imgrnd + profoto.FileName;

                context.Resims.Add(rsm);
                context.SaveChanges();
                klc.ResimID = rsm.ResimId;
            }
            context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult KullaniciSozlesmesi()
        {
            return View();
        }


    }
}