using System;
using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Test;
using Microsoft.AspNetCore.Http;


namespace ToDoList.Test.UnitTests;

public class GetByIdTestsUnit
{
    [Fact]
    public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var item = new ToDoItem { ToDoItemId = 7, Name = "test", Description = "desc", IsCompleted = false };
        repository.ReadById(7).Returns(item);

        // Act
        var result = controller.ReadById(7);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);
        Assert.Equal(7, dto.ToDoItemId);
        Assert.Equal("test", dto.Name);
        Assert.Equal("desc", dto.Description);
        Assert.False(dto.IsCompleted);

        repository.Received(1).ReadById(7);
    }
    [Fact]
    public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        repository.ReadById(999).Returns((ToDoItem?)null);

        // Act
        var result = controller.ReadById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        repository.Received(1).ReadById(999);
    }

    [Fact]
    public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repo = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repo);

        repo.ReadById(42).Returns(_ => { throw new Exception("DB down"); });

        // Act
        var result = controller.ReadById(42);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
        repo.Received(1).ReadById(42);
    }
}
