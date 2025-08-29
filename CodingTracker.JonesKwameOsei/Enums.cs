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
        ViewCodingSessions,

        [Display(Name = "Update Coding Session")]
        UpdateCodingSession,

        [Display(Name = "Delete Coding Sessions")]
        DeleteCodingSession,

        Quit
    }
}
