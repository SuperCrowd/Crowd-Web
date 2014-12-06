using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace CrowdWeb.Models.ViewModels
{
    public class CandidateViewModel
    {
        private User User { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Industry { get; set; }
        public string Industry2 { get; set; }
        public string PhotoURL { get; set; }
        public string Summary { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string School { get; set; }
        public string Skills { get; set; }
        public List<dynamic> Experience { get; set; }
        public List<dynamic> Recommendations { get; set; }
        public List<dynamic> Education { get; set; }

        public CandidateViewModel() { }
        public CandidateViewModel(User user, CrowdEntities context )
        {
            this.User = user;
            this.ID = user.ID;
            this.Name = user.FirstName + " " + user.LastName;

            this.City = user.LocationCity;
            this.State = user.LocationState;
            this.Country = user.LocationCountry;
            this.Industry = user.Industry;
            this.Industry2 = user.Industry2;
            this.PhotoURL = ConfigurationManager.AppSettings["PhotoBaseURL"] + user.PhotoURL;
            this.Summary = user.Summary;

            var currentJob = user.UserEmployments.Where(u => u.EndYear == null).FirstOrDefault();
            
            if (currentJob != null)
            {
                this.Company = currentJob.EmployerName;
                this.Title = currentJob.Title;

            }

            var lastSchool = user.UserEducations.Where(u => u.EndYear == null).FirstOrDefault();
            //this grabs their current school if they have one
            if (lastSchool == null)
            {
                //no current school, so get the most recent
                lastSchool = user.UserEducations.OrderByDescending(s => s.EndYear).FirstOrDefault();
                
            }
            this.School = lastSchool.Name;

            var schoolsOrderedByDate = user.UserEducations.OrderByDescending(s => s.EndYear).ToList();
            this.Education = new List<dynamic>();
            object currentEducation = null;

            //populate education history
            foreach (UserEducation school in schoolsOrderedByDate)
            {
                var dateString = "";
                if (school.EndYear == null)
                {
                    dateString = school.StartYear + " to Present";
                   
                }
                else
                {
                    dateString = school.StartYear + " to " + school.EndYear;
                }
                dynamic schoolVM = new ExpandoObject();
                
                schoolVM.School = school.Name;
                schoolVM.Degree = school.Degree;
                schoolVM.Date = dateString;
                
                if (school.EndYear == null)
                {
                    currentEducation = schoolVM;
                }
                else
                {
                    this.Education.Add(schoolVM);
                }
                
                
            }

            if (currentEducation != null)
            {
                this.Education.Insert(0, currentEducation);
            }

            //populate recommendations
            var userRecommendations = user.UserEmploymentRecommendations.ToList();
            this.Recommendations = new List<dynamic>();

            foreach (UserEmploymentRecommendation recommendation in userRecommendations)
            {
                dynamic recommendationVM = new  ExpandoObject();
                
                recommendationVM.Recommendation = recommendation.Recommendation;
                recommendationVM.Name=recommendation.RecommenderName;
                this.Recommendations.Add(recommendationVM);
            }

            //populate job history
            var jobsOrderedByDate = user.UserEmployments.OrderByDescending(j => j.EndYear).ToList();
            this.Experience = new List<dynamic>();
            object currentEmployment = null;

            foreach (UserEmployment job in jobsOrderedByDate)
            {
                var dateString = "";
                if (job.EndYear == null)
                {
                    dateString = job.StartYear + " to Present";
             
                }
                else
                {
                    dateString = job.StartYear + " to " + job.EndYear;
                }

            
                dynamic jobVM = new ExpandoObject();
                jobVM.Title = job.Title;
                jobVM.Company = job.EmployerName;
                jobVM.Date = dateString;
                jobVM.Summary = job.Summary;
                                
                if (job.EndYear != null)
                {
                    this.Experience.Add(jobVM);
                }
                else
                {
                    currentEmployment = jobVM;
                }
               
            }

            if (currentEmployment != null)
            {
                this.Experience.Insert(0, currentEmployment);
            }

            

           if (user.UserSkills != null &&
               user.UserSkills.Count() > 0)
           {
               var userSkills = user.UserSkills.ToList();

               for (int i = 0; i < userSkills.Count(); i++)
               {
                   UserSkill uk = userSkills[i];
                   if (i < userSkills.Count() - 1)
                   {
                       this.Skills += uk.Skill.ToString() + ", ";
                   }
                   else
                   {
                       this.Skills += uk.Skill.ToString();

                   }
               }
           }


        
        }
    }
}