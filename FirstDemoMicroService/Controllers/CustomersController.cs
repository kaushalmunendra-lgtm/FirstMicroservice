using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private static readonly Customer[] Customers =
    [
        new(1, "Avery Johnson", "avery@example.com"),
        new(2, "Jordan Lee", "jordan@example.com"),
        new(3, "Taylor Morgan", "taylor@example.com")
    ];

    [HttpGet]
    [ProducesResponseType<IEnumerable<Customer>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Customer>> GetCustomers() => Ok(Customers);

    [HttpGet("{id:int}")]
    [ProducesResponseType<Customer>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Customer> GetCustomer(int id)
    {
        var customer = Array.Find(Customers, customer => customer.Id == id);
        return customer is null ? NotFound() : Ok(customer);
    }
}
