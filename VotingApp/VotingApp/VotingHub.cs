

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

/// <summary>
/// @Author: Jacob Buckley, WME Intern, October 2018, for the IT Shar Fair this app levredges a TechBrij Template
/// there are a lot of paths in here your going to have to change because I hard coded them in this page and also the HomeController.CS for the IT share Fair
/// this app also need a Config/Tables.txt with one table per line
/// and a Config/config.txt that has some modular questions on each line ex: title, header, question and vote button text
/// A "Results/ folder must also be created 
///  Good luck!
/// 
/// 
/// </summary>
namespace VotingApp
{
    public class VotingHub : Hub
    {

        public static Dictionary<string, int> poll = populateDict();
        /// <summary>
        /// 
        /// so here we load in some custom tables from the Config Tables path
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> populateDict()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            string path = @"C:\inetpub\wwwroot\Config\Tables.txt";
            StreamReader sr = new StreamReader(path);
             
            while (!sr.EndOfStream)
            {
                temp.Add(" "+sr.ReadLine(), 0); //each line will be added into dictionary with 0 as its default votes
            }
            sr.Close();
            sr.Dispose();
            try //here we're trying o presreve votes across memory wipes by pulling a temp file that has a json in it
            {
                poll = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(@"C:\inetpub\wwwroot\Config\temp.temp"));
            }
            catch (Exception e) { }
            return temp; //returns a disctionary of (option,VoteCount)

        }

        /// <summary>
        /// sends a new vote up to a dictionary and fires off the count event 
        /// </summary>
        /// <param name="name"></param>
        public void Send(string name)
        {
            try {
                poll = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(@"C:\inetpub\wwwroot\Config\temp.temp"));
                //save the current dictionary off to a json called Temp.Temp incase of a memory wipe
            } catch (Exception e) { }
           
            DateTime dt = DateTime.Now; //i want to make log files with vote counts and adding dt in name makes them unique
            poll[name]++;//add one to that string in dict


            string json = JsonConvert.SerializeObject(poll);
            File.WriteAllText(@"C:\inetpub\wwwroot\Results\temp.temp", json);

            //sort the dictionary out so it appears right in the log file
            var tmp = poll.OrderBy(x => x.Value).Select(x => new { name = x.Key, count = x.Value }).ToList();
            string data = JsonConvert.SerializeObject(tmp);

            //this will be the title for the log file
            string test = (@"C:\inetpub\wwwroot\Results\" + dt.ToShortDateString().Replace("/", ".") +"-"+ dt.ToShortDateString().Replace("/", ".") + ".txt");
            StreamWriter sw = new StreamWriter(test);
            sw.WriteLine(FormatJson(data));
            sw.Close();
            sw.Dispose();

            //here is the actual send fire.
            Clients.All.showLiveResult(data);
        }

       
    

        /// <summary>
        /// when we output the json it looks pretty bad, if we output it like this it looks pretty good. ::l
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        static string FormatJson(string json)
        {
            string INDENT_STRING = "    ";

            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null
                            ? openChar.Length > 1
                                ? openChar
                                : closeChar
                            : lineBreak;

            return String.Concat(result); ///its not perfect but its good
        }
    }
}

