using SwapSquare.Registry.Api.Models.Common;

namespace SwapSquare.Registry.Api.Models;

public class Application : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Icon { get; set; }
}