using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ANYI_Cafe.Models;

public class OrderDetailViewModel
{
    public Orders OrderMaster { get; set; }
    public List<OrdersDetail> OrderDetail { get; set; }
}
