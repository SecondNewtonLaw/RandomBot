namespace Reddit.PostDownloader.Exceptions;

public class NoPostsException : System.Exception
{
    public NoPostsException() { }
    public NoPostsException(string message) : base(message) { }
    public NoPostsException(string message, System.Exception inner) : base(message, inner) { }
    public NoPostsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}