namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain(string name, string description, bool isCompleted)
    {
        return new ToDoItem
        {
            Name = name,
            Description = description,
            IsCompleted = isCompleted
        }
        ;
    }
}
