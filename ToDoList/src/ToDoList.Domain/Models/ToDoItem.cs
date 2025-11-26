namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

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

    [Key]
    public int ToDoItemId { get; set; } // EF Core looks for <id> or <nameId>
    [Length(1, 50)]
    public string Name { get; set; }
    [StringLength(250)]
    public string Description { get; set; }

    public bool IsCompleted { get; set; }


}
