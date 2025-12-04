namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted, string? Category)
{
    public ToDoItem ToDomain(string name, string description, bool isCompleted, string? category)
    {
        return new ToDoItem
        {
            Name = name,
            Description = description,
            IsCompleted = isCompleted,
            Category = category
        };
    }
}
