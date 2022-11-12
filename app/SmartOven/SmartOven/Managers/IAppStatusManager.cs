// <copyright file="IAppStatusManager.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using System;
using SmartOvenV2.Models;

namespace SmartOvenV2.Managers
{
    internal interface IAppStatusManager
    {
        AppStatus AppStatus { get; }
        void UpdateRecipeTimer(bool isPaused);
        void UpdateOvenTimer();

        void Persist();
        void Restore();
        void ResetOvenTimer();
        void RemoveOvenTimer();
        void ResetOvenTimerIfNotStarted();

        void ResetRecipeTimer();
        void RemoveRecipeTimer();
        void ResumeRecipeTimer();
        void PauseRecipeTimer();
    }
}