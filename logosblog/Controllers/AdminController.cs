using logosblog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace logosblog.Controllers
{
    [Authorize(Roles = "Admin")]
    //[RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        logosblogEntities context = new logosblogEntities();
        public ActionResult Index()
        {
            return View();
        }

        [Route("{id?}/MakaleOnay")]
        public ActionResult MakaleOnay(int? id)
        {
            if (id != null)
            {
                Makale mkg = context.Makales.FirstOrDefault(x => x.MakaleId == id);
                mkg.MakaleOnay = true;
                context.SaveChanges();
            }
            List<Makale> mk = context.Makales.Where(x => x.MakaleOnay == false).ToList();
            return View(mk);
        }
        [Route("{id?}/YorumOnay")]
        public ActionResult YorumOnay(int? id)
        {
            if (id != null)
            {
                Yorum yrmo = context.Yorums.FirstOrDefault(x => x.YorumId == id);
                yrmo.YorumOnay = true;
                context.SaveChanges();
            }
            List<Yorum> yrm = context.Yorums.Where(x => x.YorumOnay == false).ToList();
            return View(yrm);
        }

        public ActionResult KategoriDuzenle()
        {
            return View(context.Kategoris.ToList());
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            context.Kategoris.Add(ktg);
            context.SaveChanges();
            return RedirectToAction("KategoriDuzenle");
        }

        [Route("{id}/KategoriSil")]
        public ActionResult KategoriSil(int id)
        {
            Kategori ktg = context.Kategoris.FirstOrDefault(x => x.KategoriId == id);
            context.Kategoris.Remove(ktg);
            context.SaveChanges();
            return RedirectToAction("KategoriDuzenle");
        }

        public ActionResult EtiketDuzenle()
        {
            return View(context.Etikets.ToList());
        }

        [HttpPost]
        public ActionResult EtiketEkle(Etiket etk)
        {
            context.Etikets.Add(etk);
            context.SaveChanges();
            return RedirectToAction("EtiketDuzenle");
        }

        [Route("{id}/EtiketSil")]
        public ActionResult EtiketSil(int id)
        {
            Etiket etk = context.Etikets.FirstOrDefault(x => x.EtiketId == id);
            context.Etikets.Remove(etk);
            context.SaveChanges();
            return RedirectToAction("EtiketDuzenle");
        }

        public ActionResult RolDuzenle()
        {
            return View(context.Rols.ToList());
        }

        [HttpPost]
        public ActionResult RolEkle(Rol rol)
        {
            context.Rols.Add(rol);
            context.SaveChanges();
            return RedirectToAction("RolDuzenle");
        }

        [Route("{id}/RolSil")]
        public ActionResult RolSil(int id)
        {
            Rol rol = context.Rols.FirstOrDefault(x => x.RolId == id);
            if (rol.RolAdi != "Admin")
            {
                context.Rols.Remove(rol);
                context.SaveChanges();
            }
            return RedirectToAction("RolDuzenle");
        }


        public ActionResult KullaniciDuzenle(int? page)
        {
            ViewBag.Roller = context.Rols.ToList();
            var pageNumber = page ?? 1;
            return View(context.Kullanicis.ToList().OrderByDescending(x => x.KayitTarihi).ToPagedList(pageNumber, 10));
        }

        [HttpPost]
        public ActionResult RolAta(KullaniciRol rl)
        {
            context.KullaniciRols.Add(rl);
            context.SaveChanges();
            return RedirectToAction("KullaniciDuzenle");
        }

        [Route("{id}/KullaniciSil")]
        public ActionResult KullaniciSil(int id)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciId == id);
            if (kl.Resim.OrtaBoyut != null)
            {
                string fullPath = Request.MapPath(kl.Resim.OrtaBoyut);
                System.IO.File.Delete(fullPath);

            }
            kl.Kullanici1.Clear();
            kl.KullaniciRols.Clear();
            kl.Makales.Clear();
            kl.Yorums.Clear();
            kl.Kullanicis.Clear();
            context.Kullanicis.Remove(kl);
            context.SaveChanges();

            return RedirectToAction("KullaniciDuzenle");
        }

        public ActionResult KullaniciEkle(Kullanici kl)
        {

            return RedirectToAction("KullaniciDuzenle");
        }

        public ActionResult MakaleDuzenle(int? page)
        {
            var pageNumber = page ?? 1;
            return View(context.Makales.ToList().OrderByDescending(x => x.EklenmeTarihi).ToPagedList(pageNumber, 10));
        }

        public ActionResult SiteBaslik(string SBaslik, int mkid)
        {
            Makale mkl = context.Makales.FirstOrDefault(x => x.MakaleId == mkid);
            if (SBaslik == "SiteBaslik1")
            {
                mkl.SiteBaslik1 = true;
                mkl.SiteBaslik2 = false;
            }
            else if (SBaslik == "SiteBaslik2")
            {
                mkl.SiteBaslik2 = true;
                mkl.SiteBaslik1 = false;

            }
            else
            {
                mkl.SiteBaslik2 = false;
                mkl.SiteBaslik1 = false;
            }
            context.SaveChanges();
            return RedirectToAction("MakaleDuzenle");
        }
    }
}