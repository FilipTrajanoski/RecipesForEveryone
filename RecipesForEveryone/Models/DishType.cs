using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class DishType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public virtual Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}