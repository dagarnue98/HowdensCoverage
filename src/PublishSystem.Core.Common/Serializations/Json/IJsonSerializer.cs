using System.Text.Json;

namespace PublishSystem.Core.Common.Serializations.Json
{
    public interface IJsonSerializer
    {
        JsonSerializerOptions DefaultSettings { get; }

        Task<string> SerializeAsync<TSource>(TSource source);

        Task<string> SerializeAsync<TSource>(TSource source, JsonSerializerOptions? settings = null);

        Task<TDestination> DeserializeAsync<TDestination>(string source);

        Task<TDestination> DeserializeAsync<TDestination>(string source, JsonSerializerOptions? settings = null);

        Task<object> DeserializeAsync(string source, Type type);

        Task<object> DeserializeAsync(string source, Type type, JsonSerializerOptions? settings = null);

        TDestination Deserialize<TDestination>(ReadOnlySpan<byte> source);

        TDestination Deserialize<TDestination>(ReadOnlySpan<byte> source, JsonSerializerOptions? settings = null);
    }
}
