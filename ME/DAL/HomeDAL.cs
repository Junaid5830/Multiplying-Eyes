using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using ME.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace ME.DAL
{
    public class HomeDAL
    {
        MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
        string filePath = @"C:\Inetpub\vhosts\totaldevportal.com\me.totaldevportal.com\Assets\HelpImages\";//live db
        //string mobileImagePath = "me.totaldevportal.com/Assets/HelpImages/";
        //string filePath = @"F:\Ahsan\Total\Multiplying Eyes\Code\ME\ME\Assets\HelpImages\";
        public HomeDAL(MultiplyingEyesEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        public User getUser(string uname, string upassword)
        {
            try
            {
                var query = (from u in dbContext.Users
                             where u.Email == uname
                             && u.Password == upassword
                             select u);
                if (query.Any())
                    return query.FirstOrDefault();
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public User signUp(User u)
        {
            u.CreatedOn = DateTime.Now;
            u.IsActive = true;
            u.UserTypeId = 2;
            dbContext.Users.Add(u);
            dbContext.SaveChanges();
            return u;
        }
        public sp_Get_Dashboard_Result getDashboardData()
        {
            return dbContext.sp_Get_Dashboard().FirstOrDefault();
        }

        public List<GetAllQuestionList_Result> GetAllQuestionList()
        {
            try
            {
                List<GetAllQuestionList_Result> question = dbContext.GetAllQuestionList().ToList();

                return question;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //public List<Category> GetAllCatogries()
        //{
        //  try
        //  {

        //    List<Category> category = dbContext.Categories.ToList();

        //    return category;
        //  }
        //  catch (Exception ex)
        //  {
        //    throw;
        //  }
        //}


        public int SaveQuestionData(Question ques)
        {
            ques.IsActive = true;
            dbContext.Questions.Add(ques);
            if (ques.ImagePath != null && ques.ImagePath != string.Empty)
            {
                //extension check missing
                ques.ImagePath = filePath + ques.ImagePath;
            }
            dbContext.SaveChanges();
            return ques.QsId;
        }

        public int DelQuestionData(int quesId, int userId)
        {
            try
            {
                Question ques = dbContext.Questions.Where(x => x.QsId == quesId).FirstOrDefault();

                if (ques != null)
                {
                    ques.IsActive = false;
                    ques.UpdatedBy = userId;
                    ques.UpdatedOn = DateTime.Now;

                    dbContext.Questions.AddOrUpdate(ques);
                    dbContext.SaveChanges();
                    return userId;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int UpdateQuestionData(Question question)
        {
            try
            {
                //Question updateques = dbContext.Questions.Where(x => x.QsId == question.QsId).FirstOrDefault();
                //if (updateques != null)
                //{
                //    updateques.Text = question.Text;
                //    updateques.ImagePath = question.ImagePath;
                //    updateques.CategoryId = question.CategoryId;
                //    dbContext.SaveChanges();
                //    return question.QsId;
                //}
                dbContext.Questions.AddOrUpdate(question);
                dbContext.SaveChanges();
                return question.QsId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region User
        public List<sp_GetAllUsers_Result> GetAllUsers()
        {
            try
            {
                List<sp_GetAllUsers_Result> user = dbContext.sp_GetAllUsers().ToList();

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int updateUser(User user)
        {
            user.IsActive = true;
            dbContext.Users.AddOrUpdate(user);
            dbContext.SaveChanges();
            return user.UserId;
        }
        public bool isEmailUnique(string email)
        {
            User user = dbContext.Users.Where(x => x.Email == email && x.IsActive == true).FirstOrDefault();
            if (user == null)
                return true;
            else
                return false;
        }
        public bool isEmailUnique(string email, int UserId)
        {
            User user = dbContext.Users.Where(x => x.Email == email && x.IsActive == true && x.UserId != UserId).FirstOrDefault();
            if (user == null)
                return true;
            else
                return false;
        }
        public List<SelectListItem> UserTypeLoad()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Type--", Value = "0" },


            };
            //string options = "< option value = \"0\" > --Select City-- </ option >";
            try
            {
                List<UserType> UserType = dbContext.UserTypes.ToList();
                foreach (var data in UserType)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.UserTypeId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }
        public List<SelectListItem> DepartmentLoad()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Department--", Value = "0" },


            };
            //string options = "< option value = \"0\" > --Select City-- </ option >";
            try
            {
                List<Department> dept = dbContext.Departments.ToList();
                foreach (var data in dept)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.DepartmentId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }
        public List<SelectListItem> RegionsLoad()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Type--", Value = "0" },


            };
            //string options = "< option value = \"0\" > --Select City-- </ option >";
            try
            {
                List<Region> Region = dbContext.Regions.ToList();
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
        public List<SelectListItem> AreasLoad()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Type--", Value = "0" },


            };
            //string options = "< option value = \"0\" > --Select City-- </ option >";
            try
            {
                List<Area> Area = dbContext.Areas.ToList();
                foreach (var data in Area)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.AreaId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }
        public int DeleteUser(int? userId, int editorId)
        {
            try
            {
                User data = dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = editorId;
                    data.UpdatedOn = DateTime.Now;

                    dbContext.Users.AddOrUpdate(data);
                    dbContext.SaveChanges();
                    return data.UserId;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int UpdateUser(User user)
        {
            try
            {
                //User data = dbContext.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                //if (data != null)
                //{
                //    data.Name = user.Name;
                //    data.Email = user.Email;
                //    data.Password = user.Password;
                //    data.UserTypeId = user.UserTypeId;
                //    data.City = user.City;
                //    data.Designation = user.Designation;
                //    data.RegionId = user.RegionId;
                //    data.AreaId = user.AreaId;
                //    dbContext.SaveChanges();
                //    return user.UserId;
                //}
                user.IsActive = true;
                dbContext.Users.AddOrUpdate(user);
                dbContext.SaveChanges();
                return user.UserId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SaveUser(User user)
        {
            user.IsActive = true;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user.UserId;
        }
        #endregion


        #region Manager
        public List<sp_GetAllManager_Result> GetAllManager()
        {
            try
            {
                List<sp_GetAllManager_Result> man = dbContext.sp_GetAllManager().ToList();

                return man;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int DeleteManager(int Id)
        {
            Manager man = dbContext.Managers.Where(x => x.Id == Id).FirstOrDefault();
            dbContext.Managers.Remove(man);
            dbContext.SaveChanges();
            return Id;
        }

        public int UpdateManager(Manager man)
        {
            try
            {
                Manager data = dbContext.Managers.Where(x => x.Id == man.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Name = man.Name;

                    dbContext.SaveChanges();
                    return man.Id;
                }
                return man.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SaveManager(Manager man)
        {
            man.CreatedOn = DateTime.Now;
            man.Is_Active = true;
            dbContext.Managers.Add(man);
            dbContext.SaveChanges();
            return man.Id;
        }
        #endregion

        #region Survey
        //public List<sp_GetAllSurvey_Result> GetAllSurvey(int? pumpId)
        //{
        //    List<sp_GetAllSurvey_Result> survey = dbContext.sp_GetAllSurvey(pumpId).ToList();
        //    return survey;
        //}
        public List<sp_GetAllSurvey_Result> GetAllSurveys(SurveySearchModel model)
        {
            List<sp_GetAllSurvey_Result> survey = dbContext.sp_GetAllSurvey(model.pumpId, model.areaId, model.regionId, model.deptId, model.catId).ToList();
            return survey;
        }
        public List<Area> GetAllAreaRegionWise(int? RegionId)
        {
            List<Area> area = dbContext.Areas.Where(x => x.RegionId == RegionId).ToList();
            return area;
        }
        public List<Department> LoadDepartments()
        {
            List<Department> department = dbContext.Departments.ToList();
            return department;
        }


        public List<RetailOutlet> GetAllOutletsAreaWise(int? AreaId)
        {
            List<RetailOutlet> outlets = dbContext.RetailOutlets.Where(x => x.AreaId == AreaId).ToList();
            return outlets;
        }

        //public List<sp_GetAllSurvey_Result> GetAllSurveys(SurveySearchModel model)
        //{

        //    List<sp_GetAllSurvey_Result> survey = dbContext.sp_GetAllSurvey(model.pumpId,model.areaId,model.regionId,model.deptId).ToList();
        //    return survey;
        //}
        public List<Region> loadRegions()
        {
            return dbContext.Regions.ToList();
        }
        public Region loadRegionsByRegionId(int? regionId)
        {
            return dbContext.Regions.Where(x => x.RegionId == regionId).FirstOrDefault();
        }
        public Area loadAreaByAreaId(int? areaId)
        {
            return dbContext.Areas.Where(x => x.AreaId == areaId).FirstOrDefault();
        }
        public List<sp_GetSurveyAnswers_Result> getSurveyAnswers(int? surveyId)
        {
            List<sp_GetSurveyAnswers_Result> answers = dbContext.sp_GetSurveyAnswers(surveyId).ToList();
            return answers;
        }

        #endregion

        #region RetailOutlets
        public List<SelectListItem> Areas()
        {
            MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
            List<SelectListItem> ObjList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "--Select Area--", Value = "0" },
            };
            try
            {
                List<Area> Area = dbContext.Areas.ToList();
                foreach (var data in Area)
                {
                    ObjList.Add(new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.AreaId.ToString()
                    });
                }
                return ObjList;
            }
            catch (Exception ex)
            {
                return ObjList;
            }
        }
        public List<sp_GetAllRetailOutlet_Result> GetRetailOutletList()
        {
            List<sp_GetAllRetailOutlet_Result> pump = dbContext.sp_GetAllRetailOutlet().ToList();
            return pump;
        }

        public int SaveRetailOutlet(RetailOutlet outlet)
        {
            outlet.IsActive = true;
            dbContext.RetailOutlets.Add(outlet);
            dbContext.SaveChanges();
            return outlet.RetailOutletId;
        }

        public int DeleteRetailOutlet(int? outletId)
        {
            try
            {
                RetailOutlet outlet = dbContext.RetailOutlets.Where(x => x.RetailOutletId == outletId).FirstOrDefault();
                outlet.IsActive = false;
                dbContext.RetailOutlets.AddOrUpdate(outlet);
                dbContext.SaveChanges();
                return outlet.RetailOutletId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public User getUserById(int? id)
        {
            return dbContext.Users.Where(x => x.UserId == id).FirstOrDefault();
        }
        public RetailOutlet getRetailOutletById(int? id)
        {
            return dbContext.RetailOutlets.Where(x => x.RetailOutletId == id).FirstOrDefault();
        }
        public int updateRetailOutlet(RetailOutlet outlet)
        {
            outlet.IsActive = true;
            dbContext.RetailOutlets.AddOrUpdate(outlet);
            dbContext.SaveChanges();
            return outlet.RetailOutletId;
        }
        #endregion
        public Question getQuestionById(int? id)
        {
            return dbContext.Questions.Where(x => x.QsId == id).FirstOrDefault();
        }
        public int updateQuestion(Question outlet)
        {
            outlet.IsActive = true;
            dbContext.Questions.AddOrUpdate(outlet);
            dbContext.SaveChanges();
            return outlet.QsId;
        }
        public int DeleteQuestion(int? qsId)
        {
            try
            {
                Question outlet = dbContext.Questions.Where(x => x.QsId == qsId).FirstOrDefault();
                outlet.IsActive = false;
                dbContext.Questions.AddOrUpdate(outlet);
                dbContext.SaveChanges();
                return outlet.QsId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int saveImage(string path, int id)
        {
            Question outlet = dbContext.Questions.Where(x => x.QsId == id).FirstOrDefault();
            outlet.ImagePath = path;
            dbContext.Questions.AddOrUpdate(outlet);
            dbContext.SaveChanges();
            return outlet.QsId;

        }
    }
}