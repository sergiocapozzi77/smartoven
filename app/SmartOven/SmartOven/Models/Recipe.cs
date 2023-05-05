using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;
using Syncfusion.DataSource.Extensions;
using Syncfusion.XForms.ProgressBar;

namespace SmartOvenV2.Models
{
    

    class Recipe : INotifyPropertyChanged
    {
        private bool isSelected;
        private ObservableCollection<RecipeStep> steps;
        private ObservableCollection<Ingredient> ingredients;
        private ObservableCollection<Method> method;

        public string Id { get; private set; }
        public string[] StepIds { get; private set; }
        public string[] IngredientsIds { get; private set; }
        public string[] MethodsIds { get; private set; }
        public bool StepsLoaded { get; set; }

        public Recipe(string id, string image, string imageTitle, double topTemperature, double bottomTemperature, string[] steps, string[] ingredientsIds, string[] methodsIds)
        {
            Id = id;
            Image = image;
            ImageTitle = imageTitle;
            TopTemperature = topTemperature;
            BottomTemperature = bottomTemperature;
            StepIds = steps;
            IngredientsIds = ingredientsIds;
            MethodsIds = methodsIds;
        }

        public void AddIngredients(Ingredient[] ingredients)
        {
            if (this.IngredientsLoaded)
            {
                return;
            }

            this.Ingredients = new ObservableCollection<Ingredient>(ingredients);

            this.IngredientsLoaded = true;
        }

        public void AddMethod(Method[] method)
        {
            if (this.MethodLoaded)
            {
                return;
            }

            this.Method = new ObservableCollection<Method>(method);

            this.MethodLoaded = true;
        }

        public void AddSteps(RecipeStep[] steps)
        {
            if(this.StepsLoaded)
            {
                return;
            }

            var firstStep = steps.First();

            var startStep = new RecipeStep(firstStep.TopTemperature, firstStep.BottomTemperature, firstStep.TopMaxPower, firstStep.BottomMaxPower, TimeSpan.FromSeconds(0),
                "Start", false);

            Steps = new ObservableCollection<RecipeStep>();
            //Steps.Add(startStep);

            var time = TimeSpan.FromSeconds(0);
            foreach (var recipeStep in steps)
            {
                recipeStep.Time = time.ToString();
                time += recipeStep.Duration;
                Steps.Add(recipeStep);
            }

            var endStep = new RecipeStep(firstStep.TopTemperature, firstStep.BottomTemperature, firstStep.TopMaxPower, firstStep.BottomMaxPower, steps.Last().Duration,
                "Finish", false);
            endStep.Time = time.ToString();
            Steps.Add(endStep);

            this.StepsLoaded = true;
        }

        public static Recipe FromJToken(JToken recipe)
        {
            return new Recipe(
                recipe.Value<string>("_id"),
                recipe.Value<JArray>("Image")?.FirstOrDefault()?.ToString(),
                recipe.Value<string>("Name"),
                recipe.Value<double>("Starting Top Temperature"),
                recipe.Value<double>("Starting Bottom Temperature"),
                recipe.Value<JArray>("Steps")?.ToObject<string[]>(),
                recipe.Value<JArray>("Ingredients")?.ToObject<string[]>(),
                recipe.Value<JArray>("Method")?.ToObject<string[]>()
                );
        }

        public string Image { get; set; }
        public string ImageTitle { get; }

        public double TopTemperature { get; }

        public double BottomTemperature { get; }
        public ObservableCollection<RecipeStep> Steps
        {
            get => steps; private set
            {
                steps = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get => ingredients; private set
            {
                ingredients = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Method> Method
        {
            get => method; private set
            {
                method = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                this.OnPropertyChanged();
            }
        }

        public bool IngredientsLoaded { get; private set; }
        public bool MethodLoaded { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ClearStatus()
        {
            foreach (var step in Steps)
            {
                step.Status = StepStatus.NotStarted;
                step.ProgressValue = 0;
            }
        }
    }
}
