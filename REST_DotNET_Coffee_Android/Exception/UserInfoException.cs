using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;

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

    public InvalidRequest(int Code) : base($"Invalid client request. Code: {Code}") { }

    public InvalidRequest(string Message, Exception innerException) : base(Message, innerException) { }

}

public class AlreadyExisted : UserInfoException
{

    public AlreadyExisted() : base("User already registered client request.") { }

    public AlreadyExisted(string Email, int Code) : base($"Given email {Email} already in used. Code: {Code}") { }

    public AlreadyExisted(string Message, Exception innerException) : base(Message, innerException) { }

}