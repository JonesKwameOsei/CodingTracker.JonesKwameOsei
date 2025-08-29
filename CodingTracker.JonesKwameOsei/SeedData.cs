namespace CodingTracker.JonesKwameOsei;

internal static class SeedData
{
    private static readonly string[] DefaultLanguages =
    [
        "C#", "JavaScript", "Python", "Java", "Ruby", "Go", "Swift", "Kotlin", "PHP", "TypeScript"
    ];

    internal static void SeedRecords(int count) =>
        SeedRecords(count, DefaultLanguages);

    internal static void SeedRecords(int count, string language)
    {
        SeedInternal(count, i => language);
    }

    internal static void SeedRecords(int count, IReadOnlyList<string> languages)
    {
        if (languages is null || languages.Count == 0)
            throw new ArgumentException("Languages list cannot be null or empty.", nameof(languages));
        Random random = new Random();
        SeedInternal(count, _ => languages[random.Next(languages.Count)]);
    }

    internal static void SeedRecords(int count, Func<int, string> languages)
    {
        if (languages is null) throw new ArgumentNullException(nameof(languages));
        SeedInternal(count, languages);
    }

    private static void SeedInternal(int count, Func<int, string> languageSelector)
    {
        Random random = new();
        DateTime currentDate = DateTime.Now.Date;
        List<CodingRecord> codingRecords = new(count);

        for (int i = 1; i <= count; i++)
        {
            DateTime startDate = currentDate.AddHours(random.Next(13));
            int addedHours = random.Next(1, 13);
            DateTime endDate = startDate.AddHours(addedHours);

            codingRecords.Add(new CodingRecord
            {
                Language = languageSelector(i),
                DateStart = startDate,
                DateEnd = endDate,
                Duration = endDate - startDate
            });

            currentDate = currentDate.AddDays(1);
        }

        var dataAccess = new DataAccess();
        dataAccess.InsertMultipleRecords(codingRecords);
    }
}
