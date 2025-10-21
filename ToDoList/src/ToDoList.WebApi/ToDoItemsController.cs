namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] // localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> Todos = new()
    {
        new ToDoItem(1, "garbage", "taking out the garbage", false),
        new ToDoItem(2, "windows", "cleaning the windows", false ),
        new ToDoItem(3, "shopping", "the new clothing is needed", false),
        new ToDoItem(4, "gift", "buy a gift for friend's nameday", true),
    };

    private static int todoId = 5;


    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) // použijeme DTO - Data transfer object
    {

        try
        {
            var todo = request.ToDomain(request.Name, request.Description, request.IsCompleted);
            todo.ToDoItemId = ++todoId;
            Todos.Add(todo);
            return CreatedAtAction(nameof(ReadById), new { id = todo.ToDoItemId }, todo);
            //StatusCode(StatusCodes.Status201Created);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read() // api/ToDoItems GET
    {
        try
        {

            if (Todos.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(Todos.Select(ToDoItemGetResponseDto.From));

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);

        }
    }
    [HttpGet("{toDoItemId:int}")]
    public ActionResult ReadById(int todoId) // api/ToDoItems/<id> GET
    {
        // return BadRequest();
        try
        {
            var rightTodo = Todos.Find(t => t.ToDoItemId == todoId);

            if (rightTodo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }


            return Ok(ToDoItemGetResponseDto.From(rightTodo));

        }
        catch (Exception ex)
        {
            // pětistovka
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        ;



    }
    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            int updatedIndex = Todos.FindIndex(t => t.ToDoItemId == toDoItemId);


            if (updatedIndex < 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var updatedTodo = Todos[toDoItemId];
            updatedTodo.Name = request.Name;
            updatedTodo.Description = request.Description;
            updatedTodo.IsCompleted = request.IsCompleted;

            return StatusCode(StatusCodes.Status204NoContent);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

    }
    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            var deadTodo = Todos.Find(t => t.ToDoItemId == toDoItemId);
            if (deadTodo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);

            }

            Todos.Remove(deadTodo);
            return StatusCode(StatusCodes.Status204NoContent);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    public void AddItemToStorage(ToDoItem item)
    {
        Todos.Add(item);
    }
}
