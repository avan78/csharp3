namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

public class CreateTests
{
    [Theory]
    [InlineData("voňavý koutek", "koupit koření", false)]
    [InlineData("univerzita", "udělat úkol", true)]
    public void Create_Item_ReturnsToDoItem(string name, string description, bool isCompleted)
    {
        var connectionString = "Data Source=/../data/localDbTestDb.db";
        // Arrange
        using var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();


        try
        {
            var controller = new ToDoItemsController(context: context, repository: null);
            var request = new ToDoItemCreateRequestDto(name, description, isCompleted);

            // Act
            var result = controller.Create(request);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
        // cleanup
        finally
        {
            context.Database.EnsureDeleted();
        }


    }

}
