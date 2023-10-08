using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class RecipeStep
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("number")]
        public int StepNumber { get; set; }

        [JsonProperty("step")]
        public string Description { get; set; }
        public virtual RecipeInstructions RecipeInstructions { get; set; }
        public int RecipeInstructionsId { get; set; }
    }
}