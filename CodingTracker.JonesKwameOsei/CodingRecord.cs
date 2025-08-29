namespace CodingTracker.JonesKwameOsei;

internal class CodingRecord
{
    internal int Id { get; set; }
    internal string Language { get; set; } = "C#";
    internal DateTime DateStart { get; set; }
    internal DateTime DateEnd { get; set; }
    internal TimeSpan Duration { get; set; }
}
