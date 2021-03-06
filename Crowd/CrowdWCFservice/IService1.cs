﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CrowdWCFservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        //TODO: Add your service operations here
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "IsUserExists", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetIsUserExistsResult IsUserExists(string LinkedInID, string DeviceToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "AddEditUserDetails", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetIsUserExistsResult AddEditUserDetails(string UserID, string UserToken, string Email, string FirstName, string LastName, string LocationCity, string LocationState, string LocationCountry, string Industry, string Industry2, string Summary, string PhotoData, string LinkedInId, string DeviceToken, string ExperienceLevel, PrSkill[] UserSkills, PrUserEmployment[] UserEmployments, PrUserEducation[] UserEducations, PrEmploymentRecommendation[] EmploymentRecommendation);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserDetails", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetUserDetailsResult GetUserDetails(string UserID, string UserToken, string OtherUserID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetActivityFeeds", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetActivityFeedsResult GetActivityFeeds(string UserID, string UserToken, string PageNumber, string IsRequiredAcceptDeclineJobFeed);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetMyCrowd", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetMyCrowdResult GetMyCrowd(string UserID, string UserToken, string PageNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "FollowUnfollowUser", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetFollowUnfollowUserResult FollowUnfollowUser(string UserID, string UserToken, string FollowUserID,string Status);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "AddEditJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetAddEditJobResult AddEditJob(string UserID, string UserToken, string JobID,string Company,string Industry,string Industry2,string Title,string LocationCity,string LocationState,string LocationCountry,string ExperienceLevel,string Responsibilities,string Qualifications,string EmployerIntroduction,string JobURL,PrSkill[] Skills);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetJobDetails", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetJobDetailsResult GetJobDetails(string UserID, string UserToken, string JobID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "FavoriteJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetFavoriteJobResult FavoriteJob(string UserID, string UserToken, string JobID, string Status);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ApplyToJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetApplyToJobResult ApplyToJob(string UserID, string UserToken, string JobID, string Status);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "FillReopenJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetFillReopenJobResult FillReopenJob(string UserID, string UserToken, string JobID, string Status);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetDeleteJobResult DeleteJob(string UserID, string UserToken, string JobID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SearchJob", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetSearchJobResult SearchJob(string UserID, string UserToken, string Industry, string Industry2, string ExperienceLevel, string Position, string LocationCity, string LocationState, string LocationCountry, string Company, string PageNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SearchCandidates", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetSearchCandidatesResult SearchCandidates(string UserID, string UserToken, string Industry, string Industry2, string ExperienceLevel, string LocationCity, string LocationState, string LocationCountry, string Company, string PageNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserJobs", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetUserJobsResult GetUserJobs(string UserID, string UserToken, string OtherUserID,string PageNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetMessageList", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetMessageListResult GetMessageList(string UserID, string UserToken, string PageNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetMessageThread", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetMessageThreadResult GetMessageThread(string UserID, string UserToken, string SenderID, string MessageCount);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "LogoutUser", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResultStatus LogoutUser(string UserID, string UserToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateMessageState", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResultStatus UpdateMessageState(string UserID, string UserToken, string MessageID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendMessage", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetSendMessageResult SendMessage(string UserID, string UserToken, string ReceiverID, string Message, string LinkURL, string LinkUserID, string LinkJobID);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetPastMessages", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetPastMessagesResult GetPastMessages(string UserID, string UserToken, string SenderID, string MessageID, string MessageCount);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "AcceptDeclineJobApplication", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetAcceptDeclineJobApplicationResult AcceptDeclineJobApplication(string UserID, string UserToken, string MessageID, string JobID, string Status, string IsRequiredFeedCreated);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUnreadMessageCount", BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetUnreadMessageCountResult GetUnreadMessageCount(string UserID, string UserToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "TestNotification", BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResultStatus TestNotification();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SetAvailableForCall", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AvailabilityResult SetAvailableForCall(string UserID, string UserToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SetUnAvailableForCall", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AvailabilityResult SetUnAvailableForCall(string UserID, string UserToken);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCallAvailability", BodyStyle = WebMessageBodyStyle.Wrapped)]
        AvailabilityResult GetCallAvailability(string UserID);
    }    

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [Serializable]

    #region ResultStatus
    [DataContract]
    public class ResultStatus
    {
        private string _status = string.Empty;
        private string _statusMessage = string.Empty;

        [DataMember]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [DataMember]
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; }
        }
    }
    #endregion

    #region AvailabilityResult
    [DataContract]
    public class AvailabilityResult
    {
        private DateTime _dateExpires;
        private int _renewAfterSeconds;
        private bool _isAvailableForCall;
        private ResultStatus _ResultStatus;

        [DataMember]
        public int RenewAfterSeconds
        {
            get { return _renewAfterSeconds; }
            set { _renewAfterSeconds = value; }
        }

        [DataMember]
        public DateTime DateExpires
        {
            get {return _dateExpires;}
            set {_dateExpires = value;}
        }

        [DataMember]
        public bool IsAvailableForCall
        {
            get {return _isAvailableForCall;}
            set {_isAvailableForCall = value;}
        }

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }


    }
    
    #endregion


    #region GetUserResult

    [DataContract]
    public class GetUserResult
    {
        private string _UserId = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _Email = string.Empty;
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Summary = string.Empty;
        private string _PhotoURL = string.Empty;
        private string _LinkedInId = string.Empty;
        private string _ExperienceLevel = string.Empty;
        private string _Token = string.Empty;
        private string _NumberOfUnreadMessage = string.Empty;
        private bool _IsAvailableForCall = false;

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }

        [DataMember]
        public string PhotoURL
        {
            get { return _PhotoURL; }
            set { _PhotoURL = value; }
        }

        [DataMember]
        public string LinkedInId
        {
            get { return _LinkedInId; }
            set { _LinkedInId = value; }
        }

        [DataMember]
        public string ExperienceLevel
        {
            get { return _ExperienceLevel; }
            set { _ExperienceLevel = value; }
        }

        [DataMember]
        public string Token
        {
            get { return _Token; }
            set { _Token = value; }
        }

        [DataMember]
        public string NumberOfUnreadMessage
        {
            get { return _NumberOfUnreadMessage; }
            set { _NumberOfUnreadMessage = value; }
        }

        [DataMember]
        public bool IsAvailableForCall
        {
            get { return _IsAvailableForCall; }
            set { _IsAvailableForCall = value; }
        }
    }
    #endregion

    #region GetUserSkillResult

    [DataContract]
    public class GetUserSkillResult
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _UserID = string.Empty;
        private string _Skills = string.Empty;
       
        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string Skills
        {
            get { return _Skills; }
            set { _Skills = value; }
        }
    }
    #endregion

    #region GetUserEmploymentResult

    [DataContract]
    public class GetUserEmploymentResult
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserID = string.Empty;
        private string _EmployerName = string.Empty;
        private string _Title = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _StartYear = string.Empty;
        private string _EndYear = string.Empty;
        private string _Summary = string.Empty;
        private string _StartMonth = string.Empty;
        private string _EndMonth = string.Empty;
        //private List<GetUserEmploymentRecommendationResult> _GetUserEmploymentRecommendationResult;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string EmployerName
        {
            get { return _EmployerName; }
            set { _EmployerName = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        [DataMember]
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }

        [DataMember]
        public string StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        [DataMember]
        public string EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        //[DataMember]
        //public List<GetUserEmploymentRecommendationResult> GetUserEmploymentRecommendationResult
        //{
        //    get { return _GetUserEmploymentRecommendationResult; }
        //    set { _GetUserEmploymentRecommendationResult = value; }
        //}
    }
    #endregion

    #region GetUserEmploymentRecommendationResult

    [DataContract]
    public class GetUserEmploymentRecommendationResult
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _UserID = string.Empty;
        private string _Recommendation = string.Empty;
        private string _RecommenderName = string.Empty;
     
        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string Recommendation
        {
            get { return _Recommendation; }
            set { _Recommendation = value; }
        }

        [DataMember]
        public string RecommenderName
        {
            get { return _RecommenderName; }
            set { _RecommenderName = value; }
        }
    }
    #endregion

    #region GetUserEducationWithCourseResult

    [DataContract]
    public class GetUserEducationWithCourseResult
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserID = string.Empty;
        private string _Name = string.Empty;
        private string _Degree = string.Empty;  
        private string _StartYear = string.Empty;
        private string _EndYear = string.Empty;
        private string _StartMonth = string.Empty;
        private string _EndMonth = string.Empty;
        private List<GetUserEducationCourseResult> _GetUserEducationCourseResult;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [DataMember]
        public string Degree
        {
            get { return _Degree; }
            set { _Degree = value; }
        }

        [DataMember]
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        [DataMember]
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        [DataMember]
        public string StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        [DataMember]
        public string EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        [DataMember]
        public List<GetUserEducationCourseResult> GetUserEducationCourseResult
        {
            get { return _GetUserEducationCourseResult; }
            set { _GetUserEducationCourseResult = value; }
        }
    }
    #endregion

    #region GetUserEducationCourseResult

    [DataContract]
    public class GetUserEducationCourseResult
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _EducationID = string.Empty;
        private string _Course = string.Empty;
        
        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string EducationID
        {
            get { return _EducationID; }
            set { _EducationID = value; }
        }

        [DataMember]
        public string Course
        {
            get { return _Course; }
            set { _Course = value; }
        }
    }
    #endregion

    #region GetIsUserExistsResult
    [DataContract]
    public class GetIsUserExistsResult
    {
        private ResultStatus _ResultStatus;
        private GetUserResult _GetUserResult;
        private List<GetUserSkillResult> _GetUserSkillResult;
        private List<GetUserEmploymentResult> _GetUserEmploymentResult;
        private List<GetUserEducationWithCourseResult> _GetUserEducationWithCourseResult;
        private List<GetUserEmploymentRecommendationResult> _GetUserEmploymentRecommendationResult;
     
        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public GetUserResult GetUserResult
        {
            get { return _GetUserResult; }
            set { _GetUserResult = value; }
        }

        [DataMember]
        public List<GetUserSkillResult> GetUserSkillResult
        {
            get { return _GetUserSkillResult; }
            set { _GetUserSkillResult = value; }
        }
        
        [DataMember]
        public List<GetUserEmploymentResult> GetUserEmploymentResult
        {
            get { return _GetUserEmploymentResult; }
            set { _GetUserEmploymentResult = value; }
        }

        [DataMember]
        public List<GetUserEducationWithCourseResult> GetUserEducationWithCourseResult
        {
            get { return _GetUserEducationWithCourseResult; }
            set { _GetUserEducationWithCourseResult = value; }
        }

        [DataMember]
        public List<GetUserEmploymentRecommendationResult> GetUserEmploymentRecommendationResult
        {
            get { return _GetUserEmploymentRecommendationResult; }
            set { _GetUserEmploymentRecommendationResult = value; }
        }
    }
    #endregion

    #region PrUserEmployment
    [DataContract]
    public class PrUserEmployment
    {

        private string _EmployerName = string.Empty;
        private string _Title = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _StartYear = string.Empty;
        private string _EndYear = string.Empty;
        private string _Summary = string.Empty;
        private string _StartMonth = string.Empty;
        private string _EndMonth = string.Empty;
        //private List<PrEmploymentRecommendation> _EmploymentRecommendation;

        [DataMember]
        public string EmployerName
        {
            get { return _EmployerName; }
            set { _EmployerName = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        [DataMember]
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }

        [DataMember]
        public string StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        [DataMember]
        public string EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        //[DataMember]
        //public List<PrEmploymentRecommendation> EmploymentRecommendation
        //{
        //    get { return _EmploymentRecommendation; }
        //    set { _EmploymentRecommendation = value; }
        //}

    }

    #endregion

    #region PrEmploymentRecommendation
    [DataContract]
    public class PrEmploymentRecommendation
    {

        private string _Recommendation = string.Empty;
        private string _RecommenderName = string.Empty;      

        [DataMember]
        public string Recommendation
        {
            get { return _Recommendation; }
            set { _Recommendation = value; }
        }
        [DataMember]
        public string RecommenderName
        {
            get { return _RecommenderName; }
            set { _RecommenderName = value; }
        }
    }
    #endregion

    #region PrSkill
    [DataContract]
    public class PrSkill
    {
        private string _Skill = string.Empty;

        [DataMember]
        public string Skill
        {
            get { return _Skill; }
            set { _Skill = value; }
        }        
    }
    #endregion

    #region PrUserEducation
    [DataContract]
    public class PrUserEducation
    {
        private string _Name = string.Empty;
        private string _Degree = string.Empty;   
        private string _StartYear = string.Empty;
        private string _EndYear = string.Empty;
        private string _StartMonth = string.Empty;
        private string _EndMonth = string.Empty;
        private List<PrUserEducationCourse> _UserEducationCourse;

        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [DataMember]
        public string Degree
        {
            get { return _Degree; }
            set { _Degree = value; }
        }

        [DataMember]
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        [DataMember]
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        [DataMember]
        public string StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        [DataMember]
        public string EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        [DataMember]
        public List<PrUserEducationCourse> UserEducationCourse
        {
            get { return _UserEducationCourse; }
            set { _UserEducationCourse = value; }
        }

    }

    #endregion

    #region PrUserEducationCourse
    [DataContract]
    public class PrUserEducationCourse
    {
        private string _Course = string.Empty;

        [DataMember]
        public string Course
        {
            get { return _Course; }
            set { _Course = value; }
        }        
    }
    #endregion

    #region GetUserDetailsResult
    [DataContract]
    public class GetUserDetailsResult
    {
        private GetIsUserExistsResult _UserDetailResult;
        private string _UserFollowStatus = string.Empty;

        [DataMember]
        public GetIsUserExistsResult UserDetailResult
        {
            get { return _UserDetailResult; }
            set { _UserDetailResult = value; }
        }

        [DataMember]
        public string UserFollowStatus
        {
            get { return _UserFollowStatus; }
            set { _UserFollowStatus = value; }
        }
    }
    #endregion

    #region GetUserDetailForFeed
    [DataContract]
    public class GetUserDetailForFeed
    {
        private string _UserId = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _Email = string.Empty;
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Summary = string.Empty;
        private string _PhotoURL = string.Empty;
        private string _LinkedInId = string.Empty;
        private string _ExperienceLevel = string.Empty;

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }

        [DataMember]
        public string PhotoURL
        {
            get { return _PhotoURL; }
            set { _PhotoURL = value; }
        }

        [DataMember]
        public string LinkedInId
        {
            get { return _LinkedInId; }
            set { _LinkedInId = value; }
        }

        [DataMember]
        public string ExperienceLevel
        {
            get { return _ExperienceLevel; }
            set { _ExperienceLevel = value; }
        }
    }
    #endregion

    #region GetJobDetails
    [DataContract]
    public class GetJobDetails
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserId = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Title = string.Empty;
        private string _Responsibilities = string.Empty;
        private string _Qualifications = string.Empty;
        private string _Company = string.Empty;
        private string _EmployerIntroduction = string.Empty;
        private string _URL = string.Empty;
        private string _ShareURL = string.Empty;
        private string _State = string.Empty;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string Responsibilities
        {
            get { return _Responsibilities; }
            set { _Responsibilities = value; }
        }

        [DataMember]
        public string Qualifications
        {
            get { return _Qualifications; }
            set { _Qualifications = value; }
        }

        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }

        [DataMember]
        public string EmployerIntroduction
        {
            get { return _EmployerIntroduction; }
            set { _EmployerIntroduction = value; }
        }

        [DataMember]
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        [DataMember]
        public string ShareURL
        {
            get { return _ShareURL; }
            set { _ShareURL = value; }
        }

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }        
    }
    #endregion

    #region GetActivityFeeds
    [DataContract]
    public class GetActivityFeeds
    {        
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _UserId = string.Empty;
        private string _Type = string.Empty;
        private string _JobID = string.Empty;
        private string _OtherUserID = string.Empty;
        private GetUserDetailForFeed _OtherUserDetails;
        private GetJobDetails _JobDetail;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [DataMember]
        public string JobID
        {
            get { return _JobID; }
            set { _JobID = value; }
        }

        [DataMember]
        public string OtherUserID
        {
            get { return _OtherUserID; }
            set { _OtherUserID = value; }
        }      

        [DataMember]
        public GetUserDetailForFeed OtherUserDetails
        {
            get { return _OtherUserDetails; }
            set { _OtherUserDetails = value; }
        }

        [DataMember]
        public GetJobDetails JobDetail
        {
            get { return _JobDetail; }
            set { _JobDetail = value; }
        }

    }
    #endregion

    #region GetActivityFeedsResult
    [DataContract]
    public class GetActivityFeedsResult
    {        
        private ResultStatus _ResultStatus;
        private List<GetActivityFeeds> _GetActivityFeeds;
        
        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetActivityFeeds> GetActivityFeeds
        {
            get { return _GetActivityFeeds; }
            set { _GetActivityFeeds = value; }
        }
       
    }
    #endregion

    #region GetUserDetailForCrowd
    [DataContract]
    public class GetUserDetailForCrowd
    {
        private string _UserId = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _Email = string.Empty;
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Summary = string.Empty;
        private string _PhotoURL = string.Empty;
        private string _LinkedInId = string.Empty;
        private string _ExperienceLevel = string.Empty;
        private bool _IsAvailableForCall = false;
        private List<GetUserEmployment> _UserCurrentEmployer;

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }

        [DataMember]
        public string PhotoURL
        {
            get { return _PhotoURL; }
            set { _PhotoURL = value; }
        }

        [DataMember]
        public string LinkedInId
        {
            get { return _LinkedInId; }
            set { _LinkedInId = value; }
        }

        [DataMember]
        public string ExperienceLevel
        {
            get { return _ExperienceLevel; }
            set { _ExperienceLevel = value; }
        }

        [DataMember]
        public bool IsAvailableForCall
        {
            get { return _IsAvailableForCall; }
            set { _IsAvailableForCall = value; }
        }

        [DataMember]
        public List<GetUserEmployment> UserCurrentEmployer
        {
            get { return _UserCurrentEmployer; }
            set { _UserCurrentEmployer = value; }
        }
    }
    #endregion

    #region GetUserEmployment
    [DataContract]
    public class GetUserEmployment
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserId = string.Empty;
        private string _EmployerName = string.Empty;
        private string _Title = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _StartMonth = string.Empty;
        private string _StartYear = string.Empty;
        private string _EndMonth = string.Empty; 
        private string _EndYear = string.Empty;
        private string _Summary = string.Empty;      

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string EmployerName
        {
            get { return _EmployerName; }
            set { _EmployerName = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string StartMonth
        {
            get { return _StartMonth; }
            set { _StartMonth = value; }
        }

        [DataMember]
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        [DataMember]
        public string EndMonth
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        [DataMember]
        public string EndYear
        {
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        [DataMember]
        public string Summary
        {
            get { return _Summary; }
            set { _Summary = value; }
        }
    }
    #endregion

    #region GetMyCrowdResult
    [DataContract]
    public class GetMyCrowdResult
    {
        private ResultStatus _ResultStatus;
        private List<GetUserDetailForCrowd> _IAmFollowingUser;
        private List<GetUserDetailForCrowd> _FollowingMeUser;
      
        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetUserDetailForCrowd> IAmFollowingUser
        {
            get { return _IAmFollowingUser; }
            set { _IAmFollowingUser = value; }
        }

        [DataMember]
        public List<GetUserDetailForCrowd> FollowingMeUser
        {
            get { return _FollowingMeUser; }
            set { _FollowingMeUser = value; }
        }
    }
    #endregion

    #region GetFollowUnfollowUserResult
    [DataContract]
    public class GetFollowUnfollowUserResult
    {
        private ResultStatus _ResultStatus;
      
        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion
    
    #region GetJobSkillDetail
    [DataContract]
    public class GetJobSkillDetail
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _JobID = string.Empty;
        private string _Skill = string.Empty;      

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string JobID
        {
            get { return _JobID; }
            set { _JobID = value; }
        }

        [DataMember]
        public string Skill
        {
            get { return _Skill; }
            set { _Skill = value; }
        }
    }
    #endregion

    #region GetJobDetailsWithSkill
    [DataContract]
    public class GetJobDetailsWithSkill
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserId = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _ExperienceLevel = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Title = string.Empty;
        private string _Responsibilities = string.Empty;
        private string _Qualifications = string.Empty;
        private string _Company = string.Empty;
        private string _EmployerIntroduction = string.Empty;
        private string _URL = string.Empty;
        private string _ShareURL = string.Empty;
        private string _State = string.Empty;
        private List<GetJobSkillDetail> _JobSkills;

        private string _JobCreatorFirstName = string.Empty;
        private string _JobCreatorLastName = string.Empty;
        private string _JobCreatorPhotoURL = string.Empty;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string ExperienceLevel
        {
            get { return _ExperienceLevel; }
            set { _ExperienceLevel = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string Responsibilities
        {
            get { return _Responsibilities; }
            set { _Responsibilities = value; }
        }

        [DataMember]
        public string Qualifications
        {
            get { return _Qualifications; }
            set { _Qualifications = value; }
        }

        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }

        [DataMember]
        public string EmployerIntroduction
        {
            get { return _EmployerIntroduction; }
            set { _EmployerIntroduction = value; }
        }

        [DataMember]
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        [DataMember]
        public string ShareURL
        {
            get { return _ShareURL; }
            set { _ShareURL = value; }
        }

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }   
     
        [DataMember]
        public List<GetJobSkillDetail> JobSkills
        {
            get { return _JobSkills; }
            set { _JobSkills = value; }
        }

        [DataMember]
        public string JobCreatorFirstName
        {
            get { return _JobCreatorFirstName; }
            set { _JobCreatorFirstName = value; }
        }

        [DataMember]
        public string JobCreatorLastName
        {
            get { return _JobCreatorLastName; }
            set { _JobCreatorLastName = value; }
        }

        [DataMember]
        public string JobCreatorPhotoURL
        {
            get { return _JobCreatorPhotoURL; }
            set { _JobCreatorPhotoURL = value; }
        }
    }
    #endregion

    #region GetAddEditJobResult
    [DataContract]
    public class GetAddEditJobResult
    {
        private ResultStatus _ResultStatus;
        private GetJobDetailsWithSkill _JobDetailsWithSkills;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public GetJobDetailsWithSkill JobDetailsWithSkills
        {
            get { return _JobDetailsWithSkills; }
            set { _JobDetailsWithSkills = value; }
        }
    }
    #endregion

    #region GetJobDetailsResult
    [DataContract]
    public class GetJobDetailsResult
    {
        private ResultStatus _ResultStatus;
        private GetJobDetailsWithSkill _JobDetailsWithSkills;
        private string _IsJobFavorite = string.Empty;
        private string _IsJobApplied = string.Empty;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public GetJobDetailsWithSkill JobDetailsWithSkills
        {
            get { return _JobDetailsWithSkills; }
            set { _JobDetailsWithSkills = value; }
        }

        [DataMember]
        public string IsJobFavorite
        {
            get { return _IsJobFavorite; }
            set { _IsJobFavorite = value; }
        }

        [DataMember]
        public string IsJobApplied
        {
            get { return _IsJobApplied; }
            set { _IsJobApplied = value; }
        }
    }
    #endregion

    #region GetFavoriteJobResult
    [DataContract]
    public class GetFavoriteJobResult
    {
        private ResultStatus _ResultStatus;
      
        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion

    #region GetApplyToJobResult
    [DataContract]
    public class GetApplyToJobResult
    {
        private ResultStatus _ResultStatus;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion

    #region GetFillReopenJobResult
    [DataContract]
    public class GetFillReopenJobResult
    {
        private ResultStatus _ResultStatus;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion

    #region GetDeleteJobResult
    [DataContract]
    public class GetDeleteJobResult
    {
        private ResultStatus _ResultStatus;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion

    #region GetCompleteJobDetails
    [DataContract]
    public class GetCompleteJobDetails
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _DateModified = string.Empty;
        private string _UserId = string.Empty;
        private string _LocationCity = string.Empty;
        private string _LocationState = string.Empty;
        private string _LocationCountry = string.Empty;
        private string _ExperienceLevel = string.Empty;
        private string _Industry = string.Empty;
        private string _Industry2 = string.Empty;
        private string _Title = string.Empty;
        private string _Responsibilities = string.Empty;
        private string _Qualifications = string.Empty;
        private string _Company = string.Empty;
        private string _EmployerIntroduction = string.Empty;
        private string _URL = string.Empty;
        private string _ShareURL = string.Empty;
        private string _State = string.Empty;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        [DataMember]
        public string LocationCity
        {
            get { return _LocationCity; }
            set { _LocationCity = value; }
        }

        [DataMember]
        public string LocationState
        {
            get { return _LocationState; }
            set { _LocationState = value; }
        }

        [DataMember]
        public string LocationCountry
        {
            get { return _LocationCountry; }
            set { _LocationCountry = value; }
        }

        [DataMember]
        public string ExperienceLevel
        {
            get { return _ExperienceLevel; }
            set { _ExperienceLevel = value; }
        }

        [DataMember]
        public string Industry
        {
            get { return _Industry; }
            set { _Industry = value; }
        }

        [DataMember]
        public string Industry2
        {
            get { return _Industry2; }
            set { _Industry2 = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        [DataMember]
        public string Responsibilities
        {
            get { return _Responsibilities; }
            set { _Responsibilities = value; }
        }

        [DataMember]
        public string Qualifications
        {
            get { return _Qualifications; }
            set { _Qualifications = value; }
        }

        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }

        [DataMember]
        public string EmployerIntroduction
        {
            get { return _EmployerIntroduction; }
            set { _EmployerIntroduction = value; }
        }

        [DataMember]
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        [DataMember]
        public string ShareURL
        {
            get { return _ShareURL; }
            set { _ShareURL = value; }
        }

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
    }
    #endregion

    #region GetSearchJobResult
    [DataContract]
    public class GetSearchJobResult
    {
        private ResultStatus _ResultStatus;
        private List<GetCompleteJobDetails> _JobDetails;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }


        [DataMember]
        public List<GetCompleteJobDetails> JobDetails
        {
            get { return _JobDetails; }
            set { _JobDetails = value; }
        }
    }
    #endregion

    #region GetSearchCandidatesResult
    [DataContract]
    public class GetSearchCandidatesResult
    {
        private ResultStatus _ResultStatus;
        private List<GetUserDetailForCrowd> _UserDetail;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }


        [DataMember]
        public List<GetUserDetailForCrowd> UserDetail
        {
            get { return _UserDetail; }
            set { _UserDetail = value; }
        }
    }
    #endregion

    #region GetUserJobsResult
    [DataContract]
    public class GetUserJobsResult
    {
        private ResultStatus _ResultStatus;
        private List<GetCompleteJobDetails> _PostedByMe;
        private List<GetCompleteJobDetails> _JobApplied;
        private List<GetCompleteJobDetails> _JobFavorited;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetCompleteJobDetails> PostedByMe
        {
            get { return _PostedByMe; }
            set { _PostedByMe = value; }
        }

        [DataMember]
        public List<GetCompleteJobDetails> JobApplied
        {
            get { return _JobApplied; }
            set { _JobApplied = value; }
        }

        [DataMember]
        public List<GetCompleteJobDetails> JobFavorited
        {
            get { return _JobFavorited; }
            set { _JobFavorited = value; }
        }

    }
    #endregion

    #region GetMessageListResult
    [DataContract]
    public class GetMessageListResult
    {
        private ResultStatus _ResultStatus;
        private List<GetMessageDetailWithSender> _MesssageList;      

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetMessageDetailWithSender> MesssageList
        {
            get { return _MesssageList; }
            set { _MesssageList = value; }
        }       
    }
    #endregion

    #region GetMessageDetailWithSender
    [DataContract]
    public class GetMessageDetailWithSender
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _SenderID = string.Empty;
        private GetUserDetailForFeed _SenderDetail;
        private string _ReceiverID = string.Empty;
        private string _State = string.Empty;
        private string _Message = string.Empty;
        private string _LincURL = string.Empty;
        private string _LincUserID = string.Empty;
        private string _LincJobID = string.Empty;
        private string _Type = string.Empty;

        private string _IsUnreadMessages = string.Empty;

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string SenderID
        {
            get { return _SenderID; }
            set { _SenderID = value; }
        }

        [DataMember]
        public GetUserDetailForFeed SenderDetail
        {
            get { return _SenderDetail; }
            set { _SenderDetail = value; }
        }

        [DataMember]
        public string ReceiverID
        {
            get { return _ReceiverID; }
            set { _ReceiverID = value; }
        }

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        [DataMember]
        public string LincURL
        {
            get { return _LincURL; }
            set { _LincURL = value; }
        }

        [DataMember]
        public string LincUserID
        {
            get { return _LincUserID; }
            set { _LincUserID = value; }
        }

        [DataMember]
        public string LincJobID
        {
            get { return _LincJobID; }
            set { _LincJobID = value; }
        }

        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [DataMember]
        public string IsUnreadMessages
        {
            get { return _IsUnreadMessages; }
            set { _IsUnreadMessages = value; }
        }
    }
    #endregion

    #region GetMessageThreadResult
    [DataContract]
    public class GetMessageThreadResult
    {
        private ResultStatus _ResultStatus;
        private List<GetMessageDetail> _MesssageList;
        private string _IsUnreadMessages = string.Empty;

        private string _OtherUserFirstName = string.Empty;
        private string _OtherUserLastName = string.Empty;
        private string _OtherUserPhotoURL = string.Empty;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetMessageDetail> MesssageList
        {
            get { return _MesssageList; }
            set { _MesssageList = value; }
        }

        [DataMember]
        public string IsUnreadMessages
        {
            get { return _IsUnreadMessages; }
            set { _IsUnreadMessages = value; }
        }

        [DataMember]
        public string OtherUserFirstName
        {
            get { return _OtherUserFirstName; }
            set { _OtherUserFirstName = value; }
        }

        [DataMember]
        public string OtherUserLastName
        {
            get { return _OtherUserLastName; }
            set { _OtherUserLastName = value; }
        }

        [DataMember]
        public string OtherUserPhotoURL
        {
            get { return _OtherUserPhotoURL; }
            set { _OtherUserPhotoURL = value; }
        }
    }
    #endregion

    #region GetMessageDetail
    [DataContract]
    public class GetMessageDetail
    {
        private string _ID = string.Empty;
        private string _DateCreated = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _State = string.Empty;
        private string _Message = string.Empty;
        private string _LincURL = string.Empty;
        private string _LincUserID = string.Empty;
        private string _LincJobID = string.Empty;
        private string _Type = string.Empty;
        private string _LinkJobCreatorID = string.Empty;

        [DataMember]
        public string LinkJobCreatorID
        {
            get { return _LinkJobCreatorID; }
            set { _LinkJobCreatorID = value; }
        }

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        [DataMember]
        public string SenderID
        {
            get { return _SenderID; }
            set { _SenderID = value; }
        }

        [DataMember]
        public string ReceiverID
        {
            get { return _ReceiverID; }
            set { _ReceiverID = value; }
        }

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        [DataMember]
        public string LincURL
        {
            get { return _LincURL; }
            set { _LincURL = value; }
        }

        [DataMember]
        public string LincUserID
        {
            get { return _LincUserID; }
            set { _LincUserID = value; }
        }

        [DataMember]
        public string LincJobID
        {
            get { return _LincJobID; }
            set { _LincJobID = value; }
        }

        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
             
    }
    #endregion

    #region GetSendMessageResult
    [DataContract]
    public class GetSendMessageResult
    {
        private string _MessageID = string.Empty;
        private ResultStatus _ResultStatus;
       
        [DataMember]
        public string MessageID
        {
            get { return _MessageID; }
            set { _MessageID = value; }
        }

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

    }
    #endregion

    #region GetPastMessagesResult
    [DataContract]
    public class GetPastMessagesResult
    {
        private ResultStatus _ResultStatus;
        private List<GetMessageDetail> _MesssageList;
        private string _IsUnreadMessages = string.Empty;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public List<GetMessageDetail> MesssageList
        {
            get { return _MesssageList; }
            set { _MesssageList = value; }
        }

        [DataMember]
        public string IsUnreadMessages
        {
            get { return _IsUnreadMessages; }
            set { _IsUnreadMessages = value; }
        }
    }
    #endregion

    #region GetAcceptDeclineJobApplicationResult
    [DataContract]
    public class GetAcceptDeclineJobApplicationResult
    {
        private ResultStatus _ResultStatus;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
    }
    #endregion

    #region GetUnreadMessageCountResult
    [DataContract]
    public class GetUnreadMessageCountResult
    {
        private ResultStatus _ResultStatus;
        private string _NumberOfUnreadMessage = string.Empty;

        [DataMember]
        public ResultStatus ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        [DataMember]
        public string NumberOfUnreadMessage
        {
            get { return _NumberOfUnreadMessage; }
            set { _NumberOfUnreadMessage = value; }
        }
    }
    #endregion
}
