using System.ComponentModel.DataAnnotations;

namespace CodingTracker.JonesKwameOsei;

internal class Enums
{
    internal enum MainMenuOptions
    {
        [Display(Name = "Add Coding Session")]
        AddCodingSession,

        [Display(Name = "Start Live Coding Session")]
        StartLiveCodingSession,

        [Display(Name = "View Coding Sessions")]
        ViewAllCodingSessions,

        [Display(Name = "Filter Coding Sessions")]
        FilterCodingSessions,

        [Display(Name = "Update Coding Session")]
        UpdateCodingSession,

        [Display(Name = "Delete Coding Sessions")]
        DeleteCodingSession,

        Quit
    }

    internal enum PeriodFilterOption
    {
        All,
        Today,
        Yesterday,
        Last7Days,
        ThisWeek,
        LastWeek,
        ThisMonth,
        LastMonth,
        ThisYear,
        LastYear,
        CustomRange
    }
}
