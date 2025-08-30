using Spectre.Console;
using Spectre.Console.Rendering;
using static CodingTracker.JonesKwameOsei.Enums;

namespace CodingTracker.JonesKwameOsei;

internal class UserInterface
{
    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            Console.Clear();
            var userOption = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        MainMenuOptions.AddCodingSession,
                        MainMenuOptions.StartLiveCodingSession,
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
                case MainMenuOptions.StartLiveCodingSession:
                    StartLiveCodingSession();
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

    private static void StartLiveCodingSession()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]Starting live coding session...[/]");
        var language = GetLanguageInput();
        var session = new LiveSession(language);

        AnsiConsole.MarkupLine("[yellow]Commands:[/] P = pause/resume | L = lap | S = stop & save | Q = cancel | H = help");
        AnsiConsole.MarkupLine("[grey]Press any key at any time...[/]");

        AnsiConsole.Live(new Panel("Starting...").Header("Live Coding Session")).Start(ctx =>
        {
            while (!session.IsFinished)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.P:
                            if (session.IsPaused) session.Resume(); else session.Pause();
                            break;
                        case ConsoleKey.L:
                            session.AddLap();
                            break;
                        case ConsoleKey.S:
                            session.Stop(save: true);
                            break;
                        case ConsoleKey.Q:
                            session.Stop(save: false);
                            break;
                        case ConsoleKey.H:
                            AnsiConsole.MarkupLine("[yellow]P:[/] = pause/resume  [yellow]L[/]= lap  [yellow]S[/]= stop & save  [yellow]Q[/]= cancel  [yellow]H[/]= help");
                            break;
                    }
                }

                var panel = BuidSessionPanel(session);
                ctx.UpdateTarget(panel);
                Thread.Sleep(200);
            }
        });

        if (!session.Saved)
        {
            AnsiConsole.MarkupLine("[red]Session discarded. Press any key to return to main menu.[/]");
            Console.ReadKey(true);
            return;
        }

        var record = new CodingRecord
        {
            Language = language,
            DateStart = session.StartTime,
            DateEnd = session.EndTime
        };

        new DataAccess().InsertRecord(record);

        AnsiConsole.MarkupLine($"[green]Saved[/] Duration: {session.Elapsed:hh\\:mm\\:ss}. Press any key to return.");
        Console.ReadKey(true);
    }

    private static IRenderable? BuidSessionPanel(LiveSession session)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddRow($"[bold]Language:[/] {session.Language}");
        grid.AddRow($"[bold]Started:[/] {session.StartTime:yyyy-MM-dd HH:mm:ss}");
        grid.AddRow($"[bold]Status:[/] {(session.IsPaused ? "[red]Paused[/]" : session.IsFinished ? "[green]Finished[/]" : "[green]Running[/]")}");
        grid.AddRow($"[bold]Elapsed:[/] {session.Elapsed:hh\\:mm\\:ss}");
        if (session.Laps.Count > 0)
        {
            var latest = session.Laps.Last();
            grid.AddRow($"[bold]Laps:[/] {session.Laps.Count} (Latest: {latest:hh\\:mm\\:ss})");
        }

        return new Panel(grid).Header("Live Coding Session");

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
            // Prefer stored Duration if it looks valid; otherwise compute.
            var duration = codingRecord.Duration != default
                ? codingRecord.Duration
                : codingRecord.DateEnd - codingRecord.DateStart;

            var totalHours = (int)duration.TotalHours;
            var minutesRemainder = duration.Minutes; // already remainder after hours
            var formattedDuration = $"{totalHours} hours {minutesRemainder} minutes";

            table.AddRow(
                codingRecord.Id.ToString(),
                codingRecord.Language ?? string.Empty,
                codingRecord.DateStart.ToString("dd/MM/yyyy HH:mm:ss"),
                codingRecord.DateEnd.ToString("dd/MM/yyyy HH:mm:ss"),
                formattedDuration
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
