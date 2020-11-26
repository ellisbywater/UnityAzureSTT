using System;

namespace Reader
{
    public static class SttResult
    {
        public static string Message { get; set; }
        public static bool IsRecording { get; set; } = false;
        public static Reason Reason { get; set; } = Reason.Canceled;
        public static DateTime TimeStamp { get; set; } = DateTime.Now;
    }

    public enum Reason
    {
        Recognized,
        NoMatch,
        Canceled
    }
    
}