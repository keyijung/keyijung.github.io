using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class cvmOrdersViewModel
    {

        [Display(Name = "付款人姓名 :")]
        [Required(ErrorMessage = "付款人姓名不可空白")]
        public string receive_name { get; set; }
        [Display(Name = "付款人信箱 :")]
        [Required(ErrorMessage = "付款人信箱不可空白")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
        public string receive_email { get; set; }
        [Display(Name = "付款人電話 :")]
        [Required(ErrorMessage = "付款人電話不可空白")]
        public string receive_phone { get; set; }
        [Display(Name = "付款人地址 :")]
        [Required(ErrorMessage = "付款人地址不可空白")]
        public string receive_address { get; set; }
        [Display(Name = "付款方式 :")]
        [Required(ErrorMessage = "付款方式不可空白")]
        public string payment_no { get; set; }
        [Display(Name = "運送方式 :")]
        [Required(ErrorMessage = "運送方式不可空白")]
        public string shipping_no { get; set; }
        //[Display(Name = "選擇取貨地點 :")]
        //[Required(ErrorMessage = "取貨地點不可空白")]
        //public string receipt_place { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Road { get; set; }

        public string Msg { get; set; }
        [Display(Name = "收件人姓名 :")]
        [Required(ErrorMessage = "收件人姓名不可空白")]
        public string addressee_name { get; set; }

        [Display(Name = "收件人電話 :")]
        [Required(ErrorMessage = "收件人電話不可空白")]
        public string addressee_phone { get; set; }
        [Display(Name = "備註 :")]
        public string remark { get; set; }

        public List<Payments> PaymentsList { get; set; }
        public List<Shippings> ShippingsList { get; set; }
    }
}