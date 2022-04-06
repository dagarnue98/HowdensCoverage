namespace PublishSystem.Core.Common.Options.LogsOptions
{
    public class LogsOptions
    {
        public const string Section = "Logs";

        public string DBLogTable { get; set; }
        public string LogFormat { get; set; }
    }
}
