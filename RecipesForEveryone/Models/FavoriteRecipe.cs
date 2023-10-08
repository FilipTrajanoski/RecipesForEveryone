using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class FavoriteRecipe
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
    }
}