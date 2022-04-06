namespace PublishSystem.Integration.Messaging.Extensions
{
    /// <summary>
    /// Implementation of EntityPathAttribute class
    /// </summary>
    public class EntityPathAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="EntityPathAttribute"/>
        /// </summary>
        /// <param name="eventType"></param>
        public EntityPathAttribute(string eventType) : base()
        {
            this.EntityPath = eventType;
        }

        public string EntityPath { get; }
    }
}
