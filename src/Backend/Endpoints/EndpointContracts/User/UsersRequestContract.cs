using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints.EndpointContracts.User;

public class UsersRequestContract
{
    [FromQuery]
    public string? Username { get; set; }
    
    [FromQuery]
    public string? Id { get; set; }
}