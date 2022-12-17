using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryStorageLibrary
{
    public class InMemoryStorage : IMemoryStorage
    {
        private Dictionary<string, List<object>> storage = new Dictionary<string, List<object>>();
        public void Add<T>(T item)
        {
            var key = typeof(T).Name;
            if (!storage.ContainsKey(key))
            {
                storage.Add(key, new List<object>());
            }
            if (storage[key].FirstOrDefault(obj=> item.Equals(obj)) == null)
                storage[key].Add(item);
        }
        public void Remove<T>(T item)
        {
            var key = typeof(T).Name;
            if (storage.ContainsKey(key))
            {
                storage[key].Remove(item);
                if (storage[key].Count == 0)
                {
                    storage.Remove(key);
                }
            }
        }
        public IEnumerable<T> GetAll<T>()
        {
            var type = typeof(T).Name;
            if (storage.ContainsKey(type))
            {
                return storage[type].Cast<T>();
            }
            return new List<T>();
        }
    }

}
