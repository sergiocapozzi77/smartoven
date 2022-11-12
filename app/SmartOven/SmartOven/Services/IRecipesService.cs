// <copyright file="IRecipesService.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    internal interface IRecipesService
    {
        Recipe[] GetRecipes();
    }
}