using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using System.Web.Mvc;

namespace ME.ViewModels
{
    public class HomeViewModel
    {
        public List<sp_GetAllRetailOutlet_Result> GetRetailOutletList { set; get; }
        public List<GetAllQuestionList_Result> GetAllQuestionList { set; get; }
        public List<sp_GetAllUsers_Result> GetAllUserList { set; get; }
        public List<sp_GetAllManager_Result> GetAllManagerList { set; get; }
        public List<sp_GetAllSurvey_Result> GetSurveyList { set; get; }
        public Region region { set; get; }
        public Area area { set; get; }
        public RetailOutlet reatialOutlet { set; get; }
        public Manager manager { set; get; }
        public Question question { set; get; }
        public Category category { set; get; }
        public User user { set; get; }
        public IEnumerable<SelectListItem> CategoryList { set; get; }
    }
}