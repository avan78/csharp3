namespace ToDoList;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Test;

public class GetTests
{

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var context = new ToDoItemsContext();
        var controller = new ToDoItemsController(context: context, repository: repositoryMock);
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



        // controller.AddItemToStorage(toDoItem1);
        //controller.AddItemToStorage(toDoItem2);



        // Act
        var result = controller.Read();
        var value = result.GetValue<IEnumerable<ToDoItemGetResponseDto>>(); //přidat v extension

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

    }

    //   [Fact] namísto fact můžu použít theory a vložit inline data
    [Theory]
    [InlineData(10, "Jana", "okna", true)]
    [InlineData(20, "Gabriel", "koupit květiny", false)]
    public void Get_ItemById_ReturnsItemById(int id, string name, string description, bool isCompleted)
    {
        var context = new ToDoItemsContext();

        var controller = new ToDoItemsController(context: context, repository: null);
        controller.AddItemToStorage(new ToDoItem { ToDoItemId = id, Name = name, Description = description, IsCompleted = isCompleted });
        var result = controller.ReadById(id);

        //assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var item = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);



        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal(name, item.Name);
        Assert.Equal(isCompleted, item.IsCompleted);
        Assert.Equal(description, item.Description);

        Assert.IsType<bool>(item.IsCompleted);
        // Assert.NotSame(item.ToDoItemId);

        var value = ok.Value;
        //   Assert.Contains(value, item => item.Name == "Alena");

        var badResult = controller.ReadById(33);
        // Assert.IsNotType("string", badResult);

    }

    //něco, co má negativní výsledek.


}
