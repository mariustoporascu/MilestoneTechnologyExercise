using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryStorageLibrary
{
    public interface IMemoryStorage
    {
        void Add<T>(T item);
        void Remove<T>(T item);
        IEnumerable<T> GetAll<T>();
    }
}
