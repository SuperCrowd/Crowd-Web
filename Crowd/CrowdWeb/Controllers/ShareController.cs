using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrowdWeb.Models.ViewModels;
using Portal.Model;
using Portal.Repository;

namespace CrowdWeb.Controllers
{
    public class ShareController : Controller
    {

        UnitOfWork db = new UnitOfWork();
        // GET: Share
        public ActionResult Job(long? jobID)
        {
            List<Job> lstJob = db.Job.Get(n => n.ID == jobID).ToList();
            if (lstJob.Count > 0)
            {
                Portal.Model.Job job = lstJob.FirstOrDefault();
                JobViewModel jvm = new JobViewModel(job);
                return View("Job", jvm);
            }
            else
            {
                //erorr
                return View("Error");
            }
        }

        public ActionResult User(long? userID)
        {

            List<User> lstUser = db.User.Get(n => n.ID == userID).ToList();
            if (lstUser.Count > 0)
            {
                Portal.Model.User user = lstUser.FirstOrDefault();
                if (user != null)
                {
                    //no error
                    CandidateViewModel jvm = new CandidateViewModel(user);
                    return View("User", jvm);
                }
            }
            else
            {
                //error
                return View("Error");
            }

            return View();
        }
    }
}