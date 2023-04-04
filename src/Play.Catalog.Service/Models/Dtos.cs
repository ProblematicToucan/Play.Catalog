using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service;

public record ItemDto(Guid id, string name, string desc, decimal price, DateTimeOffset createdTime);
public record CreateItemDto( [Required] string name, [Required] string desc, [Range(0, 99999999)] decimal price);
public record UpdateItemDto( [Required] string name, [Required] string desc, [Range(0, 99999999)] decimal price);