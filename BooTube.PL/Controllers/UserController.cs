using BooTube.BLL;
using BooTube.BLL.Repos;
using BooTube.DAL;
using BooTube.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Controllers
{
    public class UserController : Controller
    {

        public ActionResult ViewProfile(int id = 0)
        {
            UserRepo ur = new UserRepo();
            if (id < 1)
            {
                if (Session["user"] != null)
                {
                    User current = Session["user"] as User;
                    ChannelRepo cr = new ChannelRepo();
                    List<Channel> chsOfUser = cr.GetChannelsByID(current.UserID, false);
                    return View(Tuple.Create<User, List<Channel>>(current, chsOfUser));

                }
                else
                {
                    return RedirectToAction("NotFound", "Home");
                }
            }
            else
            {
                try
                {
                    User current = ur.GetUserByID(Convert.ToInt32(id));
                    ChannelRepo cr = new ChannelRepo();
                    List<Channel> chsOfUser = cr.GetChannelsByID(current.UserID, false);
                    return View(Tuple.Create(current, chsOfUser));

                }
                catch (FormatException)
                {
                    return RedirectToAction("NotFound", "Home");
                }
                catch (NullReferenceException)
                {
                    return RedirectToAction("NotFound", "Home");
                }
            }

        }

    }
}