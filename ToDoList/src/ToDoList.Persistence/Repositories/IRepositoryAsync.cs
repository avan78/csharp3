using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories
{

    public interface IRepositoryAsync<T>
where T : class
    {
        public Task CreateAsync(T item);

        public Task<List<T>> ReadAsync();

        public Task<T?> ReadByIdAsync(int id);

        public Task<T?> UpdateByIdAsync(T item);

        public Task<bool> DeleteByIdAsync(int id);
    }


}
