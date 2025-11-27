namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence.Repositories;

public class GetTests
{

    [Fact]
    public async Task Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        using var context = new ToDoItemsContext(connectionString);
        await context.Database.MigrateAsync();


        var toDoItem1 = new ToDoItem

        {
            ToDoItemId = 1,
            Name = "Alena",
            Description = "koš",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Petr",
            Description = "odpadky",
            IsCompleted = true
        };

        context.ToDoItems.AddRange(toDoItem1, toDoItem2);
        await context.SaveChangesAsync();

        // controller.AddItemToStorage(toDoItem1);
        //controller.AddItemToStorage(toDoItem2);

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(context: context, repository: repository);


        // Act
        var result = await controller.Read();
        IEnumerable<ToDoItemGetResponseDto> value = result.Result is OkObjectResult ok ? Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(ok.Value) : Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(result.Value);
        // var value = result.GetValue(); //přidat v extension

        // Assert
        Assert.NotNull(value);
        // Assert.Collection();

        var firstToDo = value.First();
        Assert.Equal(toDoItem1.ToDoItemId, firstToDo.ToDoItemId);
        // nebo napsat manuálně, což není závislé na změnách dat v aplikaci
        Assert.Equal(1, firstToDo.ToDoItemId);
        Assert.Equal(toDoItem1.Name, firstToDo.Name);
        Assert.Equal(toDoItem1.Description, firstToDo.Description);
        Assert.Equal(toDoItem1.IsCompleted, firstToDo.IsCompleted);


        await context.Database.EnsureDeletedAsync();


    }

    //   [Fact] namísto fact můžu použít theory a vložit inline data
    [Theory]
    [InlineData(10, "Jana", "okna", true)]
    [InlineData(20, "Gabriel", "koupit květiny", false)]
    public async Task Get_ItemById_ReturnsItemById(int id, string name, string description, bool isCompleted)
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();


        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(context: context, repository: repository);

        controller.AddItemToStorage(new ToDoItem { ToDoItemId = id, Name = name, Description = description, IsCompleted = isCompleted });
        await context.SaveChangesAsync();

        // Act
        ActionResult<ToDoItemGetResponseDto> result = await controller.ReadById(id);

        // Assert
        // var action = Assert.IsType<ActionResult<ToDoItemGetResponseDto>>(result);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);

        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal(name, item.Name);
        Assert.Equal(isCompleted, item.IsCompleted);
        Assert.Equal(description, item.Description);
        Assert.IsType<bool>(item.IsCompleted);

        // Negative result
        var notFound = await controller.ReadById(777);
        Assert.IsType<NotFoundResult>(notFound.Result);

        await context.Database.EnsureDeletedAsync();

    }
}
