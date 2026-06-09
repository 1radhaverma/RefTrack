namespace RefTrack.Domain.Enums;
public enum ApplicationStatus
{
    Applied,      // submitted application
    HRScreen,     // HR called
    TechRound1,   // first tech interview
    TechRound2,   // second tech interview
    Final,        // final round
    Offered,      // GOT THE JOB ??
    Rejected      // not this time
}