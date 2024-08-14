using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable disable

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Type { get; set; }

    [JsonConverter(typeof(PriceConverter))]
    public double BasePrice { get; set; }

    public int Quantities { get; set; }

    public bool Active { get; set; }

    public int CategoryId { get; set; }

    public string AvatarUrl { get; set; }

}

public class PriceConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (double.TryParse(reader.GetString(), out double value))
            { return value; }
            else { return double.NaN; }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }
        return double.NaN;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}