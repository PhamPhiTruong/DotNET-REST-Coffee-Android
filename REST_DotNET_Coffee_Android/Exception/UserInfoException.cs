public class UserInfoException : Exception
{

    public UserInfoException() : base("User not found.") { }

    public UserInfoException(string Message) : base(Message) { }

    public UserInfoException(string Message, Exception innerException) : base(Message, innerException) { }

}

public class InvalidRequest : UserInfoException
{

    public InvalidRequest() : base("Invalid client request.") { }

    public InvalidRequest(string Message) : base(Message) { }

    public InvalidRequest(string Message, Exception innerException) : base(Message, innerException) { }

}