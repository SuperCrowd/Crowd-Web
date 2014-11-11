using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace CrowdTests
{

   
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void TestInitialize()
        {
            using (CrowdEntities context = new CrowdEntities())
            {
                //context.Messages.RemoveRange(context.Messages);
                //context.Feeds.RemoveRange(context.Feeds);
                //context.UserJobApplications.RemoveRange(context.UserJobApplications);
                //context.UserJobFavorites.RemoveRange(context.UserJobFavorites);
       
                //context.JobSkills.RemoveRange(context.JobSkills);
                //context.Jobs.RemoveRange(context.Jobs);
                //context.Follows.RemoveRange(context.Follows);

                //context.UserSkills.RemoveRange(context.UserSkills);
                //context.UserEmploymentRecommendations.RemoveRange(context.UserEmploymentRecommendations);
                //context.UserEducationCourses.RemoveRange(context.UserEducationCourses);
                //context.UserEducations.RemoveRange(context.UserEducations);
                //context.UserEmployments.RemoveRange(context.UserEmployments);

                //context.Users.RemoveRange(context.Users);
                //context.SaveChanges();
               
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            string activityName = "TestMethod1:";
            var companies = TestUtilities.GetCompanies();
            var locations = TestUtilities.GetLocations();
            var industries = TestUtilities.GetIndustryList();
            var names = TestUtilities.GetNameList();
            var jobTitles = TestUtilities.GetJobTitles();

            int numberOfUsersToCreate =int.Parse(ConfigurationManager.AppSettings["NumberOfUsersToCreate"]);
            int numberOfUsersCreated = 0;

            Random r = new Random();

            var usersCreated = new List<User>();
            var namesCreated = new List<string>();
            using (CrowdEntities context = new CrowdEntities())
            {
                while (numberOfUsersCreated < numberOfUsersToCreate)
                {
                    //need a industry
                    string industry = TestUtilities.GetRandomIndustry(industries).ToString();
                    Location location = (Location)TestUtilities.GetRandomIndustry(locations);
                    string name = null;
                    
                    while (name == null)
                    {
                        string potentialName = TestUtilities.GetRandomIndustry(names).ToString();

                        if (!namesCreated.Contains(potentialName))
                        {
                            name = potentialName;
                            
                        }
                    }
                    namesCreated.Add(name);
                    string firstName = name.Split(' ')[0];
                    string lastName = name.Split(' ')[1];
                    string username = name.Replace(" ","").Trim();
                    string email = string.Format("{0}@crowd.com",username);
                    string linkedinid = email;
                    int experienceLevel = 1;
                    string summary = string.Format("Sample summary for user {0}",username);
                    
                    User user = TestUtilities.CreateUserIfDoesntExist(
                        email,
                        firstName,
                        lastName,
                        location.City,
                        location.State,
                        location.Country,
                        industry,
                        summary,
                        experienceLevel,
                        linkedinid,
                        context
                        );

                    numberOfUsersCreated++;
                    Trace.TraceInformation("{0}Created {1} of {2} users, {3}", activityName, numberOfUsersCreated, numberOfUsersToCreate, name);

                    context.Users.Add(user);
                    usersCreated.Add(user);
                }

                context.SaveChanges();

                Trace.TraceInformation("{0}User creation completed", activityName);
                Trace.TraceInformation("{0}Beginning follower creation...", activityName);

                int maxNumberOfFollowersForAUser = int.Parse(ConfigurationManager.AppSettings["MaxNumbersOfFollowersForUser"]);
                List<User> usersInSystem = context.Users.ToList();

                for (int i = 0; i < usersCreated.Count();i++)
                {
                    User user = usersCreated[i];
                    int numberOfFollowers = r.Next(maxNumberOfFollowersForAUser);
                    int countFollowers = 0;

                    while (countFollowers < numberOfFollowers)
                    {
                        int index = 0; ;
                        User follower = null;

                        while (follower == null)
                        {
                            index = r.Next(usersInSystem.Count);
                            User potentialFollower = usersInSystem[index];

                            if (potentialFollower.ID != user.ID)
                            {
                                follower = potentialFollower;
                            }
                        }


                        //we make this user a follower of the user we are processing
                        TestUtilities.FollowUser(follower, user, context);
                        countFollowers++;
                    }

                    Trace.TraceInformation("{0}Created {1} followers for user {2}", activityName, numberOfFollowers, user.Email);

                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
                Trace.TraceInformation("{0}Completed creation of followers", activityName);

                int maxNumberOfJobPostingsForUser =int.Parse(ConfigurationManager.AppSettings["MaxNumbersOfJobPostingsForUser"]);

                for (int i = 0; i < usersCreated.Count(); i++)
                {
                    User user = usersCreated[i];
                    int numberOfJobsToCreate = r.Next(maxNumberOfJobPostingsForUser);
                    int countJobs = 0;

                    while (countJobs < numberOfJobsToCreate)
                    {
                        string industry = TestUtilities.GetRandomIndustry(industries).ToString();
                        Location location = (Location)TestUtilities.GetRandomIndustry(locations);
                        string company = TestUtilities.GetRandomIndustry(companies).ToString();
                        string jobTitle = TestUtilities.GetRandomIndustry(jobTitles).ToString();
                        string responsibilities = string.Format("{0} responsibilities are numerous", jobTitle);
                        string qualifications = string.Format("Qualifications for the role of {0}",jobTitle);
                        string employerIntroduction = string.Format("Introduction to {0}",company);
                        string url = "http://www.bluelabellabs.com";
                        bool jobState = false;
                        ExperienceLevelType experienceLevel = TestUtilities.GetRandomJobExperienceLevel(context);

                        Job newJobPosting = TestUtilities.CreateJob(user,company,location.City,location.State,location.Country,industry,jobTitle, responsibilities, qualifications, employerIntroduction,url,jobState, experienceLevel, context);


                        countJobs++;

                        Trace.TraceInformation("{0}Created job {1} of {2} for user {3} (#{4} of {5}), job title: {6}, company:{7}",
                            activityName,countJobs,numberOfJobsToCreate,user.Email,i,usersCreated.Count(),jobTitle,company);

                    }

                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e2)
                {
                    Trace.TraceError(e2.ToString());
                }
                
                Trace.TraceInformation("{0}Completed creation of job postings",activityName);
                Trace.TraceInformation("{0}Begin creation of messages",activityName);

                int maxNumberOfThreadsPerUser = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfThreadsForUser"]);
                int maxNumberOfMessagesPerThread = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfMessagesInThread"]);


                for (int i = 0; i < usersCreated.Count(); i++)
                {
                    User user = usersCreated[i];
                    int numberOfThreadsToCreateForUser = r.Next(maxNumberOfThreadsPerUser);
                    int countThreads = 0;

                    List<long> userIdsWithThreads = new List<long>();

                    while (countThreads < numberOfThreadsToCreateForUser)
                    {
                        int index = 0; ;
                        User otherUser = null;

                        while (otherUser == null)
                        {
                            index = r.Next(usersInSystem.Count);
                            User potentialUser = usersInSystem[index];

                            if (potentialUser.ID != user.ID &&
                                !userIdsWithThreads.Contains(potentialUser.ID))
                            {
                                otherUser = potentialUser;
                            }
                        }

                        //now we have found another user to create a thread with
                        int numberOfMessagesToCreate = r.Next(maxNumberOfMessagesPerThread);
                        int countNumberOfMessages = 0;

                        while (countNumberOfMessages < numberOfMessagesToCreate)
                        {
                            User receiver = null;
                            User sender = null;
                            //if this is an event number message, than we make the sender the original user, otherwise we reverse it
                            if (countNumberOfMessages % 2 == 0)
                            {
                                sender = user;
                                receiver = otherUser;
                            }
                            else
                            {
                                sender = otherUser;
                                receiver = user;
                            }
                            string message = string.Format("Hello {0}",receiver.FirstName);
                            Message messageObject = TestUtilities.CreateMessage(sender,receiver,false,message,1,context);
                             countNumberOfMessages++;
                            Trace.TraceInformation("{0}Created message {1} of {2} for {3}, message sent from {4} to {5}",
                                activityName,countNumberOfMessages,numberOfMessagesToCreate,user.Email,sender.Email,receiver.Email);
                           
                        }

                        countThreads++;
                        userIdsWithThreads.Add(otherUser.ID);
                        Trace.TraceInformation("{0}Created thread {1} of {2} for {3}",activityName,countThreads,numberOfThreadsToCreateForUser, user.Email);
                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
               
                Trace.TraceInformation("{0}Completed creationg of messages",activityName);


                int maxNumberOfJobsFavoritedForUser = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfFavoriteJobsForUser"]);
                var jobsInSystem = context.Jobs.ToList();

                for (int i = 0; i < usersCreated.Count(); i++)
                {
                    User user = usersCreated[i];
                    int numberOfJobsToFavorite = r.Next(maxNumberOfJobsFavoritedForUser);
                    int countNumberOfJobsFavorited = 0;

                    var jobsFavoritedForUser = new List<long>();

                    while (countNumberOfJobsFavorited < numberOfJobsToFavorite)
                    {
                        Job jobToFavorite = null;

                        while (jobToFavorite == null)
                        {
                            Job potentialJob = TestUtilities.GetRandomIndustry(jobsInSystem);
                            if (!jobsFavoritedForUser.Contains(potentialJob.ID) &&
                                potentialJob.User.ID != user.ID)
                            {
                                jobToFavorite = potentialJob;
                            }
                        }

                        UserJobFavorite favorite = TestUtilities.CreateFavorite(user, jobToFavorite, context);
                        jobsFavoritedForUser.Add(jobToFavorite.ID);
                        countNumberOfJobsFavorited++;

                        Trace.TraceInformation("{0}Created favorite {1} of {2} for user {3}", activityName, countNumberOfJobsFavorited, numberOfJobsToFavorite, user.Email);


                    }
                }
                context.SaveChanges();
                Trace.TraceInformation("{0}Completed favoriting jobs", activityName);

                int maxNumberOfJobsToApplyForAUser = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfAppliesJobsForUser"]);
                for (int i = 0; i < usersCreated.Count(); i++)
                {
                    User user = usersCreated[i];
                    int numberOfJobsToApplyTo = r.Next(maxNumberOfJobsToApplyForAUser);
                    int countNumbersOfJobsAppliedTo = 0;

                    var jobsAppliedForUser = new List<long>();

                    while (countNumbersOfJobsAppliedTo < numberOfJobsToApplyTo)
                    {
                        Job jobToApplyTo = null;

                        while (jobToApplyTo == null)
                        {
                            Job potentialJob = TestUtilities.GetRandomIndustry(jobsInSystem);
                            if (!jobsAppliedForUser.Contains(potentialJob.ID) &&
                                potentialJob.User.ID != user.ID)
                            {
                                jobToApplyTo = potentialJob;
                            }
                        }

                        UserJobApplication jobApplication = TestUtilities.CreateJobApplication(user, jobToApplyTo, context);
                        jobsAppliedForUser.Add(jobApplication.ID);
                        countNumbersOfJobsAppliedTo++;
                        Trace.TraceInformation("{0}Created application {1} of {2} for user {3}", activityName, countNumbersOfJobsAppliedTo, numberOfJobsToApplyTo, user.Email);


                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    Trace.TraceError(ex.ToString());
                }

            }


        }
    }
}
