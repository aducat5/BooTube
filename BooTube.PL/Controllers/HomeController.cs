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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [UserAuth]
        public PartialViewResult LastOnesFromFolloweds()
        {
            PostRepo pr = new PostRepo();
            Random rnd = new Random();
            User current = Session["user"] as User;

            List<Post> posts = pr.GetFollowedPosts(current.UserID, 2);
            List<Post> shortPosts = new List<Post>();

            if (posts.Count > 12)
            {
                for (int i = 0; i < 12;)
                {
                    Post p = posts[rnd.Next(0, posts.Count)];
                    if (!shortPosts.Contains(p))
                    {
                        shortPosts.Add(p);
                        i++;
                    }
                }
            }
            else shortPosts = posts;
           
            return PartialView("~/Views/Shared/_PostShowcasePartial.cshtml", shortPosts);
        }

        public PartialViewResult LastOnesFromTrends()
        {
            PostRepo pr = new PostRepo();
            Random rnd = new Random();

            List<Post> posts = pr.GetTrendPosts(30);
            List<Post> shortPosts = new List<Post>();

            for (int i = 0; i < 12;)
            {
                Post p = posts[rnd.Next(0, posts.Count)];
                if (!shortPosts.Contains(p))
                {
                    shortPosts.Add(p);
                    i++;
                }
            }

            return PartialView("~/Views/Shared/_PostShowcasePartial.cshtml", shortPosts);
        }
    }
}