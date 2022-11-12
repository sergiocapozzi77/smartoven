using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SmartOvenV2.Annotations;
using Syncfusion.DataSource.Extensions;
using Syncfusion.XForms.ProgressBar;

namespace SmartOvenV2.Models
{
    class RecipeStep : INotifyPropertyChanged
    {
        private StepStatus status;
        private double progressValue;
        private string description;

        public RecipeStep(double topTemperature, double bottomTemperature, double topMaxPower, double bottomMaxPower, TimeSpan duration, string title, bool pause = true)
        {
            TopTemperature = topTemperature;
            BottomTemperature = bottomTemperature;
            Duration = duration;
            Title = title;
            TopMaxPower = topMaxPower;
            BottomMaxPower = bottomMaxPower;
            Pause = pause;

            Description = $"Temp: {TopTemperature}°/{BottomTemperature}°  Power: {TopMaxPower}%/{BottomMaxPower}%";
        }

        public bool Pause { get; set; }

        public double TopTemperature { get; }
        
        public double BottomTemperature { get; }


        public double TopMaxPower{ get; }

        public double BottomMaxPower { get; }

        public TimeSpan Duration { get; }

        public StepStatus Status
        {
            get => this.status;
            set => this.SetProperty(ref this.status, value);
        }

        public double ProgressValue
        {
            get => this.progressValue;
            set => this.SetProperty(ref this.progressValue, value);
        }

        public string Time { get; set; }

        public string Title { get; set; }

        public string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

    }


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
            Steps.Add(startStep);

            var time = TimeSpan.FromSeconds(0);
            foreach (var recipeStep in steps)
            {
                recipeStep.Time = time.ToString();
                time += recipeStep.Duration;
                Steps.Add(recipeStep);
            }

            var endStep = new RecipeStep(firstStep.TopTemperature, firstStep.BottomTemperature, firstStep.TopMaxPower, firstStep.BottomMaxPower, TimeSpan.FromSeconds(0),
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
