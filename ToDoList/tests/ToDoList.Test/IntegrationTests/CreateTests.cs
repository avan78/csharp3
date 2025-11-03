namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;

public class CreateTests
{
    [Theory]
    [InlineData("voňavý koutek", "koupit koření", false)]
    [InlineData("univerzita", "udělat úkol", true)]
    public void Create_Item_ReturnsToDoItem(string name, string description, bool isCompleted)
    {
        // Arrange
        var controller = new ToDoItemsController();
        var request = new ToDoItemCreateRequestDto(name, description, isCompleted);

        // Act
        var result = controller.Create(request);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);


    }

}
