public class IngredientException : Exception
{

    public IngredientException() : base("Ingredient not found.") { }

    public IngredientException(string Message) : base(Message) { }

    public IngredientException(string Message, Exception innerException) : base(Message, innerException) { }

}

public class IngredientAlreadyExistedException : IngredientException
{

    public IngredientAlreadyExistedException() : base("Can not add ingredient to database, given ingredient already existed in database.") { }

    public IngredientAlreadyExistedException(string Message) : base(Message) { }

    public IngredientAlreadyExistedException(string Message, Exception innerException) : base(Message, innerException) { }

}

public class IngredientNullException : IngredientException
{

    public IngredientNullException() : base("Given ingredient is not supposed to be null.") { }

    public IngredientNullException(string Message) : base(Message) { }

    public IngredientNullException(string Message, Exception innerException) : base(Message, innerException) { }

}