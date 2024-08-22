using Microsoft.AspNetCore.Mvc;

public interface IProductService : IInitializerData
{
    public Task<List<ProductRespondeDTO>> GetAllProduct();

    public Task<ProductRespondeDTO> GetProductById(int id);

    public Task<List<ProductRespondeDTO>> GetProductWithType(EProductType type);

    public Task<List<ProductRespondeDTO>> GetProductByCategory(string category);

    public Task<List<ProductRespondeDTO>> AddProduct(ProductRequestDTO request);

    public Task<List<ProductRespondeDTO>> UpdateProduct(ProductRequestDTO request);

    public Task<List<ProductRespondeDTO>> DeleteProduct(int id);

    // Future service here
}