using Microsoft.AspNetCore.Mvc;

namespace REST_DotNET_Coffee_Android.Controllers
{
    [ApiController]
    [Route("v1/")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // GET: Get all ingredients
        [HttpGet("ingredient/")]
        public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredients()
        {
            return Ok(await _ingredientService.GetIngredients());
        }

        // GET: Get ingredients
        [HttpGet("ingredient/{id}")]
        public async Task<ActionResult<IngredientRespondeDTO>> GetIngredientById(int id)
        {
            return Ok(await _ingredientService.GetIngredientById(id));
        }

        // GET: Get ingredients by type
        [HttpGet("ingredient/type")]
        public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredientByType(EIngredientType type)
        {
            return Ok(await _ingredientService.GetIngredientByType(type));
        }
    }
}
