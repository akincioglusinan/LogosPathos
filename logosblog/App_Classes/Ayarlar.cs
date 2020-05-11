using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logosblog.App_Classes
{
    public class Ayarlar
    {
        public static Size ResimKucukBoyut
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["sw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["sh"]);
                return sonuc;
            }
        }

        public static Size ResimOrtaBoyut
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["mw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["mh"]);
                return sonuc;
            }
        }

        public static Size ResimBuyukBoyut
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["bw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["bh"]);
                return sonuc;
            }
        }

        public static Size ResimYazarBoyut
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["yazar"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["yazar"]);
                return sonuc;
            }
        }

    }
}