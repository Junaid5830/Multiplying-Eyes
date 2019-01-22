using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ME.Models;
using ME.Services;
namespace ME.Controllers
{
    public class AppController : ApiController
    {
        AppService service = new AppService();
        [HttpGet]
        public HttpResponseMessage getQuestions(int categoryId)
        {
            try
            {
                List<Question> list = service.getQuestions(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpPost]
        public HttpResponseMessage saveSurvey(Survey data)
        {
            try
            {
                bool result = service.saveSurvey(data);
                if (result)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message + ex.InnerException);
            }
        }
        [HttpPost]
        public HttpResponseMessage saveSurveyAndroid(Survey data)
        {
            try
            {
                JsonResponse msg = new JsonResponse();

                bool result = service.saveSurvey(data);
                if (result)
                    return Request.CreateResponse(HttpStatusCode.OK, msg);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpPost]
        public HttpResponseMessage validateLogin(User obj)
        {
            try
            {
                User user = service.validateLogin(obj);
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
        }
        [HttpPost]
        public HttpResponseMessage signUp(User obj)
        {
            try
            {
                //improve this check
                if (obj.Email.Contains("totalparco.com.pk") == false)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
                }
                if (service.isEmailUnique(obj.Email) == false)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");

                User user = service.signUp(obj);

                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
        }
        [HttpPost]
        public HttpResponseMessage forgetPassword(User obj)
        {
            try
            {
                if (service.isEmailUnique(obj.Email))
                {
                    if (service.forgetPassword(obj))
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpPost]
        public HttpResponseMessage getNearestRetailOutlet(Location location)
        {
            try
            {
                Object outlet = service.getNearestRetailOutlet(location.latitude, location.longitude);

                if (outlet != null)
                    return Request.CreateResponse(HttpStatusCode.OK, outlet);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpGet]
        public HttpResponseMessage validateUserByEmail(string email)
        {
            try
            {
                User user = service.validateUserByEmail(email);
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, 0);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, 0);
            }
        }
        [HttpGet]
        public HttpResponseMessage getRegions()
        {
            try
            {
                List<Region> list = service.getRegions();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpGet]
        public HttpResponseMessage getAreas()
        {
            try
            {
                List<Area> list = service.getAreas();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpGet]
        public HttpResponseMessage getRetailOutlets()
        {
            try
            {
                List<RetailOutlet> list = service.getRetailOutlets();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
        [HttpPost]
        public HttpResponseMessage saveRating(Rating rate)
        {
            int ratingId = service.saveRating(rate);
            if (ratingId > 0)
                return Request.CreateResponse(HttpStatusCode.OK, ratingId);
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ratingId);
        }
    }
}