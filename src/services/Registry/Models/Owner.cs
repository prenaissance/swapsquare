using SwapSquare.Registry.Api.Models.Common;

namespace SwapSquare.Registry.Api.Models;

public class Owner : Entity<Guid>
{
    public string Username { get; set; } = null!;
    public List<Application> Applications { get; set; } = null!;
}