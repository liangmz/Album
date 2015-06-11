using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlbumAdmin.Models.Admin;
using AlbumAdmin;
using DB_Service;
using DB_Service.Service;
using System.Reflection;

namespace AlbumAdmin.Controllers
{
    public class AdminController : Controller
    {
        private a_userService US = null;
        private a_upictureService UPS = null;
        private a_evaluationService ES = null;
        private a_ablumService AS = null;
        private a_pictureService PS = null;

        public AdminController()
        {
            US = new a_userService();
            UPS = new a_upictureService();
            ES = new a_evaluationService();
            AS = new a_ablumService();
            PS = new a_pictureService();
        }
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (LoginOn())
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult List(ListTypeModel _Model)
        //{
        //    if (LoginOff())
        //    {
        //        string type = "UserList";
        //        switch (_Model.type)
        //        {
        //            case "UserList": type = "a_user"; break;
        //            case "AlbumList": type = "a_ablum"; break;
        //            case "UpicList": type = "a_upicture"; break;
        //            case "PicList": type = "a_picture"; break;
        //        }
        //        Type T = Type.GetType("a_user");
        //        ListModel<typeof(Type.GetType("a_user")> Model = new ListModel<a_user>();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}


        public ActionResult UserList(ListModel<a_user,Object,Object> Model)
        {
            if (LoginOn())
            {
                int Page = -1;
                if ((int)Model.Page == 0)
                {
                    Page = 1;
                    Model.Page = Page;
                }
                int PageCount = 1;
                if (Model.UID == 0)
                {
                    Model.List = US.List(Model.Page, out PageCount, 50).ToList();
                }
                else
                {
                    Model.List = new List<a_user>();
                    Model.List.Add(US.SelectUser(Model.UID));
                }
                Model.PageCount = PageCount;
                return View(Model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AlbumList(ListModel<a_ablum, a_user, Object> Model)
        {
            if (LoginOn())
            {
                int Page = -1;
                if ((int)Model.Page == 0)
                {
                    Page = 1;
                    Model.Page = Page;
                }
                int PageCount = 1;
                if (Model.AID != 0)
                {
                    Model.List = new List<a_ablum>();
                    Model.List.Add(AS.GetById(Model.AID));
                }
                else if (Model.UID == 0)
                {
                    Model.List = AS.List(Model.Page, out PageCount, 50).ToList();
                }
                else {
                    Model.List = AS.UserList(Model.UID, Model.Page, out PageCount, 50).ToList();
                    Model.model_f = US.SelectUser(Model.UID);
                }
                Model.PageCount = PageCount;
                return View(Model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpicList(ListModel<a_upicture, a_ablum, a_picture> Model)
        {
            if (LoginOn())
            {
                int Page = -1;
                if ((int)Model.Page == 0)
                {
                    Page = 1;
                    Model.Page = Page;
                }
                int PageCount = 1;
                if (Model.UPID != 0)
                {
                    Model.List = new List<a_upicture>();
                    Model.List.Add(UPS.GetById(Model.UPID));
                }
                else if (Model.PID != 0)
                {
                    Model.List = UPS.ListPID(Model.PID, Model.Page, out PageCount, 50).ToList();
                    Model.model_s = PS.GetById(Model.PID);
                }
                else if (Model.AID == 0)
                {
                    Model.List = UPS.List(Model.Page, out PageCount, 50).ToList();
                }
                else
                {
                    Model.List = UPS.AblumUPicList(Model.AID, Model.Page, out PageCount, 50).ToList();
                    Model.model_f = AS.GetById(Model.AID);
                }
                Model.PageCount = PageCount;
                Model.action = "UpicList";
                return View(Model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PicList(ListModel<a_picture, a_upicture, a_user> Model)
        {
            if (LoginOn())
            {
                int Page = 1;
                if ((int)Model.Page == 0)
                {
                    Page = 1;
                    Model.Page = Page;
                }
                int PageCount = 1;
                if (Model.PID != 0)
                {
                    Model.List = new List<a_picture>();
                    Model.List.Add(PS.GetById(Model.PID));
                }
                else 
                {
                    Model.List = PS.List(Model.Page, out PageCount, 50).ToList();
                }


                if (Model.UPID != 0)
                {
                    Model.model_f = UPS.GetById(Model.UPID);
                }
                if (Model.UID != 0)
                {
                    Model.model_s = US.SelectUser(Model.UID);
                }
                
                Model.PageCount = PageCount;
                return View(Model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EvaList(ListModel<a_evaluation, a_upicture, Object> Model)
        {
            if (LoginOn())
            {
                int Page = -1;
                if ((int)Model.Page == 0)
                {
                    Page = 1;
                    Model.Page = Page;
                }
                int PageCount;
                if (Model.UPID == 0)
                {
                    Model.List = ES.List(Model.Page, out PageCount, 50).ToList();
                }
                else
                {
                    Model.List = ES.PicList(Model.UPID, Model.Page, out PageCount, 50).ToList();
                    Model.model_f = UPS.GetById(Model.UPID);
                }

                Model.PageCount = PageCount;
                return View(Model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(DeleteModel Model)
        {
            if (LoginOn())
            {
                switch (Model.type)
                {
                    case "UserList":
                        {
                            US.Delete(Model.id);
                        } break;
                    case "AlbumList":
                        {
                            AS.Delete(Model.id);
                        } break;
                    case "UpicList":
                        {
                            UPS.Delete(Model.id);
                        } break;
                    case "PicList":
                        {
                            PS.Delete(Model.id);
                        } break;
                    case "EvaList":
                        {
                            ES.Delete(Model.id);
                        } break;
                }
                return RedirectToAction(Model.type, "Admin", new { Page = Model.Page ,UID=Model.UID,AID = Model.AID ,PID=Model.PID, UPID = Model.UPID });
            }
            return RedirectToAction("Index", "Home");
        }

        public bool LoginOn()
        {
            if (Session["login"] != null && Session["login"].ToString() == "yes")
            {
                return true;
            }
            return false;
        }
    }
}
