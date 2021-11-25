using System.Collections.Generic;

namespace WebbAppOOP.Data.DataAccess.Interfaces
{
    public interface IDataAccess<T> 
    {
        List<T> GetAll();
        T GetById(int id);
        T GetByKey(string key);
        void Update(T item);
        void Add(T item);
        void Remove(T item);
        int GetNewID();
    }
}
