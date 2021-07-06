using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 自定義權限 Filter
/// </summary>
public class LoginAuthorize : AuthorizeAttribute
{
    public string RoleList { get; set; } = "";
    /// <summary>
    /// 覆寫 Authorize 設定
    /// </summary>
    /// <param name="httpContext">httpContext</param>
    /// <returns>驗證結果</returns>
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        //除錯模式
        if (AppService.DebugMode) return true;

        //未限制角色不檢查權限
        if (string.IsNullOrEmpty(RoleList)) return true;  //RoleList here, goshopping use RoleNo

        //檢查登入者角色是否包含在限制的角色中
        bool bln_authorized = false;
        List<string> lists = RoleList.Split(',').ToList();  //RoleList here, goshopping use RoleNo
        if (lists.Count > 0)
        {
            foreach (string role in lists)
            {
                if (UserAccount.RoleName == role) { bln_authorized = true; break; }
            }
        }
        return bln_authorized;
    }

    /// <summary>
    /// 驗證不通過時返回登入頁面
    /// </summary>
    /// <param name="filterContext"></param>
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        //預設為 IIS 位址,加入虛擬目錄名稱anyicafe
        string str_route = "/anyicafe/User/Login";
        string str_host = HttpContext.Current.Request.Url.Host.ToLower();
        //如果為本機(localhost)測試,不用加虛擬目錄名稱 anyicafe
        if (str_host.Contains("localhost")) str_route = "/User/Login";
        //返回登入頁
        filterContext.Result = new RedirectResult(str_route);
    }

}
