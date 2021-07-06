using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANYI_Cafe.Models;
using PagedList;

namespace ANYI_Cafe.Areas.Admin.Controllers
{
    public class PaymentController : Controller
    {   // GET: Admin/Payment
        anyicafeEntities db = new anyicafeEntities();
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
                    var data = db.Payments.OrderBy(m => m.mno)
                    .ToPagedList(page, pageSize);
                    return View(data);
                }
                var data1 = db.Payments.OrderBy(m => m.mno).Where(m =>
                m.mno.Contains(searchText) ||
                m.mname.Contains(searchText))
                .ToPagedList(page, pageSize);
                return View(data1);
            }
        }
        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["searchText"];
            return RedirectToAction("Index", "Payment",
            new { page = AppService.PageNo, pageSize = AppService.PageSize, searchText = str_text });
        }

        //[LoginAuthorize(RoleList = "Admin")]
        //public ActionResult GetDataList()
        //{
        //    using (anyicafeEntities db = new anyicafeEntities())
        //    {
        //        var models = db.Payments.OrderBy(m => m.mno).ToList();
        //        return Json(new { data = models }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        private SelectList GetSpecOpt()
        {
            var specopt = db.Payments.OrderBy(m => m.rowid).ToList();
            SelectList specoptlist = new SelectList(specopt, "mno", "mname");
            return specoptlist;
        }
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                Payments model = new Payments()
                {
                    mno = "",
                    mname = "",

                };
                ViewBag.specopt = GetSpecOpt();
                return View(model);
            }
        }
        [HttpPost]



        public ActionResult Create(Payments model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Payments.Where(m => m.mno == model.mno).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("mno", "此付款方式已存在");
                    return View(model);
                }

                Payments data1 = new Payments();
                data1.mno = model.mno;
                data1.mname = model.mname;


                db.Payments.Add(data1);
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
                var model = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
                return View(model);
            }

        }
        [HttpPost]
        //[LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(Payments model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Payments.Where(m => m.rowid == model.rowid).FirstOrDefault();

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
                var data = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.Payments.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

        }

        //[HttpPost]
        //[ActionName("Delete")]
        ////[LoginAuthorize(RoleList = "Admin")]
        //public ActionResult DeleteData(int id)
        //{
        //    bool status = false;
        //    using (anyicafeEntities db = new anyicafeEntities())
        //    {
        //        var model = db.Payments.Where(m => m.rowid == id).FirstOrDefault();
        //        if (model != null)
        //        {
        //            db.Payments.Remove(model);
        //            db.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    return new JsonResult { Data = new { status = status } };
        //}
    }
}
