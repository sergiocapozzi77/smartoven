using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using SmartOvenV2.Managers;
using SmartOvenV2.Models;
using SmartOvenV2.Services;
using Syncfusion.XForms.ProgressBar;
using Xamarin.Forms;

namespace SmartOvenV2.ViewModels
{
    class RecipeViewModel : BaseViewModel
    {
        public RecipeViewModel(IStatusPoller statusPoller, IBleConnector dataService, IRecipesService recipes, IRecipesManager recipesManager, IAppStatusManager appStatusManager) : base(statusPoller, dataService)
        {
            this.recipesManager = recipesManager;
            this.appStatusManager = appStatusManager;
            StartCommand = new Command(StartTimer, () => TimerEnabled);
            PauseCommand = new Command(PauseTimer);
            Recipes = recipes.GetRecipes();

            this.recipesManager.RecipeChanged += OnRecipeChanged;

            TimerValue = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
            ResetCommand = new Command(ResetTimer);
            currentRecipeStep = -1;
        }

        private readonly IRecipesManager recipesManager;
        private readonly IAppStatusManager appStatusManager;
        private DateTime startTime;
        private CancellationTokenSource cancellation;
        private int currentRecipeStep;

        public Command StartCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        public Recipe SelectedItem
        {
            get => this.recipesManager.GetSelectedRecipe();
            set
            {
                if (value == null)
                {
                    this.recipesManager.GetSelectedRecipe().IsSelected = false;
                    this.recipesManager.SetSelectedRecipe(null);
                   // this.IsOvenOn = false;
                }
                else
                {
                    if (this.recipesManager.GetSelectedRecipe() != null)
                    {
                        this.recipesManager.GetSelectedRecipe().IsSelected = false;
                    }

                    this.recipesManager.SetSelectedRecipe(value);
                    value.IsSelected = true;
                 //   this.IsOvenOn = true;
                    isSelectingNewRecipe = true;
                }
            }
        }

        protected override void OnElementsStatusUpdated(ElementsStatusInfo status)
        {
            base.OnElementsStatusUpdated(status);

            if (this.OvenStatus.Status == 1 && this.isSelectingNewRecipe) //oven on
            {
                this.dataService.SetTopTemperature(this.SelectedItem.TopTemperature);
                this.dataService.SetBottomTemperature(this.SelectedItem.BottomTemperature);
                this.isSelectingNewRecipe = false;
            }
        }

        private volatile bool isSelectingNewRecipe;

        public bool IsStarted
        {
            get => isStarted;
            set
            {
                isStarted = value; 
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(CanPause));
                this.OnPropertyChanged(nameof(CanStart));
            }
        }

        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(CanPause));
                this.OnPropertyChanged(nameof(CanStart));
            }
        }

        public bool CanPause => IsStarted && !IsPaused;
        public bool CanStart => !IsStarted || IsPaused;

        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                this.OnPropertyChanged();
            }
        }

        private string timerValue;
        private bool isStarted;
        private Recipe selectedRecipe;
        private bool isPaused;
        public IList<Recipe> Recipes { get; }

#if DEBUG
        //public bool TimerEnabled => true;
        public bool TimerEnabled => this.OvenStatus.Status == 1 && recipesManager.GetSelectedRecipe() != null;
#else
        public bool TimerEnabled => this.OvenStatus.Status == 1 && recipesManager.GetSelectedRecipe() != null;
#endif
        public string TimerValue
        {
            get => timerValue;
            set
            {
                this.timerValue = value; 
                this.OnPropertyChanged();
            }
        }

        private void OnRecipeChanged(object sender, EventArgs args)
        {
            this.SelectedRecipe = this.recipesManager.GetSelectedRecipe();
            this.appStatusManager.ResetRecipeTimer();
            StartCommand?.ChangeCanExecute();
        }

        protected override void OnOvenStatusUpdated(OvenStatus status)
        {
            base.OnOvenStatusUpdated(status);
            StartCommand?.ChangeCanExecute();
        }

        private void ResetTimer()
        {
            currentRecipeStep = -1;
            this.appStatusManager.ResetRecipeTimer();
            StopTimer();
        }

        void StartTimer()
        {
            if(IsPaused)
            {
                this.appStatusManager.ResumeRecipeTimer();
                IsPaused = false;
                return;
            }

            startTime = DateTime.Now;
            IsStarted = true;
            this.cancellation = new CancellationTokenSource();
            this.appStatusManager.ResetRecipeTimer();

            // set the start step as initiated
            this.SelectedRecipe.Steps.First().Status = StepStatus.Completed;
            this.SelectedRecipe.Steps.First().ProgressValue = 100;


            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.appStatusManager.UpdateRecipeTimer(IsPaused);
                TimerValue = this.appStatusManager.AppStatus.RecipeTimer.Elapsed.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                FollowRecipe(this.appStatusManager.AppStatus.RecipeTimer.Elapsed);

                return !this.cancellation.IsCancellationRequested;
            });
        }

        void FollowRecipe(TimeSpan elapsed)
        {
            var recipe = this.recipesManager.GetSelectedRecipe();
            if (recipe == null)
            {
                StopTimer();
                return;
            }

            var previousDuration = TimeSpan.Zero;
            
            for (var i = 0; i < recipe.Steps.Count; i++)
            {
                var step = recipe.Steps[i];
                if (elapsed > previousDuration &&
                    elapsed < previousDuration + step.Duration)
                {
                    if (this.currentRecipeStep < i)
                    {
                        this.dataService.SetTopTemperature(step.TopTemperature);
                        this.dataService.SetBottomTemperature(step.BottomTemperature);

                        this.dataService.SetTopMaxPower(step.TopMaxPower);
                        this.dataService.SetBottomMaxPower(step.BottomMaxPower);

                        if(step.Pause)
                        {
                            this.PauseTimer();
                        }

                        this.currentRecipeStep = i;
                        step.ProgressValue = 100;
                        step.Status = StepStatus.Completed;
                    } else if(i < recipe.Steps.Count - 1)
                    {
                        var nextStep = recipe.Steps[i+1];
                        nextStep.ProgressValue = ((elapsed.TotalSeconds - previousDuration.TotalSeconds) /
                                                  nextStep.Duration.TotalSeconds) * 100;
                    }

                    break;
                }

                previousDuration += step.Duration;
            }
        }

        void PauseTimer()
        {
            IsPaused = true;
            this.appStatusManager.PauseRecipeTimer();
        }

        void StopTimer()
        {
            IsStarted = false;
            this.cancellation.Cancel();
            this.selectedRecipe?.ClearStatus();
        }

    }
}
