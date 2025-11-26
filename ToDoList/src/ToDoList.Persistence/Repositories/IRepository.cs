namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public List<ToDoItem> Read(); //IEnumerable<T>

    public T? ReadById(int id);

    public T? UpdateById(T item); //void

    public bool DeleteById(int id); //void
}

public interface IRepositoryAsync<T>
where T : class
{
    public Task CreateAsync(T item);

    public Task<List<ToDoItem>> ReadAsync(); //IEnumerable<T>

    public Task<T?> ReadByIdAsync(int id);

    public Task<T?> UpdateByIdAsync(T item); //void

    public Task<bool> DeleteByIdAsync(int id); //void
}
