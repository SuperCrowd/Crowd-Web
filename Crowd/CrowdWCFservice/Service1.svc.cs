using JdSoft.Apple.Apns.Notifications;
using PushSharp.Apple;
using Portal.Model;
using Portal.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PushSharp;

namespace CrowdWCFservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public CrowdWCFservice.Token.tbl_TokenInfo objTokenInfo = new CrowdWCFservice.Token.tbl_TokenInfo();

        public void DoWork()
        {
        }

        #region IsUserExists

        public GetIsUserExistsResult IsUserExists(string LinkedInID, string DeviceToken)
        {
            GetIsUserExistsResult IsUserExistsResult = new GetIsUserExistsResult();
            GetUserResult UserResult = new GetUserResult();
            List<GetUserSkillResult> lstUserSkillResult = new List<GetUserSkillResult>();
            List<GetUserEmploymentResult> lstUserEmploymentWithRecommendationResult = new List<GetUserEmploymentResult>();
            List<GetUserEducationWithCourseResult> lstUserEducationWithCourseResult = new List<GetUserEducationWithCourseResult>();
            ResultStatus ResultStatus = new ResultStatus();
            try
            {
                writeLog("IsUserExists", "START", LinkedInID);

                UnitOfWork db = new UnitOfWork();

                User objUser = db.User.Get().FirstOrDefault(n => n.LinkedInId.ToUpper() == LinkedInID.ToUpper());

                if (objUser != null)
                {
                    //--------Check for devicetoken already exist or not-----------//
                    User userDeviceToken = db.User.Get().FirstOrDefault(m => m.DeviceToken == DeviceToken);
                    if (userDeviceToken != null)
                    {
                        userDeviceToken.DeviceToken = null;
                        db.User.Update(userDeviceToken);
                        db.SaveChanges();
                    }
                    //------------------------------------------------------------//

                    objUser.DeviceToken = DeviceToken;
                    db.User.Update(objUser);
                    db.SaveChanges();
                    //========================================================//


                    IsUserExistsResult = GetUserDetails(objUser.ID, true);



                }
                else
                {
                    ResultStatus.Status = "0";
                    ResultStatus.StatusMessage = "Requested LinkedInId is not registered with Crowd!";
                    IsUserExistsResult.ResultStatus = ResultStatus;
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                IsUserExistsResult.ResultStatus = ResultStatus;
            }
            writeLog("IsUserExists", "STOP", LinkedInID);
            return IsUserExistsResult;
        }

        #endregion

        #region AddEditUserDetails

        public GetIsUserExistsResult AddEditUserDetails(string UserID, string UserToken, string Email, string FirstName, string LastName, string LocationCity, string LocationState, string LocationCountry, string Industry, string Industry2, string Summary, string PhotoData, string LinkedInId, string DeviceToken, string ExperienceLevel, PrSkill[] UserSkills, PrUserEmployment[] UserEmployments, PrUserEducation[] UserEducations, PrEmploymentRecommendation[] EmploymentRecommendation)
        {
            GetIsUserExistsResult IsUserExistsResult = new GetIsUserExistsResult();
            ResultStatus ResultStatus = new ResultStatus();

            //Comment by himanshu: As per mail on 03-10-14 3rd point. 
            //we need blank array in response. Currently we are getting “<null>” for blank array. This applies to GetUserEducationWithCourseResult, GetUserEducationCourseResult, GetUserEmploymentRecommendationResult, GetUserEmploymentResult, GetUserResult, GetUserSkillResult. 
            GetUserResult blnkGetUserResult = new GetUserResult();
            List<GetUserSkillResult> blnkLstUserSkillResult = new List<GetUserSkillResult>();            
            List<GetUserEmploymentResult> blnkLstUserEmploymentResult = new List<GetUserEmploymentResult>();                        
            List<GetUserEducationWithCourseResult> blnkLstUserEducationWithCourseResult = new List<GetUserEducationWithCourseResult>();            
            List<GetUserEmploymentRecommendationResult> blnkLstUserEmploymentRecommendationResult = new List<GetUserEmploymentRecommendationResult>();            
            //End Comment by himanshu

            try
            {
                writeLog("IsUserExists", "START", Email);
                UnitOfWork db = new UnitOfWork();
                long lngUserID = Convert.ToInt64(UserID);
                Boolean IsInValidParameter = false;
                if (UserID == "" || FirstName == "" || LastName == "" || Industry == "" || LocationCity == "" || LocationCountry == "" || ExperienceLevel == "")
                {
                    IsInValidParameter = true;
                }
                if (UserSkills.Length > 0)
                {
                    foreach (var skill in UserSkills)
                    {
                        if (skill.Skill == "")
                        {
                            IsInValidParameter = true;
                        }
                    }
                }
                if (UserEmployments.Length > 0)
                {
                    foreach (var em in UserEmployments)
                    {
                        if (em.EmployerName == "" || em.Title == "" || em.LocationCity == "" || em.LocationCountry == "" || em.StartYear == "" || em.StartMonth == "")
                        {
                            IsInValidParameter = true;
                        }
                    }
                }
                if (EmploymentRecommendation.Length > 0)
                {
                    foreach (var uer in EmploymentRecommendation)
                    {
                        if (uer.Recommendation == "" || uer.RecommenderName == "")
                        {
                            IsInValidParameter = true;
                        }
                    }
                }

                if (UserEducations.Length > 0)
                {
                    //Comment by himanshu: As per mail on 03-10-14 2nd point. Field degree is optional.
                    foreach (var ed in UserEducations)
                    {
                        if (ed.Name == "" || ed.StartYear == "" || ed.StartMonth == "") //ed.Degree == "" ||
                        {
                            IsInValidParameter = true;
                        }
                        if (ed.UserEducationCourse != null)
                        {
                            foreach (var uec in ed.UserEducationCourse)
                            {
                                if (uec.Course == "")
                                {
                                    IsInValidParameter = true;
                                }
                            }
                        }
                    }
                }


                if (IsInValidParameter == false)
                {
                    if (lngUserID == 0 )
                    {
                        User objChecklinkedInID = db.User.Get().FirstOrDefault(n => n.LinkedInId.ToUpper() == LinkedInId.ToUpper());
                        if (objChecklinkedInID != null)
                        {
                            //Linkedin Id already exist
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "Requested LinkedInId is already registered!";
                            IsUserExistsResult.ResultStatus = ResultStatus;
                            //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                            IsUserExistsResult.GetUserResult = blnkGetUserResult;
                            IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                            IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                            IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                            IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                            //End Comment by himanshu
                        }
                        else
                        {
                            //Check Email already exist or not if provided in request(optional field if user entered then only we need to check this)
                            if (Email != null && Email != "")
                            {
                                User objCheckEmail = db.User.Get().FirstOrDefault(n => n.Email != null && n.Email.ToUpper() == Email.ToUpper());
                                if (objCheckEmail != null)
                                {
                                    //Email already exist
                                    ResultStatus.Status = "0";
                                    ResultStatus.StatusMessage = "Requested Email is already registered!";
                                    IsUserExistsResult.ResultStatus = ResultStatus;
                                    
                                    //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                                    IsUserExistsResult.GetUserResult = blnkGetUserResult;
                                    IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                                    IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                                    IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                                    IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                                    //End Comment by himanshu

                                    writeLog("IsUserExists", "STOP", Email);
                                    return IsUserExistsResult;
                                }
                            }
                            //Register User,UserSkill, UserEmployment, UserEmploymentRecommendation, UserEducation & UserEducationCourse
                            //===================Register User ================//
                            User objNewUser = new User();
                            objNewUser.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                            objNewUser.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                            objNewUser.Email = Email;
                            objNewUser.FirstName = FirstName;
                            objNewUser.LastName = LastName;
                            objNewUser.LocationCity = LocationCity;
                            objNewUser.LocationState = LocationState;
                            objNewUser.LocationCountry = LocationCountry;
                            objNewUser.Industry = Industry;
                            objNewUser.Industry2 = Industry2;
                            objNewUser.Summary = Summary;
                            string filePath = string.Empty;
                            if (PhotoData.Length > 0) // as per rajendra's inst in mail - mail101014
                            {
                                filePath = SaveImage(PhotoData, "Profile", 100, 100);
                                objNewUser.PhotoURL = filePath;
                            }
                            //===============================================================//
                            objNewUser.LinkedInId = LinkedInId;

                            //--------Check for devicetoken already exist or not-----------//
                            User userDeviceToken = new User();
                            userDeviceToken = db.User.Get().FirstOrDefault(m => m.DeviceToken == DeviceToken);
                            if (userDeviceToken != null)
                            {
                                userDeviceToken.DeviceToken = null;
                                db.User.Update(userDeviceToken);
                                db.SaveChanges();
                            }
                            //------------------------------------------------------------//

                            objNewUser.DeviceToken = DeviceToken;
                            objNewUser.ExperienceLevelType = Convert.ToInt32(ExperienceLevel);
                            db.User.Add(objNewUser);
                            db.SaveChanges();
                            //========================================================//

                            User objGetLatestUser = db.User.Get().OrderByDescending(n => n.ID).FirstOrDefault();
                            if (objGetLatestUser != null)
                            {
                                //===================Register UserSkill ================//
                                if (UserSkills.Length > 0)
                                {
                                    foreach (var skill in UserSkills)
                                    {
                                        UserSkill objNewUserSkill = new UserSkill();
                                        objNewUserSkill.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUserSkill.UserID = objGetLatestUser.ID;
                                        objNewUserSkill.Skill = skill.Skill;
                                        db.UserSkill.Add(objNewUserSkill);
                                        db.SaveChanges();
                                    }
                                }
                                //=================================================//

                                //===================Register UserEmployment ================//
                                if (UserEmployments.Length > 0)
                                {
                                    foreach (var employment in UserEmployments)
                                    {
                                        UserEmployment objNewUserEmployment = new UserEmployment();
                                        objNewUserEmployment.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUserEmployment.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUserEmployment.UserID = objGetLatestUser.ID;
                                        objNewUserEmployment.EmployerName = employment.EmployerName;
                                        objNewUserEmployment.Title = employment.Title;
                                        objNewUserEmployment.LocationCity = employment.LocationCity;
                                        objNewUserEmployment.LocationState = employment.LocationState;
                                        objNewUserEmployment.LocationCountry = employment.LocationCountry;
                                        objNewUserEmployment.StartYear = Convert.ToInt32(employment.StartYear);
                                        if (employment.EndYear != null && employment.EndYear != "")
                                        {
                                            objNewUserEmployment.EndYear = Convert.ToInt32(employment.EndYear);
                                        }
                                        else
                                        {
                                            objNewUserEmployment.EndYear = null;
                                        }
                                        objNewUserEmployment.Summary = employment.Summary;
                                        objNewUserEmployment.StartMonth = Convert.ToInt32(employment.StartMonth);
                                        if (employment.EndMonth != null && employment.EndMonth != "")
                                        {
                                            objNewUserEmployment.EndMonth = Convert.ToInt32(employment.EndMonth);
                                        }
                                        else
                                        {
                                            objNewUserEmployment.EndMonth = null;
                                        }
                                        db.UserEmployment.Add(objNewUserEmployment);
                                        db.SaveChanges();
                                    }
                                }
                                //=====================================================//

                                //--------------                                   

                                if (EmploymentRecommendation.Length > 0)
                                {
                                    foreach (var recommendation in EmploymentRecommendation)
                                    {
                                        UserEmploymentRecommendation objNewUER = new UserEmploymentRecommendation();
                                        objNewUER.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUER.UserID = objGetLatestUser.ID;
                                        objNewUER.Recommendation = recommendation.Recommendation;
                                        objNewUER.RecommenderName = recommendation.RecommenderName;
                                        db.UserEmploymentRecommendation.Add(objNewUER);
                                        db.SaveChanges();
                                    }
                                }

                                //-----------------

                                //===================Register UserEducation ================//
                                if (UserEducations.Length > 0)
                                {
                                    foreach (var education in UserEducations)
                                    {
                                        UserEducation objNewUserEducation = new UserEducation();
                                        objNewUserEducation.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUserEducation.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewUserEducation.UserID = objGetLatestUser.ID;
                                        objNewUserEducation.Name = education.Name;
                                        objNewUserEducation.Degree = education.Degree;
                                        objNewUserEducation.StartYear = Convert.ToInt32(education.StartYear);
                                        if (education.EndYear != null && education.EndYear != "")
                                        {
                                            objNewUserEducation.EndYear = Convert.ToInt32(education.EndYear);
                                        }
                                        else
                                        {
                                            objNewUserEducation.EndYear = null;
                                        }
                                        objNewUserEducation.StartMonth = Convert.ToInt32(education.StartMonth);
                                        if (education.EndMonth != null && education.EndMonth != "")
                                        {
                                            objNewUserEducation.EndMonth = Convert.ToInt32(education.EndMonth);
                                        }
                                        else
                                        {
                                            objNewUserEducation.EndMonth = null;
                                        }
                                        db.UserEducation.Add(objNewUserEducation);
                                        db.SaveChanges();

                                        long lngLatestEducationId = db.UserEducation.Get().OrderByDescending(n => n.ID).FirstOrDefault().ID;
                                        if (education.UserEducationCourse != null)
                                        {
                                            foreach (var course in education.UserEducationCourse)
                                            {
                                                UserEducationCourse objNewUEC = new UserEducationCourse();
                                                objNewUEC.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                objNewUEC.EducationID = lngLatestEducationId;
                                                objNewUEC.Course = course.Course;
                                                db.UserEducationCourse.Add(objNewUEC);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                //=====================================================//

                                //Prepare response
                                IsUserExistsResult = GetUserDetails(objGetLatestUser.ID, true);
                            }
                        }
                    }
                    else if (lngUserID > 0)
                    {
                        objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
                        if (objTokenInfo != null && objTokenInfo.EmailID != null)
                        {
                            User objChecklinkedInID = db.User.Get().FirstOrDefault(n => n.LinkedInId.ToUpper() == LinkedInId.ToUpper() && n.ID != lngUserID);
                            if (objChecklinkedInID != null)
                            {
                                //Linkedin Id already exist
                                ResultStatus.Status = "0";
                                ResultStatus.StatusMessage = "Requested LinkedInId is already registered!";
                                IsUserExistsResult.ResultStatus = ResultStatus;

                                //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                                IsUserExistsResult.GetUserResult = blnkGetUserResult;
                                IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                                IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                                IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                                IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                                //End Comment by himanshu
                            }
                            else
                            {
                                //Check Email already exist or not if provided in request
                                if (Email != null && Email != "")
                                {
                                    User objCheckEmail = db.User.Get().FirstOrDefault(n => n.Email != null && n.Email.ToUpper() == Email.ToUpper() && n.ID != lngUserID);
                                    if (objCheckEmail != null)
                                    {
                                        //Email already exist
                                        ResultStatus.Status = "0";
                                        ResultStatus.StatusMessage = "Requested Email is already registered!";
                                        IsUserExistsResult.ResultStatus = ResultStatus;

                                        //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                                        IsUserExistsResult.GetUserResult = blnkGetUserResult;
                                        IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                                        IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                                        IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                                        IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                                        //End Comment by himanshu

                                        writeLog("IsUserExists", "STOP", Email);
                                        return IsUserExistsResult;
                                    }
                                }
                                User objGetUser = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);
                                if (objGetUser != null)
                                {
                                    //Update User,UserSkill, UserEmployment, UserEmploymentRecommendation, UserEducation & UserEducationCourse

                                    //===================Update User ================//
                                    objGetUser.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objGetUser.Email = Email;
                                    objGetUser.FirstName = FirstName;
                                    objGetUser.LastName = LastName;
                                    objGetUser.LocationCity = LocationCity;
                                    objGetUser.LocationState = LocationState;
                                    objGetUser.LocationCountry = LocationCountry;
                                    objGetUser.Industry = Industry;
                                    objGetUser.Industry2 = Industry2;
                                    objGetUser.Summary = Summary;
                                    //=====convert base64string in image and save it in folder=======//
                                    string filePath = "";
                                    if (PhotoData.Length > 0) // as per rajendra's inst in mail - mail101014
                                    {
                                        filePath = SaveImage(PhotoData, "Profile", 100, 100);
                                        objGetUser.PhotoURL = filePath;
                                    }
                                    //===============================================================//
                                    objGetUser.LinkedInId = LinkedInId;

                                    //--------Check for devicetoken already exist or not-----------//
                                    User userDeviceToken = new User();
                                    userDeviceToken = db.User.Get().FirstOrDefault(m => m.DeviceToken == DeviceToken);
                                    if (userDeviceToken != null)
                                    {
                                        userDeviceToken.DeviceToken = null;
                                        db.User.Update(userDeviceToken);
                                        db.SaveChanges();
                                    }
                                    //------------------------------------------------------------//

                                    objGetUser.DeviceToken = DeviceToken;
                                    objGetUser.ExperienceLevelType = Convert.ToInt32(ExperienceLevel);
                                    db.User.Update(objGetUser);
                                    db.SaveChanges();
                                    //========================================================//

                                    //===================Update UserSkill ================//
                                    if (UserSkills.Length > 0)
                                    {
                                        //=========Delete All skills========//
                                        List<UserSkill> objGetUserSkill = db.UserSkill.Get().Where(n => n.UserID == objGetUser.ID).ToList();
                                        if (objGetUserSkill.Count > 0)
                                        {
                                            foreach (var us in objGetUserSkill)
                                            {
                                                db.UserSkill.Remove(us);
                                                db.SaveChanges();
                                            }
                                        }
                                        //================================//

                                        //=========Add New skills========//
                                        foreach (var skill in UserSkills)
                                        {
                                            UserSkill objNewUserSkill = new UserSkill();
                                            objNewUserSkill.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUserSkill.UserID = objGetUser.ID;
                                            objNewUserSkill.Skill = skill.Skill;
                                            db.UserSkill.Add(objNewUserSkill);
                                            db.SaveChanges();
                                        }
                                        //=================================//
                                    }
                                    //=================================================//


                                    //===================Update UserEmployment ================//
                                    if (UserEmployments.Length > 0)
                                    {
                                        //=========Delete All UserEmployment,Employment recommendation========//
                                        List<UserEmployment> objGetUserEmployment = db.UserEmployment.Get().Where(n => n.UserID == objGetUser.ID).ToList();
                                        if (objGetUserEmployment.Count > 0)
                                        {
                                            foreach (var ue in objGetUserEmployment)
                                            {
                                                db.UserEmployment.Remove(ue);
                                                db.SaveChanges();
                                            }
                                        }
                                        //================================//

                                        //=========Add New UserEmployment========//

                                        foreach (var employment in UserEmployments)
                                        {
                                            UserEmployment objNewUserEmployment = new UserEmployment();
                                            objNewUserEmployment.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUserEmployment.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUserEmployment.UserID = objGetUser.ID;
                                            objNewUserEmployment.EmployerName = employment.EmployerName;
                                            objNewUserEmployment.Title = employment.Title;
                                            objNewUserEmployment.LocationCity = employment.LocationCity;
                                            objNewUserEmployment.LocationState = employment.LocationState;
                                            objNewUserEmployment.LocationCountry = employment.LocationCountry;
                                            objNewUserEmployment.StartYear = Convert.ToInt32(employment.StartYear);
                                            if (employment.EndYear != null && employment.EndYear != "")
                                            {
                                                objNewUserEmployment.EndYear = Convert.ToInt32(employment.EndYear);
                                            }
                                            else
                                            {
                                                objNewUserEmployment.EndYear = null;
                                            }
                                            objNewUserEmployment.Summary = employment.Summary;
                                            objNewUserEmployment.StartMonth = Convert.ToInt32(employment.StartMonth);
                                            if (employment.EndMonth != null && employment.EndMonth != "")
                                            {
                                                objNewUserEmployment.EndMonth = Convert.ToInt32(employment.EndMonth);
                                            }
                                            else
                                            {
                                                objNewUserEmployment.EndMonth = null;
                                            }
                                            db.UserEmployment.Add(objNewUserEmployment);
                                            db.SaveChanges();
                                        }
                                        //=================================//
                                    }
                                    //=================================================//

                                    if (EmploymentRecommendation != null)
                                    {
                                        //------------------DeleteEmploymentRecommendation--------------//
                                        List<UserEmploymentRecommendation> objGetUER = db.UserEmploymentRecommendation.Get().Where(n => n.UserID == objGetUser.ID).ToList();
                                        if (objGetUER.Count > 0)
                                        {
                                            foreach (var uer in objGetUER)
                                            {
                                                db.UserEmploymentRecommendation.Remove(uer);
                                                db.SaveChanges();
                                            }
                                        }
                                        //--------------------------------------------------------------//

                                        foreach (var recommendation in EmploymentRecommendation)
                                        {
                                            UserEmploymentRecommendation objNewUER = new UserEmploymentRecommendation();
                                            objNewUER.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUER.UserID = objGetUser.ID;
                                            objNewUER.Recommendation = recommendation.Recommendation;
                                            objNewUER.RecommenderName = recommendation.RecommenderName;
                                            db.UserEmploymentRecommendation.Add(objNewUER);
                                            db.SaveChanges();
                                        }
                                    }


                                    //===================Update UserEducations ================//
                                    if (UserEducations.Length > 0)
                                    {
                                        //=========Delete All UserEducations ,UserEducationCourses========//
                                        List<UserEducation> objGetUserEducation = db.UserEducation.Get().Where(n => n.UserID == objGetUser.ID).ToList();
                                        if (objGetUserEducation.Count > 0)
                                        {
                                            foreach (var ue in objGetUserEducation)
                                            {
                                                //------------------DeleteUserEducationCourses--------------//
                                                List<UserEducationCourse> objGetUEC = db.UserEducationCourse.Get().Where(n => n.EducationID == ue.ID).ToList();
                                                if (objGetUEC.Count > 0)
                                                {
                                                    foreach (var uec in objGetUEC)
                                                    {
                                                        db.UserEducationCourse.Remove(uec);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                //--------------------------------------------------------------//

                                                db.UserEducation.Remove(ue);
                                                db.SaveChanges();
                                            }
                                        }
                                        //================================//

                                        //===================Add New UserEducation ================//
                                        foreach (var education in UserEducations)
                                        {
                                            UserEducation objNewUserEducation = new UserEducation();
                                            objNewUserEducation.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUserEducation.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                            objNewUserEducation.UserID = objGetUser.ID;
                                            objNewUserEducation.Name = education.Name;
                                            objNewUserEducation.Degree = education.Degree;
                                            objNewUserEducation.StartYear = Convert.ToInt32(education.StartYear);
                                            if (education.EndYear != null && education.EndYear != "")
                                            {
                                                objNewUserEducation.EndYear = Convert.ToInt32(education.EndYear);
                                            }
                                            else
                                            {
                                                objNewUserEducation.EndYear = null;
                                            }
                                            objNewUserEducation.StartMonth = Convert.ToInt32(education.StartMonth);
                                            if (education.EndMonth != null && education.EndMonth != "")
                                            {
                                                objNewUserEducation.EndMonth = Convert.ToInt32(education.EndMonth);
                                            }
                                            else
                                            {
                                                objNewUserEducation.EndMonth = null;
                                            }
                                            db.UserEducation.Add(objNewUserEducation);
                                            db.SaveChanges();

                                            long lngLatestEducationId = db.UserEducation.Get().OrderByDescending(n => n.ID).FirstOrDefault().ID;
                                            if (education.UserEducationCourse != null)
                                            {
                                                foreach (var course in education.UserEducationCourse)
                                                {
                                                    UserEducationCourse objNewUEC = new UserEducationCourse();
                                                    objNewUEC.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                    objNewUEC.EducationID = lngLatestEducationId;
                                                    objNewUEC.Course = course.Course;
                                                    db.UserEducationCourse.Add(objNewUEC);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        //=====================================================//

                                        //=================================================//
                                       
                                    }
                                    //Comment by himanshu: as per mail 03-10-14 (1st point).
                                    //else
                                    //{
                                    //    ResultStatus.Status = "0";
                                    //    ResultStatus.StatusMessage = "User does not exist!";
                                    //    IsUserExistsResult.ResultStatus = ResultStatus;
                                    //}
                                    //Close Comment by himanshu

                                    IsUserExistsResult = GetUserDetails(objGetUser.ID, false);
                                }
                            }
                        }
                        else
                        {
                            throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                        }
                    }
                }
                else
                {
                    ResultStatus.Status = "0";
                    ResultStatus.StatusMessage = "Invalid Parameter!";
                    IsUserExistsResult.ResultStatus = ResultStatus;
                    IsUserExistsResult.GetUserResult = blnkGetUserResult;

                    //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                    IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                    IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                    IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                    IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                    //End Comment by himanshu
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                IsUserExistsResult.ResultStatus = ResultStatus;

                //Comment by himanshu: As per mail on 03-10-14 3rd point. 
                IsUserExistsResult.GetUserResult = blnkGetUserResult;
                IsUserExistsResult.GetUserSkillResult = blnkLstUserSkillResult;
                IsUserExistsResult.GetUserEmploymentResult = blnkLstUserEmploymentResult;
                IsUserExistsResult.GetUserEducationWithCourseResult = blnkLstUserEducationWithCourseResult;
                IsUserExistsResult.GetUserEmploymentRecommendationResult = blnkLstUserEmploymentRecommendationResult;
                //End Comment by himanshu
            }
            writeLog("IsUserExists", "STOP", Email);
            return IsUserExistsResult;
        }

        #endregion

        #region GetUserDetails

        public GetUserDetailsResult GetUserDetails(string UserID, string UserToken, string OtherUserID)
        {
            GetUserDetailsResult UserDetailResult = new GetUserDetailsResult();
            GetIsUserExistsResult IsUserExistResult = new GetIsUserExistsResult();
            ResultStatus ResultStatus = new ResultStatus();
            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetUserDetails", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngOtherUserID = Convert.ToInt64(OtherUserID);
                    IsUserExistResult = GetUserDetails(lngOtherUserID, false);

                    //not required if Requested UserID and OtherUserID are different.
                    if (lngUserID != lngOtherUserID)
                    {
                        if (IsUserExistResult.GetUserResult != null)
                        {
                            IsUserExistResult.GetUserResult.NumberOfUnreadMessage = "";
                        }
                    }

                    UserDetailResult.UserDetailResult = IsUserExistResult;

                    //not required if requested UserID and Requested OtherUserID are same.
                    if (lngUserID != lngOtherUserID)
                    {
                        Follow objFollow = db.Follow.Get().FirstOrDefault(n => n.FollowerUserID == lngUserID && n.FollowingUserID == lngOtherUserID);
                        if (objFollow != null)
                        {
                            if (IsUserExistResult != null)
                            {
                                UserDetailResult.UserFollowStatus = "True";
                            }
                        }
                    }

                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                UserDetailResult.UserDetailResult = IsUserExistResult;
                UserDetailResult.UserDetailResult.ResultStatus = ResultStatus;
            }
            writeLog("GetUserDetails", "STOP", UserID);
            return UserDetailResult;
        }

        #endregion

        #region GetActivityFeeds

        public GetActivityFeedsResult GetActivityFeeds(string UserID, string UserToken, string PageNumber)
        {
            GetActivityFeedsResult ActivityFeedsList = new GetActivityFeedsResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetActivityFeeds> ActivityFeeds = new List<GetActivityFeeds>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetActivityFeeds", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);

                    List<Feed> objGetFeed = db.Feed.Get().Where(n => n.UserID == lngUserID).OrderByDescending(n => n.DateCreated).ToList();

                    if (objGetFeed.Count > 0)
                    {
                        if (PageNumber != null && PageNumber != "")
                        {
                            //Paging parameters
                            int pagesize = 10;
                            int currentpage = Convert.ToInt32(PageNumber);
                            int currentsize = pagesize;
                            int skipcount = currentsize * (currentpage - 1);
                            int takecount = currentsize * currentpage;
                            objGetFeed = objGetFeed.Take(takecount).Skip(skipcount).ToList();
                        }

                        if (objGetFeed.Count > 0)
                        {
                            foreach (Feed feed in objGetFeed)
                            {
                                GetActivityFeeds objCurrFeed = new GetActivityFeeds();
                                objCurrFeed.ID = Convert.ToString(feed.ID);
                                objCurrFeed.DateCreated = Convert.ToString(feed.DateCreated);
                                objCurrFeed.UserId = Convert.ToString(feed.UserID);
                                objCurrFeed.Type = Convert.ToString(feed.FeedTypeID);
                                objCurrFeed.JobID = Convert.ToString(feed.JobID);

                                GetJobDetails JobDetail = new GetJobDetails();
                                if (feed.JobID != null && feed.JobID != 0)
                                {
                                    Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == feed.JobID);
                                    if (objGetJob != null)
                                    {
                                        JobDetail.ID = Convert.ToString(objGetJob.ID);
                                        JobDetail.DateCreated = Convert.ToString(objGetJob.DateCreated);
                                        JobDetail.DateModified = Convert.ToString(objGetJob.DateModified);
                                        JobDetail.UserId = Convert.ToString(objGetJob.UserID);
                                        JobDetail.LocationCity = objGetJob.LocationCity;
                                        JobDetail.LocationState = objGetJob.LocationState;
                                        JobDetail.LocationCountry = objGetJob.LocationCountry;
                                        JobDetail.Industry = objGetJob.Industry;
                                        JobDetail.Industry2 = objGetJob.Industry2;
                                        JobDetail.Title = objGetJob.Title;
                                        JobDetail.Responsibilities = objGetJob.Responsibilities;
                                        JobDetail.Qualifications = objGetJob.Qualifications;
                                        JobDetail.Company = objGetJob.Company;
                                        JobDetail.EmployerIntroduction = objGetJob.EmployerIntroduction;
                                        JobDetail.URL = objGetJob.URL;
                                        JobDetail.ShareURL = objGetJob.ShareURL;
                                        JobDetail.State = Convert.ToString(objGetJob.State);
                                    }                                    
                                }
                                objCurrFeed.JobDetail = JobDetail;

                                GetUserDetailForFeed UserDetail = new GetUserDetailForFeed();
                                objCurrFeed.OtherUserID = Convert.ToString(feed.OtherUserID);
                                if (feed.OtherUserID != null && feed.OtherUserID != 0)
                                {
                                    User objUser = db.User.Get().FirstOrDefault(n => n.ID == feed.OtherUserID);
                                    if (objUser != null)
                                    {
                                        UserDetail.UserId = Convert.ToString(objUser.ID);
                                        UserDetail.DateCreated = Convert.ToString(objUser.DateCreated);
                                        UserDetail.DateModified = Convert.ToString(objUser.DateModified);
                                        UserDetail.Email = objUser.Email;
                                        UserDetail.FirstName = objUser.FirstName;
                                        UserDetail.LastName = objUser.LastName;
                                        UserDetail.LocationCity = objUser.LocationCity;
                                        UserDetail.LocationState = objUser.LocationState;
                                        UserDetail.LocationCountry = objUser.LocationCountry;
                                        UserDetail.Industry = objUser.Industry;
                                        UserDetail.Industry2 = objUser.Industry2;
                                        UserDetail.Summary = objUser.Summary;
                                        UserDetail.PhotoURL = objUser.PhotoURL;
                                        UserDetail.LinkedInId = objUser.LinkedInId;
                                        UserDetail.ExperienceLevel = Convert.ToString(objUser.ExperienceLevelType);
                                    }                                   
                                }
                                objCurrFeed.OtherUserDetails = UserDetail;
                                ActivityFeeds.Add(objCurrFeed);
                            }

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            ActivityFeedsList.ResultStatus = ResultStatus;
                            ActivityFeedsList.GetActivityFeeds = ActivityFeeds;
                        }
                        else
                        {
                            ResultStatus.Status = "0"; // as per rajendra's instruction in  mail on dated 10/10/2014
                            ResultStatus.StatusMessage = "No feeds on this Page Number!";
                            ActivityFeedsList.ResultStatus = ResultStatus;
                            ActivityFeedsList.GetActivityFeeds = ActivityFeeds;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0"; // as per rajendra's instruction in  mail on dated 10/10/2014
                        ResultStatus.StatusMessage = "No feeds!";
                        ActivityFeedsList.ResultStatus = ResultStatus;
                        ActivityFeedsList.GetActivityFeeds = ActivityFeeds;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                ActivityFeedsList.ResultStatus = ResultStatus;
                ActivityFeedsList.GetActivityFeeds = ActivityFeeds;
            }
            writeLog("GetActivityFeeds", "STOP", UserID);
            return ActivityFeedsList;
        }

        #endregion

        #region GetMyCrowd

        public GetMyCrowdResult GetMyCrowd(string UserID, string UserToken, string PageNumber)
        {
            GetMyCrowdResult MyCrowdResult = new GetMyCrowdResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetUserDetailForCrowd> lstFollowingMeUser = new List<GetUserDetailForCrowd>();
            List<GetUserDetailForCrowd> lstIAmFollowingUser = new List<GetUserDetailForCrowd>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetMyCrowd", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);

                    //Get Following Me User - users who follow requested UserID 
                    List<Int64> lstFollowerUserID = db.Follow.Get().Where(n => n.FollowingUserID == lngUserID).Select(n => n.FollowerUserID).ToList();
                    lstFollowerUserID = lstFollowerUserID.Distinct().ToList();

                    if (lstFollowerUserID.Count > 0)
                    {
                        foreach (Int64 follower in lstFollowerUserID)
                        {
                            User objGetFollowerDetail = db.User.Get().FirstOrDefault(n => n.ID == follower);
                            if (objGetFollowerDetail != null)
                            {
                                //==============================Follower Response=============================================//
                                GetUserDetailForCrowd objCurrFollower = new GetUserDetailForCrowd();
                                objCurrFollower.UserId = Convert.ToString(objGetFollowerDetail.ID);
                                objCurrFollower.DateCreated = Convert.ToString(objGetFollowerDetail.DateCreated);
                                objCurrFollower.DateModified = Convert.ToString(objGetFollowerDetail.DateModified);
                                objCurrFollower.Email = objGetFollowerDetail.Email;
                                objCurrFollower.FirstName = objGetFollowerDetail.FirstName;
                                objCurrFollower.LastName = objGetFollowerDetail.LastName;
                                objCurrFollower.LocationCity = objGetFollowerDetail.LocationCity;
                                objCurrFollower.LocationState = objGetFollowerDetail.LocationState;
                                objCurrFollower.LocationCountry = objGetFollowerDetail.LocationCountry;
                                objCurrFollower.Industry = objGetFollowerDetail.Industry;
                                objCurrFollower.Industry2 = objGetFollowerDetail.Industry2;
                                objCurrFollower.Summary = objGetFollowerDetail.Summary;
                                objCurrFollower.PhotoURL = objGetFollowerDetail.PhotoURL;
                                objCurrFollower.LinkedInId = objGetFollowerDetail.LinkedInId;
                                objCurrFollower.ExperienceLevel = Convert.ToString(objGetFollowerDetail.ExperienceLevelType);

                                //---------------------UserEmployment Response-------------------------------//
                                List<GetUserEmployment> lstCurrentEmployerList = new List<GetUserEmployment>();
                                List<UserEmployment> objUserEmploymentList = db.UserEmployment.Get().Where(n => n.UserID == objGetFollowerDetail.ID && (n.EndMonth == 0 || n.EndMonth == null)).ToList();
                                if (objUserEmploymentList.Count > 0)
                                {
                                    foreach (var ue in objUserEmploymentList)
                                    {
                                        GetUserEmployment objCurrEmployment = new GetUserEmployment();
                                        objCurrEmployment.ID = Convert.ToString(ue.ID);
                                        objCurrEmployment.DateCreated = Convert.ToString(ue.DateCreated);
                                        objCurrEmployment.DateModified = Convert.ToString(ue.DateModified);
                                        objCurrEmployment.UserId = Convert.ToString(ue.UserID);
                                        objCurrEmployment.EmployerName = ue.EmployerName;
                                        objCurrEmployment.Title = ue.Title;
                                        objCurrEmployment.LocationCity = ue.LocationCity;
                                        objCurrEmployment.LocationState = ue.LocationState;
                                        objCurrEmployment.LocationCountry = ue.LocationCountry;
                                        objCurrEmployment.StartMonth = Convert.ToString(ue.StartMonth);
                                        objCurrEmployment.StartYear = Convert.ToString(ue.StartYear);
                                        objCurrEmployment.EndMonth = Convert.ToString(ue.EndMonth);
                                        objCurrEmployment.EndYear = Convert.ToString(ue.EndYear);
                                        objCurrEmployment.Summary = ue.Summary;
                                        lstCurrentEmployerList.Add(objCurrEmployment);
                                    }
                                }
                                objCurrFollower.UserCurrentEmployer = lstCurrentEmployerList;
                                //---------------------------------------------------------------------------------//

                                lstFollowingMeUser.Add(objCurrFollower);
                                //========================================================================================================//
                            }
                        }
                        //Order by
                        lstFollowingMeUser = lstFollowingMeUser.OrderBy(n => n.FirstName).ThenBy(n => n.LastName).ToList();
                    }

                    //Get I am Following User - users whom requested user is following
                    List<Int64> lstFollowingUserID = db.Follow.Get().Where(n => n.FollowerUserID == lngUserID).Select(n => n.FollowingUserID).ToList();
                    lstFollowingUserID = lstFollowingUserID.Distinct().ToList();

                    if (lstFollowingUserID.Count > 0)
                    {
                        foreach (Int64 following in lstFollowingUserID)
                        {
                            User objGetFollowingDetail = db.User.Get().FirstOrDefault(n => n.ID == following);
                            if (objGetFollowingDetail != null)
                            {
                                //==============================Following Response=============================================//
                                GetUserDetailForCrowd objCurrFollowing = new GetUserDetailForCrowd();
                                objCurrFollowing.UserId = Convert.ToString(objGetFollowingDetail.ID);
                                objCurrFollowing.DateCreated = Convert.ToString(objGetFollowingDetail.DateCreated);
                                objCurrFollowing.DateModified = Convert.ToString(objGetFollowingDetail.DateModified);
                                objCurrFollowing.Email = objGetFollowingDetail.Email;
                                objCurrFollowing.FirstName = objGetFollowingDetail.FirstName;
                                objCurrFollowing.LastName = objGetFollowingDetail.LastName;
                                objCurrFollowing.LocationCity = objGetFollowingDetail.LocationCity;
                                objCurrFollowing.LocationState = objGetFollowingDetail.LocationState;
                                objCurrFollowing.LocationCountry = objGetFollowingDetail.LocationCountry;
                                objCurrFollowing.Industry = objGetFollowingDetail.Industry;
                                objCurrFollowing.Industry2 = objGetFollowingDetail.Industry2;
                                objCurrFollowing.Summary = objGetFollowingDetail.Summary;
                                objCurrFollowing.PhotoURL = objGetFollowingDetail.PhotoURL;
                                objCurrFollowing.LinkedInId = objGetFollowingDetail.LinkedInId;
                                objCurrFollowing.ExperienceLevel = Convert.ToString(objGetFollowingDetail.ExperienceLevelType);

                                //--------------------------------Useremployment Response------------------------------------//
                                List<GetUserEmployment> lstCurrentEmployerList = new List<GetUserEmployment>();
                                List<UserEmployment> objUserEmploymentList = db.UserEmployment.Get().Where(n => n.UserID == objGetFollowingDetail.ID && (n.EndMonth == 0 || n.EndMonth == null)).ToList();
                                if (objUserEmploymentList.Count > 0)
                                {
                                    foreach (var ue in objUserEmploymentList)
                                    {
                                        GetUserEmployment objCurrEmployment = new GetUserEmployment();
                                        objCurrEmployment.ID = Convert.ToString(ue.ID);
                                        objCurrEmployment.DateCreated = Convert.ToString(ue.DateCreated);
                                        objCurrEmployment.DateModified = Convert.ToString(ue.DateModified);
                                        objCurrEmployment.UserId = Convert.ToString(ue.UserID);
                                        objCurrEmployment.EmployerName = ue.EmployerName;
                                        objCurrEmployment.Title = ue.Title;
                                        objCurrEmployment.LocationCity = ue.LocationCity;
                                        objCurrEmployment.LocationState = ue.LocationState;
                                        objCurrEmployment.LocationCountry = ue.LocationCountry;
                                        objCurrEmployment.StartMonth = Convert.ToString(ue.StartMonth);
                                        objCurrEmployment.StartYear = Convert.ToString(ue.StartYear);
                                        objCurrEmployment.EndMonth = Convert.ToString(ue.EndMonth);
                                        objCurrEmployment.EndYear = Convert.ToString(ue.EndYear);
                                        objCurrEmployment.Summary = ue.Summary;
                                        lstCurrentEmployerList.Add(objCurrEmployment);
                                    }
                                }
                                objCurrFollowing.UserCurrentEmployer = lstCurrentEmployerList;
                                //-----------------------------------------------------------------------------//

                                lstIAmFollowingUser.Add(objCurrFollowing);
                                //===================================================================================//
                            }
                        }

                        //Order by
                        lstIAmFollowingUser = lstIAmFollowingUser.OrderBy(n => n.FirstName).ThenBy(n => n.LastName).ToList();
                    }

                    if (PageNumber != null && PageNumber != "")
                    {
                        //paging                   
                        int pagesize = 10;
                        int currentpage = Convert.ToInt32(PageNumber);
                        int currentsize = pagesize;
                        int skipcount = currentsize * (currentpage - 1);
                        int takecount = currentsize * currentpage;

                        if (lstFollowingMeUser.Count > 0)
                        {
                            lstFollowingMeUser = lstFollowingMeUser.Take(takecount).Skip(skipcount).ToList();
                        }
                        if (lstIAmFollowingUser.Count > 0)
                        {
                            lstIAmFollowingUser = lstIAmFollowingUser.Take(takecount).Skip(skipcount).ToList();
                        }
                    }

                    //Response
                    ResultStatus.Status = "1";
                    ResultStatus.StatusMessage = "";
                    MyCrowdResult.ResultStatus = ResultStatus;
                    MyCrowdResult.FollowingMeUser = lstFollowingMeUser;
                    MyCrowdResult.IAmFollowingUser = lstIAmFollowingUser;

                    if (MyCrowdResult.FollowingMeUser.Count == 0 && MyCrowdResult.IAmFollowingUser.Count == 0)
                    {
                        ResultStatus.Status = "0";//as per rajendra's instruction in mail on dated 10/10/2014
                        ResultStatus.StatusMessage = "No Records!";
                        MyCrowdResult.ResultStatus = ResultStatus;
                        MyCrowdResult.FollowingMeUser = lstFollowingMeUser;
                        MyCrowdResult.IAmFollowingUser = lstIAmFollowingUser;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                MyCrowdResult.ResultStatus = ResultStatus;
                MyCrowdResult.FollowingMeUser = lstFollowingMeUser;
                MyCrowdResult.IAmFollowingUser = lstIAmFollowingUser;
            }
            writeLog("GetMyCrowd", "STOP", UserID);
            return MyCrowdResult;
        }

        #endregion

        #region FollowUnfollowUser

        public GetFollowUnfollowUserResult FollowUnfollowUser(string UserID, string UserToken, string FollowUserID, string Status)
        {
            GetFollowUnfollowUserResult FollowUnfollowResult = new GetFollowUnfollowUserResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("FollowUnfollowUser", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    if (Status != null && Status != "")
                    {
                        long lngUserID = Convert.ToInt64(UserID);
                        long lngFollowUserID = Convert.ToInt64(FollowUserID);
                        Boolean blnFlagResult = false;
                        if (Status == "1")
                        {
                            //follow
                            //============add new record in Follow table =============//
                            Follow objNewFollow = new Follow();
                            objNewFollow.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                            objNewFollow.FollowerUserID = lngUserID;
                            objNewFollow.FollowingUserID = lngFollowUserID;
                            db.Follow.Add(objNewFollow);
                            db.SaveChanges();
                            //========================================================//

                            //============add new record in Feed Table=================//
                            Feed objNewFeed = new Feed();
                            objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                            objNewFeed.UserID = lngFollowUserID;
                            objNewFeed.FeedTypeID = 1;
                            objNewFeed.JobID = null;
                            objNewFeed.OtherUserID = lngUserID;
                            db.Feed.Add(objNewFeed);
                            db.SaveChanges();
                            //=========================================================//
                            blnFlagResult = true;

                            //================Code for push notification===========//
                            User objUserInfo = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);

                            //Get device token of receiver
                            string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == lngFollowUserID).DeviceToken;
                            ///
                            if (UserDeviceToken != null)
                            {                                
                                var msg = objUserInfo.FirstName + " " + objUserInfo.LastName + " is now following you.";
                                Dictionary<string, string> objParam = new Dictionary<string, string>();

                                objParam.Add("testDeviceToken", UserDeviceToken);
                                objParam.Add("pushMessage", msg);
                                objParam.Add("sourceTable", "FollowUser");
                                objParam.Add("UserID", lngUserID.ToString());
                                SendNotificationMessage(objParam);
                            }
                            //=========================end=======================//

                        }
                        else if (Status == "0")
                        {
                            //unfollow
                            //==============delete record from Follow table =============//
                            List<Follow> objGetFollowList = db.Follow.Get().Where(n => n.FollowerUserID == lngUserID && n.FollowingUserID == lngFollowUserID).ToList();
                            if (objGetFollowList.Count > 0)
                            {
                                foreach (Follow follow in objGetFollowList)
                                {
                                    db.Follow.Remove(follow);
                                    db.SaveChanges();
                                }
                                blnFlagResult = true;
                            }
                            //===========================================================//
                        }

                        if (blnFlagResult == true)
                        {
                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            FollowUnfollowResult.ResultStatus = ResultStatus;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "";
                            FollowUnfollowResult.ResultStatus = ResultStatus;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Invalid Parameter!";
                        FollowUnfollowResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                FollowUnfollowResult.ResultStatus = ResultStatus;
            }
            writeLog("FollowUnfollowUser", "STOP", UserID);
            return FollowUnfollowResult;
        }

        #endregion

        #region AddEditJob

        public GetAddEditJobResult AddEditJob(string UserID, string UserToken, string JobID, string Company, string Industry, string Industry2, string Title, string LocationCity, string LocationState, string LocationCountry, string ExperienceLevel, string Responsibilities, string Qualifications, string EmployerIntroduction, string JobURL, PrSkill[] Skills)
        {
            GetAddEditJobResult AddEditJobResult = new GetAddEditJobResult();
            ResultStatus ResultStatus = new ResultStatus();
            GetJobDetailsWithSkill JobDetailsWithSkill = new GetJobDetailsWithSkill();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("AddEditJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);
                    if (lngJobID == 0)
                    {
                        //Add New Job
                        Job objNewJob = new Job();
                        objNewJob.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                        objNewJob.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                        objNewJob.UserID = lngUserID;
                        objNewJob.Company = Company;
                        objNewJob.Industry = Industry;
                        objNewJob.Industry2 = Industry2;
                        objNewJob.Title = Title;
                        objNewJob.LocationCity = LocationCity;
                        objNewJob.LocationCountry = LocationCountry;
                        objNewJob.LocationState = LocationState;
                        if (ExperienceLevel != null && ExperienceLevel != "")
                        {
                            objNewJob.ExperienceLevelType = Convert.ToInt64(ExperienceLevel);
                        }
                        else
                        {
                            objNewJob.ExperienceLevelType = null;
                        }
                        objNewJob.Responsibilities = Responsibilities;
                        objNewJob.Qualifications = Qualifications;
                        objNewJob.EmployerIntroduction = EmployerIntroduction;
                        objNewJob.URL = JobURL;
                        objNewJob.State = false;
                        db.Job.Add(objNewJob);
                        db.SaveChanges();

                        long objGetLatestJobID = db.Job.Get().OrderByDescending(n => n.ID).FirstOrDefault().ID;

                        //---------------------Add JobSkill--------------------------------------//
                        if (Skills.Length > 0)
                        {
                            foreach (var skill in Skills)
                            {
                                JobSkill objNewJobSkill = new JobSkill();
                                objNewJobSkill.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                objNewJobSkill.JobID = objGetLatestJobID;
                                objNewJobSkill.Skill = skill.Skill;
                                db.JobSkill.Add(objNewJobSkill);
                                db.SaveChanges();
                            }
                        }
                        //-----------------------------------------------------------------------------//

                        //Prepare Response                       
                        Job objGetLatestJob = db.Job.Get().OrderByDescending(n => n.ID).FirstOrDefault();
                        if (objGetLatestJob != null)
                        {
                            JobDetailsWithSkill.ID = Convert.ToString(objGetLatestJob.ID);
                            JobDetailsWithSkill.DateCreated = Convert.ToString(objGetLatestJob.DateCreated);
                            JobDetailsWithSkill.DateModified = Convert.ToString(objGetLatestJob.DateModified);
                            JobDetailsWithSkill.UserId = Convert.ToString(objGetLatestJob.UserID);
                            JobDetailsWithSkill.LocationCity = objGetLatestJob.LocationCity;
                            JobDetailsWithSkill.LocationState = objGetLatestJob.LocationState;
                            JobDetailsWithSkill.LocationCountry = objGetLatestJob.LocationCountry;
                            JobDetailsWithSkill.ExperienceLevel = objGetLatestJob.ExperienceLevelType != null ? Convert.ToString(objGetLatestJob.ExperienceLevelType) : string.Empty;
                            JobDetailsWithSkill.Industry = objGetLatestJob.Industry;
                            JobDetailsWithSkill.Industry2 = objGetLatestJob.Industry2;
                            JobDetailsWithSkill.Title = objGetLatestJob.Title;
                            JobDetailsWithSkill.Responsibilities = objGetLatestJob.Responsibilities;
                            JobDetailsWithSkill.Qualifications = objGetLatestJob.Qualifications;
                            JobDetailsWithSkill.Company = objGetLatestJob.Company;
                            JobDetailsWithSkill.EmployerIntroduction = objGetLatestJob.EmployerIntroduction;
                            JobDetailsWithSkill.URL = objGetLatestJob.URL;
                            JobDetailsWithSkill.ShareURL = objGetLatestJob.ShareURL;
                            JobDetailsWithSkill.State = Convert.ToString(objGetLatestJob.State);
                            List<GetJobSkillDetail> lstJobskill = new List<GetJobSkillDetail>();

                            //----------------Get Job skills---------//
                            List<JobSkill> objJobskill = db.JobSkill.Get().Where(n => n.JobID == objGetLatestJob.ID).ToList();
                            if (objJobskill.Count > 0)
                            {
                                foreach (JobSkill skill in objJobskill)
                                {
                                    GetJobSkillDetail objCurrSkill = new GetJobSkillDetail();
                                    objCurrSkill.ID = Convert.ToString(skill.ID);
                                    objCurrSkill.DateCreated = Convert.ToString(skill.DateCreated);
                                    objCurrSkill.JobID = Convert.ToString(skill.JobID);
                                    objCurrSkill.Skill = skill.Skill;
                                    lstJobskill.Add(objCurrSkill);
                                }
                            }
                            //----------------------------------------//
                            JobDetailsWithSkill.JobSkills = lstJobskill;

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            AddEditJobResult.ResultStatus = ResultStatus;
                            AddEditJobResult.JobDetailsWithSkills = JobDetailsWithSkill;
                        }
                    }
                    else if (lngJobID > 0)
                    {
                        //Update Job
                        Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                        objGetJob.DateModified = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                        objGetJob.Company = Company;
                        objGetJob.Industry = Industry;
                        objGetJob.Industry2 = Industry2;
                        objGetJob.Title = Title;
                        objGetJob.LocationCity = LocationCity;
                        objGetJob.LocationCountry = LocationCountry;
                        objGetJob.LocationState = LocationState;
                        if (ExperienceLevel != null && ExperienceLevel != "")
                        {
                            objGetJob.ExperienceLevelType = Convert.ToInt64(ExperienceLevel);
                        }
                        else
                        {
                            objGetJob.ExperienceLevelType = null;
                        }
                        objGetJob.Responsibilities = Responsibilities;
                        objGetJob.Qualifications = Qualifications;
                        objGetJob.EmployerIntroduction = EmployerIntroduction;
                        objGetJob.URL = JobURL;
                        db.Job.Update(objGetJob);
                        db.SaveChanges();

                        //=============Update Jobskill - delete old JobSkill and add new JobSkill ======================//
                        //-------------------------------Delete old JobSkill----------------------//
                        List<JobSkill> objGetJobskill = db.JobSkill.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (objGetJobskill.Count > 0)
                        {
                            foreach (JobSkill skill in objGetJobskill)
                            {
                                db.JobSkill.Remove(skill);
                                db.SaveChanges();
                            }
                        }
                        //---------------------------------------------------------------------------//
                        //---------------------Add new JobSkill--------------------------------------//
                        if (Skills.Length > 0)
                        {
                            foreach (var skill in Skills)
                            {
                                JobSkill objNewJobSkill = new JobSkill();
                                objNewJobSkill.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                objNewJobSkill.JobID = lngJobID;
                                objNewJobSkill.Skill = skill.Skill;
                                db.JobSkill.Add(objNewJobSkill);
                                db.SaveChanges();
                            }
                        }
                        //-----------------------------------------------------------------------------//
                        //==================================================================================================//

                        //Prepare Response
                        Job objGetUpdatedJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                        if (objGetUpdatedJob != null)
                        {
                            JobDetailsWithSkill.ID = Convert.ToString(objGetUpdatedJob.ID);
                            JobDetailsWithSkill.DateCreated = Convert.ToString(objGetUpdatedJob.DateCreated);
                            JobDetailsWithSkill.DateModified = Convert.ToString(objGetUpdatedJob.DateModified);
                            JobDetailsWithSkill.UserId = Convert.ToString(objGetUpdatedJob.UserID);
                            JobDetailsWithSkill.LocationCity = objGetUpdatedJob.LocationCity;
                            JobDetailsWithSkill.LocationState = objGetUpdatedJob.LocationState;
                            JobDetailsWithSkill.LocationCountry = objGetUpdatedJob.LocationCountry;
                            JobDetailsWithSkill.ExperienceLevel = objGetUpdatedJob.ExperienceLevelType != null ? Convert.ToString(objGetUpdatedJob.ExperienceLevelType) : string.Empty;
                            JobDetailsWithSkill.Industry = objGetUpdatedJob.Industry;
                            JobDetailsWithSkill.Industry2 = objGetUpdatedJob.Industry2;
                            JobDetailsWithSkill.Title = objGetUpdatedJob.Title;
                            JobDetailsWithSkill.Responsibilities = objGetUpdatedJob.Responsibilities;
                            JobDetailsWithSkill.Qualifications = objGetUpdatedJob.Qualifications;
                            JobDetailsWithSkill.Company = objGetUpdatedJob.Company;
                            JobDetailsWithSkill.EmployerIntroduction = objGetUpdatedJob.EmployerIntroduction;
                            JobDetailsWithSkill.URL = objGetUpdatedJob.URL;
                            JobDetailsWithSkill.ShareURL = objGetUpdatedJob.ShareURL;
                            JobDetailsWithSkill.State = Convert.ToString(objGetUpdatedJob.State);
                            List<GetJobSkillDetail> lstJobskill = new List<GetJobSkillDetail>();

                            //----------------Get Job skills---------//
                            List<JobSkill> objJobskill = db.JobSkill.Get().Where(n => n.JobID == lngJobID).ToList();
                            if (objJobskill.Count > 0)
                            {
                                foreach (JobSkill skill in objJobskill)
                                {
                                    GetJobSkillDetail objCurrSkill = new GetJobSkillDetail();
                                    objCurrSkill.ID = Convert.ToString(skill.ID);
                                    objCurrSkill.DateCreated = Convert.ToString(skill.DateCreated);
                                    objCurrSkill.JobID = Convert.ToString(skill.JobID);
                                    objCurrSkill.Skill = skill.Skill;
                                    lstJobskill.Add(objCurrSkill);
                                }
                            }
                            //----------------------------------------//
                            JobDetailsWithSkill.JobSkills = lstJobskill;

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            AddEditJobResult.ResultStatus = ResultStatus;
                            AddEditJobResult.JobDetailsWithSkills = JobDetailsWithSkill;
                        }
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                AddEditJobResult.ResultStatus = ResultStatus;
                AddEditJobResult.JobDetailsWithSkills = JobDetailsWithSkill;
            }
            writeLog("AddEditJob", "STOP", UserID);
            return AddEditJobResult;
        }

        #endregion

        #region GetJobDetails

        public GetJobDetailsResult GetJobDetails(string UserID, string UserToken, string JobID)
        {
            GetJobDetailsResult JobDetailsResult = new GetJobDetailsResult();
            ResultStatus ResultStatus = new ResultStatus();
            GetJobDetailsWithSkill JobDetailsWithSkill = new GetJobDetailsWithSkill();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetJobDetails", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);

                    Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                    if (objGetJob != null)
                    {
                        JobDetailsWithSkill.ID = Convert.ToString(objGetJob.ID);
                        JobDetailsWithSkill.DateCreated = Convert.ToString(objGetJob.DateCreated);
                        JobDetailsWithSkill.DateModified = Convert.ToString(objGetJob.DateModified);
                        JobDetailsWithSkill.UserId = Convert.ToString(objGetJob.UserID);
                        JobDetailsWithSkill.LocationCity = objGetJob.LocationCity;
                        JobDetailsWithSkill.LocationState = objGetJob.LocationState;
                        JobDetailsWithSkill.LocationCountry = objGetJob.LocationCountry;
                        JobDetailsWithSkill.ExperienceLevel = objGetJob.ExperienceLevelType != null ? Convert.ToString(objGetJob.ExperienceLevelType) : string.Empty;
                        JobDetailsWithSkill.Industry = objGetJob.Industry;
                        JobDetailsWithSkill.Industry2 = objGetJob.Industry2;
                        JobDetailsWithSkill.Title = objGetJob.Title;
                        JobDetailsWithSkill.Responsibilities = objGetJob.Responsibilities;
                        JobDetailsWithSkill.Qualifications = objGetJob.Qualifications;
                        JobDetailsWithSkill.Company = objGetJob.Company;
                        JobDetailsWithSkill.EmployerIntroduction = objGetJob.EmployerIntroduction;
                        JobDetailsWithSkill.URL = objGetJob.URL;
                        JobDetailsWithSkill.ShareURL = objGetJob.ShareURL;
                        JobDetailsWithSkill.State = Convert.ToString(objGetJob.State);

                        //----------------Get Job skills---------//
                        List<GetJobSkillDetail> lstJobskill = new List<GetJobSkillDetail>();
                        List<JobSkill> objJobskill = db.JobSkill.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (objJobskill.Count > 0)
                        {
                            foreach (JobSkill skill in objJobskill)
                            {
                                GetJobSkillDetail objCurrSkill = new GetJobSkillDetail();
                                objCurrSkill.ID = Convert.ToString(skill.ID);
                                objCurrSkill.DateCreated = Convert.ToString(skill.DateCreated);
                                objCurrSkill.JobID = Convert.ToString(skill.JobID);
                                objCurrSkill.Skill = skill.Skill;
                                lstJobskill.Add(objCurrSkill);
                            }
                        }
                        //----------------------------------------//
                        JobDetailsWithSkill.JobSkills = lstJobskill;

                        //User Job Favorite
                        UserJobFavorite objUserJobFavorite = db.UserJobFavorite.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                        JobDetailsResult.IsJobFavorite = objUserJobFavorite != null ? "True" : "False";

                        //User Job Applied

                        UserJobApplication objUserJobApplication = db.UserJobApplication.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                        JobDetailsResult.IsJobApplied = objUserJobApplication != null ? "True" : "False";

                        ResultStatus.Status = "1";
                        ResultStatus.StatusMessage = "";
                        JobDetailsResult.ResultStatus = ResultStatus;
                        JobDetailsResult.JobDetailsWithSkills = JobDetailsWithSkill;
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Job does not exist!";
                        JobDetailsResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                JobDetailsResult.ResultStatus = ResultStatus;
            }
            writeLog("GetJobDetails", "STOP", UserID);
            return JobDetailsResult;
        }

        #endregion

        #region FavoriteJob

        public GetFavoriteJobResult FavoriteJob(string UserID, string UserToken, string JobID, string Status)
        {
            GetFavoriteJobResult FavoriteJobResult = new GetFavoriteJobResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("FavoriteJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);

                    if (Status != null && Status != "")
                    {
                        Job objCheckJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                        if (objCheckJob != null)
                        {
                            if (Convert.ToInt32(Status) == 1)
                            {
                                //add new record
                                UserJobFavorite objCheckJobFavorite = db.UserJobFavorite.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                                if (objCheckJobFavorite == null)
                                {
                                    UserJobFavorite objNewUserJobFavorite = new UserJobFavorite();
                                    objNewUserJobFavorite.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objNewUserJobFavorite.UserID = lngUserID;
                                    objNewUserJobFavorite.JobID = lngJobID;
                                    db.UserJobFavorite.Add(objNewUserJobFavorite);
                                    db.SaveChanges();

                                    ResultStatus.Status = "1";
                                    ResultStatus.StatusMessage = "";
                                    FavoriteJobResult.ResultStatus = ResultStatus;
                                }
                                else
                                {
                                    //already favorited
                                    ResultStatus.Status = "0";
                                    ResultStatus.StatusMessage = "Record alerady exist!";
                                    FavoriteJobResult.ResultStatus = ResultStatus;
                                }
                            }
                            else if (Convert.ToInt32(Status) == 0)
                            {
                                //delete record 
                                UserJobFavorite objCheckJobFavorite = db.UserJobFavorite.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                                if (objCheckJobFavorite != null)
                                {
                                    db.UserJobFavorite.Remove(objCheckJobFavorite);
                                    db.SaveChanges();

                                    ResultStatus.Status = "1";
                                    ResultStatus.StatusMessage = "";
                                    FavoriteJobResult.ResultStatus = ResultStatus;
                                }
                                else
                                {
                                    //already favorited
                                    ResultStatus.Status = "0";
                                    ResultStatus.StatusMessage = "Record does not exist!";
                                    FavoriteJobResult.ResultStatus = ResultStatus;
                                }
                            }
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "Job does not exist!";
                            FavoriteJobResult.ResultStatus = ResultStatus;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Invalid Parameter!";
                        FavoriteJobResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                FavoriteJobResult.ResultStatus = ResultStatus;
            }
            writeLog("FavoriteJob", "STOP", UserID);
            return FavoriteJobResult;
        }

        #endregion

        #region ApplyToJob

        public GetApplyToJobResult ApplyToJob(string UserID, string UserToken, string JobID, string Status)
        {
            GetApplyToJobResult ApplyToJobResult = new GetApplyToJobResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("ApplyToJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);

                    if (Status != null && Status != "")
                    {
                        Job objCheckJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                        if (objCheckJob != null)
                        {
                            if (Convert.ToInt32(Status) == 1)
                            {
                                //add new record
                                UserJobApplication objCheckJobApplication = db.UserJobApplication.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                                if (objCheckJobApplication == null)
                                {
                                    //----------------add record UserJobApplication-----------------------------//
                                    UserJobApplication objNewUserJobApplication = new UserJobApplication();
                                    objNewUserJobApplication.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objNewUserJobApplication.UserID = lngUserID;
                                    objNewUserJobApplication.JobID = lngJobID;
                                    db.UserJobApplication.Add(objNewUserJobApplication);
                                    db.SaveChanges();
                                    //-------------------------------------------------------------------------------//

                                    //-----------------add record in Feed Table--------------------------------//
                                    Feed objNewFeed = new Feed();
                                    objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    long lngJobUserID = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID).UserID; //Get Job UserId
                                    objNewFeed.UserID = lngJobUserID;
                                    objNewFeed.FeedTypeID = 3;
                                    objNewFeed.JobID = lngJobID;
                                    objNewFeed.OtherUserID = lngUserID;
                                    db.Feed.Add(objNewFeed);
                                    db.SaveChanges();
                                    //-------------------------------------------------------------------------//

                                    //--------------Add Record in Message table -------------------------------//
                                    Message objNewMessage = new Message();
                                    objNewMessage.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objNewMessage.SenderID = lngUserID;
                                    objNewMessage.ReceiverID = lngJobUserID;
                                    objNewMessage.Message1 = "Job Application";
                                    objNewMessage.MessageTypeID = 2;
                                    objNewMessage.LinkURL = null;
                                    objNewMessage.LinkUserID = null;
                                    objNewMessage.LinkJobID = lngJobID;
                                    objNewMessage.State = false;
                                    db.Message.Add(objNewMessage);
                                    db.SaveChanges();
                                    //-------------------------------------------------------------------------//

                                    //================Code for push notification===========//
                                    User objUserInfo = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);
                                   
                                    //Get device token of receiver
                                    string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == lngJobUserID).DeviceToken;
                                    ///
                                    if (UserDeviceToken != null)
                                    {
                                        var msg = objUserInfo.FirstName + " " + objUserInfo.LastName + " has sent you a job application.";
                                        Dictionary<string, string> objParam = new Dictionary<string, string>();

                                        objParam.Add("testDeviceToken", UserDeviceToken);
                                        objParam.Add("pushMessage", msg);
                                        objParam.Add("sourceTable", "JobApplication");
                                        objParam.Add("UserID", lngUserID.ToString());
                                        objParam.Add("JobID", lngJobID.ToString());
                                        SendNotificationMessage(objParam);
                                    }
                                    //=========================end=======================//

                                    ResultStatus.Status = "1";
                                    ResultStatus.StatusMessage = "";
                                    ApplyToJobResult.ResultStatus = ResultStatus;
                                }
                                else
                                {
                                    //already applied
                                    ResultStatus.Status = "0";
                                    ResultStatus.StatusMessage = "Record already exist!";
                                    ApplyToJobResult.ResultStatus = ResultStatus;
                                }
                            }
                            else if (Convert.ToInt32(Status) == 0)
                            {
                                //delete record 
                                UserJobApplication objCheckJobApplication = db.UserJobApplication.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == lngUserID);
                                if (objCheckJobApplication != null)
                                {
                                    db.UserJobApplication.Remove(objCheckJobApplication);
                                    db.SaveChanges();

                                    ResultStatus.Status = "1";
                                    ResultStatus.StatusMessage = "";
                                    ApplyToJobResult.ResultStatus = ResultStatus;
                                }
                                else
                                {
                                    //already applied
                                    ResultStatus.Status = "0";
                                    ResultStatus.StatusMessage = "Record does not exist!";
                                    ApplyToJobResult.ResultStatus = ResultStatus;
                                }
                            }
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "Job does not exist!";
                            ApplyToJobResult.ResultStatus = ResultStatus;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Invalid Parameter!";
                        ApplyToJobResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                ApplyToJobResult.ResultStatus = ResultStatus;
            }
            writeLog("ApplyToJob", "STOP", UserID);
            return ApplyToJobResult;
        }

        #endregion

        #region FillReopenJob

        public GetFillReopenJobResult FillReopenJob(string UserID, string UserToken, string JobID, string Status)
        {
            GetFillReopenJobResult FillReopenResult = new GetFillReopenJobResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("FillReopenJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngFeedTypeID = 0;
                    if (Status != null && Status != "")
                    {
                        Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID);
                        if (objGetJob != null)
                        {
                            //Get List of Favorited User of Job
                            List<Int64> lstUserOfFavoritedJob = db.UserJobFavorite.Get().Where(n => n.JobID == lngJobID).Select(n => n.UserID).ToList();

                            if (Convert.ToInt32(Status) == 1)
                            {
                                objGetJob.State = true;
                                db.Job.Update(objGetJob);
                                db.SaveChanges();
                                lngFeedTypeID = 4;

                                if (lstUserOfFavoritedJob.Count > 0)
                                {
                                    foreach (Int64 id in lstUserOfFavoritedJob)
                                    {
                                        //================Code for push notification===========//                                        
                                        //Get device token of receiver
                                        string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == id).DeviceToken;
                                        ///
                                        if (UserDeviceToken != null)
                                        {
                                            var msg = "The job, "+ objGetJob.Title +" has been filled.";
                                            Dictionary<string, string> objParam = new Dictionary<string, string>();

                                            objParam.Add("testDeviceToken", UserDeviceToken);
                                            objParam.Add("pushMessage", msg);
                                            objParam.Add("sourceTable", "FillJob");
                                            objParam.Add("UserID", lngUserID.ToString());
                                            objParam.Add("JobID", lngJobID.ToString());
                                            SendNotificationMessage(objParam);
                                        }
                                        //=========================end=======================//
                                    }
                                }
                            }
                            else if (Convert.ToInt32(Status) == 0)
                            {
                                objGetJob.State = false;
                                db.Job.Update(objGetJob);
                                db.SaveChanges();
                                lngFeedTypeID = 5;
                            }

                            List<Int64> lstUserID = new List<Int64>();
                         
                            if (lstUserOfFavoritedJob.Count > 0)
                            {
                                foreach (long id in lstUserOfFavoritedJob)
                                {
                                    lstUserID.Add(id);
                                }
                            }
                            //Get List of applied user of Job
                            List<Int64> lstUserOfAppliedJob = db.UserJobApplication.Get().Where(n => n.JobID == lngJobID).Select(n => n.UserID).ToList();
                            if (lstUserOfAppliedJob.Count > 0)
                            {
                                foreach (long id in lstUserOfAppliedJob)
                                {
                                    lstUserID.Add(id);
                                }
                            }

                            if (lstUserID.Count > 0)
                            {
                                foreach (long id in lstUserID)
                                {
                                    Feed objCheckForFeed = db.Feed.Get().FirstOrDefault(n => n.JobID == lngJobID && n.UserID == id && n.FeedTypeID == lngFeedTypeID);
                                    if (objCheckForFeed == null)
                                    {
                                        Feed objNewFeed = new Feed();
                                        objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objNewFeed.UserID = id;
                                        if (Convert.ToInt32(Status) == 1)
                                        {
                                            objNewFeed.FeedTypeID = 4;
                                        }
                                        else if (Convert.ToInt32(Status) == 0)
                                        {
                                            objNewFeed.FeedTypeID = 5;
                                        }
                                        objNewFeed.JobID = lngJobID;
                                        objNewFeed.OtherUserID = lngUserID;
                                        db.Feed.Add(objNewFeed);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            FillReopenResult.ResultStatus = ResultStatus;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "Job does not exist!";
                            FillReopenResult.ResultStatus = ResultStatus;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Invalid Parameter!";
                        FillReopenResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                FillReopenResult.ResultStatus = ResultStatus;
            }
            writeLog("FillReopenJob", "STOP", UserID);
            return FillReopenResult;
        }

        #endregion

        #region DeleteJob

        public GetDeleteJobResult DeleteJob(string UserID, string UserToken, string JobID)
        {
            GetDeleteJobResult DeleteJobResult = new GetDeleteJobResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("DeleteJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngJobID = Convert.ToInt64(JobID);
                    long lngUserID = Convert.ToInt64(UserID);

                    //Check for the user is creator of job or not
                    Job objCheckJob = db.Job.Get().FirstOrDefault(n => n.ID == lngJobID && n.UserID == lngUserID);
                    if (objCheckJob != null)
                    {
                        //Remove UserJobApplication
                        List<UserJobApplication> lstGetUJApplication = db.UserJobApplication.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (lstGetUJApplication.Count > 0)
                        {
                            foreach (var uja in lstGetUJApplication)
                            {
                                db.UserJobApplication.Remove(uja);
                                db.SaveChanges();
                            }
                        }

                        //Remove UserJobFavourite
                        List<UserJobFavorite> lstGetUJFavourite = db.UserJobFavorite.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (lstGetUJFavourite.Count > 0)
                        {
                            foreach (var ujf in lstGetUJFavourite)
                            {
                                db.UserJobFavorite.Remove(ujf);
                                db.SaveChanges();
                            }
                        }

                        //Remove Message
                        List<Message> lstMessage = db.Message.Get().Where(n => n.LinkJobID == lngJobID).ToList();
                        if (lstMessage.Count > 0)
                        {
                            foreach (var msg in lstMessage)
                            {
                                db.Message.Remove(msg);
                                db.SaveChanges();
                            }
                        }

                        //Remove Feed
                        List<Feed> lstFeed = db.Feed.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (lstFeed.Count > 0)
                        {
                            foreach (var feed in lstFeed)
                            {
                                db.Feed.Remove(feed);
                                db.SaveChanges();
                            }
                        }

                        //Remove JobSkill
                        List<JobSkill> lstJobSkill = db.JobSkill.Get().Where(n => n.JobID == lngJobID).ToList();
                        if (lstJobSkill.Count > 0)
                        {
                            foreach (var skill in lstJobSkill)
                            {
                                db.JobSkill.Remove(skill);
                                db.SaveChanges();
                            }
                        }

                        //Remove Job
                        db.Job.Remove(objCheckJob);
                        db.SaveChanges();

                        ResultStatus.Status = "1";
                        ResultStatus.StatusMessage = "Job Deleted!";
                        DeleteJobResult.ResultStatus = ResultStatus;
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "You can not delete this job as you had not post this job!";
                        DeleteJobResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                DeleteJobResult.ResultStatus = ResultStatus;
            }
            writeLog("DeleteJob", "STOP", UserID);
            return DeleteJobResult;
        }

        #endregion

        #region SearchJob

        public GetSearchJobResult SearchJob(string UserID, string UserToken, string Industry, string Industry2, string ExperienceLevel, string Position, string LocationCity, string LocationState, string LocationCountry, string Company, string PageNumber)
        {
            GetSearchJobResult SearchJobResult = new GetSearchJobResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetCompleteJobDetails> lstJobDetail = new List<GetCompleteJobDetails>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("SearchJob", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);

                    List<Job> objGetjob = db.Job.Get().Where(n=>n.State != true).OrderByDescending(n => n.DateModified).ToList();
                    if (Industry != null && Industry != "" && Industry2 != "")
                    {
                        objGetjob = objGetjob.Where(r => r.Industry != null && r.Industry2!=null  && (r.Industry.ToUpper().Contains(Industry.ToUpper()) || r.Industry2.ToUpper().Contains(Industry.ToUpper()) || r.Industry.ToUpper().Contains(Industry2.ToUpper()) || r.Industry2.ToUpper().Contains(Industry2.ToUpper()))).ToList();
                    }
                    if (Industry2 != null && Industry2 != "" && Industry == "")
                    {
                        objGetjob = objGetjob.Where(r => r.Industry2 != null && (r.Industry2.ToUpper().Contains(Industry2.ToUpper()) || r.Industry.ToUpper().Contains(Industry2.ToUpper()))).ToList();
                    }
                    if (Industry != null && Industry != "" && Industry2 == "")
                    {
                        objGetjob = objGetjob.Where(r => r.Industry != null && (r.Industry2.ToUpper().Contains(Industry.ToUpper()) || r.Industry.ToUpper().Contains(Industry.ToUpper()))).ToList();
                    }
                    if (ExperienceLevel != null && ExperienceLevel != "")
                    {
                        objGetjob = objGetjob.Where(r => r.ExperienceLevelType != null && r.ExperienceLevelType == Convert.ToInt32(ExperienceLevel)).ToList();
                    }
                    if (Position != null && Position != "")
                    {
                        objGetjob = objGetjob.Where(r => r.Title != null && r.Title.ToUpper().Contains(Position.ToUpper())).ToList();
                    }
                    if (LocationCity != null && LocationCity != "")
                    {
                        objGetjob = objGetjob.Where(r => r.LocationCity != null && r.LocationCity.ToUpper().Contains(LocationCity.ToUpper())).ToList();
                    }
                    if (LocationState != null && LocationState != "")
                    {
                        objGetjob = objGetjob.Where(r => r.LocationState != null && r.LocationState.ToUpper().Contains(LocationState.ToUpper())).ToList();
                    }
                    if (LocationCountry != null && LocationCountry != "")
                    {
                        objGetjob = objGetjob.Where(r => r.LocationCountry != null && r.LocationCountry.ToUpper().Contains(LocationCountry.ToUpper())).ToList();
                    }
                    if (Company != null && Company != "")
                    {
                        objGetjob = objGetjob.Where(r => r.Company != null && r.Company.ToUpper().Contains(Company.ToUpper())).ToList();
                    }

                    if (objGetjob.Count > 0)
                    {
                        if (PageNumber != null && PageNumber != "")
                        {
                            //Paging parameters
                            int pagesize = 10;
                            int currentpage = Convert.ToInt32(PageNumber);
                            int currentsize = pagesize;
                            int skipcount = currentsize * (currentpage - 1);
                            int takecount = currentsize * currentpage;
                            objGetjob = objGetjob.Take(takecount).Skip(skipcount).ToList();
                        }
                        if (objGetjob.Count > 0)
                        {
                            foreach (var job in objGetjob)
                            {
                                GetCompleteJobDetails objCurrJob = new GetCompleteJobDetails();
                                objCurrJob.ID = Convert.ToString(job.ID);
                                objCurrJob.DateCreated = Convert.ToString(job.DateCreated);
                                objCurrJob.DateModified = Convert.ToString(job.DateModified);
                                objCurrJob.UserId = Convert.ToString(job.UserID);
                                objCurrJob.LocationCity = job.LocationCity;
                                objCurrJob.LocationState = job.LocationState;
                                objCurrJob.LocationCountry = job.LocationCountry;
                                objCurrJob.ExperienceLevel = job.ExperienceLevelType != null ? Convert.ToString(job.ExperienceLevelType) : string.Empty;
                                objCurrJob.Industry = job.Industry;
                                objCurrJob.Industry2 = job.Industry2;
                                objCurrJob.Title = job.Title;
                                objCurrJob.Responsibilities = job.Responsibilities;
                                objCurrJob.Qualifications = job.Qualifications;
                                objCurrJob.Company = job.Company;
                                objCurrJob.EmployerIntroduction = job.EmployerIntroduction;
                                objCurrJob.URL = job.URL;
                                objCurrJob.ShareURL = job.ShareURL;
                                objCurrJob.State = Convert.ToString(job.State);
                                lstJobDetail.Add(objCurrJob);
                            }

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            SearchJobResult.ResultStatus = ResultStatus;
                            SearchJobResult.JobDetails = lstJobDetail;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "No Records on this Page Number!";
                            SearchJobResult.ResultStatus = ResultStatus;
                            SearchJobResult.JobDetails = lstJobDetail;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records!";
                        SearchJobResult.ResultStatus = ResultStatus;
                        SearchJobResult.JobDetails = lstJobDetail;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                SearchJobResult.ResultStatus = ResultStatus;
            }
            writeLog("SearchJob", "STOP", UserID);
            return SearchJobResult;
        }

        #endregion

        #region SearchCandidates

        public GetSearchCandidatesResult SearchCandidates(string UserID, string UserToken, string Industry, string Industry2, string ExperienceLevel, string LocationCity, string LocationState, string LocationCountry, string Company, string PageNumber)
        {
            GetSearchCandidatesResult SearchCandidatesResult = new GetSearchCandidatesResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetUserDetailForCrowd> lstUserDetail = new List<GetUserDetailForCrowd>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("SearchCandidates", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {

                    List<User> objGetUser = db.User.Get().Where(n => n.ID != Convert.ToInt64(UserID)).OrderBy(n => n.FirstName).ToList();
                    if (Industry != null && Industry != "" && Industry2 != "")
                    {
                        objGetUser = objGetUser.Where(r => r.Industry != null && r.Industry2 != null && (r.Industry.ToUpper().Contains(Industry.ToUpper()) || r.Industry2.ToUpper().Contains(Industry.ToUpper()) || r.Industry.ToUpper().Contains(Industry2.ToUpper()) || r.Industry2.ToUpper().Contains(Industry2.ToUpper()))).ToList();
                    }
                    if (Industry2 != null && Industry2 != "" && Industry == "")
                    {
                        objGetUser = objGetUser.Where(r => r.Industry2 != null && (r.Industry2.ToUpper().Contains(Industry2.ToUpper()) || r.Industry.ToUpper().Contains(Industry2.ToUpper()))).ToList();
                    }
                    if (Industry != null && Industry != "" && Industry2 == "")
                    {
                        objGetUser = objGetUser.Where(r => r.Industry != null && (r.Industry2.ToUpper().Contains(Industry.ToUpper()) || r.Industry.ToUpper().Contains(Industry.ToUpper()))).ToList();
                    }                   
                    if (ExperienceLevel != null && ExperienceLevel != "")
                    {
                        objGetUser = objGetUser.Where(r => r.ExperienceLevelType != null && r.ExperienceLevelType == Convert.ToInt32(ExperienceLevel)).ToList();
                    }
                    if (LocationCity != null && LocationCity != "")
                    {
                        objGetUser = objGetUser.Where(r => r.LocationCity != null && r.LocationCity.ToUpper().Contains(LocationCity.ToUpper())).ToList();
                    }
                    if (LocationState != null && LocationState != "")
                    {
                        objGetUser = objGetUser.Where(r => r.LocationState != null && r.LocationState.ToUpper().Contains(LocationState.ToUpper())).ToList();
                    }
                    if (LocationCountry != null && LocationCountry != "")
                    {
                        objGetUser = objGetUser.Where(r => r.LocationCountry != null && r.LocationCountry.ToUpper().Contains(LocationCountry.ToUpper())).ToList();
                    }

                    if (objGetUser.Count > 0)
                    {
                        foreach (var user in objGetUser)
                        {
                            GetUserDetailForCrowd objCurrUser = new GetUserDetailForCrowd();
                            objCurrUser.UserId = Convert.ToString(user.ID);
                            objCurrUser.DateCreated = Convert.ToString(user.DateCreated);
                            objCurrUser.DateModified = Convert.ToString(user.DateModified);
                            objCurrUser.Email = user.Email;
                            objCurrUser.FirstName = user.FirstName;
                            objCurrUser.LastName = user.LastName;
                            objCurrUser.LocationCity = user.LocationCity;
                            objCurrUser.LocationState = user.LocationState;
                            objCurrUser.LocationCountry = user.LocationCountry;
                            objCurrUser.Industry = user.Industry;
                            objCurrUser.Industry2 = user.Industry2;
                            objCurrUser.Summary = user.Summary;
                            objCurrUser.PhotoURL = user.PhotoURL;
                            objCurrUser.LinkedInId = user.LinkedInId;
                            objCurrUser.ExperienceLevel = Convert.ToString(user.ExperienceLevelType);

                            //---------------------UserEmployment Response-------------------------------//
                            List<GetUserEmployment> lstCurrentEmployerList = new List<GetUserEmployment>();
                            List<UserEmployment> objUserEmploymentList = db.UserEmployment.Get().Where(n => n.UserID == user.ID && (n.EndMonth == 0 || n.EndMonth == null)).ToList();
                            if (objUserEmploymentList.Count > 0)
                            {
                                foreach (var ue in objUserEmploymentList)
                                {
                                    //If filter applied on Company
                                    if (Company != null && Company != "")
                                    {
                                        if (ue.EmployerName.ToUpper().Contains(Company.ToUpper()))
                                        {
                                            GetUserEmployment objCurrEmployment = new GetUserEmployment();
                                            objCurrEmployment.ID = Convert.ToString(ue.ID);
                                            objCurrEmployment.DateCreated = Convert.ToString(ue.DateCreated);
                                            objCurrEmployment.DateModified = Convert.ToString(ue.DateModified);
                                            objCurrEmployment.UserId = Convert.ToString(ue.UserID);
                                            objCurrEmployment.EmployerName = ue.EmployerName;
                                            objCurrEmployment.Title = ue.Title;
                                            objCurrEmployment.LocationCity = ue.LocationCity;
                                            objCurrEmployment.LocationState = ue.LocationState;
                                            objCurrEmployment.LocationCountry = ue.LocationCountry;
                                            objCurrEmployment.StartMonth = Convert.ToString(ue.StartMonth);
                                            objCurrEmployment.StartYear = Convert.ToString(ue.StartYear);
                                            objCurrEmployment.EndMonth = ue.EndMonth != null ? Convert.ToString(ue.EndMonth) : string.Empty;
                                            objCurrEmployment.EndYear = ue.EndYear != null ? Convert.ToString(ue.EndYear) : string.Empty;
                                            objCurrEmployment.Summary = ue.Summary;
                                            lstCurrentEmployerList.Add(objCurrEmployment);
                                        }
                                    }
                                    else // If Filter not applied on Company all employement
                                    {
                                        GetUserEmployment objCurrEmployment = new GetUserEmployment();
                                        objCurrEmployment.ID = Convert.ToString(ue.ID);
                                        objCurrEmployment.DateCreated = Convert.ToString(ue.DateCreated);
                                        objCurrEmployment.DateModified = Convert.ToString(ue.DateModified);
                                        objCurrEmployment.UserId = Convert.ToString(ue.UserID);
                                        objCurrEmployment.EmployerName = ue.EmployerName;
                                        objCurrEmployment.Title = ue.Title;
                                        objCurrEmployment.LocationCity = ue.LocationCity;
                                        objCurrEmployment.LocationState = ue.LocationState;
                                        objCurrEmployment.LocationCountry = ue.LocationCountry;
                                        objCurrEmployment.StartMonth = Convert.ToString(ue.StartMonth);
                                        objCurrEmployment.StartYear = Convert.ToString(ue.StartYear);
                                        objCurrEmployment.EndMonth = ue.EndMonth != null ? Convert.ToString(ue.EndMonth) : string.Empty;
                                        objCurrEmployment.EndYear = ue.EndYear != null ? Convert.ToString(ue.EndYear) : string.Empty;
                                        objCurrEmployment.Summary = ue.Summary;
                                        lstCurrentEmployerList.Add(objCurrEmployment);
                                    }
                                }
                                objCurrUser.UserCurrentEmployer = lstCurrentEmployerList;
                            }

                            //---------------------------------------------------------------------------------//
                            if (lstCurrentEmployerList.Count > 0)
                            {
                                lstUserDetail.Add(objCurrUser);
                            }
                        }

                        if (PageNumber != null && PageNumber != "")
                        {
                            //Paging parameters
                            int pagesize = 10;
                            int currentpage = Convert.ToInt32(PageNumber);
                            int currentsize = pagesize;
                            int skipcount = currentsize * (currentpage - 1);
                            int takecount = currentsize * currentpage;
                            lstUserDetail = lstUserDetail.Take(takecount).Skip(skipcount).ToList();
                        }

                        if (lstUserDetail.Count > 0)
                        {
                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            SearchCandidatesResult.ResultStatus = ResultStatus;
                            SearchCandidatesResult.UserDetail = lstUserDetail;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "No Records on this Page Number!"; //Changes by himanshu: As per email on 03-10-14.
                            SearchCandidatesResult.ResultStatus = ResultStatus;
                            SearchCandidatesResult.UserDetail = lstUserDetail;
                        }

                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records!";
                        SearchCandidatesResult.ResultStatus = ResultStatus;
                        SearchCandidatesResult.UserDetail = lstUserDetail;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                SearchCandidatesResult.ResultStatus = ResultStatus;
            }
            writeLog("SearchCandidates", "STOP", UserID);
            return SearchCandidatesResult;
        }

        #endregion

        #region GetUserJobs

        public GetUserJobsResult GetUserJobs(string UserID, string UserToken, string OtherUserID, string PageNumber)
        {
            GetUserJobsResult UserJobResult = new GetUserJobsResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetCompleteJobDetails> lstJobPostedByMe = new List<GetCompleteJobDetails>();
            List<GetCompleteJobDetails> lstJobApplied = new List<GetCompleteJobDetails>();
            List<GetCompleteJobDetails> lstJobFavorited = new List<GetCompleteJobDetails>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetUserJobs", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngOtherUserID = Convert.ToInt64(OtherUserID);
                    //Posted By Me
                    List<Job> objJobPostedByMe = db.Job.Get().Where(n => n.UserID == Convert.ToInt64(lngOtherUserID)).OrderByDescending(n => n.DateModified).ToList();
                    if (objJobPostedByMe.Count > 0)
                    {
                        foreach (var job in objJobPostedByMe)
                        {
                            GetCompleteJobDetails objCurrJob = new GetCompleteJobDetails();
                            objCurrJob.ID = Convert.ToString(job.ID);
                            objCurrJob.DateCreated = Convert.ToString(job.DateCreated);
                            objCurrJob.DateModified = Convert.ToString(job.DateModified);
                            objCurrJob.UserId = Convert.ToString(job.UserID);
                            objCurrJob.LocationCity = job.LocationCity;
                            objCurrJob.LocationState = job.LocationState;
                            objCurrJob.LocationCountry = job.LocationCountry;
                            objCurrJob.ExperienceLevel = job.ExperienceLevelType != null ? Convert.ToString(job.ExperienceLevelType) : string.Empty;
                            objCurrJob.Industry = job.Industry;
                            objCurrJob.Industry2 = job.Industry2;
                            objCurrJob.Title = job.Title;
                            objCurrJob.Responsibilities = job.Responsibilities;
                            objCurrJob.Qualifications = job.Qualifications;
                            objCurrJob.Company = job.Company;
                            objCurrJob.EmployerIntroduction = job.EmployerIntroduction;
                            objCurrJob.URL = job.URL;
                            objCurrJob.ShareURL = job.ShareURL;
                            objCurrJob.State = Convert.ToString(job.State);
                            lstJobPostedByMe.Add(objCurrJob);
                        }
                    }

                    if (lngUserID == lngOtherUserID)
                    {
                        //Job applied
                        List<Int64> objJobApplied = db.UserJobApplication.Get().Where(n => n.UserID == Convert.ToInt64(OtherUserID)).OrderByDescending(n => n.ID).Select(n => n.JobID).ToList();
                        if (objJobApplied.Count > 0)
                        {
                            foreach (Int64 jobid in objJobApplied)
                            {
                                Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == jobid);
                                if (objGetJob != null)
                                {
                                    GetCompleteJobDetails objCurrJob = new GetCompleteJobDetails();
                                    objCurrJob.ID = Convert.ToString(objGetJob.ID);
                                    objCurrJob.DateCreated = Convert.ToString(objGetJob.DateCreated);
                                    objCurrJob.DateModified = Convert.ToString(objGetJob.DateModified);
                                    objCurrJob.UserId = Convert.ToString(objGetJob.UserID);
                                    objCurrJob.LocationCity = objGetJob.LocationCity;
                                    objCurrJob.LocationState = objGetJob.LocationState;
                                    objCurrJob.LocationCountry = objGetJob.LocationCountry;
                                    objCurrJob.ExperienceLevel = objGetJob.ExperienceLevelType != null ? Convert.ToString(objGetJob.ExperienceLevelType) : string.Empty;
                                    objCurrJob.Industry = objGetJob.Industry;
                                    objCurrJob.Industry2 = objGetJob.Industry2;
                                    objCurrJob.Title = objGetJob.Title;
                                    objCurrJob.Responsibilities = objGetJob.Responsibilities;
                                    objCurrJob.Qualifications = objGetJob.Qualifications;
                                    objCurrJob.Company = objGetJob.Company;
                                    objCurrJob.EmployerIntroduction = objGetJob.EmployerIntroduction;
                                    objCurrJob.URL = objGetJob.URL;
                                    objCurrJob.ShareURL = objGetJob.ShareURL;
                                    objCurrJob.State = Convert.ToString(objGetJob.State);
                                    lstJobApplied.Add(objCurrJob);
                                }
                            }
                        }

                        //Job Favorited
                        List<Int64> objJobFavourited = db.UserJobFavorite.Get().Where(n => n.UserID == Convert.ToInt64(OtherUserID)).OrderByDescending(n => n.ID).Select(n => n.JobID).ToList();
                        if (objJobFavourited.Count > 0)
                        {
                            foreach (Int64 jobid in objJobFavourited)
                            {
                                Job objGetJob = db.Job.Get().FirstOrDefault(n => n.ID == jobid);
                                if (objGetJob != null)
                                {
                                    GetCompleteJobDetails objCurrJob = new GetCompleteJobDetails();
                                    objCurrJob.ID = Convert.ToString(objGetJob.ID);
                                    objCurrJob.DateCreated = Convert.ToString(objGetJob.DateCreated);
                                    objCurrJob.DateModified = Convert.ToString(objGetJob.DateModified);
                                    objCurrJob.UserId = Convert.ToString(objGetJob.UserID);
                                    objCurrJob.LocationCity = objGetJob.LocationCity;
                                    objCurrJob.LocationState = objGetJob.LocationState;
                                    objCurrJob.LocationCountry = objGetJob.LocationCountry;
                                    objCurrJob.ExperienceLevel = objGetJob.ExperienceLevelType != null ? Convert.ToString(objGetJob.ExperienceLevelType) : string.Empty;
                                    objCurrJob.Industry = objGetJob.Industry;
                                    objCurrJob.Industry2 = objGetJob.Industry2;
                                    objCurrJob.Title = objGetJob.Title;
                                    objCurrJob.Responsibilities = objGetJob.Responsibilities;
                                    objCurrJob.Qualifications = objGetJob.Qualifications;
                                    objCurrJob.Company = objGetJob.Company;
                                    objCurrJob.EmployerIntroduction = objGetJob.EmployerIntroduction;
                                    objCurrJob.URL = objGetJob.URL;
                                    objCurrJob.ShareURL = objGetJob.ShareURL;
                                    objCurrJob.State = Convert.ToString(objGetJob.State);
                                    lstJobFavorited.Add(objCurrJob);
                                }
                            }
                        }
                    }

                    if (PageNumber != null && PageNumber != "")
                    {
                        //Paging parameters
                        int pagesize = 10;
                        int currentpage = Convert.ToInt32(PageNumber);
                        int currentsize = pagesize;
                        int skipcount = currentsize * (currentpage - 1);
                        int takecount = currentsize * currentpage;
                        lstJobPostedByMe = lstJobPostedByMe.Take(takecount).Skip(skipcount).ToList();
                        lstJobApplied = lstJobApplied.Take(takecount).Skip(skipcount).ToList();
                        lstJobFavorited = lstJobFavorited.Take(takecount).Skip(skipcount).ToList();
                    }

                    if (lstJobPostedByMe.Count > 0 || lstJobFavorited.Count > 0 || lstJobApplied.Count > 0)
                    {
                        ResultStatus.Status = "1";
                        ResultStatus.StatusMessage = "";
                        UserJobResult.ResultStatus = ResultStatus;
                        UserJobResult.PostedByMe = lstJobPostedByMe;
                        UserJobResult.JobApplied = lstJobApplied;
                        UserJobResult.JobFavorited = lstJobFavorited;
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records!";
                        UserJobResult.ResultStatus = ResultStatus;
                        UserJobResult.PostedByMe = lstJobPostedByMe;
                        UserJobResult.JobApplied = lstJobApplied;
                        UserJobResult.JobFavorited = lstJobFavorited;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                UserJobResult.ResultStatus = ResultStatus;
            }
            writeLog("GetUserJobs", "STOP", UserID);
            return UserJobResult;
        }

        #endregion

        #region GetMessageList

        public GetMessageListResult GetMessageList(string UserID, string UserToken, string PageNumber)
        {
            GetMessageListResult MessageListResult = new GetMessageListResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetMessageDetailWithSender> MesssageList = new List<GetMessageDetailWithSender>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetMessageList", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    List<Message> lstFinal = new List<Message>();

                    //Change as per mail 151014
                    var lstMessageType0 = db.Message.Get(n => n.ReceiverID == lngUserID && n.MessageTypeID == 1).OrderByDescending(n => n.DateCreated).GroupBy(n => n.SenderID).ToList(); // Login user is reciever By Rajendra

                    //var lstMessageType0 = db.Message.Get(n => (n.ReceiverID == lngUserID || n.SenderID == lngUserID) && n.MessageTypeID == 1).OrderByDescending(n => n.DateCreated).GroupBy(n => n.SenderID).ToList();
                    if (lstMessageType0.Count > 0)
                    {
                        foreach (var message in lstMessageType0)
                        {
                            lstFinal.Add(message.FirstOrDefault());
                        }
                    }

                    // By Rajendra
                    var lstMessageType0_2 = db.Message.Get(n => n.SenderID == lngUserID && n.MessageTypeID == 1).OrderByDescending(n => n.DateCreated).GroupBy(n => n.ReceiverID).ToList(); //  Login user is sender By Rajendra
                    if (lstMessageType0_2.Count > 0)
                    {
                        foreach (var message in lstMessageType0_2)
                        {

                            Message msgObject = message.FirstOrDefault();

                            Message TempMsg = lstFinal.Where(n => n.SenderID == msgObject.ReceiverID).FirstOrDefault();

                            if (TempMsg == null)
                            {
                                lstFinal.Add(message.FirstOrDefault());
                            }
                        }
                    }

                    //

                    //Change as per mail 151014
                    List<Message> lstMessageType1 = db.Message.Get().Where(n => (n.ReceiverID == lngUserID) && (n.MessageTypeID == 2 || n.MessageTypeID == 3 || n.MessageTypeID == 4)).ToList();
                    if (lstMessageType1.Count > 0)
                    {
                        foreach (var message in lstMessageType1)
                        {
                            lstFinal.Add(message);
                        }
                    }

                    var lstFinalOrdered = lstFinal.OrderByDescending(n => n.ID).ToList();

                    if (lstFinalOrdered.Count > 0)
                    {
                        if (PageNumber != null && PageNumber != "")
                        {
                            //Paging parameters
                            int pagesize = 10;
                            int currentpage = Convert.ToInt32(PageNumber);
                            int currentsize = pagesize;
                            int skipcount = currentsize * (currentpage - 1);
                            int takecount = currentsize * currentpage;
                            lstFinalOrdered = lstFinalOrdered.Take(takecount).Skip(skipcount).ToList();
                        }
                        if (lstFinalOrdered.Count > 0)
                        {
                            foreach (var msg in lstFinalOrdered)
                            {
                                //prepare response
                                GetMessageDetailWithSender objCurrMessage = new GetMessageDetailWithSender();
                                objCurrMessage.ID = Convert.ToString(msg.ID);
                                objCurrMessage.DateCreated = Convert.ToString(msg.DateCreated);
                                objCurrMessage.SenderID = Convert.ToString(msg.SenderID);
                                //GetSenderdetail
                                User objUser = new User();
                                if(msg.SenderID == lngUserID)
                                    objUser = db.User.Get().FirstOrDefault(n => n.ID == msg.ReceiverID);
                                else
                                    objUser = db.User.Get().FirstOrDefault(n => n.ID == msg.SenderID);

                                if (objUser != null)
                                {
                                    GetUserDetailForFeed UserDetail = new GetUserDetailForFeed();
                                    UserDetail.UserId = Convert.ToString(objUser.ID);
                                    UserDetail.DateCreated = Convert.ToString(objUser.DateCreated);
                                    UserDetail.DateModified = Convert.ToString(objUser.DateModified);
                                    UserDetail.Email = objUser.Email;
                                    UserDetail.FirstName = objUser.FirstName;
                                    UserDetail.LastName = objUser.LastName;
                                    UserDetail.LocationCity = objUser.LocationCity;
                                    UserDetail.LocationState = objUser.LocationState;
                                    UserDetail.LocationCountry = objUser.LocationCountry;
                                    UserDetail.Industry = objUser.Industry;
                                    UserDetail.Industry2 = objUser.Industry2;
                                    UserDetail.Summary = objUser.Summary;
                                    UserDetail.PhotoURL = objUser.PhotoURL;
                                    UserDetail.LinkedInId = objUser.LinkedInId;
                                    UserDetail.ExperienceLevel = Convert.ToString(objUser.ExperienceLevelType);
                                    objCurrMessage.SenderDetail = UserDetail;
                                }
                                objCurrMessage.ReceiverID = Convert.ToString(msg.ReceiverID);
                                objCurrMessage.State = Convert.ToString(msg.State);
                                objCurrMessage.Message = Convert.ToString(msg.Message1);
                                objCurrMessage.LincURL = Convert.ToString(msg.LinkURL);
                                objCurrMessage.LincUserID = Convert.ToString(msg.LinkUserID);
                                objCurrMessage.LincJobID = Convert.ToString(msg.LinkJobID);
                                objCurrMessage.Type = Convert.ToString(msg.MessageTypeID);

                                //IsUnreadMessage
                                Message objCheckMessage = db.Message.Get().FirstOrDefault(n => n.State == false && n.ReceiverID == lngUserID && n.SenderID == msg.SenderID && n.MessageTypeID == 1);
                                if (objCheckMessage != null)
                                {
                                    objCurrMessage.IsUnreadMessages = "True";
                                }
                                else
                                {
                                    objCurrMessage.IsUnreadMessages = "False";
                                }

                                MesssageList.Add(objCurrMessage);
                            }

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            MessageListResult.ResultStatus = ResultStatus;
                            MessageListResult.MesssageList = MesssageList;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "No Records on this Page Number!";
                            MessageListResult.ResultStatus = ResultStatus;
                            MessageListResult.MesssageList = MesssageList;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records";
                        MessageListResult.ResultStatus = ResultStatus;
                        MessageListResult.MesssageList = MesssageList;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                MessageListResult.ResultStatus = ResultStatus;
            }
            writeLog("GetMessageList", "STOP", UserID);
            return MessageListResult;
        }

        #endregion

        #region GetMessageThread

        public GetMessageThreadResult GetMessageThread(string UserID, string UserToken, string SenderID, string MessageCount)
        {
            GetMessageThreadResult MessageThreadResult = new GetMessageThreadResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetMessageDetail> MesssageList = new List<GetMessageDetail>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetMessageThread", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngSenderID = Convert.ToInt64(SenderID);
                    
                    //Change as per mail 151014
                    var lstMessageType0 = db.Message.Get(n => ((n.SenderID == lngSenderID && n.ReceiverID == lngUserID) || (n.SenderID == lngUserID  && n.ReceiverID == lngSenderID)) && n.MessageTypeID == 1).OrderByDescending(n => n.ID).ToList();

                    if (lstMessageType0.Count > 0)
                    {
                        if (MessageCount != null && MessageCount != "")
                        {
                            lstMessageType0 = lstMessageType0.Take(Convert.ToInt32(MessageCount)).ToList();
                        }
                        if (lstMessageType0.Count > 0)
                        {
                            foreach (var msg in lstMessageType0)
                            {
                                //Update MessageState
                                msg.State = true;
                                db.Message.Update(msg);
                                db.SaveChanges();

                                //prepare response
                                GetMessageDetail objCurrMessage = new GetMessageDetail();
                                objCurrMessage.ID = Convert.ToString(msg.ID);
                                objCurrMessage.DateCreated = Convert.ToString(msg.DateCreated);
                                objCurrMessage.SenderID = Convert.ToString(msg.SenderID);
                                objCurrMessage.ReceiverID = Convert.ToString(msg.ReceiverID);
                                objCurrMessage.State = "True";
                                objCurrMessage.Message = Convert.ToString(msg.Message1);
                                objCurrMessage.LincURL = Convert.ToString(msg.LinkURL);
                                objCurrMessage.LincUserID = Convert.ToString(msg.LinkUserID);

                                //Added by Rajendra to add Job Creator ID on 28th October
                                objCurrMessage.LinkJobCreatorID = string.Empty;
                                if (msg.LinkJobID > 0) 
                                {
                                    Int64 intJobCreatorID = db.Job.Get().Where(n => n.ID == msg.LinkJobID).Select(n => n.UserID).FirstOrDefault();
                                    objCurrMessage.LinkJobCreatorID = intJobCreatorID.ToString();
                                }
                                ///

                                objCurrMessage.LincJobID = Convert.ToString(msg.LinkJobID);
                                objCurrMessage.Type = Convert.ToString(msg.MessageTypeID);
                                MesssageList.Add(objCurrMessage);
                            }

                            //IsUnreadMessage
                            Message objCheckMessage = db.Message.Get().FirstOrDefault(n => n.State == false && n.ReceiverID == lngUserID && n.SenderID == lngSenderID && n.MessageTypeID == 1);
                            if (objCheckMessage != null)
                            {
                                MessageThreadResult.IsUnreadMessages = "True";
                            }
                            else
                            {
                                MessageThreadResult.IsUnreadMessages = "False";
                            }

                            //Add by Rajendra on 28th October for sender details
                            User objUser = db.User.Get(n => n.ID == lngSenderID).FirstOrDefault();
                            MessageThreadResult.OtherUserFirstName = objUser.FirstName;
                            MessageThreadResult.OtherUserLastName = objUser.LastName;
                            MessageThreadResult.OtherUserPhotoURL = objUser.PhotoURL;

                            //

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            MessageThreadResult.ResultStatus = ResultStatus;
                            MessageThreadResult.MesssageList = MesssageList;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "No Records on this Page Number!";
                            MessageThreadResult.ResultStatus = ResultStatus;
                            MessageThreadResult.MesssageList = MesssageList;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records";
                        MessageThreadResult.ResultStatus = ResultStatus;
                        MessageThreadResult.MesssageList = MesssageList;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                MessageThreadResult.ResultStatus = ResultStatus;
            }
            writeLog("GetMessageThread", "STOP", UserID);
            return MessageThreadResult;
        }

        #endregion

        #region SendMessage

        public GetSendMessageResult SendMessage(string UserID, string UserToken, string ReceiverID, string Message, string LinkURL, string LinkUserID, string LinkJobID)
        {
            GetSendMessageResult SendMessageResult = new GetSendMessageResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("SendMessage", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngReceiverId = Convert.ToInt64(ReceiverID);

                    //Insert record in Message table
                    Message objNewMessage = new Message();
                    objNewMessage.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                    objNewMessage.SenderID = lngUserID;
                    objNewMessage.ReceiverID = lngReceiverId;
                    objNewMessage.Message1 = Message;
                    if (LinkJobID != null && LinkJobID != "")
                    {
                        objNewMessage.LinkJobID = Convert.ToInt64(LinkJobID);
                    }
                    else
                    {
                        objNewMessage.LinkJobID = null;
                    }
                    objNewMessage.LinkURL = LinkURL;
                    if (LinkUserID != null && LinkUserID != "")
                    {
                        objNewMessage.LinkUserID = Convert.ToInt64(LinkUserID);
                    }
                    else
                    {
                        objNewMessage.LinkUserID = null;
                    }
                    objNewMessage.State = false;
                    objNewMessage.MessageTypeID = 1;
                    db.Message.Add(objNewMessage);
                    db.SaveChanges();

                    //Insert record in feed table
                    Feed objNewFeed = new Feed();
                    objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                    objNewFeed.UserID = lngReceiverId;
                    objNewFeed.FeedTypeID = 2;
                    objNewFeed.JobID = null;
                    objNewFeed.OtherUserID = lngUserID;
                    db.Feed.Add(objNewFeed);
                    db.SaveChanges();

                    //================Code for push notification===========//
                    User objUserInfo = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);
                                       
                    //Get device token of receiver
                    string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == lngReceiverId).DeviceToken;
                    ///
                    if (UserDeviceToken != null)
                    {
                        string strMessage = Message.Length > 40 ? Message.Substring(0, 40) + "..." : Message;
                        var msg = "Message from " + objUserInfo.FirstName + " " + objUserInfo.LastName +": "+ strMessage;
                        Dictionary<string, string> objParam = new Dictionary<string, string>();

                        objParam.Add("testDeviceToken", UserDeviceToken);
                        objParam.Add("pushMessage", msg);
                        objParam.Add("sourceTable", "NewMessage");
                        objParam.Add("UserID", lngUserID.ToString());
                        SendNotificationMessage(objParam);
                    }
                    //=========================end=======================//

                    //Prepare Response
                    long lngLatestMessageId = db.Message.Get().OrderByDescending(n => n.ID).FirstOrDefault().ID;
                    if (lngLatestMessageId != 0)
                    {
                        ResultStatus.Status = "1";
                        ResultStatus.StatusMessage = "";
                        SendMessageResult.ResultStatus = ResultStatus;
                        SendMessageResult.MessageID = Convert.ToString(lngLatestMessageId);
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "";
                        SendMessageResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                SendMessageResult.ResultStatus = ResultStatus;
            }
            writeLog("SendMessage", "STOP", UserID);
            return SendMessageResult;
        }

        #endregion

        #region GetPastMessages

        public GetPastMessagesResult GetPastMessages(string UserID, string UserToken, string SenderID, string MessageID, string MessageCount)
        {
            GetPastMessagesResult GetPastMessageResult = new GetPastMessagesResult();
            ResultStatus ResultStatus = new ResultStatus();
            List<GetMessageDetail> MesssageList = new List<GetMessageDetail>();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetPastMessages", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    long lngSenderID = Convert.ToInt64(SenderID);
                    long lngMessageID = Convert.ToInt64(MessageID);

                    var lstMessageType0 = db.Message.Get(n => ((n.SenderID == lngSenderID && n.ReceiverID == lngUserID) || (n.SenderID == lngUserID && n.ReceiverID == lngSenderID)) && n.MessageTypeID == 1 && n.ID < lngMessageID).OrderByDescending(n => n.ID).ToList();

                    if (lstMessageType0.Count > 0)
                    {
                        if (MessageCount != null && MessageCount != "")
                        {
                            lstMessageType0 = lstMessageType0.Take(Convert.ToInt32(MessageCount)).ToList();
                        }
                        if (lstMessageType0.Count > 0)
                        {
                            foreach (var msg in lstMessageType0)
                            {
                                //Update MessageState
                                msg.State = true;
                                db.Message.Update(msg);
                                db.SaveChanges();

                                //prepare response
                                GetMessageDetail objCurrMessage = new GetMessageDetail();
                                objCurrMessage.ID = Convert.ToString(msg.ID);
                                objCurrMessage.DateCreated = Convert.ToString(msg.DateCreated);
                                objCurrMessage.SenderID = Convert.ToString(msg.SenderID);
                                objCurrMessage.ReceiverID = Convert.ToString(msg.ReceiverID);
                                objCurrMessage.State = "True";
                                objCurrMessage.Message = Convert.ToString(msg.Message1);
                                objCurrMessage.LincURL = Convert.ToString(msg.LinkURL);
                                //Added by Rajendra to add Job Creator ID on 28th October

                                objCurrMessage.LinkJobCreatorID = string.Empty;
                                if (msg.LinkJobID > 0)
                                {
                                    Int64 intJobCreatorID = db.Job.Get().Where(n => n.ID == msg.LinkJobID).Select(n => n.UserID).FirstOrDefault();
                                    objCurrMessage.LinkJobCreatorID = intJobCreatorID.ToString();
                                }
                                ///

                                objCurrMessage.LincUserID = Convert.ToString(msg.LinkUserID);
                                objCurrMessage.LincJobID = Convert.ToString(msg.LinkJobID);
                                objCurrMessage.Type = Convert.ToString(msg.MessageTypeID);
                                MesssageList.Add(objCurrMessage);
                            }

                            //IsUnreadMessage
                            Message objCheckMessage = db.Message.Get().FirstOrDefault(n => n.State == false && n.ReceiverID == lngUserID && n.SenderID == lngSenderID && n.MessageTypeID == 1);
                            if (objCheckMessage != null)
                            {
                                GetPastMessageResult.IsUnreadMessages = "True";
                            }
                            else
                            {
                                GetPastMessageResult.IsUnreadMessages = "False";
                            }

                            ResultStatus.Status = "1";
                            ResultStatus.StatusMessage = "";
                            GetPastMessageResult.ResultStatus = ResultStatus;
                            GetPastMessageResult.MesssageList = MesssageList;
                        }
                        else
                        {
                            ResultStatus.Status = "0";
                            ResultStatus.StatusMessage = "No more records!";
                            GetPastMessageResult.ResultStatus = ResultStatus;
                            GetPastMessageResult.MesssageList = MesssageList;
                        }
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "No Records";
                        GetPastMessageResult.ResultStatus = ResultStatus;
                        GetPastMessageResult.MesssageList = MesssageList;
                    }

                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                GetPastMessageResult.ResultStatus = ResultStatus;
            }
            writeLog("GetPastMessages", "STOP", UserID);
            return GetPastMessageResult;
        }

        #endregion

        #region LogoutUser

        public ResultStatus LogoutUser(string UserID, string UserToken)
        {
            ResultStatus ResultStatus = new ResultStatus();
            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("LogoutUser", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    long lngUserID = Convert.ToInt64(UserID);
                    User objUser = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);
                    if (objUser != null)
                    {
                        objUser.DeviceToken = null;
                        objUser.Token = null;
                        db.User.Update(objUser);
                        db.SaveChanges();
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }

                ResultStatus.Status = "1";
                ResultStatus.StatusMessage = "Logout user successfully!";
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
            }
            writeLog("LogoutUser", "STOP", UserID);
            return ResultStatus;
        }

        #endregion

        #region UpdateMessageState

        public ResultStatus UpdateMessageState(string UserID, string UserToken, string MessageID)
        {
            ResultStatus ResultStatus = new ResultStatus();
            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("UpdateMessageState", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                bool isMessageExist = false;
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {
                    string[] Messages = MessageID.Split(',');
                    for (int i = 0; i <= Messages.Length - 1; i++)
                    {
                        long lngMsgID = 0;
                        long.TryParse(Messages[i], out lngMsgID);
                        if (lngMsgID > 0)
                        {
                            Message ObjMsg = db.Message.Get().FirstOrDefault(n => n.ID == lngMsgID);
                            if (ObjMsg != null)
                            {
                                ObjMsg.State = true;
                                db.Message.Update(ObjMsg);
                                db.SaveChanges();
                                isMessageExist = true;
                            }
                        }
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }

                if (!isMessageExist)
                {
                    ResultStatus.Status = "0";
                    ResultStatus.StatusMessage = "message(s) does not exist!";
                }
                else
                {
                    ResultStatus.Status = "1";
                    ResultStatus.StatusMessage = "Update message(s) state successfully!";
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
            }
            writeLog("UpdateMessageState", "STOP", UserID);
            return ResultStatus;
        }

        #endregion

        #region AcceptDeclineJobApplication

        public GetAcceptDeclineJobApplicationResult AcceptDeclineJobApplication(string UserID, string UserToken, string MessageID, string JobID, string Status)
        {
            GetAcceptDeclineJobApplicationResult AcceptDeclineJobApplicationResult = new GetAcceptDeclineJobApplicationResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("AcceptDeclineJobApplication", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                { 
                    long lngJobID ;
                    if (JobID != null && JobID != string.Empty)
                    {
                        lngJobID = Convert.ToInt64(JobID);
                    }
                    long lngUserID = Convert.ToInt64(UserID);

                    if (Status != null && Status != "")
                    {                      
                        if (Convert.ToInt32(Status) == 1)
                        {
                            Message objGetMessage = db.Message.Get().FirstOrDefault(n => n.ID == Convert.ToInt64(MessageID));
                            if (objGetMessage != null)
                            {
                                objGetMessage.MessageTypeID = 3;
                                objGetMessage.State = true;
                                db.Message.Update(objGetMessage);
                                db.SaveChanges();

                                //================Code for push notification===========//
                                User objUserInfo = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);

                                //Get sender of messge
                                long lngMessageSenderID = db.Message.Get().FirstOrDefault(n => n.ID == objGetMessage.ID).SenderID;

                                //Get Title of job
                                string strJobTitle = db.Job.Get().FirstOrDefault(n => n.ID == Convert.ToInt64(JobID)).Title;

                                //Get device token of receiver
                                string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == lngMessageSenderID).DeviceToken;
                                ///
                                if (UserDeviceToken != null)
                                {
                                    var msg = "Your job application for " + strJobTitle + " has been approved.";
                                    Dictionary<string, string> objParam = new Dictionary<string, string>();

                                    objParam.Add("testDeviceToken", UserDeviceToken);
                                    objParam.Add("pushMessage", msg);
                                    objParam.Add("sourceTable", "ApproveJobApplication");
                                    objParam.Add("UserID", lngUserID.ToString());
                                    objParam.Add("JobID", JobID);
                                    SendNotificationMessage(objParam);
                                }
                                //=========================end=======================//

                                ResultStatus.Status = "1";
                                ResultStatus.StatusMessage = "";
                                AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
                            }
                            else
                            {
                                ResultStatus.Status = "0";
                                ResultStatus.StatusMessage = "Message does not exist!";
                                AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
                            }
                        }
                        else if (Convert.ToInt32(Status) == 0)
                        {
                            Message objGetMessage = db.Message.Get().FirstOrDefault(n => n.ID == Convert.ToInt32(MessageID));
                            if (objGetMessage != null)
                            {
                                objGetMessage.MessageTypeID = 4;
                                objGetMessage.State = true;
                                db.Message.Update(objGetMessage);
                                db.SaveChanges();

                                //================Code for push notification===========//
                                User objUserInfo = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);

                                //Get sender of messge
                                long lngMessageSenderID = db.Message.Get().FirstOrDefault(n => n.ID == objGetMessage.ID).SenderID;

                                //Get Title of job
                                string strJobTitle = db.Job.Get().FirstOrDefault(n => n.ID == Convert.ToInt64(JobID)).Title;

                                //Get device token of receiver
                                string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == lngMessageSenderID).DeviceToken;
                                ///
                                if (UserDeviceToken != null)
                                {
                                    var msg = "Your job application for " + strJobTitle + " has been declined.";
                                    Dictionary<string, string> objParam = new Dictionary<string, string>();

                                    objParam.Add("testDeviceToken", UserDeviceToken);
                                    objParam.Add("pushMessage", msg);
                                    objParam.Add("sourceTable", "DeclineJobApplication");
                                    objParam.Add("UserID", lngUserID.ToString());
                                    objParam.Add("JobID", JobID);
                                    SendNotificationMessage(objParam);
                                }
                                //=========================end=======================//

                                ResultStatus.Status = "1";
                                ResultStatus.StatusMessage = "";
                                AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
                            }
                            else
                            {
                                ResultStatus.Status = "0";
                                ResultStatus.StatusMessage = "Message does not exist!";
                                AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
                            } 
                        }
                       
                    }
                    else
                    {
                        ResultStatus.Status = "0";
                        ResultStatus.StatusMessage = "Invalid Parameter!";
                        AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
                    }
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                AcceptDeclineJobApplicationResult.ResultStatus = ResultStatus;
            }
            writeLog("AcceptDeclineJobApplication", "STOP", UserID);
            return AcceptDeclineJobApplicationResult;
        }

        #endregion

        #region GetUnreadMessageCount

        public GetUnreadMessageCountResult GetUnreadMessageCount(string UserID, string UserToken)
        {
            GetUnreadMessageCountResult UnreadMessageCountResult = new GetUnreadMessageCountResult();
            ResultStatus ResultStatus = new ResultStatus();

            objTokenInfo = LoginStatus.ValidateToken(UserToken, UserID);
            try
            {
                writeLog("GetUnreadMessageCount", "START", UserID);
                UnitOfWork db = new UnitOfWork();
                if (objTokenInfo != null && objTokenInfo.EmailID != null)
                {                 
                    long lngUserID = Convert.ToInt64(UserID);
                    //Get Unread Message Count
                    int objMessageCount = db.Message.Get().Where(n => n.ReceiverID == lngUserID && n.State == false).ToList().Count;

                    UnreadMessageCountResult.NumberOfUnreadMessage = Convert.ToString(objMessageCount);
                    //====================================================================//
                    ResultStatus.Status = "1";
                    ResultStatus.StatusMessage = "";
                    UnreadMessageCountResult.ResultStatus = ResultStatus;
                }
                else
                {
                    throw new WebFaultException<string>("Please enter validate token.", HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = ex.Message;
                UnreadMessageCountResult.ResultStatus = ResultStatus;
            }
            writeLog("GetUnreadMessageCount", "STOP", UserID);
            return UnreadMessageCountResult;
        }

        #endregion

        public ResultStatus TestNotification()
        {
            writeLog("Notification", "START", "0");
            ResultStatus objResultStatus = new ResultStatus();
            CheckJobStatusAndSendNotification();
            writeLog("Notification", "STOP", "0");
            return objResultStatus;
        }

        public static void CheckJobStatusAndSendNotification()
        {
            UnitOfWork db = new UnitOfWork();
            //List<Job> objGetJob = db.Job.Get().Where(n => n.State != true && DateTime.Compare(n.DateModified, DateTime.Now.Date) > 30)).ToList();
            List<Job> objGetJob = db.Job.Get().Where(n => n.State != true && n.DateModified.Date < TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Date.AddDays(-30)).ToList();
            if (objGetJob != null)
            {
                foreach (var job in objGetJob)
                { 
                    //Get UserDevice token of JobUserID
                    string UserDeviceToken = db.User.Get().FirstOrDefault(n => n.ID == job.UserID).DeviceToken;
                    ///
                    if (UserDeviceToken != null)
                    {
                        var msg = "Is the job, "+ job.Title  +" still active? Log in to update the status.";
                        Dictionary<string, string> objParam = new Dictionary<string, string>();

                        objParam.Add("testDeviceToken", UserDeviceToken);
                        objParam.Add("pushMessage", msg);
                        objParam.Add("sourceTable", "Unfilled30days");
                        objParam.Add("UserID", job.UserID.ToString());
                        objParam.Add("JobID", job.ID.ToString());
                        SendNotificationMessage(objParam);
                    }
                    //=========================end=======================//

                }
            }
        }

        #region Common Methods

        #region Helper methods

        /// <summary>
        /// Gets the Web response context for the request being received.
        /// </summary>
        public IncomingWebRequestContext Request
        {
            get { return WebOperationContext.Current.IncomingRequest; }
        }

        /// <summary>
        /// Gets the Web request context for the request being sent.
        /// </summary>
        public OutgoingWebResponseContext Response
        {
            get { return WebOperationContext.Current.OutgoingResponse; }
        }

        #endregion

        /// <summary>
        /// Find File Path of folder
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        public string FindFilePath(string foldername)
        {
            return System.Web.HttpContext.Current.Server.MapPath(foldername);
        }

        /// <summary>
        /// Write log of service
        /// </summary>
        /// <param name="strMethodName"></param>
        /// <param name="strMessage"></param>
        /// <param name="strUserID"></param>
        private void writeLog(string strMethodName, string strMessage, string strUserID)
        {
            try
            {
                if (!Directory.Exists(FindFilePath("WCFLog")))
                {
                    Directory.CreateDirectory(FindFilePath("WCFLog"));
                }

                string isWriteLog = System.Configuration.ConfigurationManager.AppSettings["writeLog"];
                if (isWriteLog.ToLower() == "true")
                {
                    string strPath = System.Web.HttpContext.Current.Server.MapPath("WCFLog");
                    string filePath = strPath + "\\WCFLog.txt";
                    TextWriter tsw3 = new StreamWriter(filePath, true);
                    string strLog = "Date: " + DateTime.Now.ToString() + " ::: Method name: " + strMethodName + " ::: UserID: " + strUserID + " ::: Message: " + strMessage;
                    tsw3.WriteLine(strLog);
                    tsw3.Close();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void DeleteImage(string ImageName)
        {
            string filePath = Path.Combine(FindFilePath("ImageUpload")) + "\\" + ImageName;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// save image in image folder of given size,create ThumbImage
        /// </summary>
        /// <param name="base64stringImage"></param>
        /// <param name="AppendPrefix"></param>
        /// <param name="intThumbWidth"></param>
        /// <param name="intThumbHeight"></param>
        /// <returns></returns>

        //public string SaveImage(string base64stringImage, string AppendPrefix, int intThumbWidth, int intThumbHeight)
        //{
        //    string filePath = "";
        //    string _fileName = "";

        //    try
        //    {
        //        if (!Directory.Exists(FindFilePath("ImageUpload")))
        //        {
        //            Directory.CreateDirectory(FindFilePath("ImageUpload"));
        //        }
        //        _fileName = AppendPrefix + Guid.NewGuid().ToString() + ".PNG";
        //        filePath = Path.Combine(FindFilePath("ImageUpload")) + "\\" + _fileName;
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //        }


        //        var bytes = Convert.FromBase64String(base64stringImage);
        //        using (var imageFile = new FileStream(filePath, FileMode.Create))
        //        {
        //            imageFile.Write(bytes, 0, bytes.Length);
        //            imageFile.Flush();
        //        }


        //        //Create Thumbnail
        //        //Image image = Image.FromFile(filePath);
        //        Image image1 = getCroppedImage(filePath);
        //        Image thumb = image1.GetThumbnailImage(intThumbWidth, intThumbHeight, () => false, IntPtr.Zero);
        //        thumb.Save(filePath.Replace(".PNG", "_Thumb.PNG"));
        //        image1.Dispose();
        //        thumb.Dispose();
        //        return _fileName;
        //    }
        //    catch (Exception e)
        //    {
        //        return _fileName;
        //    }
        //}

        //private Image getCroppedImage(string path)
        //{
        //    Bitmap bmpImage = new Bitmap(path);

        //    int rectX = 0;
        //    int rectY = 0;
        //    int rectWidthHeight = bmpImage.Width;

        //    if (bmpImage.Width > bmpImage.Height)
        //    {
        //        rectX = (bmpImage.Width - bmpImage.Height) / 2;
        //        rectY = 0;
        //        rectWidthHeight = bmpImage.Height;
        //    }
        //    else if (bmpImage.Width < bmpImage.Height)
        //    {
        //        rectX = 0;
        //        rectY = (bmpImage.Height - bmpImage.Width) / 2;
        //        rectWidthHeight = bmpImage.Width;
        //    }

        //    Bitmap image = bmpImage.Clone(new Rectangle(rectX, rectY, rectWidthHeight, rectWidthHeight), bmpImage.PixelFormat);
        //    bmpImage.Dispose();
        //    return image;
        //}

        public string SaveImage(string base64stringImage, string AppendPrefix, int intThumbWidth, int intThumbHeight)
        {
            string filePath = "";
            string _fileName = "";

            try
            {
                if (!Directory.Exists(FindFilePath("ImageUpload")))
                {
                    Directory.CreateDirectory(FindFilePath("ImageUpload"));
                }
                _fileName = AppendPrefix + Guid.NewGuid().ToString() + ".PNG";
                filePath = Path.Combine(FindFilePath("ImageUpload")) + "\\" + _fileName;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }


                var bytes = Convert.FromBase64String(base64stringImage);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                //--------- Check for source image have required orientation
                Image SourceImage = Image.FromFile(filePath);
                bool IsReqOrientation = false;
                if (SourceImage.PropertyIdList.Contains(0x112)) //0x112 = Orientation
                {
                    var prop = SourceImage.GetPropertyItem(0x112);
                    if (prop.Type == 3 && prop.Len == 2)
                    {
                        UInt16 orientationExif = BitConverter.ToUInt16(SourceImage.GetPropertyItem(0x112).Value, 0);
                        if (orientationExif == 8)
                        {
                            IsReqOrientation = true;
                        }
                        else if (orientationExif == 3)
                        {
                            IsReqOrientation = true;
                        }
                        else if (orientationExif == 6)
                        {
                            IsReqOrientation = true;
                        }
                    }
                }
                //--------- Close Check for source image have required orientation
                
                //--------- If image required orientation then crop with 100*100 size otherwise do same as before.
                Image image1 = SourceImage;
                if (IsReqOrientation)
                {
                    image1 = getCroppedWithOrientation(filePath, SourceImage, image1.Width, image1.Width);
                }
                else
                {
                    image1 = getCroppedImage(filePath);
                }
                //-----------------------------------------------------------------------

                //Create Thumbnail  
                Image thumb = image1.GetThumbnailImage(intThumbWidth, intThumbHeight, () => false, IntPtr.Zero);
                thumb.Save(filePath.ToLower().Contains(".png") ? filePath.ToLower().Replace(".png", "_Thumb.png") : filePath.ToLower().Contains(".jpg") ? filePath.ToLower().Replace(".jpg", "_Thumb.jpg") : filePath.ToLower().Contains(".jpeg") ? filePath.ToLower().Replace(".jpeg", "_Thumb.jpeg") : filePath.ToLower().Contains(".gif") ? filePath.ToLower().Replace(".gif", "_Thumb.gif") : filePath.ToLower().Contains(".bmp") ? filePath.ToLower().Replace(".bmp", "_Thumb.bmp") : filePath.ToLower());    
                //thumb.Save(filePath.Replace(".PNG", "_Thumb.PNG"));
                image1.Dispose();
                thumb.Dispose();
                return _fileName;
            }
            catch (Exception e)
            {
                return _fileName;
            }
        }

        private Image getCroppedWithOrientation(string path, Image image1, int reqWidth, int reqHeight)
        {           
            Bitmap bmpImage = new Bitmap(path);


            if (image1.PropertyIdList.Contains(0x112)) //0x112 = Orientation
            {
                var prop = image1.GetPropertyItem(0x112);
                if (prop.Type == 3 && prop.Len == 2)
                {
                    UInt16 orientationExif = BitConverter.ToUInt16(image1.GetPropertyItem(0x112).Value, 0);
                    if (orientationExif == 8)
                    {
                        bmpImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    else if (orientationExif == 3)
                    {
                        bmpImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (orientationExif == 6)
                    {
                        bmpImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
            }

            int OrgX = bmpImage.Width;
            int OrgY = bmpImage.Height;

            int RectX = 0;
            int RectY = 0;
           
                int center = Convert.ToInt32(OrgX / 2);
                RectX = center - (OrgX / 2);
           

           
                int centery = Convert.ToInt32(OrgY / 2);
                RectY = centery - (OrgX / 2);


                Bitmap image = bmpImage.Clone(new Rectangle(RectX, RectY, OrgX, OrgX), bmpImage.PixelFormat);
            bmpImage.Dispose();
            return image;
        }

        private Image getCroppedImage(string path)
        {
            Bitmap bmpImage = new Bitmap(path);

            int rectX = 0;
            int rectY = 0;
            int rectWidthHeight = bmpImage.Width;

            if (bmpImage.Width > bmpImage.Height)
            {
                rectX = (bmpImage.Width - bmpImage.Height) / 2;
                rectY = 0;
                rectWidthHeight = bmpImage.Height;
            }
            else if (bmpImage.Width < bmpImage.Height)
            {
                rectX = 0;
                rectY = (bmpImage.Height - bmpImage.Width) / 2;
                rectWidthHeight = bmpImage.Width;
            }

            Bitmap image = bmpImage.Clone(new Rectangle(rectX, rectY, rectWidthHeight, rectWidthHeight), bmpImage.PixelFormat);
            bmpImage.Dispose();
            return image;
        }

        /// <summary>
        /// GetUserDetail of given User and set token if required
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="IsSetToken"></param>
        /// <returns></returns>
        public GetIsUserExistsResult GetUserDetails(long lngUserID, Boolean IsSetToken)
        {
            GetIsUserExistsResult GetUserDetailResult = new GetIsUserExistsResult();
            GetUserResult UserResult = new GetUserResult();
            List<GetUserSkillResult> lstUserSkillResult = new List<GetUserSkillResult>();            
            List<GetUserEmploymentResult> lstUserEmploymentResult = new List<GetUserEmploymentResult>();
            List<GetUserEducationWithCourseResult> lstUserEducationWithCourseResult = new List<GetUserEducationWithCourseResult>();
            List<GetUserEmploymentRecommendationResult> lstUserEmploymentrecommendation = new List<GetUserEmploymentRecommendationResult>();
            ResultStatus ResultStatus = new ResultStatus();

            UnitOfWork db = new UnitOfWork();
            User objUser = db.User.Get().FirstOrDefault(n => n.ID == lngUserID);
            if (objUser != null)
            {

                //=====================Prepare UserDetail Response==============//
                UserResult.UserId = Convert.ToString(objUser.ID);
                UserResult.DateCreated = Convert.ToString(objUser.DateCreated);
                UserResult.DateModified = Convert.ToString(objUser.DateModified);
                UserResult.Email = objUser.Email;
                UserResult.FirstName = objUser.FirstName;
                UserResult.LastName = objUser.LastName;
                UserResult.LocationCity = objUser.LocationCity;
                UserResult.LocationState = objUser.LocationState;
                UserResult.LocationCountry = objUser.LocationCountry;
                UserResult.Industry = objUser.Industry;
                UserResult.Industry2 = objUser.Industry2;
                UserResult.Summary = objUser.Summary;
                UserResult.PhotoURL = objUser.PhotoURL;
                UserResult.LinkedInId = objUser.LinkedInId;
                UserResult.ExperienceLevel = Convert.ToString(objUser.ExperienceLevelType);
                UserResult.Token = objUser.Token;

                if (IsSetToken == true)
                {
                    //======================Set Token value====================//
                    CrowdWCFservice.Token.tbl_TokenInfo tokeninfo = new CrowdWCFservice.Token.tbl_TokenInfo();
                    tokeninfo.EmailID = objUser.Email.Trim();
                    Token objToken = new Token();
                    string token = objToken.SetUserToken(tokeninfo, "30");
                    Response.Headers.Add("token", token);
                    UserResult.Token = token;
                    //================================+========================//
                }

                //Get Unread Message Count
                int objMessageCount = db.Message.Get().Where(n => n.ReceiverID == objUser.ID && n.State == false).ToList().Count;
                //
                UserResult.NumberOfUnreadMessage = Convert.ToString(objMessageCount);
                //====================================================================//

                //===========================GetUserSkill=====================================//
                List<UserSkill> objUserSkill = db.UserSkill.Get().Where(n => n.UserID == objUser.ID).ToList();
                if (objUserSkill.Count > 0)
                {
                    foreach (var skill in objUserSkill)
                    {
                        GetUserSkillResult objCurrUserSkill = new GetUserSkillResult();
                        objCurrUserSkill.ID = Convert.ToString(skill.ID);
                        objCurrUserSkill.DateCreated = Convert.ToString(skill.DateCreated);
                        objCurrUserSkill.UserID = Convert.ToString(skill.UserID);
                        objCurrUserSkill.Skills = skill.Skill;
                        lstUserSkillResult.Add(objCurrUserSkill);
                    }
                }
                //=====================================================================================//

                //============================GetUserEmployment===========================================//
                List<UserEmployment> objUserEmployment = db.UserEmployment.Get().Where(n => n.UserID == objUser.ID).ToList();
                if (objUserEmployment.Count > 0)
                {
                    foreach (var employment in objUserEmployment)
                    {
                        //Prepare UserEmploment Response
                        GetUserEmploymentResult objCurrUserEmployment = new GetUserEmploymentResult();
                        objCurrUserEmployment.ID = Convert.ToString(employment.ID);
                        objCurrUserEmployment.DateCreated = Convert.ToString(employment.DateCreated);
                        objCurrUserEmployment.DateModified = Convert.ToString(employment.DateModified);
                        objCurrUserEmployment.UserID = Convert.ToString(employment.UserID);
                        objCurrUserEmployment.EmployerName = employment.EmployerName;
                        objCurrUserEmployment.Title = employment.Title;
                        objCurrUserEmployment.LocationCity = employment.LocationCity;
                        objCurrUserEmployment.LocationCountry = employment.LocationCountry;
                        objCurrUserEmployment.LocationState = employment.LocationState;
                        objCurrUserEmployment.StartYear = Convert.ToString(employment.StartYear);
                        objCurrUserEmployment.EndYear = employment.EndYear != null ? Convert.ToString(employment.EndYear) : string.Empty;
                        objCurrUserEmployment.Summary = employment.Summary;
                        objCurrUserEmployment.StartMonth = Convert.ToString(employment.StartMonth);
                        objCurrUserEmployment.EndMonth = employment.EndMonth != null ? Convert.ToString(employment.EndMonth) : string.Empty;

                        lstUserEmploymentResult.Add(objCurrUserEmployment);
                        ////============================================================================//
                    }
                }
                //=============================================================================================================================//

                //===================GetUserEmploymentRecommendation=========================//
                List<UserEmploymentRecommendation> objEmploymentRecommendation = db.UserEmploymentRecommendation.Get().Where(n => n.UserID == lngUserID).ToList();
                if (objEmploymentRecommendation.Count > 0)
                {
                    foreach (var er in objEmploymentRecommendation)
                    {
                        GetUserEmploymentRecommendationResult objCurrRecommendation = new GetUserEmploymentRecommendationResult();
                        objCurrRecommendation.ID = Convert.ToString(er.ID);
                        objCurrRecommendation.DateCreated = Convert.ToString(er.DateCreated);
                        objCurrRecommendation.UserID = Convert.ToString(er.UserID);
                        objCurrRecommendation.Recommendation = er.Recommendation;
                        objCurrRecommendation.RecommenderName = er.RecommenderName;
                        lstUserEmploymentrecommendation.Add(objCurrRecommendation);
                    }
                }

                //========================================GetUserEducationResult===============================================================//
                List<UserEducation> objUserEducation = db.UserEducation.Get().Where(n => n.UserID == objUser.ID).ToList();
                if (objUserEducation.Count > 0)
                {
                    foreach (var education in objUserEducation)
                    {
                        //Prepare UserEducation Response
                        GetUserEducationWithCourseResult objCurrUserEducation = new GetUserEducationWithCourseResult();
                        objCurrUserEducation.ID = Convert.ToString(education.ID);
                        objCurrUserEducation.DateCreated = Convert.ToString(education.DateCreated);
                        objCurrUserEducation.DateModified = Convert.ToString(education.DateModified);
                        objCurrUserEducation.UserID = Convert.ToString(education.UserID);
                        objCurrUserEducation.Name = education.Name;
                        objCurrUserEducation.Degree = education.Degree;
                        objCurrUserEducation.StartYear = Convert.ToString(education.StartYear);
                        objCurrUserEducation.EndYear = education.EndYear != null ? Convert.ToString(education.EndYear) : string.Empty;
                        objCurrUserEducation.StartMonth = Convert.ToString(education.StartMonth);
                        objCurrUserEducation.EndMonth = education.EndMonth != null ? Convert.ToString(education.EndMonth) : string.Empty;

                        //======================GetUserEducationCourse Result==============//
                        List<UserEducationCourse> objEducationCourse = db.UserEducationCourse.Get().Where(n => n.EducationID == education.ID).ToList();
                        if (objEducationCourse.Count > 0)
                        {
                            List<GetUserEducationCourseResult> lstCourseForCurrEducation = new List<GetUserEducationCourseResult>();
                            foreach (var course in objEducationCourse)
                            {
                                GetUserEducationCourseResult objCurrCourse = new GetUserEducationCourseResult();
                                objCurrCourse.ID = Convert.ToString(course.ID);
                                objCurrCourse.DateCreated = Convert.ToString(course.DateCreated);
                                objCurrCourse.EducationID = Convert.ToString(course.EducationID);
                                objCurrCourse.Course = course.Course;
                                lstCourseForCurrEducation.Add(objCurrCourse);
                            }                         
                            objCurrUserEducation.GetUserEducationCourseResult = lstCourseForCurrEducation;
                        }
                        //Added by himanshu: mail on 03-10-2014.
                        else
                        {
                            List<GetUserEducationCourseResult> lstCourseForCurrEducation = new List<GetUserEducationCourseResult>();                           
                            objCurrUserEducation.GetUserEducationCourseResult = lstCourseForCurrEducation;
                        }
                        //Done Added by himanshu
                        //==================================================================//

                        lstUserEducationWithCourseResult.Add(objCurrUserEducation);
                    }
                }
                //==================================================================================================================================//
                
                ResultStatus.Status = "1";
                ResultStatus.StatusMessage = "";
                GetUserDetailResult.ResultStatus = ResultStatus;
                GetUserDetailResult.GetUserResult = UserResult;
                GetUserDetailResult.GetUserSkillResult = lstUserSkillResult;
                GetUserDetailResult.GetUserEmploymentResult = lstUserEmploymentResult;
                GetUserDetailResult.GetUserEducationWithCourseResult = lstUserEducationWithCourseResult;
                GetUserDetailResult.GetUserEmploymentRecommendationResult = lstUserEmploymentrecommendation;
            }
            else
            {
                ResultStatus.Status = "0";
                ResultStatus.StatusMessage = "User does not exist!";
                GetUserDetailResult.ResultStatus = ResultStatus;
                GetUserDetailResult.GetUserResult = UserResult;
                GetUserDetailResult.GetUserSkillResult = lstUserSkillResult;
                GetUserDetailResult.GetUserEmploymentResult = lstUserEmploymentResult;
                GetUserDetailResult.GetUserEducationWithCourseResult = lstUserEducationWithCourseResult;
                GetUserDetailResult.GetUserEmploymentRecommendationResult = lstUserEmploymentrecommendation;
            }
            return GetUserDetailResult;
        }

        public static void SendNotificationMessage_JDSoft(object obj)
        {
            bool returnValue = false;
            string apn_developer_identity = string.Empty;
            bool IsSandbox;
            string strp12FileLocal = ConfigurationManager.AppSettings["p12FileName_Local"];
            string strp12FileLive = ConfigurationManager.AppSettings["p12FileName_Live"];
            try
            {
                if (ConfigurationManager.AppSettings["IsProductionForP12File"].ToUpper() == "true".ToUpper())
                {
                    apn_developer_identity = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strp12FileLive);
                    IsSandbox = false;
                }
                else
                {
                    apn_developer_identity = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strp12FileLocal);
                    IsSandbox = true;
                }
                string P12FilePassword = ConfigurationManager.AppSettings["p12FilePassword"];

                ////bool IsSandbox = false;
                //bool IsSandbox = true;

                Dictionary<string, string> objParam = (obj as Dictionary<string, string>);
                string testDeviceToken = objParam["testDeviceToken"];
                string pushMessage = objParam["pushMessage"];
                string sourceTable = objParam["sourceTable"];

                if (testDeviceToken == null || testDeviceToken.Length < 64)
                {
                    return;
                }

                NotificationService service = new NotificationService(IsSandbox, apn_developer_identity, P12FilePassword, 1);
                service.SendRetries = 5;
                service.ReconnectDelay = 5000;

                JdSoft.Apple.Apns.Notifications.Notification alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(testDeviceToken);
                alertNotification.Payload.Sound = "notification.wav";
                alertNotification.Payload.Alert.Body = string.Format("{0}", pushMessage);

                List<object> objSourceTable = new List<object>();
                objSourceTable.Add(sourceTable);

                string UserID = string.Empty;
                string JobID = string.Empty;

                if (objParam.ContainsKey("UserID") && objParam["UserID"] != null && objParam["UserID"] != "")
                {
                    UserID = objParam["UserID"];
                    objSourceTable.Add(UserID);
                }
                if (objParam.ContainsKey("JobID") && objParam["JobID"] != null && objParam["JobID"] != "")
                {
                    JobID = objParam["JobID"];
                    objSourceTable.Add(JobID);
                } 
                
                alertNotification.Payload.Alert.LocalizedArgs = objSourceTable;

                returnValue = service.QueueNotification(alertNotification);

                System.Threading.Thread.Sleep(1000);
                service.Close();
                service.Dispose();

            }
            catch (Exception ex)
            {

            }
        }

        public static void SendNotificationMessage(object obj)
        {

            string apn_developer_identity = string.Empty;
            string strp12FileLocal = ConfigurationManager.AppSettings["p12FileName_Local"];
            string strp12FileLive = ConfigurationManager.AppSettings["p12FileName_Live"];

            try
            {
                if (ConfigurationManager.AppSettings["IsProductionForP12File"].ToUpper() == "true".ToUpper())
                {
                    apn_developer_identity = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strp12FileLive);
                }
                else
                {
                    apn_developer_identity = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strp12FileLocal);
                }
                string P12FilePassword = ConfigurationManager.AppSettings["p12FilePassword"];

                Dictionary<string, string> objParam = (obj as Dictionary<string, string>);
                string testDeviceToken = objParam["testDeviceToken"];
                string pushMessage = objParam["pushMessage"];
                string sourceTable = objParam["sourceTable"];

                if (testDeviceToken == null || testDeviceToken.Length < 64)
                {
                    return;
                }

                //Create our push services broker
                var push = new PushBroker();

                //Registering the Apple Service and sending an iOS Notification
                var appleCert = File.ReadAllBytes(apn_developer_identity);
                push.RegisterAppleService(new ApplePushChannelSettings(appleCert, P12FilePassword));

                AppleNotification objNotification = new AppleNotification();
                objNotification.DeviceToken = testDeviceToken;
                objNotification.Payload.Alert.Body = string.Format("{0}", pushMessage);
                objNotification.Payload.Sound = "notification.wav";


                List<object> objSourceTable = new List<object>();
                objSourceTable.Add(sourceTable);

                string UserID = string.Empty;
                string JobID = string.Empty;

                if (objParam.ContainsKey("UserID") && objParam["UserID"] != null && objParam["UserID"] != "")
                {
                    UserID = objParam["UserID"];
                    objSourceTable.Add(UserID);
                }
                if (objParam.ContainsKey("JobID") && objParam["JobID"] != null && objParam["JobID"] != "")
                {
                    JobID = objParam["JobID"];
                    objSourceTable.Add(JobID);
                }

                objNotification.Payload.Alert.LocalizedArgs = objSourceTable;

                push.QueueNotification(objNotification);

                System.Threading.Thread.Sleep(500);
                //Stop and wait for the queues to drains
                push.StopAllServices();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
