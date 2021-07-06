using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANYI_Cafe.Models;
using System.IO;
using PagedList;

namespace ANYI_Cafe.Areas.Admin.Controllers
{
    public class PropertysController : Controller

    {

        anyicafeEntities db = new anyicafeEntities();
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        // GET: Admin/Propertys
        public ActionResult Index(int page = 1, int pageSize = 5, string searchText = "")
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                AppService.PageNo = page;
                AppService.PageSize = pageSize;
                if (string.IsNullOrEmpty(searchText))
                {
                    var data = db.Propertys.OrderBy(m => m.mno)
                  .ToPagedList(page, pageSize);
                    return View(data);
                }
                var data1 = db.Propertys.OrderBy(m => m.mno).Where(m =>
                m.mno.Contains(searchText) ||
                m.mname.Contains(searchText) ||
                m.mvalue.Contains(searchText))
                .ToPagedList(page, pageSize);
                return View(data1);
            }

        }
        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["searchText"];
            return RedirectToAction("Index", "Propertys",
            new { page = AppService.PageNo, pageSize = AppService.PageSize, searchText = str_text });
        }


        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                Propertys model = new Propertys()
                {
                    mno = "",
                    mname = "",
                    mvalue = "",
                    remark = ""
                };
                return View(model);
            }
        }
        [HttpPost]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(Propertys model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Propertys.Where(m => m.mname == model.mname).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("Propertys_no", "此商品已存在");
                    return View(model);
                }

                Propertys data1 = new Propertys();
                data1.mno = model.mno;
                data1.mname = model.mname;
                data1.mvalue = model.mvalue;



                db.Propertys.Add(data1);
                db.SaveChanges();

                return RedirectToAction("Index");

            }

        }
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
                return View(model);
            }

        }
        [HttpPost]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(Propertys model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Propertys.Where(m => m.rowid == model.rowid).FirstOrDefault();

                data.mno = model.mno;
                data.mname = model.mname;
                data.mvalue = model.mvalue;


                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        //[HttpGet]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.Propertys.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

        }

        //[HttpPost]
        //[ActionName("Delete")]
        ////[LoginAuthorize(RoleNo = "Admin")]
        //public ActionResult DeleteData(int id)
        //{
        //    bool status = false;
        //    using (anyicafeEntities db = new anyicafeEntities())
        //    {
        //        var model = db.Propertys.Where(m => m.rowid == id).FirstOrDefault();
        //        if (model != null)
        //        {
        //            db.Propertys.Remove(model);
        //            db.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    return new JsonResult { Data = new { status = status } };
    }
}
