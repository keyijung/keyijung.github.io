using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANYI_Cafe.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult UploadImage()
        {
            //ImageService.ImageTitle = string.Format("{0} {1} 圖片上傳", UserAccount.UserNo, UserAccount.UserName);
            //ImageService.ImageFolder = "~/Images";
            //ImageService.ImageSubFolder = "User";
            //ImageService.ImageName = UserAccount.UserNo;
            //ImageService.ImageExtention = "jpg";
            //ImageService.UploadImageMode = true;
            return View();
        }

        [HttpPost]
        public ActionResult Uploadimage(HttpPostedFileBase file)
        {
            ImageService.UserUploadimage(file);
            if (!string.IsNullOrEmpty(ImageService.ReturnAreaName))
                return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName, new { area = ImageService.ReturnActionName });
            return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName);
        }
    }
}