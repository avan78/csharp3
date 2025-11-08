using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Persistence.Repositories
{
    public class ToDoItemsRepository : IRepository<ToDoItem>
    {
        private readonly ToDoItemsContext context;
        public ToDoItemsRepository(ToDoItemsContext context)

        {
            this.context = context;
        }
        public void Create(ToDoItem item)
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }

        public List<ToDoItem> Read()

          => [.. context.ToDoItems.AsNoTracking()];


        public ToDoItem? ReadById(int id)

          => context.ToDoItems.AsNoTracking().FirstOrDefault(t => t.ToDoItemId == id);


        public ToDoItem? UpdateById(ToDoItem item)
        {
            var updatedTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == item.ToDoItemId);
            if (updatedTodo is null)
            {
                return null;
            }

            updatedTodo.Name = item.Name;
            updatedTodo.Description = item.Description;
            updatedTodo.IsCompleted = item.IsCompleted;

            context.SaveChanges();
            return updatedTodo;

        }

        public bool DeleteById(int id)
        {
            var deadTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == id);
            if (deadTodo is null)
            {
                return false;
            }

            context.ToDoItems.Remove(deadTodo);
            context.SaveChanges();
            return true;
        }
    }
}
