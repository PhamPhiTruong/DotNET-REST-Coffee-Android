public class InvalidIdException : Exception
{

    public InvalidIdException() : base("Input id was not in a correct format.") { }

    public InvalidIdException(string Message) : base(Message) { }

    public InvalidIdException(string Message, Exception innerException) : base(Message, innerException) { }

}