// RefTrack.Application/Scorers/KeywordMatchScorer.cs
namespace RefTrack.Application.Scorers;

// OCP — concrete strategy 1
// Implements IAtsScorer — can be swapped at runtime via Factory
public class KeywordMatchScorer : IAtsScorer
{
    // Common English words that add no value to matching
    private static readonly HashSet<string> StopWords = new(
        StringComparer.OrdinalIgnoreCase)
    {
        "the","a","an","is","in","of","and","to","for",
        "with","that","this","are","be","as","at","by",
        "we","you","our","your","will","have","has","from",
        "or","on","it","not","they","their","which","can"
    };

    public AtsResult Score(string resume, string jobDescription)
    {
        // Step 1: tokenize both texts into meaningful keywords
        var jdKeywords = Tokenize(jobDescription);
        var resumeKeywords = Tokenize(resume);

        // Step 2: find matches and misses
        var matched = jdKeywords
            .Intersect(resumeKeywords, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var missing = jdKeywords
            .Except(resumeKeywords, StringComparer.OrdinalIgnoreCase)
            .Take(20)  // show top 20 missing keywords
            .OrderBy(k => k)
            .ToList();

        // Step 3: calculate score
        int matchPercent = jdKeywords.Count == 0 ? 0
            : (int)((double)matched.Count / jdKeywords.Count * 100);

        int score = Math.Min(100, matchPercent);

        string summary = score >= 80
            ? $"Strong match! {matched.Count}/{jdKeywords.Count} keywords found."
            : score >= 60
            ? $"Decent match. Add {missing.Count} more keywords to improve."
            : $"Weak match. Resume needs significant work for this role.";

        return new AtsResult(
            score, matchPercent, missing, matched, summary);
    }

    // Break text into unique meaningful words
    private static HashSet<string> Tokenize(string text)
        => text.ToLower()
               .Split(new char[]
               {
                   ' ','\n','\r',',','.',':',';',
                   '(',')','/','\\','-','|','+'
               },
               StringSplitOptions.RemoveEmptyEntries)
               .Where(w => w.Length > 2        // skip tiny words
                        && !StopWords.Contains(w)  // skip stop words
                        && !int.TryParse(w, out _)) // skip numbers
               .ToHashSet(StringComparer.OrdinalIgnoreCase);
}