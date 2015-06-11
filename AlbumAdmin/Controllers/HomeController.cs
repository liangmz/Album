﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlbumAdmin.Models.Admin;
using AlbumAdmin;
using DB_Service;

namespace AlbumAdmin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["login"] != null && Session["login"].ToString() == "yes")
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Login()
        {
            if (Session["login"] != null && Session["login"].ToString() == "yes")
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel Model)
        {
            if (Model.loginName == "root" && Model.password == "root")
            {
                Session["login"] = "yes";
                return RedirectToAction("Index", "Admin");
            }
            if (Model.loginName != null)
            {
                Model.error = "口令错误";
            }
            return View(Model);
        }
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        
    }
}
