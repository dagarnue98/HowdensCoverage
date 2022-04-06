using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PublishSystem.Core.Common.Serializations.Json
{
    public class TextJsonSerializer : IJsonSerializer
    {
        private readonly ILogger<TextJsonSerializer> _logger;
        private readonly JsonSerializerOptions _settings;

        public TextJsonSerializer(
            ILogger<TextJsonSerializer> logger,
            JsonSerializerOptions settings)
        {
            _logger = logger;
            _settings = settings ?? new JsonSerializerOptions();
        }

        public JsonSerializerOptions DefaultSettings { get => _settings; }

        public async Task<string> SerializeAsync<TSource>(TSource source) => await SerializeAsync(source, _settings);

        public async Task<string> SerializeAsync<TSource>(TSource source, JsonSerializerOptions? settings = null)
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, source, source.GetType(), settings ?? _settings);
                var array = stream.ToArray();
                return Encoding.UTF8.GetString(array);
            }
        }

        public async Task<TDestination> DeserializeAsync<TDestination>(string source) => await DeserializeAsync<TDestination>(source, _settings);

        public async Task<TDestination> DeserializeAsync<TDestination>(string source, JsonSerializerOptions? settings = null)
        {
            var buffer = Encoding.UTF8.GetBytes(source);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync<TDestination?>(stream, settings ?? _settings);
                return obj!;
            }
        }

        public async Task<object> DeserializeAsync(string source, Type type) => await DeserializeAsync(source, type, _settings);

        public async Task<object> DeserializeAsync(string source, Type type, JsonSerializerOptions? settings = null)
        {
            var buffer = Encoding.UTF8.GetBytes(source);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync(stream, type, settings ?? _settings);
                return obj!;
            }
        }

        public TDestination Deserialize<TDestination>(ReadOnlySpan<byte> source) => Deserialize<TDestination>(source, _settings);

        public TDestination Deserialize<TDestination>(ReadOnlySpan<byte> source, JsonSerializerOptions? settings = null) => JsonSerializer.Deserialize<TDestination>(source, settings ?? _settings);
    }
}
