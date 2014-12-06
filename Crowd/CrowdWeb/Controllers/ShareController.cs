using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrowdWeb.Models.ViewModels;

namespace CrowdWeb.Controllers
{
    public class ShareController : Controller
    {
        // GET: Share
        public ActionResult Job(long jobID)
        {
            using (CrowdEntities context = new CrowdEntities())
            {
                var job = context.Jobs.Where(j => j.ID == jobID).FirstOrDefault();
                if (job != null)
                {
                    //no error
                    JobViewModel jvm = new JobViewModel(job, context);
                    return View("Job", jvm);
                }
                else
                {
                    //error
                }
            }
            return View();
        }

        public ActionResult User(long userID)
        {
            using (CrowdEntities context = new CrowdEntities())
            {
                var user = context.Users.Where(j => j.ID == userID).FirstOrDefault();
                if (user != null)
                {
                    //no error
                    CandidateViewModel jvm = new CandidateViewModel(user, context);
                    return View("User", jvm);
                }
                else
                {
                    //error
                }
            }
            return View();
        }
    }
}