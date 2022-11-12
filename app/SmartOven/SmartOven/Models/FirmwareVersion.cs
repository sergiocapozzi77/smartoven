// <copyright file="FirmwareVersion.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using Newtonsoft.Json;

namespace SmartOvenV2.Models
{
    public class FirmwareVersion
    {
        [JsonProperty("firmware")]
        public int Firmware { get; set; }
    }
}