using BooTube.BLL.Repos;
using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Controllers
{
    public class CommentController : Controller
    {
        CommentRepo cr = new CommentRepo();
        PostRepo pr = new PostRepo();
        
    }
}