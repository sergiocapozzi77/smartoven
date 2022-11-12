// <copyright file="IBleCommands.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using System;
using System.Threading.Tasks;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    public interface IBleCommands
    {
        event EventHandler<double> BottomDesiredTemperatureChanged;
        event EventHandler<double> TopDesiredTemperatureChanged;

        event EventHandler<double[]> OnReadCpuTemperatures;
        event EventHandler<OvenStatus> OnReadOvenStatus;
        event EventHandler<ElementsStatusInfo> OnReadElementsStatusInfo;
        event EventHandler<FirmwareVersion> OnFirmwareVersion;
        event EventHandler<string> OnOtaResult;

        void StartOta();
        Task<int> SetMtu(int mtu);
        Task<bool> SendOtaData(byte[] data);
        void StopOta();

        void ReadCpuTemperatures();
        void ReadOvenStatus();
        void ReadElementsStatus();
        void ReadFirmwareVersion();
        void SwitchOn();
        void SwitchOff();
        void SetBottomTemperature(double temperature);
        void SetTopTemperature(double temperature);
        void Boost();
        void SetTopPower(double topPower);
        void SetBottomPower(double topPower);
        void StopBoost(double top, double bottom);
        void SetBottomMaxPower(double bottomMaxPower);
        void SetTopMaxPower(double topMaxPower);

        void LightOff();
        void LightOn();

    }
}