using Microsoft.AspNetCore.Mvc;

public interface IProductService : IInitializerData
{
    public Task<ActionResult<List<ProductRespondeDTO>>> GetAllProduct();

    public Task<ActionResult<ProductRespondeDTO>> GetProductById(int id);

    public Task<ActionResult<List<ProductRespondeDTO>>> GetProductWithType(EProductType type);

    public Task<ActionResult<List<ProductRespondeDTO>>> GetProductByCategory(string category);

    public Task<ActionResult<List<ProductRespondeDTO>>> AddProduct(ProductRequestDTO request);

    public Task<ActionResult<List<ProductRespondeDTO>>> UpdateProduct(ProductRequestDTO request);

    public Task<ActionResult<List<ProductRespondeDTO>>> DeleteProduct(int id);

    // Future service here
}