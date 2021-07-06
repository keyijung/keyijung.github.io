using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ANYI_Cafe.App_Class;
using ANYI_Cafe.Models;

/// <summary>
/// 使用者資訊類別
/// </summary>
public static class UserAccount
{
    /// <summary>
    /// 登入使用者角色
    /// </summary>
    public static EnumList.LoginRole Role { get; set; } = EnumList.LoginRole.Guest;
    /// <summary>
    /// 登入使用者角色名稱
    /// </summary>
    public static string RoleName { get { return EnumList.GetRoleName(Role); } }
    /// <summary>
    /// 使用者帳號
    /// </summary>
    public static string UserNo { get; set; } = "";
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public static string UserName { get; set; } = "";
    /// <summary>
    /// 使用者電子信箱
    /// </summary>
    public static string UserEmail { get; set; } = "";
    /// <summary>
    /// 使用者是否已登入
    /// </summary>
    public static bool IsLogin { get; set; } = false;

    public static void Login()
    {
        using (anyicafeEntities db = new anyicafeEntities())
        {
            var input = db.user.Where(m => m.uemail == UserNo).FirstOrDefault();
            if (input == null)
                Logout();
            else
            {
                IsLogin = true;
                UserNo = input.uemail;
                UserName = input.uname;
                Cart.LoginCart();
                Role = EnumList.GetRoleType(input.urole);
            }
        }
    }

    public static void Logout()
    {
        IsLogin = false;
        Role = EnumList.LoginRole.Guest;
        UserNo = "";  //就是email
        UserName = "";
        //UserEmail = "";
    }

    public static string GetNewVarifyCode()
    {
        return Guid.NewGuid().ToString().ToUpper(); //產生驗證碼
    }


    //refer to goshopping
    public static int UserStatus
    {
        get
        {
            int int_value = 0;
            if (HttpContext.Current.Session["UserStatus"] != null)
            {
                string str_value = HttpContext.Current.Session["UserStatus"].ToString();
                if (!int.TryParse(str_value, out int_value)) int_value = 0;
            }
            return int_value;
        }
        set
        { HttpContext.Current.Session["UserStatus"] = value; }
    }

    //refer to goshopping
    public static int UserCode
    {
        get
        {
            int int_value = -1;
            if (HttpContext.Current.Session["UserCode"] != null)
            {
                string str_value = HttpContext.Current.Session["UserCode"].ToString();
                if (!int.TryParse(str_value, out int_value)) int_value = -1;
            }
            return int_value;
        }
        set
        { HttpContext.Current.Session["UserCode"] = value; }
    }
}
