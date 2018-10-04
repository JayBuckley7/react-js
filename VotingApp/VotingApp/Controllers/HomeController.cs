using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private static string headTag;
        private static string questTag;
        private static string voteTag;

        public ActionResult Index()
        {
            return View();
        }
        public static void test()
        {
            StreamReader sr = new StreamReader(@"C:\inetpub\wwwroot\Config\Config.txt");

            headTag = sr.ReadLine();
            questTag = sr.ReadLine();
            voteTag = sr.ReadLine();

            sr.Close();
            sr.Dispose();
        }
        public JsonResult SurveyQuiz()
        {
            test();

            var poll = new
            {   Header = headTag,
                question = questTag,
                voteTxt = voteTag,
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


