using Spectre.Console;
using System.Globalization;
using static CodingTracker.JonesKwameOsei.UserInterface;

namespace CodingTracker.JonesKwameOsei;

internal class Validation
{
    internal static int ValidateInt(string input, string prompt)
    {
        while (!int.TryParse(input, out var value) || value < 0)
        {
            input = AnsiConsole.Ask<string>($"\n\nInvalid number: {prompt}").Trim();
            if (input == "0")
            {
                MainMenu();
                return 0;
            }
        }

        return int.Parse(input);
    }

    internal static string ValidateLanguage(string languageInput)
    {
        while (string.IsNullOrWhiteSpace(languageInput))
        {
            languageInput = AnsiConsole.Ask<string>("\n\nLanguage cannot be empty. Please enter a valid programming language.").Trim();
            if (languageInput == "0")
            {
                MainMenu();
                return "C#";
            }
        }
        return languageInput;
    }

    internal static DateTime ValidateStartDate(string startDateInput)
    {
        DateTime startDate;
        while (!DateTime.TryParseExact(startDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            startDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format. Please input Start Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Please try again: \n\n");
        }

        return startDate;
    }

    internal static DateTime ValidateEndDate(string endDateInput, DateTime startDate)
    {
        DateTime endDate;
        while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format or End Date is earlier than or equal to Start Date. Please input End Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Please try again: \n\n");
        }

        while (startDate > endDate)
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start start. Please try agian\n\n");

            while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format. Please input End Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Please try again: \n\n");
            }
        }

        return endDate;
    }
}
