using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ANYI_Cafe.Models;
using ANYI_Cafe.App_Class;
using System.IO;
using PagedList;
using System.Web.Configuration;

namespace ANYI_Cafe.Controllers
{
    public class ProductController : Controller
        
    {
        anyicafeEntities db = new anyicafeEntities();
        // GET: Product
        //[HttpGet]
        //[LoginAuthorize(RoleList = "Admin")]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult List()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                return View(db.product.OrderBy(m => m.id).ToList());
            }
        }

        private SelectList GetSpecOpt()
        {
            var specopt = db.Propertys.OrderBy(m => m.rowid).ToList();
            SelectList specoptlist = new SelectList(specopt, "mno", "mname");
            return specoptlist;
        }

        public JsonResult SpecOption(string spec)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var content = db.Propertys.Where(m => m.mno == spec).FirstOrDefault();
            var result = content.mvalue.Split(',');
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                ProductCreateViewModel model = new ProductCreateViewModel()
                {
                    product_no = "",
                    product_name = "",
                    price = 0,
                    product_spec = "",
                    product_public = false
                };
                ViewBag.specopt = GetSpecOpt();
                return View(model);
            }
        }
        [HttpPost]

        

        public ActionResult Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.product.Where(m => m.product_no == model.product_no).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("product_no", "此商品已存在");
                    return View(model);
                }

                product data1 = new product();
                data1.product_no = model.product_no;
                data1.product_name = model.product_name;
                data1.price = model.price;
                data1.product_spec = model.product_spec;
                data1.product_public = model.product_public;

                db.product.Add(data1);
                db.SaveChanges();

                return RedirectToAction("List");

            }
        }

        public ActionResult Edit(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.product.Where(m => m.id == id).FirstOrDefault();
                return View(model);
            }

        }
        [HttpPost]
        public ActionResult Edit(product model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.product.Where(m => m.id == model.id).FirstOrDefault();

                data.product_no = model.product_no;
                data.product_name = model.product_name;
                data.price = model.price;
                data.product_spec = model.product_spec;
                data.product_public = model.product_public;
                db.SaveChanges();
                return RedirectToAction("List");
            }

        }

        public ActionResult Delete(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.product.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    db.product.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }

        }

        public ActionResult Upload(int id = 0)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.product.Where(m => m.id == id).FirstOrDefault();

                ImageService.ReturnAction("", "Product", "List");
                ImageService.ImageTitle = string.Format("{0} {1} 圖片上傳", model.product_no, model.product_name);
                ImageService.ImageFolder = "~/Images/Product";
                ImageService.ImageSubFolder = model.product_no;
                ImageService.ImageName = model.product_no;
                ImageService.ImageExtention = "jpg";
                ImageService.UploadImageMode = true;
                return RedirectToAction("UploadImage", "Image");
            }
        }

        //public ActionResult Upload(int id = 0)
        //{
        //    using (anyicafeEntities db = new anyicafeEntities())
        //    {
        //        var model = db.product.Where(m => m.id == id).FirstOrDefault();
        //        if (model != null)
        //        {
        //            Shop.ProductNo = model.product_no;
        //            return View();

        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }

        //    }

        //}
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            string str_folder = string.Format("~/Images/Product/{0}", Shop.ProductNo);
        //            string str_folder_path = Server.MapPath(str_folder);
        //            if (!Directory.Exists(str_folder_path)) Directory.CreateDirectory(str_folder_path);
        //            string str_file_name = Shop.ProductNo + ".jpg";
        //            var path = Path.Combine(str_folder_path, str_file_name);
        //            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        //            file.SaveAs(path);
        //            using (anyicafeEntities db = new anyicafeEntities())
        //            {
        //                var data = db.product.Where(m => m.product_no == Shop.ProductNo).FirstOrDefault();
        //                data.product_img = "../Images/Product/" + Shop.ProductNo + "/" + str_file_name;
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    return RedirectToAction("List");
        //}

        public ActionResult ProductDetail(string id)
        {
            if (id == null) id = "";
            var model = db.product.Where(m => m.product_no == id).FirstOrDefault();
            if (model == null) return RedirectToAction("Oreder", "Home");
            Shop.ProductNo = id;
            return View(model);
        }


        [HttpPost]
        public ActionResult ProductDetail(FormCollection collection)
        {
            int int_qty = 0;
            int int_price = 0;
            string str_property_no = "";
            string str_product_spec = "";
            int.TryParse(collection["qty"].ToString(), out int_qty);
            //int.TryParse(collection["price"].ToString(), out int_price);
            List<SelectListItem> list = Shop.GetPropertyList(Shop.ProductNo);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (collection[item.Value] != null)
                    {
                        str_product_spec += string.Format("{0}={1}", item.Text, collection[item.Value].ToString());
                    }
                }
            }
            Cart.AddCart(Shop.ProductNo, str_product_spec, int_qty);
            return RedirectToAction("ProductDetail", "Product", new { id = Shop.ProductNo });
        }

        public ActionResult CategoryList(string id)
        {
            int int_id = 0;
            ViewData["CategoryNo"] = id;
            ViewBag.CategoryNo = id;
            ViewBag.CategoryName = Shop.GetCategoryName(id, ref int_id);
            List<product> model = new List<product>();

            var prod1 = db.product.Where(m => m.categoryid == int_id).ToList();
            if (prod1 != null && prod1.Count > 0)
            {
                foreach (var item1 in prod1)
                { model.Add(item1); }
            }

            var model1 = db.Categorys.Where(m => m.parentid == int_id).ToList();
            if (model1 != null && model1.Count > 0)
            {
                foreach (var mod1 in model1)
                {
                    var prod2 = db.product.Where(m => m.categoryid == mod1.rowid).ToList();
                    if (prod2 != null && prod2.Count > 0)
                    {
                        foreach (var item2 in prod2)
                        { model.Add(item2); }
                    }

                    var model2 = db.Categorys.Where(m => m.parentid == mod1.rowid).ToList();
                    if (model2 != null && model2.Count > 0)
                    {
                        foreach (var mod2 in model2)
                        {
                            var prod3 = db.product.Where(m => m.categoryid == mod2.rowid).ToList();
                            if (prod3 != null && prod3.Count > 0)
                            {
                                foreach (var item3 in prod3)
                                { model.Add(item3); }
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        public JsonResult Product_price(string product_name, string product_spec)
        {
            var data = db.product.Where(m => m.product_name == product_name).Where(m => m.product_spec == product_spec).FirstOrDefault();
            Shop.ProductNo = data.product_no;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Product(string product_no, string product_price)
        {
            int price = Convert.ToInt32(product_price);
            var data = db.product.Where(m => m.product_no == product_no && m.price == price).FirstOrDefault();
            Shop.ProductNo = data.product_no;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [LoginAuthorize(RoleList = "Guest,User")] //會員登入與權限控管
        public ActionResult CartList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (UserAccount.IsLogin)
                {
                    var data1 = db.Carts
                        .Where(m => m.user_no == UserAccount.UserNo)
                        .ToList();
                    return View(data1);
                }
                var data2 = db.Carts
                   .Where(m => m.lot_no == Cart.LotNo)
                   .ToList();
                return View(data2);

            }
        }
        [LoginAuthorize(RoleList= "Guest,User")]
        public ActionResult AddCart(string id)
        {
            return RedirectToAction("ProductDetail", "Product", new { id = id });
        }

        [LoginAuthorize(RoleList = "Guest,User")]
        public ActionResult CartDelete(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                db.Carts.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("CartList"); //回傳重新導向
        }

        [LoginAuthorize(RoleList= "Guest,User")]
        public ActionResult CartPlus(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                data.qty += 1;
                data.amount = data.qty * data.price;
                db.SaveChanges();
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleList= "Guest,User")]
        public ActionResult CartMinus(int id) //減
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.qty > 1)
                {
                    data.qty -= 1;
                    data.amount = data.qty * data.price;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleList= "User")]
        public ActionResult Checkout() //付款
        {
            cvmOrdersViewModel models = new cvmOrdersViewModel()
            {
                receive_name = "",
                receive_email = "",
                receive_phone = "",
                receive_address = "",
                payment_no = "",
                shipping_no = "",
                Msg = "",
                remark = "",
                PaymentsList = db.Payments.OrderBy(m => m.mno).ToList(),
                ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList()
            };
           

            var data = db.city.OrderByDescending(m => m.rowid).ToList();
            SelectList citylist = new SelectList(data, "city1", "city1");
            ViewBag.CityList = citylist;
            return View(models);
           
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User")]
        public ActionResult Checkout(cvmOrdersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.PaymentsList == null)
                {
                    model.PaymentsList = db.Payments.OrderBy(m => m.mno).ToList();
                }
                if (model.ShippingsList == null)
                {
                    model.ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList();
                }
                var data = db.city.OrderByDescending(m => m.rowid).ToList();
                SelectList citylist = new SelectList(data, "city1", "city1");
                ViewBag.CityList = citylist;
                return View(model);

            }

            Cart.CartPayment(model);
            if(model.payment_no == "02")
            {
                return Redirect("~/ECPayment.aspx");
            }
            if (model.payment_no == "03")
            {
                //return Redirect("~/ECPayment.aspx");
            }
            return RedirectToAction("cash_on_delivery", new { order_no = Cart.OrderNo });
            
        }

        [LoginAuthorize(RoleList = "User")]
        public ActionResult CheckoutReport(string order_no)
        {
            return View();
        }

        [LoginAuthorize(RoleList = "User")]
        public ActionResult cash_on_delivery(string order_no)
        {
            
            var data = db.Orders.Where(m => m.order_no == order_no).OrderBy(m => m.rowid).ToList();
            string payment_no = data.FirstOrDefault().payment_no;
            string shipping_no = data.FirstOrDefault().shipping_no;
            data.FirstOrDefault().payment_no = db.Payments.Where(m => m.mno == payment_no).FirstOrDefault().mname;
            data.FirstOrDefault().shipping_no = db.Shippings.Where(m => m.mno == shipping_no).FirstOrDefault().mname;
            return View(data);
        }

        //[LoginAuthorize(RoleList = "guest , User")]
        [LoginAuthorize(RoleList = "User")]
        public JsonResult FinishOrder(string email)
        {
            email = email.Replace(" ", "").Replace("\n", "");
            bool result = false;
            using (GmailService mail = new GmailService())
            {
                mail.ReceiveEmail = email;
                mail.Subject = "商品訂購成功";
                mail.Body = "您的商品已訂購成功";
                mail.Send();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinishOrderECpay(string order_no)
        {
            bool result = false;
            var data = db.Orders.Where(m => m.order_no == order_no).FirstOrDefault();
            using (GmailService mail = new GmailService())
            {
                mail.ReceiveEmail = data.user_no;
                mail.Subject = "商品訂購成功";
                mail.Body = "您的商品已訂購成功";
                mail.Send();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}