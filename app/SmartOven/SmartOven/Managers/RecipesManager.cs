using System;
using System.Collections.Generic;
using System.Text;
using SmartOvenV2.Models;

namespace SmartOvenV2.Managers
{
    class RecipesManager : IRecipesManager
    {
        private Recipe selectedRecipe;

        public event EventHandler RecipeChanged;

        public void SetSelectedRecipe(Recipe recipe)
        {
            if (recipe == this.selectedRecipe) return;

            this.selectedRecipe = recipe;
            RecipeChanged?.Invoke(this, EventArgs.Empty);
        }

        public Recipe GetSelectedRecipe()
        {
            return this.selectedRecipe;
        }

    }
}
