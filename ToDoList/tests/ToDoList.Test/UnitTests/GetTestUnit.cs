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
        public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
        {
            // Arrange
            var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repositoryMock);
            var someItem = new ToDoItem { Name = "testname", Description = "testDesc", IsCompleted = false, Category = null };
            repositoryMock.ReadAsync().Returns([someItem]);

            // alternativa
            // var someItemList = new List<ToDoItem>();
            // repositoryMock.Read().Returns(someItemList);

            //Act
            var result = await controller.Read();
            var resultResult = result.Result;
            //Assert
            Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
            await repositoryMock.Received(1).ReadAsync(); //napiš počet, kolik se má vrátit
        }

        [Fact]
        public async Task Get_ReadWhenSomeItemAvailable_ReturnsOkWithEmptyList()
        {
            // Arrange
            var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repository);

            repository.ReadAsync().Returns(new List<ToDoItem>());
            // Act
            var result = await controller.Read();
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(ok.Value);
            Assert.Empty(dtos);
            await repository.Received(1).ReadAsync();

        }
        [Fact]
        public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
        {
            // Arrange
            var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
            var controller = new ToDoItemsController(context: null, repository);

            repository.ReadAsync().Returns<Task>(_ => { throw new Exception("DB down"); });
            // Act
            var result = await controller.Read();
            // Assert
            var obj = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
            await repository.Received(1).ReadAsync();

        }
    }
}
