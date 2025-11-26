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


namespace ToDoList.Test.UnitTests
{
    public class DeleteTestUnit
    {
        [Fact]
        public void DeleteReturnsNotFoundWhenMissing()
        {
            // Arrange
            //   var (controller, repo) = CreateController(); //upravit podle sebe
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repositoryMock);

            repositoryMock.ReadById(9).Returns((ToDoItem?)null);

            // Act
            var result = controller.DeleteById(9);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteRemovesEntityAndReturnsNoContent()
        {
            // Arrange
            //  var (controller, repo) = CreateController();
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            repositoryMock.DeleteById(9).Returns(true);
            var controller = new ToDoItemsController(context: null, repositoryMock);
            //   ToDoItem entity = new() { Name = "test", Description = "Something", IsCompleted = true };
            //  repositoryMock.ReadById(9).Returns((ToDoItem?)null);

            // Act
            var result = controller.DeleteById(9); //zjistit id

            // Assert
            Assert.IsType<NoContentResult>(result);

        }
        /*   [Fact(Skip = "We don't use ReadById function in controller.")]
         public void DeleteWhenReadThrows_ReturnInternalServerError()
          {
              // Arrange
              //  var (controller, repo) = CreateController();
              var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
              var controller = new ToDoItemsController(context: null, repositoryMock);
              repositoryMock.When(r => r.ReadById(42))
              .Do(_ => throw new Exception("DB down"));

              // Act
              var result = controller.DeleteById(42); //zjistit id

              // Assert
              var obj = Assert.IsType<ObjectResult>(result);
              Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);

          } */
        [Fact]
        public void Delete_WhenDeleteThrows_ReturnsInternalServerError()
        {
            // Arrange
            // var (controller, repo) = CreateController();
            //read má vrátit vše správně, má to spadnout až na delete
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            // var controller = new ToDoItemsController(context: null, repositoryMock);
            // repositoryMock.When(r => r.DeleteById(42))
            // .Do(_ => throw new Exception("DB down"));
            repositoryMock.DeleteById(42).Returns(_ => { throw new Exception("DB down"); });
            var controller = new ToDoItemsController(context: null, repositoryMock);

            // Act
            var result = controller.DeleteById(42); //zjistit id

            // Assert
            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
            //  repositoryMock.Received(1).ReadById(42);
            repositoryMock.Received(1).DeleteById(42);

        }
    }


}

