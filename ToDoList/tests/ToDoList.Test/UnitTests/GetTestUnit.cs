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
    public class GetTestUnit
    {
        [Fact]
        public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repositoryMock);
            var someItem = new ToDoItem { Name = "testname", Description = "testDesc", IsCompleted = false };
            repositoryMock.Read().Returns([someItem]);

            // alternativa
            // var someItemList = new List<ToDoItem>();
            // repositoryMock.Read().Returns(someItemList);

            //Act
            var result = controller.Read();
            var resultResult = result.Result;
            //Assert
            Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
            repositoryMock.Received(1).Read(); //napiš počet, kolik se má vrátit
        }

        [Fact]
        public void Get_ReadWhenSomeItemAvailable_ReturnsOkWithEmptyList()
        {
            // Arrange
            var repository = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repository);

            repository.Read().Returns(new List<ToDoItem>());
            // Act
            var result = controller.Read();
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(ok.Value);
            Assert.Empty(dtos);
            repository.Received(1).Read();

        }
        [Fact]
        public void Get_ReadUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repository = Substitute.For<IRepository<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repository);

            repository.Read().Returns(_ => { throw new Exception("DB down"); });
            // Act
            var result = controller.Read();
            // Assert
            var obj = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
            repository.Received(1).Read();

        }
    }
}
