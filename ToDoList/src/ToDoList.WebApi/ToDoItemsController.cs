namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")] // localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> Todos = new()
    {
        // nepotřebujeme
        new ToDoItem(1, "garbage", "taking out the garbage", false),
        new ToDoItem(2, "windows", "cleaning the windows", false ),
        new ToDoItem(3, "shopping", "the new clothing is needed", false),
        new ToDoItem(4, "gift", "buy a gift for friend's nameday", true),
    };

    private readonly ToDoItemsContext context;
    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;

        var item = new ToDoItem { Name = "První úkol", Description = "První popis", IsCompleted = false };

        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    private static int todoId = 5;


    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create([FromBody] ToDoItemCreateRequestDto request) // použijeme DTO - Data transfer object //actionresult
    {

        try
        {
            var todo = request.ToDomain(request.Name, request.Description, request.IsCompleted);
            // todo.ToDoItemId = ++todoId;
            // Todos.Add(todo);

            context.ToDoItems.Add(todo);
            context.SaveChanges();
            return CreatedAtAction(nameof(ReadById), new { toDoItemId = todo.ToDoItemId }, todo);
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
        //    => Ok(context.ToDoItems.Select(MapResponse)); //?? v hodině

        try
        {
            var tasks = context.ToDoItems.Select(ToDoItemGetResponseDto.From);
            return Ok(tasks);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);

        }
    }
    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById([FromRoute(Name = "toDoItemId")] int todoId) // api/ToDoItems/<id> GET
    {
        // return BadRequest();
        try
        {
            var rightTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == todoId);

            if (rightTodo == null)
            {
                return NotFound();
            }


            return Ok(ToDoItemGetResponseDto.From(rightTodo));

        }
        catch (Exception ex)
        {
            // pětistovka
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }



    }
    [HttpPut("{toDoItemId:int}")]
    public ActionResult UpdateById([FromRoute] int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request) //i
    {
        try

        {
            var updatedTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItemId);


            if (updatedTodo is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            updatedTodo.Name = request.Name;
            updatedTodo.Description = request.Description;
            updatedTodo.IsCompleted = request.IsCompleted;
            context.SaveChanges();

            return Ok(updatedTodo);

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
            var deadTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItemId);
            if (deadTodo == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);

            }

            context.ToDoItems.Remove(deadTodo);
            context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    public void AddItemToStorage(ToDoItem item)
    {
        context.ToDoItems.Add(item);
    }
}
