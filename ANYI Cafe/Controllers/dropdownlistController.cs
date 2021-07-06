using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANYI_Cafe.Models;
using DropDownList.Models;
namespace DropDownList.Controllers
{
    public class dropdownlistController : Controller
    {
        anyicafeEntities db = new anyicafeEntities();
        public ActionResult Index()
        {
            DDLViewModel model = new DDLViewModel();

            var data = db.city.OrderByDescending(m => m.rowid).ToList();
            SelectList citylist = new SelectList(data, "city1", "city1");
            ViewBag.CityList = citylist;


            return View(model);
        }

        public JsonResult getdistrict(string city)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.district.Where(m => m.city == city).OrderBy(m => m.rowid).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getroad(string district)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.road.Where(m => m.district == district).OrderBy(m => m.rowid).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}