using System;
using System.Collections.Generic;
using System.Text;
using SmartOvenV2.Models;
using Xamarin.Essentials;

namespace SmartOvenV2.Managers
{
    class AppStatusManager : IAppStatusManager
    {
        public AppStatus AppStatus { get; }

        public AppStatusManager()
        {
            this.AppStatus = new AppStatus();
            this.AppStatus.RecipeTimer = new System.Diagnostics.Stopwatch();

        }

        public void UpdateRecipeTimer(bool isPaused)
        {
       /*     if(isPaused)
            {
                this.AppStatus.RecipePausedTimer = DateTime.Now - this.AppStatus.RecipePausedTimerStart.Value;
            }

            if (this.AppStatus.RecipeTimerStart == null)
            {
                this.AppStatus.RecipeTimer = TimeSpan.Zero;
            }
            else
            {
                this.AppStatus.RecipeTimer = DateTime.Now - this.AppStatus.RecipeTimerStart.Value;
            }*/
        }

        public void UpdateOvenTimer()
        {
            if (this.AppStatus.OvenTimerStart == null)
            {
                this.AppStatus.OvenTimer = TimeSpan.Zero;
            }
            else
            {
                this.AppStatus.OvenTimer = DateTime.Now - this.AppStatus.OvenTimerStart.Value;
            }
        }

        public void Persist()
        {
            if (this.AppStatus.RecipeTimerStart.HasValue)
            {
                Preferences.Set("RecipeTimerStart", this.AppStatus.RecipeTimerStart.Value);
            }
            else
            {
                Preferences.Clear("RecipeTimerStart");
            }
            
            if (this.AppStatus.OvenTimerStart.HasValue)
            {
                Preferences.Set("OvenTimerStart", this.AppStatus.OvenTimerStart.Value);
            }
            else
            {
                Preferences.Clear("OvenTimerStart");
            }
            
        }
        
        public void Restore()
        {
            var timeRestore = DateTime.Now;
            if (Preferences.ContainsKey("RecipeTimerStart"))
            {
                this.AppStatus.RecipeTimerStart = Preferences.Get("RecipeTimerStart", DateTime.Now);
            }

            if (Preferences.ContainsKey("OvenTimerStart"))
            {
                this.AppStatus.OvenTimerStart = Preferences.Get("OvenTimerStart", DateTime.Now);
            }
        }

        public void ResetOvenTimer()
        {
            this.AppStatus.OvenTimerStart = DateTime.Now;
        }

        public void RemoveOvenTimer()
        {
            this.AppStatus.OvenTimerStart = null;
        }

        public void ResetOvenTimerIfNotStarted()
        {
            if (!this.AppStatus.OvenTimerStart.HasValue)
            {
                ResetOvenTimer();
            }
        }

        public void ResetRecipeTimer()
        {
            this.AppStatus.RecipeTimer.Reset();
            this.AppStatus.RecipeTimer.Start();
        }

        public void RemoveRecipeTimer()
        {
            this.AppStatus.RecipeTimerStart = null;
        }

        public void ResumeRecipeTimer()
        {
            this.AppStatus.RecipeTimer.Start();
        }

        public void PauseRecipeTimer()
        {
            this.AppStatus.RecipeTimer.Stop();
        }

    }
}
