namespace DataAccess.Interface
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(); //read

        T GetById(int id); //read

        void Add(T entity); //create

        void Update(T entity); //update

        void Delete(T entity); //delete
    }
}
