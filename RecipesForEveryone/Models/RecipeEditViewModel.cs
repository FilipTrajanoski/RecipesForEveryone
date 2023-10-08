using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class RecipeEditViewModel
    {
        public int Id { get; set; }

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
        [DataType(DataType.MultilineText)]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Ingredients (new line-separated)")]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }

        [Required]
        [Display(Name = "Recipe Steps (new line-separated)")]
        [DataType(DataType.MultilineText)]
        public string RecipeSteps { get; set; }

        [Required]
        [Display(Name = "Dish Types (comma-separated)")]
        public string DishTypesAsString { get; set; }
    }
}