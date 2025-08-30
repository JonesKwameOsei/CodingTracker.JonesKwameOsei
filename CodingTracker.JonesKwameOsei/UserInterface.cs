using Spectre.Console;
using Spectre.Console.Rendering;
using System.Globalization;
using static CodingTracker.JonesKwameOsei.Enums;

namespace CodingTracker.JonesKwameOsei;

internal class UserInterface
{
    /*
    Pseudocode:
    - Keep existing menu loop.
    - Modify Quit case:
        - Clear screen (optional) or just print farewell.
        - Show goodbye message using AnsiConsole for consistency.
        - Prompt user to press any key to exit so window does not close immediately.
        - After key press set isMenuRunning = false to break loop and end program gracefully.
    - No other behavioral changes.
    */

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
                        MainMenuOptions.ViewAllCodingSessions,
                        MainMenuOptions.FilterCodingSessions,
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
                case MainMenuOptions.ViewAllCodingSessions:
                    var dataAccess = new DataAccess();
                    var codingRecords = dataAccess.GetAllSessions();
                    ViewAllCodingSessions(codingRecords);
                    break;
                case MainMenuOptions.FilterCodingSessions:
                    ViewCodingSessionsWithFilters();
                    break;
                case MainMenuOptions.UpdateCodingSession:
                    UpdateCodingSession();
                    break;
                case MainMenuOptions.DeleteCodingSession:
                    DeleteCodingSession();
                    break;
                case MainMenuOptions.Quit:
                    AnsiConsole.MarkupLine("[yellow]Goodbye![/]");
                    AnsiConsole.MarkupLine("[grey]Press any key to close the application...[/]");
                    Console.ReadKey(true);
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

    private static void ViewAllCodingSessions(IEnumerable<CodingRecord> codingRecords)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Language");
        table.AddColumn("Date Start");
        table.AddColumn("Date End");
        table.AddColumn("Duration (Hours)");

        foreach (var codingRecord in codingRecords)
        {
            var duration = codingRecord.Duration != default
                ? codingRecord.Duration
                : codingRecord.DateEnd - codingRecord.DateStart;

            var totalHours = (int)duration.TotalHours;
            var minutesRemainder = duration.Minutes;
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
        AnsiConsole.MarkupLine("\n[grey]Press any key to return to the menu...[/]");
        Console.ReadKey(true);
    }

    private static void ViewCodingSessionsWithFilters()
    {
        Console.Clear();
        var period = AnsiConsole.Prompt(
            new SelectionPrompt<PeriodFilterOption>()
                .Title("Select a period to filter by:")
                .AddChoices(Enum.GetValues<PeriodFilterOption>())
            );

        DateTime? from = null;
        DateTime? to = null;

        var now = DateTime.Now;
        var todayStart = now.Date;
        var culture = CultureInfo.CurrentCulture;
        DateTime StartOfWeek(DateTime dt) =>
            dt.Date.AddDays(-(int)culture.DateTimeFormat.FirstDayOfWeek + (int)dt.DayOfWeek >= 7
                ? -(int)dt.DayOfWeek
                : -(int)dt.DayOfWeek + (int)culture.DateTimeFormat.FirstDayOfWeek);

        switch (period)
        {
            case PeriodFilterOption.All:
                break;
            case PeriodFilterOption.Today:
                from = todayStart;
                to = todayStart.AddDays(1).AddTicks(-1);
                break;
            case PeriodFilterOption.Yesterday:
                from = todayStart.AddDays(-1);
                to = todayStart.AddTicks(-1);
                break;
            case PeriodFilterOption.Last7Days:
                from = todayStart.AddDays(-6);
                to = todayStart.AddDays(1).AddTicks(-1);
                break;
            case PeriodFilterOption.ThisWeek:
                from = StartOfWeek(now);
                to = from.Value.AddDays(7).AddTicks(-1);
                break;
            case PeriodFilterOption.LastWeek:
                var thisWeekStart = StartOfWeek(now);
                from = thisWeekStart.AddDays(-7);
                to = thisWeekStart.AddTicks(-1);
                break;
            case PeriodFilterOption.ThisMonth:
                from = new DateTime(now.Year, now.Month, 1);
                to = from.Value.AddMonths(1).AddTicks(-1);
                break;
            case PeriodFilterOption.LastMonth:
                var thisMonthStart = new DateTime(now.Year, now.Month, 1);
                from = thisMonthStart.AddMonths(-1);
                to = thisMonthStart.AddTicks(-1);
                break;
            case PeriodFilterOption.ThisYear:
                from = new DateTime(now.Year, 1, 1);
                to = from.Value.AddYears(1).AddTicks(-1);
                break;
            case PeriodFilterOption.LastYear:
                from = new DateTime(now.Year - 1, 1, 1);
                to = from.Value.AddYears(1).AddTicks(-1);
                break;
            case PeriodFilterOption.CustomRange:
                var startStr = AnsiConsole.Ask<string>("Enter start date (yyyy-MM-dd):");
                var endStr = AnsiConsole.Ask<string>("Enter end date inclusive (yyyy-MM-dd):");
                if (!DateTime.TryParseExact(startStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var customStart) ||
                    !DateTime.TryParseExact(endStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var customEnd))
                {
                    AnsiConsole.MarkupLine("[red]Invalid custom dates. Returning.[/]");
                    Console.ReadKey();
                    return;
                }
                if (customEnd < customStart)
                {
                    AnsiConsole.MarkupLine("[red]End before start. Returning.[/]");
                    Console.ReadKey();
                    return;
                }
                from = customStart.Date;
                to = customEnd.Date.AddDays(1);
                break;
        }

        var descending = AnsiConsole.Confirm("Order by most recent first?");

        var all = new DataAccess().GetAllSessions();

        var filtered = all.Where(r =>
        {
            if (from is not null && r.DateStart < from) return false;
            if (to is not null && r.DateStart >= to) return false;
            return true;
        });

        filtered = descending
            ? filtered.OrderByDescending(r => r.DateStart)
            : filtered.OrderBy(r => r.DateStart);

        ViewAllCodingSessions(filtered);

        var totalSessions = filtered.Count();
        var totalTime = filtered.Aggregate(TimeSpan.Zero,
            (acc, r) => acc + (r.Duration != default ? r.Duration : (r.DateEnd - r.DateStart)));

        AnsiConsole.MarkupLine($"\n[bold]Summary:[/]");
        AnsiConsole.MarkupLine($"Sessions: [yellow]{totalSessions}[/]");
        AnsiConsole.MarkupLine($"Total Time: [green]{(int)totalTime.TotalHours}h {totalTime.Minutes}m[/]");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void UpdateCodingSession()
    {
        Console.Clear();

        var dataAccess = new DataAccess();
        var codingRecords = dataAccess.GetAllSessions();
        ViewAllCodingSessions(codingRecords);

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
        ViewAllCodingSessions(codingRecords);

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
