using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ANYI_Cafe.Models;

namespace ANYI_Cafe.Controllers
{
    public class VendorController : Controller
    {
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                return View(db.vendor.ToList());
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            vendor model = new vendor()
            {

                vname = "",
                vemail = ""
            };
            return View(model);
        }
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(vendor model)
        {
            //ModelState.Remove("sno");
            if (!ModelState.IsValid) return View(model);

            using (Cryptographys cryp = new Cryptographys())
            {
                using (anyicafeEntities db = new anyicafeEntities())
                {
                    vendor input = new vendor();
                    input.vno = GetNewUno();
                    input.vname = model.vname;
                    input.vtaxno = model.vtaxno;
                    input.vtel = model.vtel;
                    input.vemail = model.vemail;
                    input.vaddr = model.vaddr;
                    input.vrate = model.vrate;
                    input.vremark = model.vremark;

                    db.vendor.Add(input);
                    db.SaveChanges();

                    return RedirectToAction("List", "Vendor");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.vendor.Where(m => m.vid == id).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(vendor model)
        {
            //ModelState.Remove("sid");
            if (!ModelState.IsValid) return View(model);

            using (anyicafeEntities db = new anyicafeEntities())
            {
                //自定義檢查
                if (model.vno.Substring(0, 1) != "V") { ModelState.AddModelError("vno", "格式錯誤!!"); return View(model); }
                if (model.vno.Length != 5) { ModelState.AddModelError("vno", "格式錯誤!!"); return View(model); }
                var check1 = db.vendor.Where(m => m.vid != model.vid && m.vno == model.vno).FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("vno", "編號重覆!!"); return View(model); }
                var check2 = db.vendor.Where(m => m.vid != model.vid && m.vemail == model.vemail).FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("vemail", "電子信箱重覆!!"); return View(model); }

                var input = db.vendor.Where(m => m.vid == model.vid).FirstOrDefault();
                input.vno = model.vno;
                input.vname = model.vname;
                input.vtaxno = model.vtaxno;
                input.vtel = model.vtel;
                input.vemail = model.vemail;
                input.vaddr = model.vaddr;
                input.vrate = model.vrate;
                input.vremark = model.vremark;

                db.SaveChanges();
                return RedirectToAction("List", "Vendor");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.vendor.Where(m => m.vid == id).FirstOrDefault();
                if (model != null)
                {
                    db.vendor.Remove(model);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "Vendor");
            }
        }


        /// <summary>
        /// 取得新的廠商編號
        /// </summary>
        /// <returns></returns>
        private string GetNewUno()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string str_vno = "V0001";
                var data = db.vendor
                    .OrderByDescending(m => m.vno)
                    .FirstOrDefault();
                if (data != null) str_vno = "S" + (int.Parse(data.vno.Substring(1, 4)) + 1).ToString().PadLeft(4, '0');
                return str_vno;
            }
        }
    }
}