using Microsoft.AspNet.Identity;
using RecipesForEveryone.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecipesForEveryone.Controllers
{
    [Authorize(Roles ="User")]
    public class FavoriteRecipesController : Controller
    {
        private ApplicationDbContext db;

        public FavoriteRecipesController()
        {
            db = new ApplicationDbContext();
        }


        // GET: FavoriteRecipes
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<Recipe> favoriteRecipes = (
                from fr in db.FavoriteRecipes
                join r in db.Recipes on fr.RecipeId equals r.Id
                where fr.UserId == userId
                select r
            ).ToList();
            return View(favoriteRecipes);
        }

        public ActionResult AddToFavorites(int recipeId)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            // Check if the user has already favorited this recipe
            bool isAlreadyFavorite = db.FavoriteRecipes.Any(fr => fr.UserId == user.Id && fr.RecipeId == recipeId);

            if (!isAlreadyFavorite)
            {
                var favoriteRecipe = new FavoriteRecipe
                {
                    UserId = user.Id,
                    RecipeId = recipeId
                };
                db.FavoriteRecipes.Add(favoriteRecipe);
                db.SaveChanges();
            }

            return View("~/Views/Recipes/Index.cshtml");
        }

        public ActionResult RemoveFromFavorites(int recipeId)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            var favoriteRecipe = db.FavoriteRecipes.FirstOrDefault(fr => fr.UserId == user.Id && fr.RecipeId == recipeId);

            if (favoriteRecipe != null)
            {
                db.FavoriteRecipes.Remove(favoriteRecipe);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}