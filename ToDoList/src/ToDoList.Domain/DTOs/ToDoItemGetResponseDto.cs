namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemGetResponseDto(int ToDoItemId, string Name, string Description, bool IsCompleted)
{
    public static ToDoItemGetResponseDto From(ToDoItem t)
    {
        return new(t.ToDoItemId, t.Name, t.Description, t.IsCompleted);


    }
}
