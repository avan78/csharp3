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
    public void Put_UpdateByIdWhenItemUpdated_ReturnsOk()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 7;
        var todo = new ToDoItemUpdateRequestDto("koupit květiny", "růže", true);

        var updated = new ToDoItem { ToDoItemId = id, Name = todo.Name, Description = todo.Description, IsCompleted = todo.IsCompleted };

        repository.UpdateById(Arg.Is<ToDoItem>(t =>
            t.ToDoItemId == id &&
            t.Name == todo.Name &&
            t.Description == todo.Description &&
            t.IsCompleted == todo.IsCompleted
        )).Returns(updated);

        // Act
        var result = controller.UpdateById(id, todo);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);
        Assert.Equal(id, dto.ToDoItemId);
        Assert.Equal("koupit květiny", dto.Name);
        Assert.Equal("růže", dto.Description);
        Assert.True(dto.IsCompleted);

        repository.Received(1).UpdateById(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange

        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 999;
        var todo = new ToDoItemUpdateRequestDto("tancovat", "s mikulášskou čepicí na hlavě na firemním večírku", false);

        repository.UpdateById(Arg.Any<ToDoItem>()).Returns((ToDoItem?)null);

        // Act
        var result = controller.UpdateById(id, todo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        repository.Received(1).UpdateById(Arg.Is<ToDoItem>(t => t.ToDoItemId == id));
    }
    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange

        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var id = 42;
        var todo = new ToDoItemUpdateRequestDto("rum do perníčků", "vypít ještě v listopadu", false);

        repository.UpdateById(Arg.Any<ToDoItem>()).Returns(_ => { throw new Exception("DB down"); });

        // Act
        var result = controller.UpdateById(id, todo);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
        repository.Received(1).UpdateById(Arg.Is<ToDoItem>(t => t.ToDoItemId == id));
    }
}
