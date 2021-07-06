using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class cash_on_deliveryViewModel
    {
        public string order_name { get; set; }
        public string order_mail { get; set; }
        public string addressee_name { get; set; }
        public string addressee_address { get; set; }
        public string addressee_phone { get; set; }
        public string payment_no { get; set; }
        public string shipping_no { get; set; }
        public string remark { get; set; }

    }
}