// RefTrack.Application/Scorers/AtsScorerFactory.cs
namespace RefTrack.Application.Scorers;

// FACTORY PATTERN — decouples scorer creation from usage
// OCP — add new scorer = add one line here + new class
// Handler never knows which scorer it gets
public static class AtsScorerFactory
{
    private static readonly Dictionary<string, Func<IAtsScorer>>
        Scorers = new(StringComparer.OrdinalIgnoreCase)
        {
            ["keyword"] = () => new KeywordMatchScorer(),
            ["title"] = () => new TitleMatchScorer(),
            // Add new scorer here — nothing else changes
            // ["semantic"] = () => new SemanticScorer(),
        };

    public static IAtsScorer Create(string type = "keyword")
    {
        if (!Scorers.TryGetValue(type, out var factory))
            throw new ArgumentException(
                $"Unknown scorer type: '{type}'. " +
                $"Valid types: {string.Join(", ", Scorers.Keys)}");

        return factory();  // creates fresh instance each time
    }
}