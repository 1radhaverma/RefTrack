using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTrack.Application.Scorers
{

    // Result record — immutable, clean
    public record AtsResult(
        int Score,               // 0-100
        int MatchPercent,        // keyword match %
        List<string> MissingKeywords, // what to add to resume
        List<string> MatchedKeywords, // what's already there
        string Summary           // human readable message
    );

    public interface IAtsScorer
    {
        AtsResult Score(string resume, string jobDescription);
    }
}
