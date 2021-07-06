using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    [MetadataType(typeof(PropertysMetadata))]
    public partial class Propertys
    {
        public class PropertysMetadata
        {
            public int rowid { get; set; }
            [Display(Name = "編號")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string mno { get; set; }

            [Display(Name = "名稱")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string mname { get; set; }

            [Display(Name = "屬性值")]
            [Required(ErrorMessage = "此內容不可空白!")]
            public string mvalue { get; set; }


            [Display(Name = "備註")]
            //[Required(ErrorMessage = "此內容不可空白!")]
            public string remark { get; set; }


        }

    }
}