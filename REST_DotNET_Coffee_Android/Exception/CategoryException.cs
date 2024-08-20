public class CategoryException : Exception
{

    public CategoryException() : base("Input category was not in a correct format.") { }

    public CategoryException(string Message) : base(Message) { }

    public CategoryException(string Message, Exception innerException) : base(Message, innerException) { }

}