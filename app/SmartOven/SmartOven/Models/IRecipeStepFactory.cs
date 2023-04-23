using System;

namespace SmartOvenV2.Models
{
    internal interface IRecipeStepFactory
    {
        RecipeStep Create(double topTemperature, double bottomTemperature, double topMaxPower, double bottomMaxPower, TimeSpan duration, string title, bool pause = true);
    }
}