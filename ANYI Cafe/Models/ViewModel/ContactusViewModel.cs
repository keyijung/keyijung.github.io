using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ANYI_Cafe.Models
{

    public class ContactusViewModel
    {
        [Required(ErrorMessage = "請填入姓名!")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "請輸入正確的電子信箱!")]
        [Required(ErrorMessage = "請填入正確的電子信箱!")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "請填寫內容!")]
        public string Message { get; set; }
    }
}