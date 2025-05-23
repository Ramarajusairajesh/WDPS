using System;

namespace WDPS.Core.Models
{
    public class OnboardingProgress
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int CurrentStep { get; set; }
        public int TotalSteps { get; set; }
        public bool IsCompleted { get; set; }
    }
} 