using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using NSubstitute;
using Microsoft.AspNetCore.Http;

public class CreateTestsUnit
{
    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var todo = new ToDoItemCreateRequestDto(
            Name: "naplánovat dovolenou",
            Description: "Francie",
            IsCompleted: true
        );


        repository.When(r => r.Create(Arg.Any<ToDoItem>()))
            .Do(ci =>
            {
                var arg = ci.Arg<ToDoItem>();
                arg.ToDoItemId = 123;
            });

        // Act
        var result = controller.Create(todo);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), created.ActionName);
        Assert.Equal(123, created.RouteValues!["toDoItemId"]);

        var value = Assert.IsType<ToDoItem>(created.Value);
        Assert.Equal(123, value.ToDoItemId);
        Assert.Equal("naplánovat dovolenou", value.Name);
        Assert.Equal("Francie", value.Description);
        Assert.True(value.IsCompleted);

        repository.Received(1).Create(Arg.Is<ToDoItem>(t =>
            t.Name == todo.Name &&
            t.Description == todo.Description &&
            t.IsCompleted == todo.IsCompleted
        ));
    }

    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(null, repository);

        var todo = new ToDoItemCreateRequestDto(
            Name: "namalovat obraz",
            Description: "zátiší s kopretinami",
            IsCompleted: true
        );

        repository.When(r => r.Create(Arg.Any<ToDoItem>()))
            .Do(_ => throw new Exception("DB down"));

        // Act
        var result = controller.Create(todo);

        // Assert
        var obj = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
        repository.Received(1).Create(Arg.Any<ToDoItem>());
    }
}
