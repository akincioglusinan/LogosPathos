using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logosblog.Controllers
{
    using Models;
    using PagedList;

    [RoutePrefix("etiket")]
    public class EtiketController : Controller
    {
        // GET: Etiket

        logosblogEntities context = new logosblogEntities();

        [Route("listele/{etkid}/{etkadi}")]
        public ActionResult Index(int etkid)
        {
            ViewBag.etk = context.Etikets.FirstOrDefault(x => x.EtiketId == etkid);
            return View(etkid);
        }

        public PartialViewResult EtiketlerWidget()
        {
            return PartialView(context.Etikets.ToList());
        }
        
        public ActionResult MakaleListele(int id, int? page)
        {
            var data = context.Makales.Where(x => x.Etikets.Any(y => y.EtiketId == id) && x.MakaleOnay==true).ToList();
            var pageNumber = page ?? 1;
            int pageSplit = 12;
            List<object> pglist = new List<object>();
            pglist.Add(pageNumber);
            pglist.Add(((data.Count - 1) / pageSplit) + 1);
            pglist.Add("/Etiket/MakaleListele");
            pglist.Add(id.ToString());
            ViewBag.pndata = pglist;
            return View("MakaleListeleWidget", data.ToPagedList(pageNumber, pageSplit));
        }
    }
}