using System;
using System.Linq;
using System.Threading.Tasks;
using SmartOvenV2.Models;
using Syncfusion.DataSource.Extensions;

namespace SmartOvenV2.Services
{
    class RecipesRemoteService : IRecipesService
    {
        private readonly ISeatableDataService seatableDataService;
        private readonly IRecipeStepFactory recipeStepFactory;

        public RecipesRemoteService(ISeatableDataService seatableDataService, IRecipeStepFactory recipeStepFactory)
        {
            this.seatableDataService = seatableDataService;
            this.recipeStepFactory = recipeStepFactory;
        }

        public async Task<Recipe[]> GetRecipes()
        {
            var array = await this.seatableDataService.GetData("recipes");
            var recipes = array.Select(x => Recipe.FromJToken(x)).ToArray();
            foreach (var recipe in recipes)
            {
                if (string.IsNullOrEmpty(recipe.Image))
                {
                    continue;
                }

                recipe.Image = await this.seatableDataService.GetImageLink(recipe.Image);
                Console.WriteLine("Retrieve image link: " + recipe.Image);
            }

            Console.WriteLine("Recipes: " + recipes);
#if DEBUG
            var recipeList = recipes.ToList();
            var test = new Recipe(
                Guid.NewGuid().ToString(),
                "PizzaRound.jpg",
                "Test",
                500,
                400,
                new string[0],
                new string[0],
                new string[0]);

            test.AddSteps(new RecipeStep[] {
                      this.recipeStepFactory.Create(250, 250, 50, 50, TimeSpan.FromSeconds(5), "1st", false),
                      this.recipeStepFactory.Create(450, 450, 75, 75,TimeSpan.FromSeconds(5), "2nd"),
            });
            recipeList.Add(test);

            recipes = recipeList.ToArray();
#endif

            return recipes;
        }

        public async Task FillSteps(Recipe recipe)
        {
            var stepsArray = await this.seatableDataService.GetDataSql("steps", recipe.StepIds);
            var steps = stepsArray.Select(x => RecipeStep.FromJToken(x)).ToArray();
            recipe.AddSteps(steps);
        }

        public async Task FillIngredients(Recipe recipe)
        {
            var stepsArray = await this.seatableDataService.GetDataSql("ingredients", recipe.IngredientsIds);
            var ingredients = stepsArray.Select(x => Ingredient.FromJToken(x)).ToArray();
            recipe.AddIngredients(ingredients);
        }

        public async Task FillMethod(Recipe recipe)
        {
            var stepsArray = await this.seatableDataService.GetDataSql("method", recipe.MethodsIds);
            var method = stepsArray.Select(x => Method.FromJToken(x)).ToArray();
            recipe.AddMethod(method);
        }
    }
}
