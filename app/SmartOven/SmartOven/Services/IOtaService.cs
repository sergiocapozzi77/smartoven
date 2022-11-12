// <copyright file="IOtaService.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    interface IOtaService
    {
        Task<int> GetLatestAvailableVersion();
        Task<MemoryStream> GetFirmware();
        Task UpdateFirmware();

        event EventHandler<ProgressUpdate> OnProgressUpdate;
        event EventHandler<bool> OnUpdateEnd;
    }
}