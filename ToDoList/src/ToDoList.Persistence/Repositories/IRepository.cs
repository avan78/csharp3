namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public void Read();

    public void ReadById(int id);

    public void UpdateById(int id);

    public void DeleteById(int id);
}
