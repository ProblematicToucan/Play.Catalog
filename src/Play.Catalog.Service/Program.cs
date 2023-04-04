using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;
using Play.Catalog.Service.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get all items
app.MapGet("/api/item", () =>
{
    return DataItems.items;
}).WithName("GetAllItem").Produces<IEnumerable<ItemDto>>(200);

app.MapGet("/api/item/{id:Guid}", (Guid id) =>
{
    var result = DataItems.items.FirstOrDefault(item => item.id == id);
    return result != null ? Results.Ok(result) :  Results.NotFound();
}).WithName("GetItem").Produces<ItemDto>(200).Produces(404);

app.MapPost("/api/item", ([FromBody] CreateItemDto itemDto) =>
{
    var newItem = new ItemDto(Guid.NewGuid(), itemDto.name, itemDto.desc, itemDto.price, DateTimeOffset.UtcNow);
    DataItems.items.Add(newItem);
    return Results.Created($"/api/item/{newItem.id}", newItem);
}).WithName("CreateItem").Accepts<CreateItemDto>("application/json").Produces<ItemDto>(201).Produces(400);

app.MapPut("/api/item/{id:Guid}", (Guid id, [FromBody] UpdateItemDto itemDto) =>
{
    var indexOf = DataItems.items.FindIndex(item => item.id == id);
    if (indexOf < 0) return Results.BadRequest();
    var existingItem = DataItems.items[indexOf];
    var dummy = new ItemDto(existingItem.id, itemDto.name, itemDto.desc, itemDto.price, existingItem.createdTime);
    DataItems.items[indexOf] = dummy;
    return Results.StatusCode(201);
}).WithName("UpdateItem").Accepts<UpdateItemDto>("application/json").Produces<ItemDto>(201).Produces(400);

app.MapDelete("/api/item", (Guid id) =>
{
    var indexOf = DataItems.items.FindIndex(item => item.id == id);
    if (indexOf < 0) return Results.BadRequest();
    DataItems.items.RemoveAt(indexOf);
    return Results.StatusCode(201);
}).WithName("DeleteItem").Produces(201).Produces(400);

app.Run();
