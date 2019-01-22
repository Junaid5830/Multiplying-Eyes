using ME.Models;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Services;
using ME.ViewModels;
using System.Drawing;
using System.Drawing.Imaging;
namespace ME.Controllers
{
    public class HomeController : Controller
    {
        HomeService service = new HomeService();
        HomeViewModel viewModel = new HomeViewModel();
        // GET: Survey
        public ActionResult Index()
        {
            try
            {
                User user = (User)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }

                sp_Get_Dashboard_Result result = service.getDashboardData();
                ViewBag.Pumps = result.Pumps;
                ViewBag.Users = result.Users;
                ViewBag.Questions = result.Questions;
                ViewBag.Surveys = result.Surveys;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message + ex.InnerException;
                return View("Error");
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            //session to null

            return RedirectToAction("Login");
        }
        public ActionResult LoginCheck(string uName, string uPassword)
        {
            try
            {
                User u = service.getUser(uName, uPassword);
                if (u == null)
                {
                    Session["LoginError"] = "Invalid Credentials! Please check provided information";
                    return Redirect("/Home/Login");
                }
                Session["UserLoggedIn"] = u;
                Session["UserName"] = u.Name;
                Session["UserType"] = u.UserTypeId;

                return Redirect("/Home/Index");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult signUp(User obj)
        {
            try
            {
                if (obj.Email.Contains("totalparco.com.pk") == false)
                {
                    return Redirect("/Home/Register");
                }

                User user = service.signUp(obj);
                if (user != null)
                    return Redirect("/Home/Index");
                else
                    return Redirect("/Home/Register");
            }
            catch (Exception ex)
            {
                return Redirect("/Home/Register"); ;
            }
        }

        #region Questions

        [HttpGet]
        public ActionResult Question(Question question)
        {
            User user = (User)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CategoryId = loadCategory();
            return View();
        }
        public ActionResult List()
        {
            viewModel.GetAllQuestionList = service.GetAllQuestion();
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetAllCatogries()
        {
            HomeService service = new HomeService();
            HomeViewModel viewModel = new HomeViewModel();
            List<Category> category = new List<Category>();

            return Json(category, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [System.Web.Http.HttpPost]
        public JsonResult SaveQuestionData(Question ques)
        {
            try
            {
                User user = (User)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                ques.CreatedBy = user.UserId;
                ques.CreatedOn = DateTime.Now;
                ques.IsActive = true;
                return Json(service.SaveQuestionData(ques), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SelectListItem> loadCategory()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Category", Value = "0" },


            };
            //string options = "< option value = \"0\" > --Select City-- </ option >";
            try
            {
                List<Category> Category = dbContext.Categories.ToList();
                foreach (var data in Category)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.CategoryId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }

        public ActionResult EditQuestion(int? id)
        {
            Question question = service.getQuestionById(id);
            ViewBag.CategoryId = loadCategory();
            ViewBag.QsId = question.QsId;
            ViewBag.Path = question.ImagePath;
            return View(question);


        }
        [System.Web.Http.HttpPost]
        public JsonResult ModifyQuestion(Question ques)
        {
            try
            {
                User user = (User)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                ques.UpdatedBy = user.UserId;
                ques.UpdatedOn = DateTime.Now;
                ques.IsActive = true;
                return Json(service.updateQuestion(ques), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult DeleteQuestion(int? qsId)
        {
            try
            {
                service.DeleteQuestion(qsId);
                viewModel.GetAllQuestionList = service.GetAllQuestion();
                return RedirectToAction("List", viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void UploadImage()
        {
            try
            {
                string count = System.Web.HttpContext.Current.Request.Params.GetValues("count")[0];
                string objId = System.Web.HttpContext.Current.Request.Params.GetValues("id")[0];

                for (int i = 1; i <= int.Parse(count); i++)
                {
                    var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage" + i];

                    string fileName = objId + "" + httpPostedFile.FileName;

                    var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("../Assets/HelpImages/"), fileName);

                    var imagePath = "http://me.totaldevportal.com/Assets/HelpImages/" + fileName;
                    httpPostedFile.SaveAs(fileSavePath);
                    service.saveImage(imagePath, int.Parse(objId));
                    //IdeaAsset asset = new IdeaAsset();
                    //asset.IdeaId = int.Parse(objId);
                    //asset.Path = imagePath;
                    //service.saveIdeaAssets(asset);
                }
            }
            catch (Exception ex)
            {

            }

        }


        //[HttpPost]
        //public ActionResult DeleteQuestionData(int QuesId)
        //{
        //    try
        //    {
        //        User user = (User)Session["UserLoggedIn"];
        //        if (user == null)
        //        {
        //            return Redirect("/Home/Login");
        //        }
        //        HomeViewModel viewModel = new HomeViewModel();
        //        service.DeleteQuestionData(QuesId, user.UserId);
        //        viewModel.GetAllQuestionList = service.GetAllQuestion();
        //        return PartialView("_ManageQuestionList", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        #endregion

        #region Users
        public ActionResult Users(User user)
        {
            User session = (User)Session["UserLoggedIn"];
            if (session == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.msg = TempData["Msg"] as string;
            ViewBag.UserType = service.UserTypeLoad();
            ViewBag.Departments = service.DepartmentLoad();
            ViewBag.Areas = service.AreasLoad();
            ViewBag.Regions = service.RegionsLoad();
            HomeViewModel viewModel = new HomeViewModel();
            //HomeService service = new HomeService();

            viewModel.GetAllUserList = service.GetAllUsers();
            return View();

        }
        public ActionResult _ManageUsers()
        {
            User session = (User)Session["UserLoggedIn"];
            if (session == null)
            {
                return Redirect("/Home/Login");
            }
            HomeViewModel viewModel = new HomeViewModel();
            //HomeService service = new HomeService();

            viewModel.GetAllUserList = service.GetAllUsers();
            return View(viewModel);
        }
        public ActionResult EditUser(int? id)
        {
            User user = service.getUserById(id);
            ViewBag.UserTypeId = service.UserTypeLoad();
            ViewBag.Departments = service.DepartmentLoad();
            ViewBag.Areas = service.AreasLoad();
            ViewBag.Regions = service.RegionsLoad();
            return View(user);
        }
        [System.Web.Http.HttpPost]
        public ActionResult SaveUser(User user)
        {
            try
            {
                User session = (User)Session["UserLoggedIn"];
                if (session == null)
                {
                    return Redirect("/Home/Login");
                }
                if (user != null)
                {

                    if (user.Email.Contains("totalparco.com.pk") == false)
                    {
                        TempData["Msg"] = "Invalid Email. It must be a total parco account";
                        return RedirectToAction("Users");
                    }
                    if (user.UserId == 0)
                    {
                        user.CreatedOn = DateTime.Now;
                        user.CreatedBy = session.UserId;
                        if (service.isEmailUnique(user.Email) == false)
                        {
                            TempData["Msg"] = "Email already exists.";
                            return RedirectToAction("Users");
                        }
                        service.SaveUser(user);
                    }
                    else
                    {
                        if (service.isEmailUnique(user.Email, user.UserId) == false)
                        {
                            TempData["Msg"] = "Email already exists.";
                            return RedirectToAction("Users");
                        }
                        user.UpdatedOn = DateTime.Now;
                        user.UpdatedBy = session.UserId;
                        service.UpdateUser(user);
                    }
                }
                //model.GetAllUserList = service.GetAllUsers();
                return RedirectToAction("_ManageUsers");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Some Error Ocurred.Please try again";
                throw;
            }
        }

        //[HttpPost]
        //public ActionResult DeleteUser(int user)
        //{
        //    try
        //    {
        //        User session = (User)Session["UserLoggedIn"];
        //        if (session == null)
        //        {
        //            return Redirect("/Home/Login");
        //        }
        //        HomeViewModel viewModel = new HomeViewModel();
        //        service.DeleteUser(user, session.UserId);
        //        viewModel.GetAllUserList = service.GetAllUsers();
        //        return PartialView("_ManageUsers", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost]
        public ActionResult ModifyUser(User user)
        {
            try
            {
                User session = (User)Session["UserLoggedIn"];
                if (session == null)
                {
                    return Redirect("/Home/Login");
                }
                if (user != null)
                {
                    if (service.isEmailUnique(user.Email, user.UserId) == false)
                    {
                        TempData["Msg"] = "Email already exists.";
                        return RedirectToAction("Users");
                    }
                    user.UpdatedOn = DateTime.Now;
                    user.UpdatedBy = session.UserId;
                    service.UpdateUser(user);
                    //service.UpdateUser(user);

                }
                //viewModel.GetRetailOutletList = service.GetRetailOutletList();
                return RedirectToAction("_ManageUsers");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            try
            {
                User session = (User)Session["UserLoggedIn"];
                if (session == null)
                {
                    return Redirect("/Home/Login");
                }
                service.DeleteUser(id, session.UserId);
                //viewModel.GetRetailOutletList = service.GetRetailOutletList();
                //return RedirectToAction("RetailOutletList", viewModel);
                return RedirectToAction("_ManageUsers");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Manager
        public ActionResult Manager(Manager man)
        {
            User user = (User)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            HomeViewModel viewModel = new HomeViewModel();
            //HomeService service = new HomeService();

            viewModel.GetAllManagerList = service.GetAllManager();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveManager(HomeViewModel model, Manager man)
        {
            try
            {
                if (man != null)
                {
                    if (man.Id == 0)
                    {
                        service.SaveManager(man);
                    }
                    else
                    {
                        service.UpdateManager(man);
                    }
                }

                model.GetAllManagerList = service.GetAllManager();

                return RedirectToAction("Manager", model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteManager(int Id)
        {
            try
            {
                HomeViewModel viewModel = new HomeViewModel();
                service.DeleteManager(Id);
                viewModel.GetAllManagerList = service.GetAllManager();
                return PartialView("ManageUsers", viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Survey


        public ActionResult Survey()
        {
            User user = (User)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            //if (user.UserTypeId == 3)
            //{
            //    ViewBag.Region = LoadRegionByRegionId(user.RegionId);
            //    ViewBag.Area = LoadAreaByRegionId(user.RegionId);
            //}
            //else if (user.UserTypeId == 4)
            //{
            //    ViewBag.Area = LoadAreaByAreaId(user.AreaId);
            //}
            //else
            //{
            //    ViewBag.Region = LoadRegion();
            //    ViewBag.Area = LoadArea();
            //}
            ViewBag.Region = LoadRegion();
            ViewBag.Area = LoadArea();
            ViewBag.Pump = LoadRetailOutlet();
            ViewBag.Departments = LoadDepartments();
            ViewBag.Categories = loadCategory();
            HomeViewModel view = new HomeViewModel();
            //view.GetSurveyList = service.GetAllSurvey(0);
            return View(view);
        }
        //SurveySearchModel model = new SurveySearchModel();
        //model.pumpId = -1;
        [HttpPost]
        public ActionResult GetAllSurveys(SurveySearchModel model)
        {

            List<sp_GetAllSurvey_Result> survey = service.GetAllSurveys(model);
            return Json(survey, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllAreaRegionWise(int? RegionId)
        {

            List<Area> area = new List<Area>();
            area = service.GetAllAreaRegionWise(RegionId);
            return Json(area, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllOutletsAreaWise(int? AreaId)
        {

            List<RetailOutlet> outlet = new List<RetailOutlet>();
            outlet = service.GetAllOutletsAreaWise(AreaId);
            return Json(outlet, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GetAllSurveyPumpWise(int? RetailId)
        //{

        //    List<sp_GetAllSurvey_Result> survey = service.GetAllSurveyPumpWise(RetailId);


        //    return Json(survey, JsonRequestBehavior.AllowGet);
        //}

        public List<SelectListItem> LoadRegion()
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Region--", Value = "0" },


            };
            try
            {
                List<Models.Region> Region = service.loadRegions();
                foreach (var data in Region)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.RegionId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }

        public List<SelectListItem> LoadArea()
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Area--", Value = "0" },
            };
            return ObjList;
        }

        public List<SelectListItem> LoadAreaByAreaId(int? AreaId)
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Area--", Value = "0" },
            };
            Area data = service.loadAreaByAreaId(AreaId);
            if (data != null)
            {
                ObjList.Add(new SelectListItem
                {
                    Text = data.Name,
                    Value = data.AreaId.ToString()
                });
            }

            return ObjList;
        }
        public List<SelectListItem> LoadRegionByRegionId(int? regionId)
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select region--", Value = "0" },
            };
            Models.Region data = service.loadRegionsByRegionId(regionId);
            if (data != null)
            {
                ObjList.Add(new SelectListItem
                {
                    Text = data.Name,
                    Value = data.RegionId.ToString()
                });
            }
            return ObjList;
        }


        public List<SelectListItem> LoadAreaByRegionId(int? regionId)
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Area--", Value = "0" },
            };
            List<Models.Area> areas = service.GetAllAreaRegionWise(regionId);
            foreach (var data in areas)
            {
                ObjList.Add(new SelectListItem
                {
                    Text = data.Name,
                    Value = data.AreaId.ToString()
                });
            }
            return ObjList;
        }

        public List<SelectListItem> LoadRetailOutlet()
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Pump--", Value = "0" },
            };
            return ObjList;
        }
        public List<SelectListItem> LoadDepartments()
        {
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "-- ALL --", Value = "0" },
            };
            List<Models.Department> departments = service.LoadDepartments();
            foreach (var data in departments)
            {
                ObjList.Add(new SelectListItem
                {
                    Text = data.Name,
                    Value = data.DepartmentId.ToString()
                });
            }
            return ObjList;
        }
        public ActionResult View(int surveyId)
        {
            ViewBag.Answers = service.getSurveyAnswers(surveyId);
            return View();
        }

        #endregion

        #region Retail Outlet
        public ActionResult RetailOutlets()
        {
            try
            {
                ViewBag.AreaId = service.Areas();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message + ex.InnerException;
                return View("Error");
            }
        }

        public ActionResult RetailOutletList()
        {
            viewModel.GetRetailOutletList = service.GetRetailOutletList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveRetailOutlet(RetailOutlet outlet)
        {
            try
            {
                if (outlet != null)
                {
                    if (outlet.RetailOutletId == 0)
                    {
                        service.SaveRetailOutlet(outlet);
                    }
                    //else
                    //{
                    //  service.UpdateQuestionData(outlet);
                    //}
                }
                viewModel.GetRetailOutletList = service.GetRetailOutletList();
                return RedirectToAction("RetailOutletList", viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult EditOutlet(int? id)
        {
            RetailOutlet outlet = service.getRetailOutletById(id);
            ViewBag.AreaId = service.Areas();


            return View(outlet);


        }
        [HttpPost]
        public ActionResult ModifyRetailOutlet(RetailOutlet outlet)
        {
            try
            {
                if (outlet != null)
                {

                    service.updateRetailOutlet(outlet);

                }
                viewModel.GetRetailOutletList = service.GetRetailOutletList();
                return RedirectToAction("RetailOutletList", viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult DeleteRetailOutlet(int? OutletId)
        {
            try
            {
                service.DeleteRetailOutlet(OutletId);
                viewModel.GetRetailOutletList = service.GetRetailOutletList();
                return RedirectToAction("RetailOutletList", viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Rating
        public ActionResult RatingList()
        {
            //do this junaid
            //ViewBag.Rating = service.GetRatingList();
            return View();
        }
        #endregion

    }
}