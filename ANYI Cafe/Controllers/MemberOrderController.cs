using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using ANYI_Cafe.Models;

namespace ANYI_Cafe.Controllers
{
    public class MemberOrderController : Controller
    {
        /// <summary>
        /// 全部訂單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(RoleList = "User")]
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
        [LoginAuthorize(RoleList = "User")]
        public ActionResult OrderDetail(int id)
        {
            return View(SetOrderDetailList(GetOrderDetail(id)));
        }

        /// <summary>
        /// 訂單明細返回訂單列表
        /// </summary>
        [HttpGet]
        [LoginAuthorize(RoleList = "User")]
        public ActionResult OrderDetailToList()
        {
            //if (AppService.OrderType == "UnClose") return RedirectToAction("UnClose", "Order");
            //if (AppService.OrderType == "Closed") return RedirectToAction("Closed", "Order");
            return RedirectToAction("AllOrder", "MemberOrder");
        }

        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["searchText"];
            return RedirectToAction("AllOrder", "MemberOrder", new { page = AppService.PageNo, pageSize = AppService.PageSize, searchText = str_text });
        }

        private IPagedList<Orders> GetOrderList(string orderType, int page, int pageSize, string searchText = "")
        {
            AppService.OrderType = orderType;
            AppService.PageNo = page;
            AppService.PageSize = pageSize;
            AppService.SearchText = searchText;
            //if (orderType == "UnClose") return SetOrderList(GetUnCloseOrderList());
            //if (orderType == "Closed") return SetOrderList(GetClosedOrderList());
            return SetOrderList(GetAllOrderList());
        }

        private IPagedList<Orders> GetAllOrderList()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                if (string.IsNullOrEmpty(AppService.SearchText))
                {
                    var datas1 = db.Orders.Where(m => m.user_no == UserAccount.UserNo)  //.Where(m => m.user_no == UserAccount.UserNo)可讓會員進來訂單查詢只有他自己的單
                        .OrderByDescending(m => m.order_no)
                        .ToPagedList(AppService.PageNo, AppService.PageSize);
                    return datas1;
                }
                var datas2 = db.Orders.Where(m => m.user_no == UserAccount.UserNo)  //.Where(m => m.user_no == UserAccount.UserNo)可讓查詢結果只出現他自己的單
                    .Where(m =>
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

                return model;
            }
        }
    }
}
