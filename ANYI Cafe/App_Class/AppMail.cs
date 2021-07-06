using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ANYI_Cafe.Models;

/// <summary>
/// 系統發出電子信件類別
/// </summary>
public class AppMail : BaseClass
{
    /// <summary>
    /// 使用者註冊信件
    /// </summary>
    /// <param name="userEmail">使用者代號</param>
    /// <returns></returns>
    public string UserRegister(string userEmail)
    {
        using (anyicafeEntities db = new anyicafeEntities())
        {
            var input = db.user.Where(m => m.uemail == userEmail).FirstOrDefault();
            if (input == null) return string.Format("查無使用者代號:{0}!!", userEmail);
            if (string.IsNullOrEmpty(input.uemail)) return "使用者電子信箱空白,無法寄出!!";
            using (GmailService gmail = new GmailService())
            {
                var str_url = string.Format("/User/Verify/{0}", input.verifycode);
                var str_link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, str_url);
                string str_subject = string.Format("{0} - 帳號 {1} 成功建立通知!!", AppService.AppName, userEmail);
                string str_body = "<br/><br/>";
                str_body += "很高興告訴您，您的 " + AppService.AppName + " 帳戶已經成功建立。<br/>";
                str_body += "請點擊下方連結以完成您的帳號驗證程序!<br/><br/>";
                str_body += "<a href='" + str_link + "'>" + str_link + "</a> ";
                str_body += "<br/><br/>";
                str_body += "本信件由電腦系統自動寄出，請勿直接回覆!<br/><br/>";
                str_body += string.Format("{0} 祝您有美好安逸的一天!", AppService.AppName);

                gmail.ReceiveEmail = input.uemail;
                gmail.Subject = str_subject;
                gmail.Body = str_body;
                gmail.Send();
                return gmail.MessageText;
            }
        }
    }
}
