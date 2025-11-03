using System;
using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;

namespace ToDoList.Test;

public class UpdateTests
{

    [Fact]
    public void Update_Item()
    {
        // Arrange
        var controller = new ToDoItemsController();

        var createTask = new ToDoItemCreateRequestDto("koš", "vynést odpadky", false);
        var createResult = controller.Create(createTask);


        var newTaskType = Assert.IsType<CreatedAtActionResult>(createResult);
        var newTask = Assert.IsType<ToDoList.Domain.Models.ToDoItem>(createResult.Value);
        var id = createResult.ToDoItemId;

        // Act
        var updateTask = new ToDoItemCreateRequestDto("květiny", "udělat výzdobu", true);
        var updateResult = controller.UpdateById(id, updateTask);

        var getUpdated = controller.ReadById(id);

        // Assert
        Assert.IsType<NoContentResult>(updateResult);
        var correctUpdated = Assert.IsType<ToDoItem>(getUpdated.Result);



        var updated = Assert.IsType<ToDoItemGetResponseDto>(getUpdated.Value);
        Assert.Equal("květiny", correctUpdated.Name);
        Assert.Equal("udělat výzdobu", correctUpdated.Description);
        Assert.True(correctUpdated.IsCompleted);

        Assert.NotEqual("koš", correctUpdated.Name);
        Assert.False(correctUpdated.IsCompleted);



    }
}
