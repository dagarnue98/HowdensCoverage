using Microsoft.Extensions.Logging;

namespace PublishSystem.Integration.Messaging.Extensions
{
    public static class EventExtensions
    {
        public static string GetEntityPath<TEvent>(ILogger logger) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            return GetEntityPath(type, logger);
        }

        public static string GetEntityPath<TEvent>(this TEvent @event, ILogger logger) where TEvent : IEvent
        {
            var type = @event.GetType();
            return GetEntityPath(type, logger);
        }

        private static string GetEntityPath(Type type, ILogger logger)
        {
            var attribute = type.GetCustomAttributes(typeof(EntityPathAttribute), false).FirstOrDefault() as EntityPathAttribute;
            if (attribute is null)
            {
                logger.LogDebug($"No {nameof(EntityPathAttribute)} found for type {type.Name}");
            }

            return attribute != null ? attribute.EntityPath : type.Name;
        }
    }
}
