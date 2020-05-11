using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using logosblog.Models;
using System.Xml.Serialization;
using System.Xml.Linq;
using PagedList;

namespace logosblog.Controllers
{

    public class HomeController : Controller
    {
        logosblogEntities context = new logosblogEntities();

        public ActionResult Index(int? page)
        {
            List<Makale> mkl = context.Makales.Where(x => x.SiteBaslik1 == true || x.SiteBaslik2 == true).ToList();
            return View(mkl);
        }

        public PartialViewResult MakaleListele(int? page)
        {
            var data = context.Makales.Where(x => x.MakaleOnay == true).ToList();
            var pageNumber = page ?? 1;
            int pageSplit = 8;
            List<object> pglist = new List<object>();
            pglist.Add(pageNumber);
            pglist.Add(((data.Count-1) / pageSplit) + 1);
            pglist.Add("/Home/MakaleListele");
            pglist.Add(pageSplit);
            ViewBag.pndata = pglist;
            return PartialView("MakaleListeleWidget", data.ToPagedList(pageNumber, pageSplit));
            //: View("MakaleListeleWidget", data.ToPagedList(pageNumber, 5));
            // return View("MakaleListeleWidget", data.ToPagedList(pageNumber, 5));
        }

        public PartialViewResult OneCikanMakalelerWidget()
        {
            var model = context.Makales.Where(x => x.MakaleOnay == true).OrderByDescending(x => x.EklenmeTarihi).Take(4).ToList();
            return PartialView(model);
        }


        public ActionResult Arama(object[] arama)
        {
            return View(arama);
        }

        public ActionResult AramaListele(object[] arama, int? page)
        {
            string dd = (string)arama[0];
            var data = context.Makales.Where(x => (x.Baslik.Contains(dd) || x.Icerik.Contains(dd)) && x.MakaleOnay == true).ToList();
            var pageNumber = page ?? 1;
            int pageSplit = 12;
            List<object> pglist = new List<object>();
            pglist.Add(pageNumber);
            pglist.Add(((data.Count-1) / pageSplit ) + 1 );
            pglist.Add("/Home/AramaListele");
            pglist.Add(dd);
            ViewBag.pndata = pglist;
            return View("MakaleListeleWidget", data.ToPagedList(pageNumber, pageSplit));
        }

        public ActionResult Hakkinda()
        {
            return View();
        }

        public void MakaleBulten(string model)
        {
            string file = Server.MapPath("~/Content/files/bultenlist.xml");

            XDocument doc;
            if (!System.IO.File.Exists(file))
            {
                doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("bultenlist"));
            }
            else
            {
                doc = XDocument.Load(file);
            }

            XElement ele = new XElement("mail", model);
            doc.Root.Add(ele);
            doc.Save(file);
        }
    }
}