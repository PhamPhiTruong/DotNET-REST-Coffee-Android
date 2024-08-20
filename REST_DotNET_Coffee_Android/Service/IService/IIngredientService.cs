using Microsoft.AspNetCore.Mvc;

public interface IIngredientService : IInitializerData
{
    public Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredients();

    public Task<ActionResult<IngredientRespondeDTO>> GetIngredientById(int id);

    public Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredientByType(EIngredientType type);

    // Future service add here

}
