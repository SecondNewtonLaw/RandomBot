namespace Reddit.PostDownloader.Exceptions;

public class InvalidSubredditException : System.Exception
{
    public InvalidSubredditException() {}
    public InvalidSubredditException(string message) : base(message) {}
    public InvalidSubredditException(string message, System.Exception inner) : base(message, inner) {}
    public InvalidSubredditException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}