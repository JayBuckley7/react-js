using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SurveyQuiz()
        {
            var poll = new
            {
                question = "Vote for the best Table/Team in the IT Share Fair.",
                choices = VotingHub.poll.Select(x => new { name = x.Key, count = x.Value }).ToList()
            };
            return Json(poll, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

    }
}