// RefTrack.Application/Scorers/TitleMatchScorer.cs
namespace RefTrack.Application.Scorers;

// OCP — concrete strategy 2
// Scores based on job title keywords specifically
public class TitleMatchScorer : IAtsScorer
{
    // Common job title keywords at MNCs
    private static readonly string[] TitleKeywords =
    [
        "developer","engineer","architect","manager",
        "analyst","designer","lead","senior","junior",
        "fullstack","backend","frontend","devops","cloud",
        "software","technical","principal","staff"
    ];

    public AtsResult Score(string resume, string jobDescription)
    {
        var jdTitles = TitleKeywords
            .Where(t => jobDescription.Contains(
                t, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var resumeTitles = TitleKeywords
            .Where(t => resume.Contains(
                t, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var matched = jdTitles
            .Intersect(resumeTitles,
                StringComparer.OrdinalIgnoreCase)
            .ToList();

        var missing = jdTitles
            .Except(resumeTitles,
                StringComparer.OrdinalIgnoreCase)
            .ToList();

        int percent = jdTitles.Count == 0 ? 100
            : (int)((double)matched.Count / jdTitles.Count * 100);

        return new AtsResult(
            percent, percent, missing, matched,
            $"Title match: {percent}% — {matched.Count} of {jdTitles.Count} title keywords found");
    }
}