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
    public void Delete_ItemById(int id)
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        using var context = new ToDoItemsContext(connectionString);
        context.Database.EnsureDeleted();
        context.Database.Migrate();
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
        var result = controller.DeleteById(id);
        var exist = controller.ReadById(id);

        // Assert
        // var noContent = Assert.IsType<ObjectResult>(result);
        Assert.IsType<NotFoundResult>(exist.Result);
        // Assert.IsType<NoContentResult>(result);


        context.Database.EnsureDeleted();



    }
    [Fact]
    public void Delete_ById_Return404()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.EnsureDeleted();
        context.Database.Migrate();
        var repository = new ToDoItemsRepository(context);

        try
        {
            var controller = new ToDoItemsController(context: context, repository: repository);
            // Act
            var result = controller.DeleteById(200);
            // Assert
            var notFound = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

}
