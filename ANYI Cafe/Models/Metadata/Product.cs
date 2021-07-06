using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ANYI_Cafe.Models
{
    [MetadataType(typeof(productMetadata))]
    public partial class product
    {
        public class productMetadata
        {
            
            public int id { get; set; }
            [Display(Name = "商品編號")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string product_name { get; set; }
            [Display(Name = "單價")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public Nullable<int> price { get; set; }
            [Display(Name = "商品規格")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string product_spec { get; set; }

            [Display(Name = "縮圖")]
            
            public string product_img { get; set; }

            [Display(Name = "上架與否")]
            public bool product_public { get; set; }
        }


    }
    
}