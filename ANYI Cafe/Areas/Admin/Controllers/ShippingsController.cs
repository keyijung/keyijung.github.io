using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANYI_Cafe.Models;
using PagedList;

namespace ANYI_Cafe.Areas.Admin.Controllers
{
    public class ShippingsController : Controller
    {
        anyicafeEntities db = new anyicafeEntities();
        // GET: Admin/Payment
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Index(int page = 1, int pageSize = 5, string searchText = "")
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                AppService.PageNo = page;
                AppService.PageSize = pageSize;
                if (string.IsNullOrEmpty(searchText))
                {
                    var data = db.Shippings.OrderBy(m => m.rowid)
                     .ToPagedList(page, pageSize);
                    return View(data);
                }
                var data1 = db.Shippings.OrderBy(m => m.rowid).Where(m =>
                m.mno.Contains(searchText) ||
                m.mname.Contains(searchText))
                    .ToPagedList(page, pageSize);
                return View(data1);
            }
        }
        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["searchText"];
            return RedirectToAction("Index", "Shippings",
            new { page = AppService.PageNo, pageSize = AppService.PageSize, searchText = str_text });
        }

        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult GetDataList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var models = db.Shippings.OrderBy(m => m.mno).ToList();
                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        private SelectList GetSpecOpt()
        {
            var specopt = db.Shippings.OrderBy(m => m.rowid).ToList();
            SelectList specoptlist = new SelectList(specopt, "mno", "mname");
            return specoptlist;
        }
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                Shippings model = new Shippings()
                {
                    mno = "",
                    mname = "",

                };
                ViewBag.specopt = GetSpecOpt();
                return View(model);
            }
        }
        [HttpPost]


        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(Shippings model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Shippings.Where(m => m.mno == model.mno).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("mno", "此付款方式已存在");
                    return View(model);
                }

                Shippings data1 = new Shippings();
                data1.mno = model.mno;
                data1.mname = model.mname;


                db.Shippings.Add(data1);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        [HttpGet]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.Shippings.Where(m => m.rowid == id).FirstOrDefault();
                return View(model);
            }

        }
        [HttpPost]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(Shippings model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Shippings.Where(m => m.rowid == model.rowid).FirstOrDefault();

                data.mno = model.mno;
                data.mname = model.mname;
                data.remark = model.remark;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }



        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Shippings.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.Shippings.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

        }
    }
}