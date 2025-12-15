using System;
namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItemView
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250)]
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public string? Category { get; set; }

};
