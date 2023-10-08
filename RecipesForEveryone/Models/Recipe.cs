using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("readyInMinutes")]
        [Display(Name ="Preparation Time (mins)")]
        public int PreparationTimeInMinutes { get; set; }
        [JsonProperty("servings")]
        [Display(Name ="Servings")]
        public int NumberOfServings { get; set; }
        [JsonProperty("image")]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("analyzedInstructions")]
        public virtual List<RecipeInstructions> RecipeInstructions { get; set; }
        [JsonIgnore]
        [Display(Name ="Dish Types")]
        public string DishTypesAsString
        {
            get { return string.Join(",", DishTypes); }
            set { DishTypes = value.Split(',').ToList(); }
        }
        [JsonProperty("dishTypes")]
        public List<string> DishTypes { get; set; }
        [JsonProperty("extendedIngredients")]
        public virtual List<Ingredient> Ingredients { get; set; }
        public int CompetitionLevel { get; set; }
        public int NumberOfRatingVotes { get; set; }
        public int TotalSumOfRatings { get; set; }
        public string Status { get; set; }
        public int CompetitionVotes { get; set; }
        public virtual List<Review> Reviews { get; set; }

        public Recipe()
        {
            DishTypes = new List<string>();
            Ingredients = new List<Ingredient>();
            RecipeInstructions = new List<RecipeInstructions>();
            Reviews = new List<Review>();
        }
    }
}