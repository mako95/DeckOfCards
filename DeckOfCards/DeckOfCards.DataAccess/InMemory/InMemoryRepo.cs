using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.DataAccess.InMemory
{
    public class InMemoryRepo<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepo()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public T Find(string id)
        {
            T item = items.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                throw new Exception(className + " not found.");
            }

            return item;
        }

        public void Delete(string id)
        {
            T itemToDelete = Find(id);

            if (itemToDelete == null)
            {
                throw new Exception(className + " not found.");
            }

            items.Remove(itemToDelete);
        }

        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = Find(item.Id);

            if (itemToUpdate == null)
            {
                throw new Exception(className + " not found.");
            }

            itemToUpdate = item;
        }
    }
}
