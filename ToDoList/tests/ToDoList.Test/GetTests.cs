namespace ToDoList.Test;

using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Alena",
            Description = "koš",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Petr",
            Description = "odpadky",
            IsCompleted = true
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);



        // Act
        /*     var result = controller.Read();
             var value = result.GetValue<IEnumerable<ToDoItemGetResponseDto>>(); //přidat v extension

             // Assert
             Assert.NotNull(value);
             // Assert.Collection();

             var firstToDo = value.First();
             Assert.Equal(toDoItem1.ToDoItemId, firstToDo.ToDoItemId);
             // nebo napsat manuálně, což není závislé na změnách dat v aplikaci
             Assert.Equal(1, firstToDo.ToDoItemId);
             Assert.Equal(toDoItem1.Name, firstToDo.Name);
             Assert.Equal(toDoItem1.Description, firstToDo.Description);
             Assert.Equal(toDoItem1.IsCompleted, firstToDo.IsCompleted);

         }

         //   [Fact] namísto fact můžu použít theory a vložit inline data
         [Theory]
         [InlineData(1)]
         [InlineData(2)]
         public void Get_ItemById_ReturnsItemById(int id)
         {
             //    var result = Controller.ReadById(id);

             //assert
             var ok = Assert.IsType<OkObjectResult>(result.Result);
             var item = Assert.IsType<ToDoItemGetResponseDto>(ok.Value);
             //  Assert.Equal(id, item.id)
         }
         else {
             //něco, co má negativní výsledek.
             */
    }

}
