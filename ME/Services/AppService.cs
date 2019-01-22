using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using System.Web.Mvc;

namespace ME.Services
{
    public class AppService
    {
        private UnitOfWork uow = new UnitOfWork();

        public List<Question> getQuestions(int categoryId)
        {
            return uow.AppDAL.getQuestions(categoryId);
        }
        public bool saveSurvey(Survey data)
        {
            return uow.AppDAL.saveSurvey(data);
        }
        public User validateLogin(User u)
        {
            return uow.AppDAL.validateLogin(u);
        }
        public User signUp(User u)
        {
            return uow.AppDAL.signUp(u);
        }
        public bool forgetPassword(User u)
        {
            return uow.AppDAL.forgetPassword(u);
        }
        public object getNearestRetailOutlet(double latitude, double longitude)
        {
            return uow.AppDAL.getNearestRetailOutlet(latitude, longitude);
        }
        public User validateUserByEmail(string email)
        {
            return uow.AppDAL.validateUserByEmail(email);
        }
        public List<Region> getRegions()
        {
            return uow.AppDAL.getRegions();
        }
        public List<Area> getAreas()
        {
            return uow.AppDAL.getAreas();
        }
        public List<RetailOutlet> getRetailOutlets()
        {
            return uow.AppDAL.getRetailOutlets();
        }
        public bool isEmailUnique(string email)
        {
            return uow.AppDAL.isEmailUnique(email);
        }
        public int saveRating(Rating rate)
        {
            return uow.AppDAL.saveRating(rate);
        }
    }
}