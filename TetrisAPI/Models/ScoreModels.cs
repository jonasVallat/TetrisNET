using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TetrisAPI.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class ScoreBindingfModel
    {
        [Required]
        [Display(Name = "Score")]
        public int Value { get; set; }
    }
}