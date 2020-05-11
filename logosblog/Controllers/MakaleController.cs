using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logosblog.Controllers
{
    using logosblog.App_Classes;
    using Models;
    using PagedList;
    using System.Drawing;

    [Authorize]
    public class MakaleController : Controller
    {
        // GET: Makale
        logosblogEntities context = new logosblogEntities();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("~/{kategori}/{id:int}/{title}")]
        public ActionResult Detay(string kategori, int id, string title)
        {
            var data = context.Makales.FirstOrDefault(x => x.MakaleId == id);
            data.GoruntulenmeSayisi++;
            context.SaveChanges();
            Kullanici ak = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            if (ak == null)
            {
                ViewBag.Tkp = false;
            }
            else
            {
                ViewBag.Tkp = ak.Kullanici1.Any(x => x.KullaniciId == data.KullaniciID);
            }

            ViewBag.AktifKullanici = ak;

            return View(data);
        }


        public ActionResult MakaleEkle()
        {
            ViewBag.Etiketler = context.Etikets.ToList();

            return View(context.Kategoris.ToList());
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public string MakaleEkle(string etk, Makale mkl, HttpPostedFileBase resim)
        {
            if (resim != null)
            {
                Random rnd = new Random();
                string imgrnd = "-" + rnd.Next(0, 999999).ToString() + "-";

                Image img = Image.FromStream(resim.InputStream);
                Bitmap kucukResim = new Bitmap(img, Ayarlar.ResimKucukBoyut);
                Bitmap ortaResim = new Bitmap(img, Ayarlar.ResimOrtaBoyut);
                Bitmap buyukResim = new Bitmap(img, Ayarlar.ResimBuyukBoyut);

                kucukResim.Save(Server.MapPath("/Content/images/MakaleResim/KucukBoyut/" + User.Identity.Name + imgrnd + resim.FileName));
                ortaResim.Save(Server.MapPath("/Content/images/MakaleResim/OrtaBoyut/" + User.Identity.Name + imgrnd + resim.FileName));
                buyukResim.Save(Server.MapPath("/Content/images/MakaleResim/BuyukBoyut/" + User.Identity.Name + imgrnd + resim.FileName));


                Resim rsm = new Resim();
                rsm.BuyukBoyut = "/Content/images/MakaleResim/BuyukBoyut/" + User.Identity.Name + imgrnd + resim.FileName;
                rsm.KucukBoyut = "/Content/images/MakaleResim/KucukBoyut/" + User.Identity.Name + imgrnd + resim.FileName;
                rsm.OrtaBoyut = "/Content/images/MakaleResim/OrtaBoyut/" + User.Identity.Name + imgrnd + resim.FileName;

                context.Resims.Add(rsm);
                context.SaveChanges();
                mkl.ResimID = rsm.ResimId;
            }
            mkl.EklenmeTarihi = DateTime.Now;
            mkl.GoruntulenmeSayisi = 0;
            mkl.Begeni = 0;
            mkl.MakaleOnay = false;

            int yzrId = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name).KullaniciId;
            mkl.KullaniciID = yzrId;
            context.Makales.Add(mkl);

            foreach (var et in etk.Split('#'))
            {
                Etiket etik = context.Etikets.FirstOrDefault(x => x.Adi == et);
                mkl.Etikets.Add(etik);
            }
            context.SaveChanges();
            return ("/" + mkl.KategoriID + "/" + mkl.MakaleId + "/" + mkl.Baslik);
        }

        [AllowAnonymous]
        [Route("MakaleBegen/{id}")]
        public string Begen(int id)
        {
            Makale mkl = context.Makales.FirstOrDefault(x => x.MakaleId == id);
            mkl.Begeni++;
            context.SaveChanges();

            return mkl.Begeni.ToString();
        }

        public ActionResult Makalelerim(int? page)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            List<Makale> mkl = context.Makales.Where(x => x.KullaniciID == kl.KullaniciId).ToList();

            var pageNumber = page ?? 1;

            return View(mkl.OrderByDescending(x => x.EklenmeTarihi).ToPagedList(pageNumber, 15));
        }

        [Route("MakaleDuzenle/{id}")]
        public ActionResult MakaleDuzenle(int id)
        {
            ViewBag.Etiketler = context.Etikets.ToList();
            Makale mkl = context.Makales.FirstOrDefault(x => x.MakaleId == id);

            if (mkl.Kullanici.KullaniciAdi == User.Identity.Name)
            {
                ViewBag.ktg = context.Kategoris.ToList();

                return View(mkl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public string MakaleDuzenle(string etk, Makale mkl, HttpPostedFileBase resim)
        {
            Makale makale = context.Makales.FirstOrDefault(x => x.MakaleId == mkl.MakaleId);
            if (makale.Kullanici.KullaniciAdi == User.Identity.Name || User.IsInRole("Admin") == true)
            {
                makale.MakaleOnay = false;
                makale.KategoriID = mkl.KategoriID;
                makale.Baslik = mkl.Baslik;
                makale.Icerik = mkl.Icerik;

                if (resim != null)
                {
                    Random rnd = new Random();
                    string imgrnd = "-" + rnd.Next(0, 999999).ToString() + "-";

                    Image img = Image.FromStream(resim.InputStream);
                    Bitmap kucukResim = new Bitmap(img, Ayarlar.ResimKucukBoyut);
                    Bitmap ortaResim = new Bitmap(img, Ayarlar.ResimOrtaBoyut);
                    Bitmap buyukResim = new Bitmap(img, Ayarlar.ResimBuyukBoyut);

                    kucukResim.Save(Server.MapPath("/Content/images/MakaleResim/KucukBoyut/" + User.Identity.Name + imgrnd + resim.FileName));
                    ortaResim.Save(Server.MapPath("/Content/images/MakaleResim/OrtaBoyut/" + User.Identity.Name + imgrnd + resim.FileName));
                    buyukResim.Save(Server.MapPath("/Content/images/MakaleResim/BuyukBoyut/" + User.Identity.Name + imgrnd + resim.FileName));


                    Resim rsm = new Resim();
                    rsm.BuyukBoyut = "/Content/images/MakaleResim/BuyukBoyut/" + User.Identity.Name + imgrnd + resim.FileName;
                    rsm.KucukBoyut = "/Content/images/MakaleResim/KucukBoyut/" + User.Identity.Name + imgrnd + resim.FileName;
                    rsm.OrtaBoyut = "/Content/images/MakaleResim/OrtaBoyut/" + User.Identity.Name + imgrnd + resim.FileName;

                    context.Resims.Add(rsm);
                    context.SaveChanges();
                    makale.ResimID = rsm.ResimId;
                }

                makale.Etikets.Clear();
                foreach (var et in etk.Trim().Split('\n', '#'))
                {

                    Etiket etik = context.Etikets.FirstOrDefault(x => x.Adi == et);
                    makale.Etikets.Add(etik);
                }
                context.SaveChanges();

                return ("/" + makale.Kategori.Adi + "/" + makale.MakaleId + "/" + makale.Baslik);

            }
            else
            {
                return ("/" + makale.Kategori.Adi + "/" + makale.MakaleId + "/" + makale.Baslik);
            }

        }
        [Route("MakaleSil/{id}")]
        public ActionResult MakaleSil(int id)
        {
            Makale mkl = context.Makales.FirstOrDefault(x => x.MakaleId == id);

            if (mkl.Kullanici.KullaniciAdi == User.Identity.Name || User.IsInRole("Admin") == true)
            {
                if (mkl.Resim != null)
                {
                    string fullPath = Request.MapPath(mkl.Resim.BuyukBoyut);
                    string fullPath2 = Request.MapPath(mkl.Resim.OrtaBoyut);
                    string fullPath3 = Request.MapPath(mkl.Resim.KucukBoyut);
                    
                    System.IO.File.Delete(fullPath);
                    System.IO.File.Delete(fullPath2);
                    System.IO.File.Delete(fullPath3);
                }
                mkl.Yorums.Clear();
                mkl.Resims.Clear();
                mkl.Etikets.Clear();
                context.Makales.Remove(mkl);
                context.SaveChanges();

                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult YorumYaz(Yorum yrm)
        {
            yrm.YorumOnay = false;
            yrm.EklenmeTarihi = DateTime.Now;
            context.Yorums.Add(yrm);
            context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Route("{id}/YorumSil")]
        public ActionResult YorumSil(int id)
        {
            Yorum yrm = context.Yorums.FirstOrDefault(x => x.YorumId == id);
            if (yrm.Kullanici.Adi == User.Identity.Name || User.IsInRole("Admin") == true)
            {
                context.Yorums.Remove(yrm);
                context.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult Yorumlarim(int? page)
        {
            List<Yorum> yrm = context.Yorums.Where(x => x.Kullanici.Adi == User.Identity.Name).ToList();
            var pageNumber = page ?? 1;

            return View(yrm.OrderByDescending(x => x.EklenmeTarihi).ToPagedList(pageNumber, 15));
        }
    }
}