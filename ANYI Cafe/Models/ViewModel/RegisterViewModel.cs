using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int uid { get; set; }
   
        public string uno { get; set; }

        [Display(Name = "帳號")]
        [EmailAddress(ErrorMessage = "請輸入正確格式的電子信箱!")]
        [Required(ErrorMessage = "請輸入正確格式的電子信箱!")]
        public string uemail { get; set; }

        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入密碼!")]
        public string upw { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "請輸入姓名!")]
        public string uname { get; set; }

        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "請輸入出生日期!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<System.DateTime> ubirth { get; set; }

        [Display(Name = "手機號碼")]
        [Required(ErrorMessage = "請輸入手機號碼!")]
        public string utel { get; set; }

        [Display(Name = "送貨地址")]
        [Required(ErrorMessage = "請輸入送貨地址!")]
        public string uaddr { get; set; }

        public string urole { get; set; }

        public string uremark { get; set; }

        public bool is_valid { get; set; }

        public string verifycode { get; set; }
    }
}