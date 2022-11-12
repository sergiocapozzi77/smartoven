// <copyright file="IRecipesManager.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using System;
using SmartOvenV2.Models;

namespace SmartOvenV2.Managers
{
    internal interface IRecipesManager
    {
        event EventHandler RecipeChanged; 

        void SetSelectedRecipe(Recipe recipe);
        Recipe GetSelectedRecipe();
    }
}