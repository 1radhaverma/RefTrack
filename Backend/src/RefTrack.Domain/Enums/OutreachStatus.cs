namespace RefTrack.Domain.Enums;
public enum OutreachStatus
{
    NotContacted,  // just added
    Sent,          // message sent on LinkedIn
    Seen,          // they saw it
    Replied,       // they replied
    Referred,      // they submitted referral ✅
    Ghosted        // 5 days no reply
}