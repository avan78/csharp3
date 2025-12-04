namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence.Repositories;

public class CreateTests
{
    [Theory]
    [InlineData("voňavý koutek", "koupit koření", false, "kuchyně")]
    [InlineData("univerzita", "udělat úkol", true, "bakalář")]
    public async Task Create_Item_ReturnsToDoItem(string name, string description, bool isCompleted, string? category)
    {
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        // Arrange
        using var context = new ToDoItemsContext(connectionString);
        await context.Database.MigrateAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(context: context, repository: repository);
        var request = new ToDoItemCreateRequestDto(name, description, isCompleted, category);

        // Act
        var result = await controller.Create(request);

        // Assert
        var newTodoResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var newTodoValue = Assert.IsType<ToDoItem>(newTodoResult.Value);
        Assert.Equal(request.Name, newTodoValue.Name);

        // cleanup
        var killToDo = context.ToDoItems.Find(newTodoValue.ToDoItemId);

        if (killToDo != null)
        {
            context.ToDoItems.Remove(killToDo);
        }
        await context.SaveChangesAsync();



    }

}
