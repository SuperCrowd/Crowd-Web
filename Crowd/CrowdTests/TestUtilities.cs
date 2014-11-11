using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace CrowdTests
{
    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public static class TestUtilities
    {
        //public static string GetLoginTokenForUser(string email, CrowdEntities context)
        //{
        //    CrowdWCFservice.Token.tbl_TokenInfo tokeninfo = new CrowdWCFservice.Token.tbl_TokenInfo();
        //    tokeninfo.EmailID = email.Trim();
        //    Token objToken = new Token();
        //    string token = objToken.SetUserToken(tokeninfo, "30");
        //}

        public static T GetRandomIndustry<T>(List<T>list)
        {
            Random r = new Random();
            int index = r.Next(list.Count);
            return list[index];
        }

        public static Follow FollowUser(User follower, User userToFollow, CrowdEntities context)
        {
            Follow retVal = null;
            var isUserAlreadyFollowing = context.Follows.Where(f => f.FollowerUserID == follower.ID && f.FollowingUserID == userToFollow.ID).FirstOrDefault();

            if (isUserAlreadyFollowing == null)
            {
                Follow newFollow = new Follow(follower, userToFollow);
                context.Follows.Add(newFollow);

                Feed objNewFeed = new Feed();
                objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                objNewFeed.UserID = userToFollow.ID;
                objNewFeed.FeedTypeID = 1;
                objNewFeed.JobID = null;
                objNewFeed.OtherUserID = follower.ID;
                context.Feeds.Add(objNewFeed);

                retVal = newFollow;
            }
            else
            {
                retVal = isUserAlreadyFollowing;
            }
            return retVal;

        }

        public static Message CreateMessage(
            User sender,
            User receiver,
            bool state,
            string messageString,
            int messageType,
            CrowdEntities context)
        {
            Message message = new Message();
            message.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            message.Message1 = messageString;
            message.MessageTypeID = messageType;
            message.ReceiverID = receiver.ID;
            message.SenderID = sender.ID;
            message.State = state;
            context.Messages.Add(message);

            return message;
        }
    
        public static Job CreateJob(
            User user,
            string company,
            string city,
            string state,
            string country,
            string industry,
            string title,
            string responsibilities,
            string qualifications,
            string employerIntroduction,
            string url,
            bool jobState,
            ExperienceLevelType experienceLevel,
            CrowdEntities context)
        {

            Job job = new Job();
            job.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            job.DateModified = job.DateCreated;
            job.User = user;
            job.Company = company;
            job.LocationCity = city;
            job.LocationState = state;
            job.LocationCountry = country;
            job.Industry = industry;
            job.Title = title;
            job.Responsibilities = responsibilities;
            job.Qualifications = qualifications;
            job.EmployerIntroduction = employerIntroduction;
            job.URL = url;
            job.State = jobState;
            job.Industry2 = industry;
            job.ExperienceLevelType1 = experienceLevel;
            context.Jobs.Add(job);

            return job;


        }
       
        public static User CreateUserIfDoesntExist(
            string email, 
            string firstName, 
            string lastName, 
            string city,
            string state,
            string country,
            string industry,
            string summary,
            int experienceLevel,
            string linkedinid,
            CrowdEntities context)
        {
            User retVal = null;

            var doesUserExist = context.Users.Where(u => u.Email == email).Count() > 0;
            if (!doesUserExist)
            {
                //we need to create the yuser
                retVal = new User();
                context.Users.Add(retVal);
            }
            else
            {
                retVal = context.Users.Where(u => u.Email == email).First();

            }

            retVal.Email = email;
            retVal.FirstName = firstName;
            retVal.LastName = lastName;
            retVal.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            retVal.DateModified = retVal.DateCreated;
            retVal.ExperienceLevelType = experienceLevel;
            retVal.Industry = industry;
            retVal.LocationCity = city;
            retVal.LocationCountry = country;
            retVal.LocationState = state;
            retVal.Summary = summary;
            retVal.LinkedInId = linkedinid;

            return retVal;
        }

        public static ExperienceLevelType GetRandomJobExperienceLevel(CrowdEntities context)
        {
            var experienceLevels = context.ExperienceLevelTypes.ToList();
            Random r = new Random();

            int index = r.Next(experienceLevels.Count());
            return experienceLevels[index];


        }

        public static UserJobApplication CreateJobApplication (User user, Job job, CrowdEntities context)
        {
            UserJobApplication retVal = new UserJobApplication();
            retVal.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            retVal.Job = job;
            retVal.User = user;
            context.UserJobApplications.Add(retVal);

            //-----------------add record in Feed Table--------------------------------//
            Feed objNewFeed = new Feed();
            objNewFeed.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            long lngJobUserID = job.User.ID; //Get Job UserId
            objNewFeed.UserID = lngJobUserID;
            objNewFeed.FeedTypeID = 3;
            objNewFeed.JobID = job.ID;
            objNewFeed.OtherUserID = user.ID;
            context.Feeds.Add(objNewFeed);
           
            //-------------------------------------------------------------------------//

            //--------------Add Record in Message table -------------------------------//
            Message objNewMessage = new Message();
            objNewMessage.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            objNewMessage.SenderID = user.ID;
            objNewMessage.ReceiverID = lngJobUserID;
            objNewMessage.Message1 = "Job Application";
            objNewMessage.MessageTypeID = 2;
            objNewMessage.LinkURL = null;
            objNewMessage.LinkUserID = null;
            objNewMessage.LinkJobID = job.ID;
            objNewMessage.State = false;
            context.Messages.Add(objNewMessage);
            return retVal;
        }

        public static UserJobFavorite CreateFavorite (User user, Job job, CrowdEntities context)
        {
            UserJobFavorite retVal = new UserJobFavorite();
            retVal.DateCreated = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            retVal.Job = job;
            retVal.User = user;
            context.UserJobFavorites.Add(retVal);
            return retVal;
        }

        public static Job GetRandomJob(CrowdEntities context)
        {
            var jobs = context.Jobs.ToList();
            Random r = new Random();

            int index = r.Next(jobs.Count());
            return jobs[index];

        }
        public static List<string> GetNameList()
        {
            string filePath = ConfigurationManager.AppSettings["NameFileName"];
            List<string> retVal = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string industry = reader.ReadLine();
                    if (!string.IsNullOrEmpty(industry) &&
                        industry != " " && industry != "")
                    {
                        retVal.Add(industry);
                    }
                }
            }

            return retVal;
        }

        public static List<string> GetJobTitles()
        {
            string filePath = ConfigurationManager.AppSettings["JobTitleFileName"];
            List<string> retVal = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string industry = reader.ReadLine();
                    retVal.Add(industry.Trim());
                }
            }

            return retVal;
        }

        public static List<string> GetIndustryList()
        {
            string filePath = ConfigurationManager.AppSettings["IndustryFileName"];
            List<string> retVal = new List<string>();

            using (StreamReader reader =new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string industry = reader.ReadLine();
                    retVal.Add(industry);
                }
            }

            return retVal;
        }

        public static List<string> GetCompanies()
        {
            string filePath = ConfigurationManager.AppSettings["CompanyFileName"];
            List<string> retVal = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string industry = reader.ReadLine();
                    retVal.Add(industry.Trim());
                }
            }

            return retVal;

        }
        public static List<Location> GetLocations()
        {
            string filePath = ConfigurationManager.AppSettings["CityFileName"];
           
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Location> retVal = new List<Location>();

            string contentsOfFile;

            using (StreamReader reader = new StreamReader(filePath))
            {
                contentsOfFile = reader.ReadToEnd();
            }

            var locationsList = ser.Deserialize<dynamic>(contentsOfFile);

            foreach (var location in locationsList)
            {
                Location l = new Location();
                l.City = location["name"];
                l.Country = "USA";
                l.State = location["state_name"];
                retVal.Add(l);
            }
            return retVal;


        }
       
    }
}
