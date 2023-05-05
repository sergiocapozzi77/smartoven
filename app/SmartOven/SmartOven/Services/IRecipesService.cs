// <copyright file="IRecipesService.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using SmartOvenV2.Models;
using System.Threading.Tasks;

namespace SmartOvenV2.Services
{
    internal interface IRecipesService
    {
        Task FillIngredients(Recipe recipe);
        Task FillMethod(Recipe recipe);
        Task FillSteps(Recipe recipe);
        Task<Recipe[]> GetRecipes();
    }
}