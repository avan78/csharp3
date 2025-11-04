namespace ToDoList;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using NSubstitute;

public class CreateTests
{
    [Theory]
    [InlineData("voňavý koutek", "koupit koření", false)]
    [InlineData("univerzita", "udělat úkol", true)]
    public void Create_Item_ReturnsToDoItem(string name, string description, bool isCompleted)
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        // Arrange
        var context = new ToDoItemsContext();
        var controller = new ToDoItemsController(context, repositoryMock);
        var request = new ToDoItemCreateRequestDto(name, description, isCompleted);

        // Act
        var result = controller.Create(request);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);


    }

}
