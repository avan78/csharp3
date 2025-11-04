using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;

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

        public void Read()
        {
            context.ToDoItems.Select(ToDoItemGetResponseDto.From);
        }

        public void ReadById(int id)
        {
            context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == id);
        }

        public void UpdateById(int id)
        {
            var updatedTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == id);
            /*updatedTodo.Name = request.Name;
            updatedTodo.Description = request.Description;
            updatedTodo.IsCompleted = request.IsCompleted; */

        }

        public void DeleteById(int id)
        {
            var deadTodo = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == id);

            context.ToDoItems.Remove(deadTodo);
            context.SaveChanges();
        }
    }
}
