using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [Display(Name ="Comment")]
        public string CommentText { get; set; }

        [Required]
        public string UserId { get; set; } 

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}