using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{
    public class LoginViewModel
    {
        //[Display(Name ="")]
        [Required(ErrorMessage ="請輸入帳號!")]
        //[StringLength(10, ErrorMessage ="帳號長度不可超過10個字!")]
        public string UserNo { get; set; }

        //[Display(Name = "")]
        [Required(ErrorMessage = "請輸入密碼!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "記得我")]
        //public bool Remember { get; set; }
    }
}