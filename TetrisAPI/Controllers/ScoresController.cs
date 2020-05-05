using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TetrisAPI.Models;

namespace TetrisAPI.Controllers
{
    [Authorize]
    public class ScoresController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            string userid = User.Identity.GetUserId();
            IEnumerable<String> scores = new List<String>();
            
            if (!string.IsNullOrEmpty(userid))
            {
               scores =  (from score in context.Scores
                         where score.ApplicationUserID == userid
                         orderby score.Value descending
                         select score.Value.ToString()).Take(5);
            }

            return scores;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post(ScoreBindingfModel model)
        {
            
            
            ApplicationDbContext context = new ApplicationDbContext();

            Score score = new Score
            {
                Value = model.Value,
                Date = DateTime.Now,
                ApplicationUserID = User.Identity.GetUserId()
            };

            context.Scores.Add(score);

            context.SaveChanges();


            return "OK";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
