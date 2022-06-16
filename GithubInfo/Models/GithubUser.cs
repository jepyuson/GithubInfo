using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GithubInfo.Models
{
    public class GithubUser
    {
        [Required(ErrorMessage = "Your name Required")]
        public string username { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string avatar_url { get; set; }
        public string repos_url { get; set; }
        public List<Repository> Repositories { get; set; }
    }
}