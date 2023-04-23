using System;
using System.Collections.Generic;
using System.Text;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    class RecipesService : IRecipesService
    {
        private readonly IRecipeStepFactory recipeStepFactory;

        public RecipesService(IRecipeStepFactory recipeStepFactory)
        {
            this.recipeStepFactory = recipeStepFactory;
        }

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
                    this.recipeStepFactory.Create(500, 500, 100, 100, TimeSpan.FromSeconds(50), "Top", false),
                    this.recipeStepFactory.Create(450, 450, 100, 100,TimeSpan.FromSeconds(20), "Top"),
                }));

            recipes.Add(new Recipe(
                "PizzaTeglia.jpg",
                "Tray Pizza Red Base",
                250,
                310,
                new[]
                {
                    this.recipeStepFactory.Create(250, 310, 50, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                    this.recipeStepFactory.Create(250, 310, 50, 100, TimeSpan.FromMinutes(6), "Flip tray"),
                    this.recipeStepFactory.Create(280, 310, 100, 100, TimeSpan.FromMinutes(4), "Cooking the Top")
                }));
            recipes.Add(new Recipe(
              "TegliaBianca.jpg",
              "Tray Pizza White Base",
              250,
              310,
              new[]
              {
                    this.recipeStepFactory.Create(250, 310, 25, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                    this.recipeStepFactory.Create(250, 310, 25, 100, TimeSpan.FromMinutes(6), "Flip tray"),
                    this.recipeStepFactory.Create(280, 310, 100, 100, TimeSpan.FromMinutes(4), "Cooking the Top")
              }));
            recipes.Add(new Recipe(
              "detroit_style.jpg",
              "Detroit Style",
              220,
              300,
              new[]
              {
                    this.recipeStepFactory.Create(250, 300, 25, 100, TimeSpan.FromMinutes(7), "Cooking the bottom", false),
                    this.recipeStepFactory.Create(250, 300, 75, 100, TimeSpan.FromMinutes(6), "Add cheese"),
                    this.recipeStepFactory.Create(270, 300, 100, 100, TimeSpan.FromMinutes(2), "Add tomato sauce")
              }));
            recipes.Add(new Recipe(
                "Bread.jpg",
                "Bread",
                250,
                250,
                new[]
                {
                    this.recipeStepFactory.Create(250, 250, 100, 100, TimeSpan.FromMinutes(35), "Cooking the bread", false),
                    this.recipeStepFactory.Create(220, 220, 50, 100, TimeSpan.FromMinutes(5), "Making the crust")
                }));


            recipes.Add(new Recipe(
                "Ribs.jpg",
                "Ribs",
                170,
                170,
                new[]
                {
                    this.recipeStepFactory.Create(170, 170,100, 100, TimeSpan.FromMinutes(60), "Top", false),
                    this.recipeStepFactory.Create(150, 150,100, 100, TimeSpan.FromMinutes(50), "Top")
                }));

            recipes.Add(new Recipe(
                "Panuozzo.jpg",
                "Panuozzo",
                350,
                250,
                new[]
                {
                    this.recipeStepFactory.Create(350, 250,100, 100, TimeSpan.FromMinutes(3), "Top", false),
                }));
            recipes.Add(new Recipe(
                "casatiello_napoletano.jpg",
                "Casatiello",
                180,
                220,
                new[]
                {
                    this.recipeStepFactory.Create(180, 220, 0, 100, TimeSpan.FromMinutes(20), "Initial cooking", false),
                    this.recipeStepFactory.Create(180, 220, 25, 100, TimeSpan.FromMinutes(15), "Rotate the tray"),
                    this.recipeStepFactory.Create(190, 220, 25, 100, TimeSpan.FromMinutes(10), "Colouring the top"),
                }));
            recipes.Add(new Recipe(
        "trays.jpg",
        "Blue iron tray burning",
        200,
        200,
        new[]
        {
                    this.recipeStepFactory.Create(200, 200,75, 100, TimeSpan.FromMinutes(10), "Burning", false),
        }));

#if DEBUG
            recipes.Add(new Recipe(
                "PizzaRound.jpg",
                "Test",
                500,
                400,
                new[]
                {
                    this.recipeStepFactory.Create(500, 500, 100, 100, TimeSpan.FromSeconds(10), "1st", false),
                    this.recipeStepFactory.Create(450, 450, 75, 75,TimeSpan.FromSeconds(10), "2nd"),
                    this.recipeStepFactory.Create(400, 400, 50, 50, TimeSpan.FromSeconds(10), "3rd"),
                    this.recipeStepFactory.Create(500, 500, 100, 100,TimeSpan.FromSeconds(10), "4th"),
                }));

            recipes.Add(new Recipe(
    "PizzaRound.jpg",
    "Test2",
    500,
    400,
    new[]
    {
                    this.recipeStepFactory.Create(500, 500, 100, 100, TimeSpan.FromSeconds(5), "1st", false),
                    this.recipeStepFactory.Create(450, 450, 75, 75,TimeSpan.FromSeconds(5), "2nd"),
    }));
#endif
            return recipes.ToArray();
        }
    }
}
