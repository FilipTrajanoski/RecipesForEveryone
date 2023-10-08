using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class ReviewPostModel
    {
        public int id { get; set; }
        public int rating { get; set; }
        public string commentText { get; set; }
    }
}