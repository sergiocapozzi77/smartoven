using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SmartOvenV2.Models
{
    class AppStatus
    {
        public Stopwatch RecipeTimer { get; set; }
        public DateTime? OvenTimerStart { get; set; }
        public TimeSpan OvenTimer { get; set; }
        public DateTime? RecipeTimerStart { get; set; }
    }
}
