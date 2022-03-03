namespace Timotheus.Utility
{
    internal interface IRepository<T>
    {
        public void Create(T obj);

        public T Retrieve(string id);

        public void Update(string id, T obj);

        public void Delete(T obj);
    }
}