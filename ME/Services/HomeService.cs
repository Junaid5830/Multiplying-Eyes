using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using System.Web.Mvc;
using ME.ViewModels;
namespace ME.Services
{
    public class HomeService
    {
        UnitOfWork uow = new UnitOfWork();
        public sp_Get_Dashboard_Result getDashboardData()
        {
            return uow.HomeDAL.getDashboardData();
        }
        public User getUser(string uname, string upassword)
        {
            return uow.HomeDAL.getUser(uname, upassword);
        }
        public User signUp(User u)
        {
            return uow.HomeDAL.signUp(u);
        }

        public List<GetAllQuestionList_Result> GetAllQuestion()
        {
            return uow.HomeDAL.GetAllQuestionList();
        }



        public int SaveQuestionData(Question ques)
        {
            return uow.HomeDAL.SaveQuestionData(ques);
        }

        public int DeleteQuestionData(int quesId, int userId)
        {

            return uow.HomeDAL.DelQuestionData(quesId, userId);
        }

        public int UpdateQuestionData(Question ques)
        {
            return uow.HomeDAL.UpdateQuestionData(ques);
        }

        public List<SelectListItem> UserTypeLoad()
        {
            return uow.HomeDAL.UserTypeLoad();
        }
        public List<SelectListItem> DepartmentLoad()
        {
            return uow.HomeDAL.DepartmentLoad();
        }
        public List<SelectListItem> RegionsLoad()
        {
            return uow.HomeDAL.RegionsLoad();
        }
        public List<SelectListItem> AreasLoad()
        {
            return uow.HomeDAL.AreasLoad();
        }

        #region User
        public bool isEmailUnique(string email)
        {
            return uow.HomeDAL.isEmailUnique(email);
        }
        public bool isEmailUnique(string email, int UserId)
        {
            return uow.HomeDAL.isEmailUnique(email, UserId);
        }
        public List<sp_GetAllUsers_Result> GetAllUsers()
        {
            return uow.HomeDAL.GetAllUsers();
        }

        public int SaveUser(User user)
        {
            return uow.HomeDAL.SaveUser(user);
        }

        public int DeleteUser(int? user, int editorId)
        {

            return uow.HomeDAL.DeleteUser(user, editorId);
        }

        public int UpdateUser(User user)
        {
            return uow.HomeDAL.UpdateUser(user);
        }
        public int updateUser(User user)
        {
            return uow.HomeDAL.updateUser(user);
        }
        public User getUserById(int? id)
        {
            return uow.HomeDAL.getUserById(id);
        }
        #endregion

        #region Manager
        public List<sp_GetAllManager_Result> GetAllManager()
        {
            return uow.HomeDAL.GetAllManager();
        }

        public int SaveManager(Manager man)
        {
            return uow.HomeDAL.SaveManager(man);
        }
        public int UpdateManager(Manager man)
        {
            return uow.HomeDAL.UpdateManager(man);
        }

        public int DeleteManager(int Id)
        {
            return uow.HomeDAL.DeleteManager(Id);
        }

        #endregion

        //public List<sp_GetAllSurvey_Result> GetAllSurvey(int? pumpId)
        //{
        //    return uow.HomeDAL.GetAllSurvey(pumpId);
        //}

        public List<Area> GetAllAreaRegionWise(int? RegionId)
        {
            return uow.HomeDAL.GetAllAreaRegionWise(RegionId);
        }
        public List<Department> LoadDepartments()
        {
            return uow.HomeDAL.LoadDepartments();
        }


        public List<RetailOutlet> GetAllOutletsAreaWise(int? AreaId)
        {
            return uow.HomeDAL.GetAllOutletsAreaWise(AreaId);
        }

        public List<sp_GetAllSurvey_Result> GetAllSurveys(SurveySearchModel model)
        {
            return uow.HomeDAL.GetAllSurveys(model);
        }
        public List<Region> loadRegions()
        {
            return uow.HomeDAL.loadRegions();
        }
        public Region loadRegionsByRegionId(int? regionId)
        {
            return uow.HomeDAL.loadRegionsByRegionId(regionId);
        }
        public Area loadAreaByAreaId(int? areaID)
        {
            return uow.HomeDAL.loadAreaByAreaId(areaID);
        }
        public List<sp_GetSurveyAnswers_Result> getSurveyAnswers(int? surveyId)
        {
            return uow.HomeDAL.getSurveyAnswers(surveyId);
        }

        public Question getQuestionById(int? id)
        {
            return uow.HomeDAL.getQuestionById(id);
        }
        public int updateQuestion(Question outlet)
        {
            return uow.HomeDAL.updateQuestion(outlet);
        }
        public int DeleteQuestion(int? qsId)
        {
            return uow.HomeDAL.DeleteQuestion(qsId);
        }
        public int saveImage(string path, int id)
        {
            return uow.HomeDAL.saveImage(path, id);
        }
        #region RetailOutlets
        public List<SelectListItem> Areas()
        {
            return uow.HomeDAL.Areas();
        }
        public List<sp_GetAllRetailOutlet_Result> GetRetailOutletList()
        {
            return uow.HomeDAL.GetRetailOutletList();
        }

        public int SaveRetailOutlet(RetailOutlet outlet)
        {
            return uow.HomeDAL.SaveRetailOutlet(outlet);
        }


        public int DeleteRetailOutlet(int? outletId)
        {
            return uow.HomeDAL.DeleteRetailOutlet(outletId);
        }
        public RetailOutlet getRetailOutletById(int? id)
        {
            return uow.HomeDAL.getRetailOutletById(id);
        }
        public int updateRetailOutlet(RetailOutlet outlet)
        {
            return uow.HomeDAL.updateRetailOutlet(outlet);
        }
        #endregion
    }

}