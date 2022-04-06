using System.Text.Json.Serialization;

namespace PublishSystem.Integration.Rendering.AzureBatch.Events
{
    public class Constraints
    {
        [JsonPropertyName("maxTaskRetryCount")]
        public int MaxTaskRetryCount { get; set; }
    }
}
