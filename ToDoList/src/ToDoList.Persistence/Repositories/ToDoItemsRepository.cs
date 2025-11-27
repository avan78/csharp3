using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Persistence.Repositories
{
    public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
    {
        private readonly ToDoItemsContext context;
        public ToDoItemsRepository(ToDoItemsContext context)

        {
            this.context = context;
        }
        public async Task CreateAsync(ToDoItem item)
        {
            await context.ToDoItems.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task<List<ToDoItem>> ReadAsync()
          //??
          => [.. context.ToDoItems.AsNoTracking()];


        public async Task<ToDoItem?> ReadByIdAsync(int id)

          => await context.ToDoItems.AsNoTracking().FirstOrDefaultAsync(t => t.ToDoItemId == id);


        public async Task<ToDoItem?> UpdateByIdAsync(ToDoItem item)
        {
            var updatedTodo = await context.ToDoItems.FirstOrDefaultAsync(t => t.ToDoItemId == item.ToDoItemId);
            if (updatedTodo is null)
            {
                return null;
            }
            // alternativa: použití find a pak context.Entry(foundItem).CurrentValues.SetValues(item)
            updatedTodo.Name = item.Name;
            updatedTodo.Description = item.Description;
            updatedTodo.IsCompleted = item.IsCompleted;

            await context.SaveChangesAsync();
            return updatedTodo;

        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var deadTodo = await context.ToDoItems.FirstOrDefaultAsync(t => t.ToDoItemId == id);
            if (deadTodo is null)
            {
                return false;
            }

            context.ToDoItems.Remove(deadTodo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
