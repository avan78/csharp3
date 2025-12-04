namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public interface IRepository<T>
where T : class
{
    public void Create(T item);

    public List<T> Read(); //IEnumerable<T>

    public T? ReadById(int id);

    public T? UpdateById(T item); //void

    public bool DeleteById(int id); //void

}

