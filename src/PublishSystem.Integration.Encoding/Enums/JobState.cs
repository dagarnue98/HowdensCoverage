namespace PublishSystem.Integration.Encoding.Enums
{
    public enum JobState
    {   // Change numbers to align with state machine triggers 
        Canceled = 0,
        Canceling = 1,
        Error = 2,
        Finished = 3,
        Processing = 4,
        Queued = 5,
        Scheduled = 6
    }
}
