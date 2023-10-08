using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RecipesForEveryone.Models;

namespace RecipesForEveryone.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private readonly SpoonacularApiService _apiService;

        public RecipesController()
        {
            _apiService = new SpoonacularApiService();
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles ="Administrator")]
        public async Task<ActionResult> IndexAsync()
        {
            string apiKey = "fea71969206644438b216c2b02dc6059";
            int numberOfRecipes = 10;

            string recipesJson = await _apiService.GetRandomRecipes(numberOfRecipes, apiKey);

            var recipesResponse = JsonConvert.DeserializeObject<Dictionary<string, List<Recipe>>>(recipesJson);
            List<Recipe> newRecipes = recipesResponse["recipes"];

            List<Recipe> existingRecipes = db.Recipes.ToList();

            // Filter out recipes that are already in the database
            List<Recipe> recipesToAdd = newRecipes
                .Where(newRecipe => !existingRecipes.Any(existingRecipe => existingRecipe.Title == newRecipe.Title))
                .ToList();

            db.Recipes.AddRange(recipesToAdd);
            await db.SaveChangesAsync();

            return Json(new { data = db.Recipes.ToList() }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        // GET: Recipes
        public ActionResult Index()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.Status != "Pending").ToList();
            return View(recipes);
        }

        [AllowAnonymous]
        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        [Authorize(Roles ="User")]
        // GET: Recipes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeInputViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newRecipe = new Recipe
                {
                    Title = viewModel.Title,
                    PreparationTimeInMinutes = viewModel.PreparationTimeInMinutes,
                    NumberOfServings = viewModel.NumberOfServings,
                    ImageUrl = viewModel.ImageUrl,
                    Summary = viewModel.Summary,
                    DishTypes = viewModel.DishTypesAsString.Split(',').Select(s => s.Trim()).ToList(),
                    Status = "Pending"
                };

                newRecipe.RecipeInstructions.Add(new RecipeInstructions());

                var ingredientLines = viewModel.Ingredients.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ingredientLine in ingredientLines)
                {
                    var parts = ingredientLine.Split(':');
                    var ingredient = new Ingredient
                    {
                        Name = parts[0],
                        Amount = double.Parse(parts[1]),
                    };
                    if (parts.Length == 3)
                    {
                        ingredient.Unit = parts[2];
                    }
                    newRecipe.Ingredients.Add(ingredient);
                }

                var stepLines = viewModel.RecipeSteps.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var stepNumber = 1;
                foreach (var stepLine in stepLines)
                {
                    var step = new RecipeStep
                    {
                        StepNumber = stepNumber,
                        Description = stepLine
                    };
                    newRecipe.RecipeInstructions[0].Steps.Add(step);
                    stepNumber++;
                }
                db.Recipes.Add(newRecipe);
                db.SaveChanges();
                return View("Details", newRecipe);
            }

            return View(viewModel);
        }

        [Authorize(Roles ="Administrator")]
        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var viewModel = new RecipeEditViewModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                PreparationTimeInMinutes = recipe.PreparationTimeInMinutes,
                NumberOfServings = recipe.NumberOfServings,
                ImageUrl = recipe.ImageUrl,
                Summary = recipe.Summary,
                Ingredients = string.Join("\r\n", recipe.Ingredients.Select(i => $"{i.Name}:{i.Amount}:{i.Unit}")),
                RecipeSteps = string.Join("\r\n", recipe.RecipeInstructions.FirstOrDefault()?.Steps.Select(s => s.Description)),
                DishTypesAsString = string.Join(",", recipe.DishTypes)
            };

            return View(viewModel);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecipeEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the original recipe from the database based on the viewModel's Id
                var originalRecipe = db.Recipes.Find(viewModel.Id);

                if (originalRecipe == null)
                {
                    return HttpNotFound();
                }

                // Update the original recipe's properties with values from the viewModel
                originalRecipe.Title = viewModel.Title;
                originalRecipe.PreparationTimeInMinutes = viewModel.PreparationTimeInMinutes;
                originalRecipe.NumberOfServings = viewModel.NumberOfServings;
                originalRecipe.ImageUrl = viewModel.ImageUrl;
                originalRecipe.Summary = viewModel.Summary;
                originalRecipe.DishTypes = viewModel.DishTypesAsString.Split(',').Select(s => s.Trim()).ToList();

                List<Ingredient> ingredients = db.Ingredients.Where(x => x.RecipeId == originalRecipe.Id).ToList();
                db.Ingredients.RemoveRange(ingredients);

                // Update the Ingredients and RecipeSteps
                originalRecipe.Ingredients.Clear();
                var ingredientLines = viewModel.Ingredients.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ingredientLine in ingredientLines)
                {
                    var parts = ingredientLine.Split(':');
                    var ingredient = new Ingredient
                    {
                        Name = parts[0],
                        Amount = double.Parse(parts[1]),
                    };
                    if (parts.Length == 3)
                    {
                        ingredient.Unit = parts[2];
                    }
                    originalRecipe.Ingredients.Add(ingredient);
                }

                List<RecipeInstructions> recipeInstructions = db.RecipeInstructions.Where(x => x.RecipeId == originalRecipe.Id).ToList();
                db.RecipeInstructions.RemoveRange(recipeInstructions);

                originalRecipe.RecipeInstructions.Clear();
                originalRecipe.RecipeInstructions.Add(new RecipeInstructions());

                var stepLines = viewModel.RecipeSteps.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var stepNumber = 1;
                foreach (var stepLine in stepLines)
                {
                    var step = new RecipeStep
                    {
                        StepNumber = stepNumber,
                        Description = stepLine
                    };
                    originalRecipe.RecipeInstructions[0].Steps.Add(step);
                    stepNumber++;
                }

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Details", new { id = originalRecipe.Id });
            }

            // If ModelState is not valid, return to the Edit view with validation errors
            return View(viewModel);
        }

        [Authorize(Roles ="Administrator")]
        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Breakfast()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("breakfast") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult Lunch()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("lunch") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult Dinner()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("dinner") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult SideDish()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("side dish") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult Dessert()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("dessert") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult Drink()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.DishTypesAsString.Contains("drink") && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }
        [AllowAnonymous]
        public ActionResult Other()
        {
            List<string> excludedDishTypes = new List<string> { "breakfast", "lunch", "dinner", "side dish", "dessert", "drink" };
            List<Recipe> recipes = db.Recipes.Where(x => !excludedDishTypes.Any(y => x.DishTypesAsString.Contains(y)) && x.Status != "Pending").ToList();
            return View("Index", recipes);
        }

        [Authorize(Roles ="User")]
        public ActionResult AddToCompetitionUser(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            recipe.CompetitionLevel = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AddToCompetitionAdministrator(int id, string value)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (value != "true")
            {
                recipe.CompetitionLevel = 0;
            }
            else
            {
                recipe.CompetitionLevel = 2;
            }
            db.SaveChanges();
            return RedirectToAction("CompetitionCandidates");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CompetitionCandidates()
        {
            List<Recipe> recipes = db.Recipes.Where(m => m.CompetitionLevel == 1).ToList();
            return View(recipes);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddReview(ReviewPostModel reviewPostModel)
        {
            var userId = User.Identity.GetUserId();
            var recipe = db.Recipes.Find(reviewPostModel.id);

            if (recipe != null)
            {
                var review = new Review
                {
                    Rating = reviewPostModel.rating,
                    CommentText = reviewPostModel.commentText,
                    UserId = userId,
                    User = db.Users.Find(userId),
                    RecipeId = reviewPostModel.id,
                    DateCreated = DateTime.Now
                };

                recipe.Reviews.Add(review);
                recipe.NumberOfRatingVotes++;
                recipe.TotalSumOfRatings += reviewPostModel.rating;

                db.Reviews.Add(review);
                db.SaveChanges();
            }

            return View("Details", recipe);
        }


        [Authorize(Roles ="Administrator")]
        public ActionResult NewRecipes()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.Status == "Pending").ToList();
            return View(recipes);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ApproveRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            recipe.Status = "Approved";
            db.SaveChanges();
            return RedirectToAction("NewRecipes");
        }

        [Authorize(Roles = "User")]
        public ActionResult Competition()
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.CompetitionLevel == 2).ToList();
            var userId = User.Identity.GetUserId();
            ViewBag.UserVotes = db.RecipeVotes.Where(x => x.UserId == userId).ToList();
            return View(recipes);
        }

        [Authorize(Roles = "User")]
        public ActionResult Vote(int id)
        {
            var userId = User.Identity.GetUserId();
            if(db.RecipeVotes.Where(x => x.RecipeId == id && x.UserId == userId).Count() == 0)
            {
                var recipeVote = new RecipeVote
                {
                    UserId = userId,
                    RecipeId = id
                };
                Recipe recipe = db.Recipes.Find(id);
                recipe.CompetitionVotes++;
                db.RecipeVotes.Add(recipeVote);
                db.SaveChanges();
            }

            return RedirectToAction("Competition");
        }

        [HttpGet]
        public ActionResult GetReviews(int id)
        {
            var reviews = db.Reviews.Where(r => r.RecipeId == id).ToList();
            return PartialView("ReviewsPartial", reviews);
        }
    }
}
