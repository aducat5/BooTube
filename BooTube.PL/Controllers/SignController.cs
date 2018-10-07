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
    public class SignController : Controller
    {
        [NonUserAuth]
        public ActionResult Register() => View();
        [NonUserAuth]
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                UserRepo ur = new UserRepo();
                bool sonuc = ur.InsertUser(newUser, out string islemSonucu);
                if (sonuc)
                {
                    return RedirectToAction("Login", new { regState = "suc" });
                }
                else
                {
                    ViewBag.RegState = islemSonucu;
                    ViewBag.AlertState = "alert alert-danger";
                    return View();
                }

            }
            else
            {
                ViewBag.RegState = "Gerekli kriterleri sağlamadınız, kayıt yapılamadı.";
                ViewBag.AlertState = "alert alert-warning";
                return View();
            }
        }

        [UserAuth]
        public ActionResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(string txtUserName, string txtPassword)
        {
            UserRepo ur = new UserRepo();
            User current = ur.IsThereThisUser(txtUserName, txtPassword);
            if (current != null)
            {
                Session["user"] = current;

                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.LoginState = "Giriş başarısız oldu. Kullanıcı adı veya şifre yanlış.";
                ViewBag.AlertState = "alert alert-danger";

                return View();
            }

        }
    }
}