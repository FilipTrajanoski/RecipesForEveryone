using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class RecipeInstructions
    {
        [Key]
        public int Id { get; set; }
        
        [JsonProperty("steps")]
        public virtual List<RecipeStep> Steps { get; set; }
        public virtual Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

        public RecipeInstructions()
        {
            Steps = new List<RecipeStep>();
        }
    }
}