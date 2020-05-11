using logosblog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logosblog.Controllers
{
    //[RoutePrefix("Yazar")]
    public class YazarController : Controller
    {
        
        logosblogEntities context = new logosblogEntities();
       
        [Route("Yazarlar/{page:int?}")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            return View(context.Kullanicis.ToList().ToPagedList(pageNumber, 15));
        }

        //[Route("YazarTakip/{id?}/{page?}")]
        public ActionResult YazarTakip(int? id, int? page)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (id != null)
            {
                Kullanici x = context.Kullanicis.FirstOrDefault(y => y.KullaniciId == id);
                kl.Kullanici1.Remove(x);
                context.SaveChanges();
            }
            var pageNumber = page ?? 1;

            return View(kl.Kullanici1.ToList().ToPagedList(pageNumber, 15));
        }

        [Route("YazarTakipEt/{id}")]
        public string TakipEt(int id)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            kl.Kullanici1.Add(context.Kullanicis.FirstOrDefault(x => x.KullaniciId == id));
            context.SaveChanges();
            return "Takipten Çık";
        }
        [Route("YazarTakipCik/{id}")]
        public string TakipCik(int id)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            kl.Kullanici1.Remove(context.Kullanicis.FirstOrDefault(x => x.KullaniciId == id));
            context.SaveChanges();
            return "Takip Et";
        }

        [Route("{yid}/YazarDetay/{page?}/")]
        public ActionResult Detay(int yid, int? page)
        {
            Kullanici kl = context.Kullanicis.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
            if (kl!=null)
            {
 ViewBag.Tkp=kl.Kullanici1.Any(x => x.KullaniciId == yid);
            }
            var pageNumber = page ?? 1;
            ViewBag.ymkl = context.Makales.Where(x => x.MakaleOnay == true).ToList().ToPagedList(pageNumber, 15);
            return View(context.Kullanicis.FirstOrDefault(x=>x.KullaniciId==yid));
        }
    }
}