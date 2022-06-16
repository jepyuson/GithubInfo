using GithubInfo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GithubInfo.Controllers
{
    public class GithubUserController : Controller
    {

        private readonly string GithubApiUsersUrl = "https://api.github.com/users/";

        // GET: GithubInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UserDetails(GithubUser user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            GithubUser model = null;
            var apiUrl = GithubApiUsersUrl + user.username;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<GithubUser>(data);
                }
                else
                {
                    return View(model);
                }
            }

            model.Repositories = await GetRepositories(model.repos_url);

            return View(model);
        }

        private async Task<List<Repository>> GetRepositories(string repos_url)
        {
            List<Repository> repos = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(repos_url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                HttpResponseMessage response = await client.GetAsync(repos_url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    repos = JsonConvert.DeserializeObject<List<Repository>>(data);
                }
            }

            return repos.OrderByDescending(r => r.stargazers_count).Take(5).ToList();
        }
    }
}