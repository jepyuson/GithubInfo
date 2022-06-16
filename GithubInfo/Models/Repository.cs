using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubInfo.Models
{
    public class Repository
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int stargazers_count { get; set; }
    }
}