using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdWeb.Models.ViewModels
{
    public class JobViewModel
    {
        private Job Job { get; set; }
        public long ID { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public string Industry2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ExperienceLevel { get; set; }
        public string Education { get; set; }
        public string RolesAndResponsibilities { get; set; }
        public string SkillRequirements { get; set; }
        public string URL { get; set; }

        public JobViewModel() { }

        public JobViewModel (Job j, CrowdEntities context)
        {
            this.Job = j;
            this.ID = j.ID;
            this.Position = j.Title;
            this.Company = j.Company;
            this.Industry = j.Industry;
            this.Industry2 = j.Industry2;
            this.City = j.LocationCity;
            this.State = j.LocationState;
            this.Country = j.LocationCountry;

            if (j.ExperienceLevelType1 != null)
            {
                this.ExperienceLevel = j.ExperienceLevelType1.Type.ToString();
            }

            this.Education = j.Qualifications;
            this.RolesAndResponsibilities = j.Responsibilities;
            
            if (j.JobSkills != null)
            {
                var jobSkills = j.JobSkills.ToList();
                for (int i = 0; i < jobSkills.Count(); i++)
                {
                    JobSkill js = jobSkills[i];
                    if (i < jobSkills.Count() -1)
                    {
                        this.SkillRequirements += js.Skill.ToString() + ", ";
                    }
                    else
                    {
                        this.SkillRequirements += js.Skill.ToString();

                    }


                }
            }

            this.URL = j.URL;
        }

        public string ExperienceLevelToString(long experienceLevel)
        {
            string retVal = "";

            if (experienceLevel == 0)
            {
                retVal = "0-1 years";
            }
            else if (experienceLevel == 1)
            {
                retVal = "1-3 years";
            }
            else if (experienceLevel == 2)
            {
                retVal = "3-5 years";
            }
            else if (experienceLevel == 3)
            {
                retVal = "5-8 years";
            }
            else
            {
                retVal = "8+ years";
            }
            return retVal;

        }

    }
}