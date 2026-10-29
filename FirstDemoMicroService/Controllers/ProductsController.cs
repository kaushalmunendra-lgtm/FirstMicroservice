using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private static readonly Product[] Products =
    [
        new(1, "Keyboard", "Mechanical keyboard", 79.99m),
        new(2, "Mouse", "Wireless optical mouse", 29.99m),
        new(3, "Monitor", "27-inch 4K monitor", 349.99m)
    ];

    [HttpGet]
    [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetProducts() => Ok(Products);

    [HttpGet("{id:int}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = Array.Find(Products, product => product.Id == id);
        return product is null ? NotFound() : Ok(product);
    }
}
