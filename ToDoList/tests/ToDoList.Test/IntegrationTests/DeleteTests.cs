using System;


namespace ToDoList.Test;

using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence.Repositories;

public class DeleteTests
{
    [Theory]
    [InlineData(1)]
    public async Task Delete_ItemById(int id)
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        using var context = new ToDoItemsContext(connectionString);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        var repository = new ToDoItemsRepository(context);

        var controller = new ToDoItemsController(context: context, repository: repository);

        var task = new ToDoItem
        {
            //   ToDoItemId = 1,
            Name = "FJ",
            Description = "úkol do francouzštiny",
            IsCompleted = true
        };

        controller.AddItemToStorage(task);
        // Act
        var result = await controller.DeleteById(id);
        var exist = await controller.ReadById(id);

        // Assert
        // var noContent = Assert.IsType<ObjectResult>(result);
        Assert.IsType<NotFoundResult>(exist.Result);
        // Assert.IsType<NoContentResult>(result);

        //cleanup
        await context.Database.EnsureDeletedAsync();



    }
    [Fact]
    public async Task Delete_ById_Return404()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        var repository = new ToDoItemsRepository(context);


        var controller = new ToDoItemsController(context: context, repository: repository);
        // Act
        var result = await controller.DeleteById(200);
        // Assert
        var notFound = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFound.StatusCode);

        await context.Database.EnsureDeletedAsync();

    }

}
