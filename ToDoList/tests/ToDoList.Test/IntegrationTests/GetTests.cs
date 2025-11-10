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
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        using var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();


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
        context.SaveChanges();

        // controller.AddItemToStorage(toDoItem1);
        //controller.AddItemToStorage(toDoItem2);

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(context: context, repository: repository);


        // Act
        var result = controller.Read();
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


        context.Database.EnsureDeleted();


    }

    //   [Fact] namísto fact můžu použít theory a vložit inline data
    [Theory]
    [InlineData(10, "Jana", "okna", true)]
    [InlineData(20, "Gabriel", "koupit květiny", false)]
    public void Get_ItemById_ReturnsItemById(int id, string name, string description, bool isCompleted)
    {
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();
        var repository = new ToDoItemsRepository(context);



        var controller = new ToDoItemsController(context: context, repository: repository);
        controller.AddItemToStorage(new ToDoItem { ToDoItemId = id, Name = name, Description = description, IsCompleted = isCompleted });

        // act
        ActionResult<ToDoItemGetResponseDto> result = controller.ReadById(20);

        //assert
        // var action = Assert.IsType<ActionResult<ToDoItemGetResponseDto>>(result);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);



        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal(name, item.Name);
        Assert.Equal(isCompleted, item.IsCompleted);
        Assert.Equal(description, item.Description);

        Assert.IsType<bool>(item.IsCompleted);
        // Assert.NotSame(item.ToDoItemId);

        object value = ok.Value;
        //   Assert.Contains(value, item => item.Name == "Alena");

        var badResult = controller.ReadById(33);
        // Assert.IsNotType("string", badResult);



        context.Database.EnsureDeleted();

    }

    //něco, co má negativní výsledek.


}
