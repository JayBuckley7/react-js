using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingApp.Controllers
{

    /// <summary>
    /// This controller takes in the users votes
    /// reads all the config giles and sets up the web page on first visit
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        private static string headTag;
        private static string questTag;
        private static string voteTag;

        /// <summary>
        /// this loads the index view from Views/Home (which actually just adds things ontop of the Views/Shared/_layout) like all pages do
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// I should have named this better but this pulls config files from the path and reads them in as the "dynamic" elements
        /// </summary>
        public static void test()
        {
            string path = @"C:\inetpub\wwwroot\Config\Config.txt";

            StreamReader sr = new StreamReader(path);

            headTag = sr.ReadLine(); // this is the big words on the top of the home page
            questTag = sr.ReadLine(); //quest is question, which is what the voting app uses as instructions for voting
            voteTag = sr.ReadLine(); //this is the word on the button for Vote! (probably would be fine to just hard code this)

            sr.Close(); 
            sr.Dispose();
        }
        /// <summary>
        /// heres what gets sent up to the react script to handle the votes
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult SurveyQuiz()  //this JsonResult must reurn a Json formated file
        {
            test(); //runs the part the grabs my config files and loads them in defined right above

            var poll = new  // here we define a "poll" which is acutally a the data format that my react script reads
            {   Header = headTag,  
                question = questTag,
                voteTxt = voteTag,
                choices = VotingHub.poll.Select(x => new { name = x.Key, count = x.Value }).ToList()  //this is a linQ that sorts the choices dictionary by number of votes
            };
            return Json(poll, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// i can probably get rid of this
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}


