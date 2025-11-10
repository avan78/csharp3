using System;


namespace ToDoList;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;


public class DeleteTests
{
    [Theory]
    [InlineData(1)]
    public void Delete_ItemById(int id)
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var context = new ToDoItemsContext();
        var controller = new ToDoItemsController(context: context, repository: repositoryMock);

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
        var noContent = Assert.IsType<NotFoundResult>(result);
        Assert.IsType<NoContentResult>(result);



    }
    [Fact]
    public void Delete_ById_Return404()
    {
        // Arrange
        var context = new ToDoItemsContext();
        var controller = new ToDoItemsController(context: context, repository: null);
        // Act
        var result = controller.DeleteById(200);
        // Assert
        var notFound = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

}
