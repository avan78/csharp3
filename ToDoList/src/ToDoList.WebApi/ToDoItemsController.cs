namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] // localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> todos = new();
    private static int id = 1;

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) // použijeme DTO - Data transfer object
    {

        try
        {
            var todo = request.ToDomain(request.Name, request.Description, request.IsCompleted);
            todo.ToDoItemId = ++id;
            todos.Add(todo);
            return CreatedAtAction(nameof(), new { id = todo.ToDoItemId }, todo);
            //StatusCode(StatusCodes.Status201Created);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet]
    public IActionResult Read() // api/ToDoItems GET
    {
        try
        {

            if (todos.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(todos);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);

        }
    }
    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) // api/ToDoItems/<id> GET
    {
        try
        {
            var rightTodo = todos.Find(t => t.ToDoItemId == toDoItemId);

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
             var updatedTodo = todos.FindIndex(t => t.ToDoItemId == toDoItemId);

            if (updatedTodo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }



            return Ok(ToDoItemGetResponseDto.From(updatedTodo));

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
            var deadTodo = todos.Find(t => t.ToDoItemId == toDoItemId);
            if (deadTodo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);

            }

            deadTodo
            return StatusCode(StatusCodes.Status204NoContent);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
