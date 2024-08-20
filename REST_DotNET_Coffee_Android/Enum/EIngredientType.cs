public enum EIngredientType
{
    MILKS, SWEETENERS, TOPPINGS
}

public static class IngredientTypeExtension
{
    public static EIngredientType ToType(string productType)
    {
        return productType switch
        {
            "DRINK" => EIngredientType.MILKS,
            "FOOD" => EIngredientType.SWEETENERS,
            "FRUIT" => EIngredientType.TOPPINGS,
            _ => EIngredientType.TOPPINGS
        };
    }

    public static string ToString(this EIngredientType productType)
    {
        return productType switch
        {
            EIngredientType.MILKS => "MILKS",
            EIngredientType.SWEETENERS => "SWEETENERS",
            EIngredientType.TOPPINGS => "TOPPINGS",
            _ => productType.ToString()
        };
    }
}