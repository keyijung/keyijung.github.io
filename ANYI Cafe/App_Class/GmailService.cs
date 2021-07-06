using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Web.Configuration;

/// <summary>
/// Gmail 寄信類別
/// </summary>
public class GmailService : BaseClass
{
    #region 建構子
    /// <summary>
    /// Gmail 建構子
    /// </summary>
    public GmailService()
    {
        MessageText = "";
        SenderName = WebConfigurationManager.AppSettings["GamilSenderName"].ToString();
        SenderEmail = WebConfigurationManager.AppSettings["GamilSenderEmail"].ToString();
        AppPassword = WebConfigurationManager.AppSettings["GamilAppPassword"].ToString();
        ReceiveEmail = "";
        Subject = "";
        Body = "";
        HostUrl = "smtp.gmail.com";
        HostPort = 587;
        UseSSL = true;
    }
    #endregion
    #region 屬性
    /// <summary>
    /// 訊息文字
    /// </summary>
    public string MessageText { get; set; }
    /// <summary>
    /// 寄件者姓名
    /// </summary>
    public string SenderName { get; set; }
    /// <summary>
    /// 寄件者 (Google 帳號)
    /// </summary>
    public string SenderEmail { get; set; }
    /// <summary>
    /// Google 帳號應用程式密碼
    /// </summary>
    public string AppPassword { get; set; }
    /// <summary>
    /// 收件者 Email
    /// </summary>
    public string ReceiveEmail { get; set; }
    /// <summary>
    /// 信件主旨
    /// </summary>
    public string Subject { get; set; }
    /// <summary>
    /// 信件內文
    /// </summary>
    public string Body { get; set; }
    /// <summary>
    /// 寄件伺服器位址
    /// </summary>
    public string HostUrl { get; set; }
    /// <summary>
    /// 通訊連接埠號碼
    /// </summary>
    public int HostPort { get; set; }
    /// <summary>
    /// 啟用 SSL 機制
    /// </summary>
    public bool UseSSL { get; set; }
    #endregion
    #region 事件
    /// <summary>
    /// 送出信件
    /// </summary>
    public void Send()
    {
        var fromEmail = new MailAddress(SenderEmail, SenderName);
        var toEmail = new MailAddress(ReceiveEmail);
        try
        {
            var smtp = new SmtpClient
            {
                Host = HostUrl,
                Port = HostPort,
                EnableSsl = UseSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, AppPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        catch (Exception ex)
        {
            MessageText = ex.Message;
        }
    }
    #endregion
}
