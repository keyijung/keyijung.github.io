using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using ANYI_Cafe.Models;

namespace ANYI_Cafe.Controllers
{
    public class OrderController : Controller
    {
        /// <summary>
        /// 未結訂單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult UnClose(int page = 1, int pageSize = 10, string searchText = "")
        {
            return View(GetOrderList("UnClose", page, pageSize, searchText));
        }

        /// <summary>
        /// 已結訂單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Closed(int page = 1, int pageSize = 10, string searchText = "")
        {
            return View(GetOrderList("Closed", page, pageSize, searchText));
        }

        /// <summary>
        /// 全部訂單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AllOrder(int page = 1, int pageSize = 10, string searchText = "")
        {
            return View(GetOrderList("AllOrder", page, pageSize, searchText));
        }

        /// <summary>
        /// 訂單明細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult OrderDetail(int id)
        {
            return View(SetOrderDetailList(GetOrderDetail(id)));
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult OrderStatus(int id)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.Orders.Where(m => m.rowid == id).FirstOrDefault();
                if (model == null) return RedirectToAction("UnClose", "Order");
                List<SelectListItem> status = new List<SelectListItem>();
                var data = db.Status.OrderBy(m => m.mno).ToList();
                foreach(var item in data)
                {
                    SelectListItem ddlb = new SelectListItem();
                    ddlb.Value = item.mno;
                    ddlb.Text = item.mname;
                    if (item.mno == model.order_status)
                        ddlb.Selected = true;
                    else
                        ddlb.Selected = false;
                    status.Add(ddlb);
                }
                ViewBag.StatusNo = status;
                return View(SetOrderDataList(model));
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult OrderStatus(Orders model)
        {
            //不檢查驗證的欄位
            ModelState.Remove("payment_no");
            ModelState.Remove("shipping_no");
            ModelState.Remove("receive_name");
            ModelState.Remove("receive_phone");
            ModelState.Remove("receive_address");

            using (anyicafeEntities db = new anyicafeEntities())
            {
                var data = db.Orders.Where(m => m.rowid == model.rowid).FirstOrDefault();
                if (data != null)
                {
                    data.order_status = model.order_status;
                    //已結案訂單：已領取已付款 , 取消訂單 , 已退貨
                    if (model.order_status == "CP" || model.order_status == "OR" || model.order_status == "RT")
                        data.order_closed = 1;
                    else
                        data.order_closed = 0;

                    //已確認訂單：已領取已付款
                    if (model.order_status == "CP")
                        data.order_validate = 1;
                    else
                        data.order_closed = 0;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                return RedirectToAction("UnClose", "Order");
            }
        }

        /// <summary>
        /// 訂單明細返回訂單列表
        /// </summary>
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult OrderDetailToList()
        {
            if (AppService.OrderType == "UnClose") return RedirectToAction("UnClose", "Order");
            if (AppService.OrderType == "Closed") return RedirectToAction("Closed", "Order");
            return RedirectToAction("AllOrder", "Order");
        }

        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["searchText"];
            return RedirectToAction("UnClose", "Order", new { page = AppService.PageNo, pageSize = AppService.PageSize, searchText = str_text });
        }

        private IPagedList<Orders> GetOrderList(string orderType, int page, int pageSize, string searchText = "")
        {
            AppService.OrderType = orderType;
            AppService.PageNo = page;
            AppService.PageSize = pageSize;
            AppService.SearchText = searchText;
            if (orderType == "UnClose") return SetOrderList(GetUnCloseOrderList());
            if (orderType == "Closed") return SetOrderList(GetClosedOrderList());
            return SetOrderList(GetAllOrderList());
        }

        private IPagedList<Orders> GetUnCloseOrderList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (string.IsNullOrEmpty(AppService.SearchText))
                {
                    var datas1 = db.Orders.Where(m => m.order_closed == 0)
                        .OrderBy(m => m.order_no)
                        .ToPagedList(AppService.PageNo, AppService.PageSize);
                    return datas1;
                }
                var datas2 = db.Orders.Where(model => model.order_closed == 0).Where(m =>
                    m.order_no.Contains(AppService.SearchText) ||
                    m.user_no.Contains(AppService.SearchText) ||
                    m.receive_name.Contains(AppService.SearchText) ||
                    m.receive_email.Contains(AppService.SearchText) ||
                    m.receive_phone.Contains(AppService.SearchText) ||
                    m.receive_address.Contains(AppService.SearchText))
                        .OrderBy(model => model.order_no)
                        .ToPagedList(AppService.PageNo, AppService.PageSize);
                return datas2;
            }
        }
        private IPagedList<Orders> GetClosedOrderList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (string.IsNullOrEmpty(AppService.SearchText))
                {
                    var datas1 = db.Orders.Where(m => m.order_closed == 1)
                        .OrderByDescending(m => m.order_no)
                        .ToPagedList(AppService.PageNo, AppService.PageSize);
                    return datas1;
                }
                var datas2 = db.Orders.Where(model => model.order_closed == 1).Where(m =>
                    m.order_no.Contains(AppService.SearchText) ||
                    m.user_no.Contains(AppService.SearchText) ||
                    m.receive_name.Contains(AppService.SearchText) ||
                    m.receive_email.Contains(AppService.SearchText) ||
                    m.receive_phone.Contains(AppService.SearchText) ||
                    m.receive_address.Contains(AppService.SearchText))
                    .OrderByDescending(model => model.order_no)
                    .ToPagedList(AppService.PageNo, AppService.PageSize);
                return datas2;
            }
        }

        private IPagedList<Orders> GetAllOrderList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (string.IsNullOrEmpty(AppService.SearchText))
                {
                    var datas1 = db.Orders
                        .OrderByDescending(m => m.order_no)
                        .ToPagedList(AppService.PageNo, AppService.PageSize);
                    return datas1;
                }
                var datas2 = db.Orders.Where(m =>
                    m.order_no.Contains(AppService.SearchText) ||
                    m.user_no.Contains(AppService.SearchText) ||
                    m.receive_name.Contains(AppService.SearchText) ||
                    m.receive_email.Contains(AppService.SearchText) ||
                    m.receive_phone.Contains(AppService.SearchText) ||
                    m.receive_address.Contains(AppService.SearchText))
                    .OrderByDescending(model => model.order_no)
                    .ToPagedList(AppService.PageNo, AppService.PageSize);
                return datas2;
            }
        }

        private IPagedList<Orders> SetOrderList(IPagedList<Orders> orders)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (orders.Count > 0)
                {
                    string value1 = ""; string value2 = ""; string value3 = "";
                    for (int i = 0; i < orders.Count; i++)
                    {
                        value1 = orders[i].order_status;
                        value2 = orders[i].payment_no;
                        value3 = orders[i].shipping_no;

                        var data1 = db.Status.Where(m => m.mno == value1).FirstOrDefault();
                        var data2 = db.Payments.Where(m => m.mno == value2).FirstOrDefault();
                        var data3 = db.Shippings.Where(m => m.mno == value3).FirstOrDefault();

                        orders[i].order_status = (data1 == null) ? orders[i].order_status : data1.mname;
                        orders[i].payment_no = (data2 == null) ? orders[i].payment_no : data2.mname;
                        orders[i].shipping_no = (data3 == null) ? orders[i].shipping_no : data3.mname;
                    }
                }
                return orders;
            }
        }

        private OrderDetailViewModel GetOrderDetail(int id)
        {
            OrderDetailViewModel model = new OrderDetailViewModel();
            using (anyicafeEntities db = new anyicafeEntities())
            {
                model.OrderMaster = new Orders();
                model.OrderDetail = new List<OrdersDetail>();

                model.OrderMaster = db.Orders.Where(m => m.rowid == id).FirstOrDefault();
                model.OrderDetail = db.OrdersDetail
                    .Where(m => m.order_no == model.OrderMaster.order_no)
                    .OrderBy(m => m.product_no)
                    .ToList();

                return model;
            }
        }

        private Orders SetOrderDataList(Orders model)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string value1 = model.order_status;
                string value2 = model.payment_no;
                string value3 = model.shipping_no;

                var data1 = db.Status.Where(m => m.mno == value1).FirstOrDefault();
                var data2 = db.Payments.Where(m => m.mno == value2).FirstOrDefault();
                var data3 = db.Shippings.Where(m => m.mno == value3).FirstOrDefault();

                model.order_status = (data1 == null) ? value1 : data1.mname;
                model.payment_no = (data2 == null) ? value2 : data2.mname;
                model.shipping_no = (data3 == null) ? value3 : data3.mname;

                return model;
            }
        }

            private OrderDetailViewModel SetOrderDetailList(OrderDetailViewModel model)
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string value1 = model.OrderMaster.order_status;
                string value2 = model.OrderMaster.payment_no;
                string value3 = model.OrderMaster.shipping_no;

                var data1 = db.Status.Where(m => m.mno == value1).FirstOrDefault();
                var data2 = db.Payments.Where(m => m.mno == value2).FirstOrDefault();
                var data3 = db.Shippings.Where(m => m.mno == value3).FirstOrDefault();

                model.OrderMaster.order_status = (data1 == null) ? value1 : data1.mname;
                model.OrderMaster.payment_no = (data2 == null) ? value2 : data2.mname;
                model.OrderMaster.shipping_no = (data3 == null) ? value3 : data3.mname;

                if (model.OrderDetail.Count > 0)
                {
                    for (int i = 0; i < model.OrderDetail.Count; i++)
                    {
                        string value4 = model.OrderDetail[i].vendor_no;
                        var data4 = db.vendor.Where(m => m.vno == value4).FirstOrDefault();
                        model.OrderDetail[i].vendor_name = (data4 == null) ? value4 : data4.vname;
                    }
                }

                return model;
            }
        }
    }
}