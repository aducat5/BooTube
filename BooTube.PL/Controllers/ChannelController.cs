using BooTube.BLL;
using BooTube.BLL.Repos;
using BooTube.DAL;
using BooTube.PL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Controllers
{
    public class ChannelController : Controller
    {
        ChannelRepo cr = new ChannelRepo();

        [UserAuth]
        public ActionResult ViewChannels()
        {
            if (TempData["islemSonucu"] != null)
                ViewBag.Sonuc = TempData["islemSonucu"];
            
            if (Session["user"] != null)
            {
                User current = Session["user"] as User;
                return View(cr.GetChannelsByID(current.UserID, false, false));
            }
            else
            {
                return RedirectToAction("NotFound", "Home");
            }

        }

        [UserAuth]
        public ActionResult ViewFrozenChannels()
        {
            if (TempData["islemSonucu"] != null)
                ViewBag.Sonuc = TempData["islemSonucu"];

            if (Session["user"] != null)
            {
                User current = Session["user"] as User;
                return View(cr.GetChannelsByID(current.UserID, false, true));
            }
            else
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        [UserAuth]
        public ActionResult NewChannel() => View();

        [UserAuth, HttpPost]
        public ActionResult NewChannel(Channel newChannel, HttpPostedFileBase imgChannelProfilePicture, HttpPostedFileBase imgChannelCoverPicture)
        {
            if (imgChannelProfilePicture != null)
            {
                bool isUploaded = false;

                string uploadedFilePath = GenFunx.ImageUpload(imgChannelProfilePicture, "~/Content/Images/ChannelPictures", out isUploaded);

                if (isUploaded)
                {
                    isUploaded = false;

                    Image thumbImage = GenFunx.ResizeImage(uploadedFilePath, new Size() { Height = 200, Width = 200 });
                    string uploadedThumbPath = GenFunx.ImageUpload(thumbImage, "~/Content/Images/Thumbnails", uploadedFilePath.Split('/').Last(), out isUploaded);

                    if (isUploaded)
                        newChannel.ChannelProfilePicture = uploadedFilePath.Split('/').Last();
                }
            }

            if (imgChannelCoverPicture != null)
            {
                bool isUploaded = false;

                string uploadedCoverPath = GenFunx.ImageUpload(imgChannelCoverPicture, "~/Content/Images/ChannelCovers", out isUploaded);

                if (isUploaded)
                {
                    isUploaded = false;

                    Image thumbCover = GenFunx.ResizeImage(uploadedCoverPath, new Size() { Height = 200, Width = 256 });
                    string uploadedThumbPath = GenFunx.ImageUpload(thumbCover, "~/Content/Images/Thumbnails", uploadedCoverPath.Split('/').Last(), out isUploaded);

                    if (isUploaded)
                        newChannel.ChannelCoverPicture = uploadedCoverPath.Split('/').Last();
                }
            }

            if (cr.InsertChannel(newChannel, out string islemSonucu))
            {
                return RedirectToAction("ViewChannels");
            }
            else
            {
                ViewBag.Hata = islemSonucu;
                return View();
            }
        }

        [UserAuth]
        public ActionResult DeleteChannel(int id)
        {
            if (cr.IsUserOwnerOfChannel(((User)Session["user"]).UserID, id) && cr.DeleteChannel(id, out string islemSonucu))
                TempData["islemSonucu"] = "Silme işlemi başarılı";
            else
                TempData["islemSonucu"] = "Silme işlemi başarısız";

            return RedirectToAction("ViewChannels");
        }


        [UserAuth]
        public ActionResult FreezeChannel(int id)
        {
            if (cr.IsUserOwnerOfChannel(((User)Session["user"]).UserID, id) && cr.FreezeChannel(id, out string islemSonucu))
                TempData["islemSonucu"] = "Askıya alma işlemi başarılı";
            else
                TempData["islemSonucu"] = "Askıya alma işlemi başarısız";

            return RedirectToAction("ViewChannels");
        }

        public ActionResult ViewChannel(int id = 0)
        {
            Channel ch = cr.GetChannelByID(id, false);
            if (id > 0 && ch != null)
            {
                User current = Session["user"] as User;
                if (current != null && cr.IsUserFollowing(ch.ChannelID, current.UserID))
                    ViewBag.Follow = "true";
                else
                    ViewBag.Follow = "false";

                return View(cr.GetChannelByID(id, false));
            }
            else
                return RedirectToAction("NotFound", "Home");
        }

        [UserAuth]
        public ActionResult EditChannel(int id = 0)
        {
            Channel ch = cr.GetChannelByID(id, false);
            if (ch != null && ch.ChannelOwnerID == (Session["user"] as User).UserID)
            {
                return View(ch);
            }
            else
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        [UserAuth, HttpPost]
        public ActionResult EditChannel(Channel chToUp, HttpPostedFileBase imgChannelProfilePicture, HttpPostedFileBase imgChannelCoverPicture)
            {
            chToUp.CreateTime = DateTime.Now;
            chToUp.User = Session["user"] as User;
            if (imgChannelProfilePicture != null)
            {
                bool isUploaded = false;

                string uploadedFilePath = GenFunx.ImageUpload(imgChannelProfilePicture, "~/Content/Images/ChannelPictures", out isUploaded);

                if (isUploaded)
                {
                    isUploaded = false;

                    Image thumbImage = GenFunx.ResizeImage(uploadedFilePath, new Size() { Height = 200, Width = 200 });

                    string uploadedThumbPath = GenFunx.ImageUpload(thumbImage, "~/Content/Images/Thumbnails", uploadedFilePath.Split('/').Last(), out isUploaded);

                    if (isUploaded)
                        chToUp.ChannelProfilePicture = uploadedFilePath.Split('/').Last();
                }
                else
                {
                    ViewBag.Hata = "Profil resmi karşıya yüklenemedi";
                    return View();
                }
            }

            if (imgChannelCoverPicture != null)
            {
                bool isUploaded = false;

                string uploadedCoverPath = GenFunx.ImageUpload(imgChannelCoverPicture, "~/Content/Images/ChannelCovers", out isUploaded);

                if (isUploaded)
                {
                    isUploaded = false;

                    Image thumbCover = GenFunx.ResizeImage(uploadedCoverPath, new Size() { Height = 200, Width = 256 });

                    string uploadedThumbPath = GenFunx.ImageUpload(thumbCover, "~/Content/Images/Thumbnails", uploadedCoverPath.Split('/').Last(), out isUploaded);

                    if (isUploaded)
                        chToUp.ChannelCoverPicture = uploadedCoverPath.Split('/').Last();
                }
                else
                {
                    ViewBag.Hata = "Kapak resmi karşıya yüklenemedi";
                    return View();
                }
            }

            if (cr.EditChannel(chToUp, out string islemSonucu))
            {
                return RedirectToAction("ViewChannels");
            }
            else
            {
                ViewBag.Hata = islemSonucu;
                return View(chToUp);
            }
        }

        [UserAuth]
        public void FollowChannel(int channelID, int userID)
        {
            cr.FollowChannel(channelID, userID);
        }
        [UserAuth]
        public void UnfollowChannel(int channelID, int userID)
        {
            cr.UnfollowChannel(channelID, userID);
        }

        [UserAuth]
        public PartialViewResult GetFollowedChannels()
        {
            User u = Session["user"] as User;
            List<Channel> followedChannels = cr.GetFollowedChannels(u.UserID);
            return PartialView("~/Views/Shared/_FollowedChannelsPartial.cshtml", followedChannels);
        }
    }
}