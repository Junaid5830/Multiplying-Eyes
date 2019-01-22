using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using System.IO;

namespace ME.DAL
{
    public class AppDAL
    {
        MultiplyingEyesEntities dbContext = new MultiplyingEyesEntities();
        string filePath = @"C:\Inetpub\vhosts\totaldevportal.com\me.totaldevportal.com\Assets\Images\";//live db
        string mobileImagePath = "me.totaldevportal.com/Assets/Images/";
        //string filePath = @"F:\Ahsan\Total\Multiplying Eyes\Code\ME\ME\Assets\Images\";
        public AppDAL(MultiplyingEyesEntities dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Question> getQuestions(int categoryId)
        {
            return dbContext.Questions.Where(x => x.IsActive == true && x.CategoryId == categoryId).ToList();
        }
        public bool saveSurvey(Survey data)
        {
            try
            {
                data.CreatedOn = DateTime.Now;
                data.IsActive = true;
                dbContext.Surveys.Add(data);

                foreach (var obj in data.SurveyAnswers)
                {
                    if (obj.Image != string.Empty && obj.ImageExtension != string.Empty)
                    {
                        //extension check missing
                        Guid guid = Guid.NewGuid();
                        string filename = guid.ToString() + obj.ImageExtension;
                        obj.ImagePath = filePath + filename;
                        byte[] imgByteArray = Convert.FromBase64String(obj.Image);
                        //using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(order.IMAGE)))
                        //{
                        //    using (Bitmap bm2 = new Bitmap(ms))
                        //    {
                        //        bm2.Save(filePath + "ImageName.jpg");
                        //    }
                        //}
                        File.WriteAllBytes(obj.ImagePath, imgByteArray);
                        //obj.ImagePath = mobileImagePath + filename;
                        obj.ImagePath = "../Assets/Images/" + filename;
                    }
                }
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User validateLogin(User u)
        {
            return dbContext.Users.Where(x => x.IsActive == true && x.Email == u.Email && x.Password == u.Password && x.UserTypeId == 2).FirstOrDefault();
        }
        public User signUp(User u)
        {
            if (u.DepartmentId == 0)
            {
                u.IsCeo = true;
            }
            else
            {
                u.IsCeo = false;
            }
            u.CreatedOn = DateTime.Now;
            u.IsActive = true;
            u.UserTypeId = 2;
            u.CreatedBy = 0;
            dbContext.Users.Add(u);
            dbContext.SaveChanges();
            return u;
        }
        public bool forgetPassword(User u)
        {
            using (var db = new MultiplyingEyesEntities())
            {
                db.Users.Attach(u);
                db.Entry(u).Property(x => x.Password).IsModified = true;
                db.SaveChanges();
            }
            return true;
        }

        public object getNearestRetailOutlet(double latitude, double longitude)
        {
            double EarthRadius = 6400000.0, minD = 6400000.0;
            int nearestBranchId = 0;
            List<RetailOutlet> branches = dbContext.RetailOutlets.ToList();
            foreach (var row in branches)
            {
                double bLat = double.Parse(row.Latitude.ToString());
                double bLong = double.Parse(row.Longitude.ToString());

                Double latDistance = DegreeToRadian(bLat - latitude);
                Double lonDistance = DegreeToRadian(bLong - longitude);
                Double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                        + Math.Cos(DegreeToRadian(latitude)) * Math.Cos(DegreeToRadian(bLat))
                        * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
                Double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                Double distance = EarthRadius * c;
                if (distance < minD)
                {
                    minD = distance;
                    nearestBranchId = int.Parse(row.RetailOutletId.ToString());
                }
            }
            var outlet = (from o in dbContext.RetailOutlets
                          join a in dbContext.Areas on o.AreaId equals a.AreaId
                          join r in dbContext.Regions on a.RegionId equals r.RegionId
                          where o.RetailOutletId == nearestBranchId
                          select new
                          {
                              RetailOutletId = o.RetailOutletId,
                              Name = o.Name,
                              Address = o.Address,
                              SapCode = o.SapCode,
                              AreaName = a.Name,
                              AreaManager = a.AreaManager,
                              RegionName = r.Name,
                              RegionManager = r.RegionManager

                          }).FirstOrDefault();

            //RetailOutlet outlet = dbContext.RetailOutlets.Where(x => x.RetailOutletId == nearestBranchId).FirstOrDefault();

            return outlet;
        }
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public User validateUserByEmail(string email)
        {
            return dbContext.Users.Where(x => x.UserTypeId == 2 && x.IsActive == true && x.Email == email).FirstOrDefault();
        }
        public List<Region> getRegions()
        {
            return dbContext.Regions.ToList();
        }
        public List<Area> getAreas()
        {
            return dbContext.Areas.ToList();
        }
        public List<RetailOutlet> getRetailOutlets()
        {
            return dbContext.RetailOutlets.ToList();
        }
        public bool isEmailUnique(string email)
        {
            User user = dbContext.Users.Where(x => x.Email == email && x.IsActive == true).FirstOrDefault();
            if (user == null)
                return true;
            else
                return false;
        }
        public int saveRating(Rating rate)
        {
            rate.RatedOn = DateTime.Now;
            dbContext.Ratings.Add(rate);
            dbContext.SaveChanges();
            return rate.RatingId;
        }
    }
}