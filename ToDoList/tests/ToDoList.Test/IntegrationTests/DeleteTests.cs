using System;


namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

public class DeleteTests
{
    [Theory]
    [InlineData(1)]
    public void Delete_ItemById(int id)
    {
        // Arrange
        var connectionString = "Data Source=/../data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();
        try
        {
            var controller = new ToDoItemsController(context: context, repository: null);

            var task = new ToDoItem
            {
                ToDoItemId = 1,
                Name = "FJ",
                Description = "úkol do francouzštiny",
                IsCompleted = true
            };

            controller.AddItemToStorage(task);
            // Act
            var result = controller.DeleteById(id);
            var exist = controller.ReadById(id);

            // Assert
            var noContent = Assert.IsType<ObjectResult>(result);
            Assert.IsType<NotFoundResult>(exist);
            Assert.Equal(204, noContent.StatusCode);
        }

        finally
        {
            context.Database.EnsureDeleted();
        }


    }
    [Fact]
    public void Delete_ById_Return404()
    {
        // Arrange
        var connectionString = "Data Source=/../data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();
        try
        {
            var controller = new ToDoItemsController(context: context, repository: null);
            // Act
            var result = controller.DeleteById(200);
            // Assert
            var notFound = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

}
