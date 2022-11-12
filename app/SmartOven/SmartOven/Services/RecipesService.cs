using System;
using System.Collections.Generic;
using System.Text;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    class RecipesService : IRecipesService
    {
        public Recipe[] GetRecipes()
        {
            var recipes = new List<Recipe>();
            recipes.Add(new Recipe(
                "PizzaRound.jpg",
                "Round Pizza",
                450,
                400,
                new[]
                {
                    new RecipeStep(500, 500, 100, 100, TimeSpan.FromSeconds(50), "Top", false),
                    new RecipeStep(450, 450, 100, 100,TimeSpan.FromSeconds(20), "Top"),
                }));

            recipes.Add(new Recipe(
                "PizzaTeglia.jpg",
                "Tray Pizza Red Base",
                220,
                300,
                new[]
                {
                    new RecipeStep(250, 300, 50, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                    new RecipeStep(250, 300, 50, 100, TimeSpan.FromMinutes(6), "Flip tray"),
                    new RecipeStep(270, 300, 100, 100, TimeSpan.FromMinutes(4), "Cooking the Top")
                }));
            recipes.Add(new Recipe(
              "TegliaBianca.jpg",
              "Tray Pizza White Base",
              220,
              300,
              new[]
              {
                    new RecipeStep(250, 300, 25, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                    new RecipeStep(250, 300, 25, 100, TimeSpan.FromMinutes(6), "Flip tray"),
                    new RecipeStep(270, 300, 100, 100, TimeSpan.FromMinutes(4), "Cooking the Top")
              }));
            recipes.Add(new Recipe(
              "detroit_style.jpg",
              "Detroit Style",
              220,
              300,
              new[]
              {
                                new RecipeStep(250, 300, 25, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                                new RecipeStep(250, 300, 75, 100, TimeSpan.FromMinutes(6), "Add cheese"),
                                new RecipeStep(270, 300, 100, 100, TimeSpan.FromMinutes(2), "Add tomato sauce")
              }));
            recipes.Add(new Recipe(
                "Bread.jpg",
                "Bread",
                250,
                250,
                new[]
                {
                    new RecipeStep(250, 250, 100, 100, TimeSpan.FromMinutes(35), "Cooking the bread", false),
                    new RecipeStep(220, 220, 50, 100, TimeSpan.FromMinutes(5), "Making the crust")
                }));


            recipes.Add(new Recipe(
                "Ribs.jpg",
                "Ribs",
                170,
                170,
                new[]
                {
                    new RecipeStep(170, 170,100, 100, TimeSpan.FromMinutes(60), "Top", false),
                    new RecipeStep(150, 150,100, 100, TimeSpan.FromMinutes(50), "Top")
                }));

            recipes.Add(new Recipe(
                "Panuozzo.jpg",
                "Panuozzo",
                350,
                250,
                new[]
                {
                    new RecipeStep(350, 250,100, 100, TimeSpan.FromMinutes(3), "Top", false),
                }));

#if DEBUG
            recipes.Add(new Recipe(
                "PizzaRound.jpg",
                "Test",
                500,
                400,
                new[]
                {
                    new RecipeStep(500, 500, 100, 100, TimeSpan.FromSeconds(25), "1st", false),
                    new RecipeStep(450, 450, 75, 75,TimeSpan.FromSeconds(25), "2nd"),
                    new RecipeStep(400, 400, 50, 50, TimeSpan.FromSeconds(25), "3rd"),
                    new RecipeStep(500, 500, 100, 100,TimeSpan.FromSeconds(25), "4th"),
                }));
#endif
            return recipes.ToArray();
        }
    }
}
