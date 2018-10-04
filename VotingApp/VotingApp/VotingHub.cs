

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;


namespace VotingApp
{
    public class VotingHub : Hub
    {

        public static Dictionary<string, int> poll = populateDict();
        public static Dictionary<string, int> populateDict()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            StreamReader sr = new StreamReader(@"C:\inetpub\wwwroot\Config\Tables.txt");

            while (!sr.EndOfStream)
            {
                temp.Add(sr.ReadLine(), 0);
            }
            sr.Close();
            sr.Dispose();
            return temp;
        }

        public void Send(string name)
        {



            poll[name]++;//add one to that string in dict
            string data = JsonConvert.SerializeObject(poll.Select(x => new { name = x.Key, count = x.Value }).ToList());
            StreamWriter sw = new StreamWriter(@"C:\inetpub\wwwroot\Config\Results.txt");
            sw.WriteLine(FormatJson(data));
            sw.Close();
            sw.Dispose();

            Clients.All.showLiveResult(data);
        }

        private const string INDENT_STRING = "    ";

        static string FormatJson(string json)
        {

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

            return String.Concat(result);
        }
    }
}

