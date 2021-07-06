using ANYI_Cafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;

namespace ANYI_Cafe.App_Class
{
    public static class Shop
    {
        public static string ProductNo
        {
            get { return (HttpContext.Current.Session["ProductNo"] == null) ? "" : HttpContext.Current.Session["ProductNo"].ToString(); }
            set { HttpContext.Current.Session["ProductNo"] = value; }
        }

        /// <summary>
        /// 取得商品子分類列表
        /// </summary>
        /// <param name="id">父分類id</param>
        /// <returns></returns>
        public static List<Categorys> GetCategorys(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var lists = db.Categorys
                    .Where(m => m.parentid == id)
                    .OrderBy(m => m.category_no)
                    .ToList();
                return lists;
            }
        }

        public static string GetCategoryName(string cat_no, ref int cat_id)
        {
            cat_id = 0;
            string str_name = "";
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.Categorys.Where(m => m.category_no == cat_no).FirstOrDefault();
                if (model != null)
                {
                    str_name = model.category_name;
                    cat_id = model.rowid;
                }
                return str_name;
            }
        }

        public static string GetProductNo(string name)
        {
            string product_no = "";
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.product.Where(m => m.product_name== name).FirstOrDefault();
                if (model != null)
                {
                    product_no = model.product_no;
                }
                return product_no;
            }
        }

        public static string GetProductPropertyValue(string productNo, string propertyNo)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string str_value = "";
                var model = db.ProductsProperty
                    .Where(m => m.product_no == productNo)
                    .Where(m => m.property_no == propertyNo)
                    .FirstOrDefault();
                if (model != null) str_value = model.text_value;
                return str_value;
            }
        }

        public static List<string> GetProductPropertyValue(string productName)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                List<string> values = new List<string>();
                var data = db.product.Where(m => m.product_name == productName).OrderBy(m => m.id).ToList();
                if (data != null)
                {
                    for(int i = 0; i < data.Count(); i++)
                    {
                        string value = data[i].product_spec;
                        values.Add(value);
                    }
                }
                return values;
            }
        }


        //public static List<SelectListItem> GetPropertyList(string productNo)
        //{
        //    using (anyicafeEntities db = new anyicafeEntities())
        //    {
        //        string str_name = "";
        //        List<SelectListItem> plist = new List<SelectListItem>();
        //        var models = db.ProductsProperty.Where(m => m.product_no == productNo).ToList();
        //        if (models != null)
        //        {
        //            foreach (var item in models)
        //            {
        //                str_name = item.property_no;
        //                var datas = db.Propertys.Where(m => m.mno == item.property_no).FirstOrDefault();
        //                if (datas != null) str_name = datas.mname;
        //                SelectListItem data = new SelectListItem();
        //                data.Text = str_name;
        //                data.Value = item.property_no;
        //                plist.Add(data);
        //            }
        //        }
        //        return plist;
        //    }
        //}
        public static List<SelectListItem> GetPropertyList(string productSpec)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string str_name = "";
                List<SelectListItem> plist = new List<SelectListItem>();
                var models = db.Propertys.Where(m => m.mvalue.Contains(productSpec)).ToList();
                if (models != null)
                {
                    foreach (var item in models)
                    {
                        str_name = item.mno;
                        var datas = db.Propertys.Where(m => m.mno == item.mno).FirstOrDefault();
                        if (datas != null) str_name = datas.mname;
                        SelectListItem data = new SelectListItem();
                        data.Text = str_name;
                        data.Value = item.mno;
                        plist.Add(data);
                    }
                }
                return plist;
            }
        }

        public static int GetParentCategoryList(string categoryNo, ref List<string> list_no, ref List<string> list_name)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                int int_id = 0;
                int int_count = 0;
                var model = db.Categorys.Where(m => m.category_no == categoryNo).FirstOrDefault();
                if (model != null)
                {
                    int_id = model.rowid;
                    while (int_id > 0)
                    {
                        var item = db.Categorys.Where(m => m.rowid == int_id).FirstOrDefault();
                        if (item == null) return int_count;
                        int_count++;
                        list_no.Add(item.category_no);
                        list_name.Add(item.category_name);
                        int_id = item.parentid.GetValueOrDefault();
                    }
                }
                return int_count;
            }
        }

        /// <summary>
        /// 取得商品子分類產品數量
        /// </summary>
        /// <param name="id">父分類id</param>
        /// <returns></returns>
        public static int GetProductCount(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                return db.product.Where(m => m.categoryid == id).Count();
            }
        }

        /// <summary>
        /// 用商品編號取得分類資訊
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <param name="categoryNo">分類編號</param>
        /// <param name="categoryName">分類名稱</param>
        public static void GetCategoryByProductNo(string productNo, ref string categoryNo, ref string categoryName)
        {
            categoryNo = "";
            categoryName = "";
            int int_category_id = 0;
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.product.Where(m => m.product_no == productNo).FirstOrDefault();
                if (model != null && model.categoryid != null)
                { int.TryParse(model.categoryid.ToString(), out int_category_id); }
                if (int_category_id > 0)
                {
                    var categotymodel = db.Categorys.Where(m => m.rowid == int_category_id).FirstOrDefault();
                    if (categotymodel != null)
                    {
                        categoryNo = categotymodel.category_no;
                        categoryName = categotymodel.category_name;
                    }
                }
            }
        }

        public static bool DebugMode { get { return (GetAppConfigValue("DebugMode") == "1"); } }
        /// <summary>
        /// 取得輪播圖片列表
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// 取得 Web.config 中的 App.Config 設定值
        /// </summary>
        /// <param name="keyName">Key 值</param>
        /// <returns></returns>
        public static string GetAppConfigValue(string keyName)
        {
            object obj_value = WebConfigurationManager.AppSettings[keyName];
            return (obj_value == null) ? "" : obj_value.ToString();
        }

        /// <summary>
        /// 取得商品單價
        /// </summary>
        /// <param name="productNo">商品代號</param>
        /// <returns></returns>
        public static int GetProductPrice(string productNo)
        {
            int int_price = 0;
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var models = db.product.Where(m => m.product_no == productNo).FirstOrDefault();
                if (models != null)
                {
                    //if (models.price_discont != null && models.price_discont > 0)
                    //    int.TryParse(models.price_discont.ToString(), out int_price);
                    //else if (models.price_sale != null)
                    //    int.TryParse(models.price_sale.ToString(), out int_price);
                    int.TryParse(models.price.ToString(), out int_price);
                }
            }
            return int_price;
        }

        /// <summary>
        /// 取得商品名稱
        /// </summary>
        /// <param name="productNo">商品代號</param>
        /// <returns></returns>
        public static string GetProductName(string productNo)
        {
            string str_name = "";
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var models = db.product.Where(m => m.product_no == productNo).FirstOrDefault();
                if (models != null) str_name = models.product_name;
            }
            return str_name;
        }

        /// <summary>
        /// 取得商品圖片位址
        /// </summary>
        /// <param name="productNo">商品編號</param>
        /// <returns></returns>
        public static string GetProductImage(string productNo)
        {
            string str_image = string.Format("~/Images/product/{0}/{1}.jpg", productNo, productNo);
            if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
                str_image = string.Format("../../Images/product/{0}/{1}.jpg", productNo, productNo);
            else
                str_image = "../../Images/app/product.jpg";
            return str_image;
        }

        /// <summary>
        /// 訂單 ID
        /// </summary>
        public static int OrderID
        {
            get { return (HttpContext.Current.Session["OrderID"] == null) ? 0 : (int)HttpContext.Current.Session["OrderID"]; }
            set { HttpContext.Current.Session["OrderID"] = value; }
        }
        /// <summary>
        /// 訂單 編號
        /// </summary>
        public static string OrderNo
        {
            get { return (HttpContext.Current.Session["OrderNo"] == null) ? "0" : HttpContext.Current.Session["OrderNo"].ToString(); }
            set { HttpContext.Current.Session["OrderNo"] = value; }
        }


        public static List<product> ProductList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.product.ToList();
                return data;
            }
        }
    }
}