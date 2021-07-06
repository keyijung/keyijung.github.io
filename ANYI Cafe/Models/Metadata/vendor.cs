using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    [MetadataType(typeof(vendorMetaData))]
    public partial class vendor
    {
        private class vendorMetaData
        {
            [Key]
            public int vid { get; set; }

            [Display(Name = "廠商編號")]
            public string vno { get; set; }

            [Display(Name = "廠商名稱")]
            [Required(ErrorMessage = "請輸入廠商名稱!")]
            public string vname { get; set; }

            [Display(Name = "統一編號")]
            public string vtaxno { get; set; }

            [Display(Name = "廠商電話")]
            public string vtel { get; set; }

            [Display(Name = "電子信箱")]
            [EmailAddress(ErrorMessage = "請輸入正確格式的電子信箱!")]
            public string vemail { get; set; }

            [Display(Name = "廠商地址")]
            public string vaddr { get; set; }

            [Display(Name = "廠商等級")]
            public string vrate { get; set; }

            [Display(Name = "備註")]
            public string vremark { get; set; }
        }
    }
}