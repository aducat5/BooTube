using BooTube.BLL;
using BooTube.BLL.Repos;
using BooTube.DAL;
using BooTube.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Controllers
{
    public class PostController : Controller
    {
        PostRepo pr = new PostRepo();
        VideoRepo vr = new VideoRepo();
        AddonRepo ar = new AddonRepo();
        ChannelRepo cr = new ChannelRepo();
        CommentRepo comr = new CommentRepo();
        TagRepo tr = new TagRepo();

        public ActionResult ViewPost(int id = 0)
        {
            Post post = pr.GetPost(id, false);
            if (post != null)
            {
                User user = Session["user"] as User;
                if (user != null)
                {
                    if (cr.IsUserFollowing(post.ChannelID, user.UserID))
                        ViewBag.Follow = "true";
                    else
                        ViewBag.Follow = "false";

                    if (ar.IsAddonThere(post.PostID, user.UserID, 2, false))
                        ViewBag.Like = "true";
                    else
                        ViewBag.Like = "false";

                    if (ar.IsAddonThere(post.PostID, user.UserID, 4, false))
                        ViewBag.Dislike = "true";
                    else
                        ViewBag.Dislike = "false";

                    ar.AddClick(id, user.UserID);
                }
                else
                {
                    ar.AddClick(id);
                }
                
                return View(post);
            }
            else
                return RedirectToAction("NotFound", "Home");
        }
        
        [UserAuth]
        public ActionResult NewPost()
        {
            return View();
        }

        [HttpPost, UserAuth]
        public ActionResult NewPost(HttpPostedFileBase video, Post newPost, int channelID, string postTags)
        {
            try
            {
                ChannelRepo cr = new ChannelRepo();
                PostRepo pr = new PostRepo();
                VideoRepo vr = new VideoRepo();
                User currentUser = Session["user"] as User;

                if (cr.IsUserOwnerOfChannel(currentUser.UserID, channelID))
                {
                    if (newPost != null && !(string.IsNullOrEmpty(newPost.PostTitle)) && video != null)
                    {

                        string uploadedVideo = GenFunx.VideoUpload(video, "~/Content/Videos/Temporary", out bool isUploaded);
                        if (isUploaded)
                        {
                            string relocatedVideo = GenFunx.RelocateVideo(uploadedVideo, out bool isCopied);
                            if (isCopied)
                            {
                                string thumbnailPath = GenFunx.GetThumbnailFromVideo(relocatedVideo, out bool isDone);
                                if (isDone)
                                {
                                    newPost.ChannelID = channelID;
                                    newPost.PostThumbnailPath = thumbnailPath.Split('/').Last();

                                    bool sonuc = pr.InsertPost(newPost, out string islemSonucu);

                                    if (sonuc)
                                    {
                                        Video newVideo = GenFunx.GetVideoFromMediaFile(relocatedVideo);
                                        //TODO: Farklı boyutlara convert etme işlemi burada olacak. relocatedVideo işle işlem yapılacak.
                                        //TODO: Transaction yapısı include edilecek
                                        newVideo.PostID = newPost.PostID;
                                        bool vidOK = vr.InsertVideo(newVideo, out islemSonucu);
                                        if (vidOK)
                                        {
                                            ViewBag.Sonuc = "Yeni video başarıyla yüklendi";
                                            pr.AddTagsToPost(postTags, newPost);
                                        }
                                        else ViewBag.Sonuc = "Video yüklenirken hata oldu: " + islemSonucu;
                                    }
                                    else ViewBag.Sonuc = "Kayıt sırasında bir hata meydana geldi: " + islemSonucu;
                                }
                                else ViewBag.Sonuc = "Küçük resim alındamadı: " + thumbnailPath;
                            }
                            else ViewBag.Sonuc = "Yükleme sırasında bir hata oluştu: " + relocatedVideo;
                        }
                        else ViewBag.Sonuc = "Yükleme sırasında bir hata oluştu: " + uploadedVideo;
                    }
                    else ViewBag.Sonuc = "Video nesnesi veya başlığı boş bırakılamaz";
                }
                else ViewBag.Sonuc = "Bu kanalı kullanarak video yükleyemezsiniz";
            }
            catch (Exception ex)
            {
                ViewBag.Sonuc = ex.Message;
            }

            return View();
        }

        [UserAuth]
        public int LikePost(int postID)
        {
            User current = Session["user"] as User;
            AddonRepo ar = new AddonRepo();
            int likeCount = ar.AddLike(postID, current.UserID);

            if (likeCount == 0)
                likeCount = ar.SetAddonState(postID, current.UserID, 2, false);

            if (ar.IsAddonThere(postID, current.UserID, 4) && ar.IsAddonThere(postID, current.UserID, 4, false))
                ar.SetAddonState(postID, current.UserID, 4, true);

            return likeCount;
        }

        [UserAuth]
        public int TakeLikeBack(int postID)
        {
            User current = Session["user"] as User;
            AddonRepo ar = new AddonRepo();
            return ar.SetAddonState(postID, current.UserID, 2, true);   
        }

        [UserAuth]
        public int DislikePost(int postID)
        {
            User current = Session["user"] as User;
            AddonRepo ar = new AddonRepo();
            int dislikeCount = ar.AddDislike(postID, current.UserID);

            if (dislikeCount == 0)
                dislikeCount = ar.SetAddonState(postID, current.UserID, 4, false);

            if (ar.IsAddonThere(postID, current.UserID, 2) && ar.IsAddonThere(postID, current.UserID, 2, false))
                ar.SetAddonState(postID, current.UserID, 2, true);

            return dislikeCount;
        }

        [UserAuth]
        public int TakeDislikeBack(int postID)
        {
            User current = Session["user"] as User;
            AddonRepo ar = new AddonRepo();
            return ar.SetAddonState(postID, current.UserID, 4, true);
        }

        public PartialViewResult SuggestedPosts(int id = 0)
        {
            if (id > 0)
            {
                List<Tag> tags = pr.GetTagsOfPost(id);
                List<List<Post>> listOfLists = pr.GetSuggestedPostsForTags(tags);
                Post thisPost = pr.GetPost(id);
                listOfLists[0].Remove(thisPost);

                return PartialView("~/Views/Shared/_SuggestedPostsPartial.cshtml", listOfLists);
            }
            else
            {
                Response.Redirect(Url.Action("NotFound", "Home"));
                return null;
            }
        }

        [HttpPost]
        public ActionResult SearchResults(string txtSearch)
        {
            ViewBag.Query = txtSearch; 
            List<List<Post>> listsOfList = new List<List<Post>>();
            listsOfList.Add(pr.GetPostsWithTitle(txtSearch));
            listsOfList.Add(pr.GetPostsWithDesc(txtSearch));

            List<Tag> searchedTags = new List<Tag>();
            foreach (string tagText in txtSearch.Split(' '))
            {
                searchedTags.AddRange(tr.GetTagsWithText(tagText));
            }
            listsOfList.AddRange(pr.GetSuggestedPostsForTags(searchedTags));
            //TODO: Fuzzytext search gelecek
            return View(listsOfList);
        }

        //public PartialViewResult RenderComments(int id = 0)
        //{
        //    Post post = pr.GetPost(id);
        //    List<Comment> comments = comr.GetComments(post, false);
        //    return PartialView("~/Views/Shared/_CommentsPartial.cshtml", comments);
        //}
    }
}