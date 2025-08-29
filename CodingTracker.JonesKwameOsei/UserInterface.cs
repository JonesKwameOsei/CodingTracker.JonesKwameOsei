using Spectre.Console;
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
        Console.Clear();

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
        Console.Clear();

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

        var number = Validation.ValidateInt(input, prompt);

        return number;
    }

    private static void DeleteCodingSession()
    {
        Console.Clear();

        var dataAccess = new DataAccess();
        var codingRecords = dataAccess.GetAllSessions();
        ViewCodingSessions(codingRecords);

        var idInput = GetNumber("Enter the Id of the coding session you want to delete. Or enter [red]0[/] to return to main menu: ");
        if (idInput == 0)
        {
            MainMenu();
            return;
        }

        if (!AnsiConsole.Confirm("Are you sure you want to delete this record?"))
            return;
        var response = dataAccess.DeleteRecord(idInput);

        var responseMessage = response < 1
            ? "[red]No record with the id {idInput} exit. Press any key to return to Main Menu[/]"
            : "[green]Record deleted successfully. Press any key to return to Main Menu[/]";
        AnsiConsole.MarkupLine(responseMessage);
        Console.ReadKey();
    }

    private static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Or enter [red]0[/] to return to main menu: ");

        if (startDateInput == "0") MainMenu();

        var startDate = Validation.ValidateStartDate(startDateInput);

        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: [yellow]dd-MM-yy HH:mm (24 hour clock)[/]. Or enter [red]0[/] to return to main menu: ");

        if (endDateInput == "0") MainMenu();

        var endDate = Validation.ValidateEndDate(endDateInput, startDate);

        return [startDate, endDate];
    }

    private static string GetLanguageInput()
    {
        var languageInput = AnsiConsole.Ask<string>("What programming language will you code? Or enter [red]0[/] to return to main menu: ");
        if (languageInput == "0") MainMenu();
        return Validation.ValidateLanguage(languageInput);
    }

}
