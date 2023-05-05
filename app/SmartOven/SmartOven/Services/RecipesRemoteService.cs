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

        public RecipesRemoteService(ISeatableDataService seatableDataService)
        {
            this.seatableDataService = seatableDataService;
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
