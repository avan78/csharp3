using System;
using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test;

public class UpdateTests
{

    [Fact]
    public void Update_Item()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.EnsureDeleted();
        context.Database.Migrate();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(context: context, repository: repository);

        var createTask = new ToDoItemCreateRequestDto("koš", "vynést odpadky", false);
        var createResult = controller.Create(createTask);


        var newTaskType = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        var newTask = Assert.IsType<ToDoList.Domain.Models.ToDoItem>(createResult.Value);
        int id = newTask.ToDoItemId;

        // Act
        var updateTask = new ToDoItemUpdateRequestDto("květiny", "udělat výzdobu", true);
        var updateResult = controller.UpdateById(id, updateTask);


        // Assert
        // Assert.IsType<NoContentResult>(updateResult);

        //var getUpdated = controller.ReadById(id);
        var okUpdate = Assert.IsType<OkObjectResult>(updateResult);
        var correctUpdated = Assert.IsType<ToDoItemGetResponseDto>(okUpdate.Value);



        // var updated = Assert.IsType<ToDoItemGetResponseDto>(getUpdated.Value);
        Assert.Equal("květiny", correctUpdated.Name);
        Assert.Equal("udělat výzdobu", correctUpdated.Description);
        Assert.True(correctUpdated.IsCompleted);

        Assert.NotEqual("koš", correctUpdated.Name);
        Assert.False(correctUpdated.IsCompleted);

        context.Database.EnsureDeleted();




    }
}
