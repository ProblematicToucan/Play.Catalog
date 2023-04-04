using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers;

[ApiController, Route("Controller")]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;

    public ItemController(ILogger<ItemController> logger)
    {
        _logger = logger;
    }

    private static readonly List<ItemDto> items = new()
    {
        new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of HP", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
    };

    [HttpGet("/api/item"), Produces("Application/json")]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<ItemDto>> GetAllItem()
    {
        return Ok(items);
    }

    [HttpGet("/api/item/{id}"), Produces("Application/json")]
    [ProducesResponseType(200), ProducesResponseType(404)]
    public ActionResult<ItemDto> GetItemById(Guid id)
    {
        var result = items.SingleOrDefault(item => item.id == id);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("/api/item"), Produces("Application/json")]
    [ProducesResponseType(typeof(ItemDto), 201), ProducesResponseType(400)]
    public ActionResult AddItem(CreateItemDto newItem)
    {
        var item = new ItemDto(Guid.NewGuid(), newItem.name, newItem.desc, newItem.price, DateTimeOffset.UtcNow);
        items.Add(item);
        return CreatedAtAction(nameof(GetItemById), new { id = item.id }, item);
    }

    [HttpPut("/api/item/{id}"), Produces("Application/json")]
    [ProducesResponseType(typeof(ItemDto), 200), ProducesResponseType(400), ProducesResponseType(404)]
    public ActionResult<ItemDto> PutItem(Guid id, UpdateItemDto newItem)
    {
        var indexOf = items.FindIndex(item => item.id == id);
        if (indexOf < 0) return NotFound();
        var existingItem = items[indexOf];
        var updatedItem = new ItemDto(existingItem.id, newItem.name, newItem.desc, newItem.price, existingItem.createdTime);
        items[indexOf] = updatedItem;
        return Ok(existingItem);
    }

    [HttpDelete("/api/item/{id}"), Produces("Application/json")]
    [ProducesResponseType(typeof(ItemDto), 204), ProducesResponseType(404)]
    public ActionResult DeleteItem(Guid id)
    {
        var existingItem = items.FirstOrDefault(x => x.id == id);
        if (existingItem == null) return NotFound();
        items.Remove(existingItem);
        return NoContent();
    }

}