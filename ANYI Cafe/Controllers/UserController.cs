using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ANYI_Cafe.Models;

//RoleList here, goshopping use RoleNo

namespace ANYI_Cafe.Controllers
{
    public class UserController : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserAccount.Logout();
            LoginViewModel model = new LoginViewModel()
            {
                UserNo = "",
                Password = ""
            };
            return View(model);
        }

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model); //登入沒成功，回到Login的model
            using (anyicafeEntities db = new anyicafeEntities()) //登入成功，就要連到db查看帳密是否正確
            {
                using (Cryptographys cryp = new Cryptographys())
                {
                    string str_password = cryp.SHA256Encode(model.Password);
                    var input = db.user
                      .Where(m => m.uemail == model.UserNo) //變數m透過=>就會等於是db.user，lambda概念
                      .Where(m => m.upw == str_password)  //登入時要比對的是使用者輸入經過加密後的密碼
                      .Where(m => m.is_valid == true)  //登入者須是合格的使用者才能登入
                      .FirstOrDefault();
                    if (input == null)
                    {
                        ModelState.AddModelError("UserNo", "帳號或密碼不合!"); //"key","errorMessage"
                        return View(model);
                    }
                    UserAccount.UserNo = model.UserNo;
                    UserAccount.Login();
                    if (input.urole == "A") return RedirectToAction("List", "User");
                    return RedirectToAction("Index", "Home"); //"actionName","controlName"
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            string str_key = Guid.NewGuid()  //this and the following 4 lines are for verification code
                .ToString()
                .Replace("-", "")
                .ToUpper()
                .Substring(0, 20);

            RegisterViewModel model = new RegisterViewModel()
            {
                urole = "U",
                is_valid = false,
                verifycode = str_key  //老師2021031706影片暫時用remark來放驗證碼
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using (anyicafeEntities db = new anyicafeEntities())
            {
                var temp = db.user.Where(m => m.uemail == model.uemail).FirstOrDefault();  //防呆重覆註冊
                if (temp != null)
                {
                    ModelState.AddModelError("", "電郵帳號重覆註冊!");  //"key","errorMessage"
                    return View(model);
                }

                using (Cryptographys cryp = new Cryptographys())
                {

                    user input = new user();
                    input.uno = GetNewUno();
                    input.uemail = model.uemail;
                    input.upw = cryp.SHA256Encode(model.upw);
                    input.uname = model.uname;
                    input.ubirth = model.ubirth;
                    input.utel = model.utel;
                    input.uaddr = model.uaddr;
                    input.urole = "U";
                    input.uremark = model.uremark;
                    input.is_valid = false;
                    input.verifycode = UserAccount.GetNewVarifyCode(); //產生驗證碼
                    db.user.Add(input);

                    db.Configuration.ValidateOnSaveEnabled = false; //確認註冊的鈕按下會出錯，加這句
                    db.SaveChanges();

                    using (AppMail mail = new AppMail())
                    {
                        mail.UserRegister(model.uemail);
                    }

                    TempData["HeaderText"] = "帳號註冊完成";
                    TempData["MessageText"] = "您的帳號已完成註冊，系統會發送一封確認信件，請前往信箱進行確認，謝謝。";
                    return RedirectToAction("MessageText");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Verify(string id)  //id here refers to key
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                bool bln_valid = false;
                TempData["HeaderText"] = "使用者註冊電子信箱驗證";
                var input = db.user.Where(m => m.verifycode == id).FirstOrDefault();
                if (input == null)
                {
                    TempData["MessageText"] = "驗證碼找不到!";
                    return RedirectToAction("MessageText");
                }
                bln_valid = (bool)input.is_valid;
                if (bln_valid)
                {
                    TempData["MessageText"] = "帳號已驗證過，不可重覆驗證!";
                    return RedirectToAction("MessageText");
                }

                input.verifycode = "";
                input.is_valid = true;
                db.Configuration.ValidateOnSaveEnabled = false;  //電郵驗證碼的link按下會出錯，加這句
                db.SaveChanges();
                TempData["MessageText"] = "您的帳號已驗證成功!";
                return RedirectToAction("MessageText");
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                return View(db.user.ToList());
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            user model = new user()
            {
                uemail = "",
                uname = "",
                ubirth = DateTime.Today
            };
            return View(model);
        }
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(user model)
        {
            ModelState.Remove("uno");
            ModelState.Remove("upw");
            ModelState.Remove("urole");
            ModelState.Remove("is_valid");
            ModelState.Remove("verifycode");
            if (!ModelState.IsValid) return View(model);

            using (Cryptographys cryp = new Cryptographys())
            {
                using (anyicafeEntities db = new anyicafeEntities())
                {
                    user input = new user();
                    input.uno = GetNewUno();
                    input.uemail = model.uemail;
                    input.upw = cryp.SHA256Encode(model.uno);
                    input.uname = model.uname;
                    input.ubirth = model.ubirth;
                    input.utel = model.utel;
                    input.uaddr = model.uaddr;
                    input.urole = "U";
                    input.uremark = model.uremark;

                    db.user.Add(input);
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id) //Edit and id here refer to the ones in User/List's「@Html.ActionLink("修改", "Edit", "User", new { id = item.uid }」
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.user.Where(m => m.uid == id).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(user model)
        {
            ModelState.Remove("upw");
            ModelState.Remove("urole");
            ModelState.Remove("is_valid");
            ModelState.Remove("verifycode");
            if (!ModelState.IsValid) return View(model);

            using (anyicafeEntities db = new anyicafeEntities())
            {
                //自定義檢查
                if (model.uno.Substring(0, 1) != "U") { ModelState.AddModelError("uno", "格式錯誤!!"); return View(model); }
                if (model.uno.Length != 5) { ModelState.AddModelError("uno", "格式錯誤!!"); return View(model); }
                var check1 = db.user.Where(m => m.uid != model.uid && m.uno == model.uno).FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("uno", "編號重覆!!"); return View(model); }
                var check2 = db.user.Where(m => m.uid != model.uid && m.uemail == model.uemail).FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("uemail", "電子信箱重覆!!"); return View(model); }

                var input = db.user.Where(m => m.uid == model.uid).FirstOrDefault();
                input.uno = model.uno;
                input.uemail = model.uemail;
                input.uname = model.uname;
                input.ubirth = model.ubirth;
                input.utel = model.utel;
                input.uaddr = model.uaddr;
                input.uremark = model.uremark;

                db.SaveChanges();
                return RedirectToAction("List", "User");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id) //Delete and id here refer to the ones in User/List's「@Html.ActionLink("刪除", "Delete", "User", new { id = item.uid }」
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.user.Where(m => m.uid == id).FirstOrDefault();
                if (model != null)
                {
                    db.user.Remove(model);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "User");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult ResetPassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            return View(model);
        }
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                using (anyicafeEntities db = new anyicafeEntities())
                {
                    string str_password = cryp.SHA256Encode(model.NewPassword);
                    var data = db.user.Where(m => m.uemail == UserAccount.UserNo).FirstOrDefault();
                    if (data != null)
                    {
                        data.upw = str_password;
                        db.Configuration.ValidateOnSaveEnabled = false;  //密碼變更的link按下會出錯，加這句
                        db.SaveChanges();

                    }
                    //TempData["HeaderText"] = "密碼變更完成";
                    TempData["MessageText"] = "密碼已變更，下次登入請使用新密碼哦!";
                    return RedirectToAction("MessageText");
                }
            }
        }
        [HttpGet]
        public ActionResult MessageText()
        {
            //ViewBag.HeaderText = TempData["HeaderText"].ToString();
            ViewBag.MessageText = TempData["MessageText"].ToString();
            return View();
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult UserProfileEdit()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var model = db.user.Where(m => m.uemail == UserAccount.UserNo).FirstOrDefault();
                return View(model);
            }
        }
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult UserProfileEdit(user model)
        {
            if (!ModelState.IsValid) return View(model);
            using (anyicafeEntities db = new anyicafeEntities())
            {
                var input = db.user.Where(m => m.uemail == UserAccount.UserNo).FirstOrDefault();
                input.uname = model.uname;
                input.ubirth = model.ubirth;
                input.utel = model.utel;
                input.uaddr = model.uaddr;

                db.SaveChanges();
                return View();
            }
        }


        /// <summary>
        /// 取得新的會編號
        /// </summary>
        /// <returns></returns>
        private string GetNewUno()
        {
            using (anyicafeEntities db = new anyicafeEntities())
            {
                string str_uno = "U0001";
                var data = db.user
                    .Where(m => m.urole == "U")
                    .OrderByDescending(m => m.uno)
                    .FirstOrDefault();
                if (data != null) str_uno = "U" + (int.Parse(data.uno.Substring(1, 4)) + 1).ToString().PadLeft(4, '0');
                return str_uno;
            }
        }
    }
}

