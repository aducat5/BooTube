using BooTube.BLL;
using BooTube.BLL.Repos;
using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Controllers
{
    public class DenemelerController : Controller
    {
        BooTubeEntities db = DBTool.GetInstance();
        PostRepo pr = new PostRepo();
        UserRepo ur = new UserRepo();
        Random rnd = new Random();
        CommentRepo comr = new CommentRepo();

        static string LoremIpsum(int minWords, int maxWords)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer","adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod","tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int w = 0; w < numWords; w++)
            {
                if (w > 0) { result.Append(", "); }
                result.Append(words[rand.Next(words.Length)]);
            }

            return result.ToString();
        }

        // GET: Denemeler
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            bool sonuc = ur.InsertUser(user, out string islemSonucu);
            return View();
        }

        public ActionResult ImageUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImageUpload(HttpPostedFileBase denemeDosyasi)
        {

            bool isUploaded = false;
            string uploadedFilePath = GenFunx.ImageUpload(denemeDosyasi, "ProfilePictures", out isUploaded);
            if (isUploaded)
            {
                isUploaded = false;
                Image thumb = GenFunx.ResizeImage(uploadedFilePath, new Size() { Height = 100, Width = 100 });
                string uploadedThumbPath = GenFunx.ImageUpload(thumb, "Thumbnails", uploadedFilePath.Split('/').Last(), out isUploaded);
                if (isUploaded)
                {
                    ViewBag.Resim = uploadedFilePath;
                    ViewBag.Thumb = uploadedThumbPath;
                }
                else
                {
                    ViewBag.Sonuc = uploadedThumbPath;
                }
            }
            else
            {
                ViewBag.Sonuc = uploadedFilePath;
            }

            return View();
        }

        public ActionResult VideoUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VideoUpload(HttpPostedFileBase denemeDosyasi)
        {
            string uploadedVideo = GenFunx.VideoUpload(denemeDosyasi, "~/Content/Videos/Temporary", out bool isUploaded);
            string relocatedVideo = GenFunx.RelocateVideo(uploadedVideo, out bool isCopied);
            if (isCopied)
            {
                ViewBag.Sonuc = relocatedVideo;
            }
            else
            {
                ViewBag.Sonuc = "Bir sorun oluştu: " + uploadedVideo + " " + relocatedVideo;
            }
            return View();
        }

        public ActionResult AddTags()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTags(string tags)
        {
            try
            {
                Tag tag = db.Tags.Where(t => t.TagText == tags).First();
                ViewBag.Sonuc = tag.TagID + tag.TagText;
            }
            catch (InvalidOperationException)
            {
                Tag tag = new Tag() { TagText = tags };
                db.Tags.Add(tag);
                db.SaveChanges();
                ViewBag.Sonuc = tag.TagID + tag.TagText;
            }

            return View();
        }

        public ActionResult SuggestedVideos(int id)
        {
            List<Tag> tags = pr.GetTagsOfPost(id);
            List<List<Post>> listOfLists = pr.GetSuggestedPostsForTags(tags);
            return View(listOfLists);
        }

        //public ActionResult AddComments()
        //{
        //    List<Post> allPosts = pr.GetPosts();
        //    List<User> allUsers = ur.GetAll();

        //    foreach (Post post in allPosts)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            bool isReply = rnd.Next(0, 2) > 0;
        //            User commenter = allUsers[rnd.Next(1, allUsers.Count)];

        //            Comment newComment = new Comment()
        //            {
        //                OwnerID = commenter.UserID,
        //                CommentText = LoremIpsum(1, 5),
        //                PostID = post.PostID,
        //                CreateTime = DateTime.Now
        //            };

        //            if (isReply)
        //            {
        //                List<Comment> commentsOfPost = comr.GetComments(post);
        //                if (commentsOfPost.Count > 0)
        //                {
        //                    newComment.ReplyID = commentsOfPost[rnd.Next(1, commentsOfPost.Count)].CommentID;
        //                }
        //            }

        //            comr.InsertComment(newComment, out string islemSonucu);
        //        }
        //    }

        //    return View(comr.GetComments());
        //}

        public ActionResult UpdateUser()
        {
            List<User> users = db.Users.Where(u => u.FullName.Contains("</P>")).ToList();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            foreach (User user in users)
            {
                user.FullName = user.FullName.Replace("</P>", "");
                user.FullName = textInfo.ToTitleCase(user.FullName);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult ViewComments()
        {
            return View(db.Comments);
        }
    }
}