public class ProductException : Exception
{

    public ProductException() : base("Product not found.") { }

    public ProductException(string Message) : base(Message) { }

    public ProductException(string Message, Exception innerException) : base(Message, innerException) { }

}

public class ProductAlreadyExistedException : ProductException
{

    public ProductAlreadyExistedException() : base("Can not add product to database, given product already existed in database.") { }

    public ProductAlreadyExistedException(string Message) : base(Message) { }

    public ProductAlreadyExistedException(string Message, Exception innerException) : base(Message, innerException) { }

}

public class ProductNullException : ProductException
{

    public ProductNullException() : base("Given product is not supposed to be null.") { }

    public ProductNullException(string Message) : base(Message) { }

    public ProductNullException(string Message, Exception innerException) : base(Message, innerException) { }

}