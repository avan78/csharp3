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

public class UpdateTestsUnit
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsOk()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 7;
        var todo = new ToDoItemUpdateRequestDto("koupit květiny", "růže", true, "svátek");

        var updated = new ToDoItem { ToDoItemId = id, Name = todo.Name, Description = todo.Description, IsCompleted = todo.IsCompleted, Category = todo.Category };

        repository.UpdateByIdAsync(Arg.Is<ToDoItem>(t =>
           t.ToDoItemId == id &&
           t.Name == todo.Name &&
           t.Description == todo.Description &&
           t.IsCompleted == todo.IsCompleted &&
           t.Category == todo.Category

       )).Returns(updated);

        // Act
        var result = await controller.UpdateById(id, todo);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);
        Assert.Equal(id, dto.ToDoItemId);
        Assert.Equal("koupit květiny", dto.Name);
        Assert.Equal("růže", dto.Description);
        Assert.True(dto.IsCompleted);
        Assert.Equal("svátek", dto.Category);

        await repository.Received(1).UpdateByIdAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange

        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 999;
        var todo = new ToDoItemUpdateRequestDto("tancovat", "s mikulášskou čepicí na hlavě na firemním večírku", false, "odvaha");

        repository.UpdateByIdAsync(Arg.Any<ToDoItem>()).Returns((ToDoItem?)null);

        // Act
        var result = await controller.UpdateById(id, todo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repository.Received(1).UpdateByIdAsync(Arg.Is<ToDoItem>(t => t.ToDoItemId == id));
    }
    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange

        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 42;
        var todo = new ToDoItemUpdateRequestDto("rum do perníčků", "vypít ještě v listopadu", false, "alkoholismus");

        repository.UpdateByIdAsync(Arg.Any<ToDoItem>()).Returns<Task>(_ => { throw new Exception("DB down"); });

        // Act
        var result = await controller.UpdateById(id, todo);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
        await repository.Received(1).UpdateByIdAsync(Arg.Is<ToDoItem>(t => t.ToDoItemId == id));
    }
}
