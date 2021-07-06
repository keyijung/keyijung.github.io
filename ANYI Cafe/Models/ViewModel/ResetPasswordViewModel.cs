using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class ResetPasswordViewModel
    {
        //[Display(Name = "新密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入新密碼!")]
        public string NewPassword { get; set; }
        //[Display(Name = "確認新密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請確認新密碼!")]
        [Compare("NewPassword", ErrorMessage = "密碼不符請確認!")]
        public string ConfirmPassword { get; set; }
    }
}