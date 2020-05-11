using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logosblog.Controllers
{
    using logosblog.Models;
    using PagedList;

    public class KategoriController : Controller
    {
        logosblogEntities context = new logosblogEntities();
        [Route("Ktg/{fbaslik}")]
        public ActionResult Index(string fbaslik)
        {
            ViewBag.ktg = context.Kategoris.FirstOrDefault(x => x.Adi == fbaslik);
            return View(ViewBag.ktg.KategoriId);
        }

        public PartialViewResult KategoriWidget()
        {
            return PartialView(context.Kategoris.ToList());
        }

        public ActionResult MakaleListele(int id, int? page)
        {
            var data = context.Makales.Where(x => x.KategoriID == id && x.MakaleOnay == true).ToList();
            var pageNumber = page ?? 1;
            int pageSplit = 12;
            List<object> pglist = new List<object>();
            pglist.Add(pageNumber);
            pglist.Add(((data.Count - 1) / pageSplit) + 1);
            pglist.Add("/Kategori/MakaleListele");
            pglist.Add(id.ToString());
            ViewBag.pndata = pglist;
            return View("MakaleListeleWidget", data.ToPagedList(pageNumber, pageSplit));

        }

        
    }
}