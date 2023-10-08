using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("nameClean")]
        public string Name { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
        public virtual Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}