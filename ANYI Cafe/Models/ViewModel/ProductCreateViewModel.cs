using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class ProductCreateViewModel
    {
        public int id { get; set; }
        public Nullable<int> categoryid { get; set; }
        public string category_name { get; set; }

        [Display(Name = "商品編號")]
        [Required(ErrorMessage = "此內容不可空白!")]
        public string product_no { get; set; }

        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "此內容不可空白!")]
        public string product_name { get; set; }

        [Display(Name = "單價")]
        [Required(ErrorMessage = "此內容不可空白!")]
        public Nullable<int> price { get; set; }


        public string product_specopt { get; set; }

        [Display(Name = "商品規格")]
        [Required(ErrorMessage = "此內容不可空白!")]
        public string product_spec { get; set; }

        
        public string product_img { get; set; }

        [Display(Name = "上架與否")]
        public bool product_public { get; set; }
    }
}