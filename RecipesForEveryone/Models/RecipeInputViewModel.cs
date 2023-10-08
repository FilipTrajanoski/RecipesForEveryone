using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RecipesForEveryone.Models
{
    public class RecipeInputViewModel
    {
        [Required]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Preparation Time (minutes)")]
        public int PreparationTimeInMinutes { get; set; }

        [Required]
        [Display(Name = "Servings")]
        public int NumberOfServings { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Dish Types (comma-separated)")]
        public string DishTypesAsString { get; set; }
        [Required]
        [Display(Name = "Ingredients (new line-separated)")]
        public string Ingredients { get; set; }
        [Required]
        [Display(Name = "Recipe Steps (new line-separated)")]
        public string RecipeSteps { get; set; }
    }
}