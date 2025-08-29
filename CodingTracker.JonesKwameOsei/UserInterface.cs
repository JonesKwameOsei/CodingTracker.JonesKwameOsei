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
                        MainMenuOptions.UpdateCodingSession,
                        MainMenuOptions.DeleteCodingSession,
                        MainMenuOptions.Quit
                    )
                );

            switch (userOption)
            {
                case MainMenuOptions.AddCodingSession:
                    AddCodingSession();
                    break;
                case MainMenuOptions.ViewCodingSessions:
                    var dataAccess = new DataAccess();
                    var codingRecords = dataAccess.GetAllSessions();
                    ViewCodingSessions(codingRecords);
                    break;
                case MainMenuOptions.UpdateCodingSession:
                    UpdateCodingSession();
                    break;
                case MainMenuOptions.DeleteCodingSession:
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
        CodingRecord codingRecord = new CodingRecord();

        var dateInputs = GetDateInputs();
        codingRecord.Language = GetLanguageInput();
        codingRecord.DateStart = dateInputs[0];
        codingRecord.DateEnd = dateInputs[1];

        var dataAccess = new DataAccess();
        dataAccess.InsertRecord(codingRecord);

    }

    private static void ViewCodingSessions(IEnumerable<CodingRecord> codingRecords)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Language");
        table.AddColumn("Date Start");
        table.AddColumn("Date End");
        table.AddColumn("Duration (Hours)");

        foreach (var codingRecord in codingRecords)
        {
            table.AddRow(
                codingRecord.Id.ToString(),
                codingRecord.Language!,
                codingRecord.DateStart.ToString(),
                codingRecord.DateEnd.ToString(),
                $"{codingRecord.Duration.TotalHours} hours {codingRecord.Duration.TotalMinutes % 60} minutes"
            );
        }
        AnsiConsole.Write(table);
    }

    private static void UpdateCodingSession()
    {
        var dataAccess = new DataAccess();
        var codingRecords = dataAccess.GetAllSessions();
        ViewCodingSessions(codingRecords);

        var idInput = GetNumber("Enter the Id of the coding session you want to update. Or enter [red]0[/] to return to main menu: ");
        if (idInput == 0) MainMenu();

        var codingRecord = codingRecords.Where(crs => crs.Id == idInput).Single();
        var dateInputs = GetDateInputs();

        codingRecord.Language = GetLanguageInput();
        codingRecord.DateStart = dateInputs[0];
        codingRecord.DateEnd = dateInputs[1];

        dataAccess.UpdateRecord(codingRecord);
    }

    private static int GetNumber(string prompt)
    {
        var input = AnsiConsole.Ask<string>(prompt).Trim();

        if (input == "0")
        {
            MainMenu();
            return 0;
        }

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

    private static void DeleteCodingSession()
    {

    }

    private static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Or enter [red]0[/] to return to main menu: ");

        if (startDateInput == "0") MainMenu();

        DateTime startDate;
        while (!DateTime.TryParseExact(startDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            startDateInput = AnsiConsole.Ask<string>("\n\nInvalid date format. Please input Start Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Please try again: \n\n");
        }

        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Or enter [red]0[/] to return to main menu: ");

        if (endDateInput == "0") MainMenu();

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

        return [startDate, endDate];
    }

    private static string GetLanguageInput()
    {
        var languageInput = AnsiConsole.Ask<string>("What programming language will you code? Or enter [red]0[/] to return to main menu: ");
        if (languageInput == "0") MainMenu();
        while (string.IsNullOrWhiteSpace(languageInput))
        {
            languageInput = AnsiConsole.Ask<string>("\n\n[red]Language cannot be empty. Please enter a valid programming language. Please try again: \n\n[/]");
        }
        return languageInput;
    }

}
