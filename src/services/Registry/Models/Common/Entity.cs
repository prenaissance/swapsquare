namespace SwapSquare.Registry.Api.Models.Common;

// int or guid id
public class Entity<TId> where TId : struct
{
    public TId Id { get; set; }
}