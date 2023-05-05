using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using SmartOvenV2.Annotations;
using SmartOvenV2.Services;
using Syncfusion.XForms.ProgressBar;

namespace SmartOvenV2.Models
{

    class RecipeStep : INotifyPropertyChanged
    {
        private StepStatus status;
        private double progressValue;
        private string description;

        public RecipeStep(double topTemperature, double bottomTemperature, double topMaxPower, double bottomMaxPower, TimeSpan duration, string title, bool pause)
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

        internal static RecipeStep FromJToken(JToken recipeStep)
        {
            return new RecipeStep(
                recipeStep.Value<double>("Top Temperature"),
                recipeStep.Value<double>("Bottom Temperature"),
                recipeStep.Value<double>("Top Max Power") * 100,
                recipeStep.Value<double>("Bottom Max Power") * 100,
                TimeSpan.FromSeconds(recipeStep.Value<double>("Duration")),
                recipeStep.Value<string>("Name"),
                recipeStep.Value<bool>("Pause")
                );
        }

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
}
