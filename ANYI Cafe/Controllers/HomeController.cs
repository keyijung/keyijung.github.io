using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ANYI_Cafe.App_Class;
using ANYI_Cafe.Models;

namespace ANYI_Cafe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //public ActionResult Contact()
        //{
        //    //ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Order(string category_no)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                category_no = category_no == "" ? null : category_no;
                var model = db.product.Where(m => m.product_public == true).OrderBy(m => m.product_no).ToList();
                if (category_no != null) model = model.Where(m => m.category_name == category_no).ToList();
                return View(model);
            }

        }

        public ActionResult Aboutus()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ContactusViewModel model = new ContactusViewModel()
            {
                Name = "",
                Email = "",
                Subject = "",
                Message = ""
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Contact(ContactusViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (GmailService mail = new GmailService())
            {
                mail.ReceiveEmail = WebConfigurationManager.AppSettings["GamilSenderEmail"].ToString();
                mail.Subject = model.Subject;
                string membermail = model.Email;
                string content = model.Message;
                mail.Body = $"{membermail} \n {content}";
                mail.Send();
                TempData["HeaderText"] = "訊息發送成功!";
                TempData["MessageText"] = "謝謝您的寶貴意見!";


                return RedirectToAction("Messagepage");
            }

        }

        public ActionResult MessagePage()
        {
            ViewBag.HeaderText = TempData["HeaderText"];
            ViewBag.MessageText = TempData["MessageText"];
            return View();
        }



        public JsonResult GetCategoryMenuList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                int int_count = 0;
                string str_text = "";
                string str_href = "";
                List<Node> models = new List<Node>();

                var node1 = Shop.GetCategorys(0);
                if (node1.Count > 0)
                {
                    foreach (var item1 in node1)
                    {
                        int_count = Shop.GetProductCount(item1.rowid);
                        str_text = item1.category_name;
                        str_href = string.Format("/Product/CategoryList/{0}", item1.category_no);
                        Node model1 = new Node();
                        model1.text = str_text;

                        var node2 = Shop.GetCategorys(item1.rowid);
                        if (node2.Count == 0 && int_count > 0) model1.href = str_href;

                        if (node2.Count > 0)
                        {
                            foreach (var item2 in node2)
                            {
                                int_count = Shop.GetProductCount(item2.rowid);
                                str_text = item2.category_name;
                                str_href = string.Format("/Home/Order?category_no={0}", item2.category_no);
                                Node model2 = new Node();
                                model2.text = str_text;
                                if (int_count == 0) model2.href = str_href;
                                //var node3 = Shop.GetCategorys(item2.rowid);
                                //if (node3.Count == 0 && int_count > 0) model2.href = str_href;

                                //if (node3.Count > 0)
                                //{
                                //    foreach (var item3 in node3)
                                //    {
                                //        int_count = Shop.GetProductCount(item3.rowid);
                                //        str_text = item3.category_name;
                                //        str_href = string.Format("/Product/CategoryList/{0}", item3.category_no);
                                //        Node model3 = new Node();
                                //        model3.text = str_text;
                                //        if (int_count > 0) model3.href = str_href;

                                //        model2.nodes.Add(model3);
                                //    }
                                //}
                                model1.nodes.Add(model2);
                            }
                        }
                        models.Add(model1);
                    }
                }

                var jdata = Json(models, JsonRequestBehavior.AllowGet);
                return jdata;
            }
        }



    }
}