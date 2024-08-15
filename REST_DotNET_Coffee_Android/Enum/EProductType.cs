public enum EProductType
{
    DRINK, FOOD, FRUIT
}

public static class ProductTypeExtension
{
    public static EProductType ToType(string productType)
    {
        return productType switch
        {
            "DRINK" => EProductType.DRINK,
            "FOOD" => EProductType.FOOD,
            "FRUIT" => EProductType.FRUIT,
            _ => EProductType.FRUIT
        };
    }

    public static string ToString(this EProductType productType)
    {
        return productType switch
        {
            EProductType.DRINK => "DRINK",
            EProductType.FOOD => "FOOD",
            EProductType.FRUIT => "FRUIT",
            _ => productType.ToString()
        };
    }
}