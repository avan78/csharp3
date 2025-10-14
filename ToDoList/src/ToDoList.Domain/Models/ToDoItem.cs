namespace ToDoList.Domain.Models;

public class ToDoItem
{

    public ToDoItem() { }

    public ToDoItem(int toDoItemId, string name, string description, bool isCompleted)
    {
        ToDoItemId = toDoItemId;
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
    }

    public int ToDoItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public bool IsCompleted { get; set; }


}
