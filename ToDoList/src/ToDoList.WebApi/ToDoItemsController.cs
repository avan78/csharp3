namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using static ToDoList.Persistence.Repositories.IRepositoryAsync;

[Route("api/[controller]")] // localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{


    private readonly ToDoItemsContext context;
    private readonly IRepositoryAsync<ToDoItem> repository;
    public ToDoItemsController(ToDoItemsContext context, IRepositoryAsync<ToDoItem> repository)
    {
        this.context = context;
        this.repository = repository;
        var item = new ToDoItem { Name = "První úkol", Description = "První popis", IsCompleted = false, Category = "všeobecné" };

        // Asnotracking se používá jen u Read příkazů. Jestliže nic neměním,
        // je to pak úspornější na výpočet. Ale je to optional.
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItemGetResponseDto>> Create([FromBody] ToDoItemCreateRequestDto request) // použijeme DTO - Data transfer object //actionresult
    {

        try
        {
            var todo = request.ToDomain(request.Name, request.Description, request.IsCompleted, request.Category);

            await repository.CreateAsync(todo);

            return CreatedAtAction(nameof(ReadById), new { toDoItemId = todo.ToDoItemId }, todo);
            //StatusCode(StatusCodes.Status201Created);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> Read() // api/ToDoItems GET
    {
        //    => Ok(context.ToDoItems.Select(MapResponse)); //?? v hodině
        //   context.ToDoItems.Select(ToDoItemGetResponseDto.From);


        try
        {
            var tasks = await repository.ReadAsync();
            var dtos = tasks.Select(ToDoItemGetResponseDto.From).ToList();

            return Ok(dtos);

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);

        }
    }
    [HttpGet("{toDoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadById([FromRoute(Name = "toDoItemId")] int todoId) // api/ToDoItems/<id> GET
    {
        try
        {
            //  var rightTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == todoId);
            var rightTodo = await repository.ReadByIdAsync(todoId);
            if (rightTodo == null)
            {
                return NotFound();
            }


            return Ok(ToDoItemGetResponseDto.From(rightTodo));

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }



    }
    [HttpPut("{toDoItemId:int}")]
    public async Task<ActionResult> UpdateById([FromRoute] int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request) //i
    {
        try

        {
            /*  var updatedTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItemId);
              if (updatedTodo is null)
              {
                  return StatusCode(StatusCodes.Status404NotFound);
              }


              updatedTodo.Name = request.Name;
              updatedTodo.Description = request.Description;
              updatedTodo.IsCompleted = request.IsCompleted;
              context.SaveChanges();

              return Ok(updatedTodo);*/

            var updatedTodo = new ToDoItem
            {
                ToDoItemId = toDoItemId,
                Name = request.Name,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                Category = request.Category
            };

            var modified = await repository.UpdateByIdAsync(updatedTodo);
            if (modified is null)
            {
                return NotFound();
            }

            return Ok(ToDoItemGetResponseDto.From(modified));

        }

        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

    }
    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteById(int toDoItemId)
    {
        try
        {
            //  var deadTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItemId);

            /* var deadTodo = repository.DeleteById(toDoItemId);
             if (deadTodo == null)
             {
                 return StatusCode(StatusCodes.Status404NotFound);

             }*/

            //  context.ToDoItems.Remove(deadTodo);
            //  context.SaveChanges();
            bool ok = await repository.DeleteByIdAsync(toDoItemId);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();

        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    [NonAction]
    public void AddItemToStorage(ToDoItem item)
    {
        context.ToDoItems.Add(item);
    }
}
