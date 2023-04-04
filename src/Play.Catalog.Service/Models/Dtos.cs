namespace Play.Catalog.Service.Models
{
    public record ItemDto(Guid id, string name, string desc, decimal price, DateTimeOffset createdTime);
    public record CreateItemDto(string name, string desc, decimal price);
    public record UpdateItemDto( string name, string desc, decimal price);
}