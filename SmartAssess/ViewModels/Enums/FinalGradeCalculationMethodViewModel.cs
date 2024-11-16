using System.ComponentModel.DataAnnotations;

namespace ViewModels.Enums
{
    public enum FinalGradeCalculationMethodViewModel
    {
        [Display(Name = "Average", ResourceType = typeof(FinalGradeCalculationMethodViewModel))]
        Average,
        [Display(Name = "Max")]
        Max,
        [Display(Name = "LastAttempt")]
        LastAttempt
    }
}