namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public List<ToDoItem> Read();

    public T? ReadById(int id);

    public T? UpdateById(T item);

    public bool DeleteById(int id);
}
