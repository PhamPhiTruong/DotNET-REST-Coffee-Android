﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

[Table("having_ingredients")]
[PrimaryKey(nameof(ProductId),nameof(IngredientId))]
public class HavingIngredient
{
    [ForeignKey("Product")]
    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; }

    [ForeignKey("Ingredient")]
    [Required]
    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; }
}