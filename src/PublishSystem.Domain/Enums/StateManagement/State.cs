namespace PublishSystem.Domain.Enums.StateManagement
{
    public enum State
    {
        // Request from DesignView received
        Requested,
        // Render
        Render,
        QueuedForRendering,
        Rendering,
        Rendered,
        // Encode
        Encode,
        QueuedForEncoding,
        Encoding,
        Encoded,
        // Publish
        Publish,
        Published,
        EmailSent,
        // Errors
        QueuedForRenderingError,
        SubscribedToRenderingError,
        RenderingError,
        QueuedForEncodingError,
        SubscribedToEncodingError,
        EncodingError,
        PublishedError,
        EmailError,
        Error
    }
}
