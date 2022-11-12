using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOvenV2.Models
{
    class ProgressUpdate
    {
        public ProgressUpdate(string info, double progress = 100)
        {
            Info = info;
            Progress = progress;
        }

        public string Info { get; set; }
        public double Progress { get; set; }
    }
}
