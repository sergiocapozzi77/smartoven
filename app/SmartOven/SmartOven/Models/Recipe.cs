using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Syncfusion.DataSource.Extensions;
using Syncfusion.XForms.ProgressBar;

namespace SmartOvenV2.Models
{

    class Recipe : INotifyPropertyChanged
    {
        private bool isSelected;

        public Recipe(string image, string imageTitle, double topTemperature, double bottomTemperature, RecipeStep[] steps)
        {
            Image = image;
            ImageTitle = imageTitle;
            TopTemperature = topTemperature;
            BottomTemperature = bottomTemperature;
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
        }

        public string Image { get; }
        public string ImageTitle { get; }

        public double TopTemperature { get; }

        public double BottomTemperature { get; }
        public ObservableCollection<RecipeStep> Steps { get; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value; 
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ClearStatus()
        {
            foreach(var step in Steps)
            {
                step.Status = StepStatus.NotStarted;
                step.ProgressValue = 0;
            }
        }
    }
}
