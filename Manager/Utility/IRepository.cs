using System.Collections.Generic;

namespace Timotheus.Utility
{
    internal interface IRepository<T>
    {
        public void Create(T obj);

        public T Retrieve(string id);

        public List<T> RetrieveAll();

        public void Update(T obj);

        public void Delete(T obj);
    }
}