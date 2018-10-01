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
            StreamReader sr = new StreamReader(@"C:\Users\s292155\Desktop\Tables.txt");

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

            Clients.All.showLiveResult(data);
        }
    }

}