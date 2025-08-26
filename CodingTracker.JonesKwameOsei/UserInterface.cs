using Spectre.Console;
using System.Globalization;
using static CodingTracker.JonesKwameOsei.Enums;

namespace CodingTracker.JonesKwameOsei;

internal class UserInterface
{
    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var userOption = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        MainMenuOptions.AddCodingSession,
                        MainMenuOptions.ViewCodingSessions,
                        MainMenuOptions.UpdateCodingSessions,
                        MainMenuOptions.DeleteCodingSessions,
                        MainMenuOptions.Quit
                    )
                );

            switch (userOption)
            {
                case MainMenuOptions.AddCodingSession:
                    AddCodingSession();
                    break;
                case MainMenuOptions.ViewCodingSessions:
                    ViewCodingSessions();
                    break;
                case MainMenuOptions.UpdateCodingSessions:
                    UpdateCodingSession();
                    break;
                case MainMenuOptions.DeleteCodingSessions:
                    DeleteCodingSession();
                    break;
                case MainMenuOptions.Quit:
                    Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void AddCodingSession()
    {

    }

    private static void ViewCodingSessions()
    {

    }

    private static void UpdateCodingSession()
    {

    }

    private static void DeleteCodingSession()
    {

    }

    private static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: dd-MM-yy HH:mm (24 hour clock). Or enter 0 to return to main menu");

        if (startDateInput == "0") MainMenu();

        DateTime startDate;
        while (!DateTime.TryParseExact(startDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            startDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format. Please input Start Date with the format: dd-MM-yy HH:mm (24 hour clock). Please try again\n\n");
        }

        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-MM-yy HH:mm (24 hour clock). Or enter 0 to return to main menu");

        if (endDateInput == "0") MainMenu();

        DateTime endDate;
        while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format or End Date is earlier than or equal to Start Date. Please input End Date with the format: dd-MM-yy HH:mm (24 hour clock). Please try again\n\n");
        }

        while (startDate > endDate)
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start start. Please try agian\n\n");

            while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format. Please input End Date with the format: dd-MM-yy HH:mm (24 hour clock). Please try again\n\n");
            }
        }

        return [startDate, endDate];
    }

}
