using System;
using SmartOvenV2.Services;

namespace SmartOvenV2.Models
{
    class RecipeStepFactory : IRecipeStepFactory
    {
        private readonly INotificationService notificationService;

        public RecipeStepFactory(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public RecipeStep Create(double topTemperature, double bottomTemperature, double topMaxPower, double bottomMaxPower, TimeSpan duration, string title, bool pause = true)
        {
            return new RecipeStep(topTemperature, bottomTemperature, topMaxPower, bottomMaxPower, duration, title, pause);
        }
    }
}
