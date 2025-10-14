namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemUpdateRequestDto(/*int ToDoItemId,*/ string Name, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain(/*int toDoItemId,*/ string name, string description, bool isCompleted)
    {
        return new ToDoItem
        {
            //  ToDoItemId = toDoItemId,
            Name = name,
            Description = description,
            IsCompleted = isCompleted,
        };
    }
}
