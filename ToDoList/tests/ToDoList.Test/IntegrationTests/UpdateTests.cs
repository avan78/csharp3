using System;
using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Test;

public class UpdateTests
{

    [Fact]
    public void Update_Item()
    {
        // Arrange
        var connectionString = "Data Source=/../data/localDbTestDb.db";
        var context = new ToDoItemsContext(connectionString);
        context.Database.Migrate();

        try
        {
            var controller = new ToDoItemsController(context: context, repository: null);

            var createTask = new ToDoItemCreateRequestDto("koš", "vynést odpadky", false);
            var createResult = controller.Create(createTask);


            var newTaskType = Assert.IsType<CreatedAtActionResult>(createResult.Result);
            var newTask = Assert.IsType<ToDoList.Domain.Models.ToDoItem>(createResult.Value);
            var id = newTask.ToDoItemId;

            // Act
            var updateTask = new ToDoItemUpdateRequestDto("květiny", "udělat výzdobu", true);
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
        finally
        {
            context.Database.EnsureDeleted();
        }



    }
}
