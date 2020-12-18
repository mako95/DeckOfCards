using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.DataAccess.SQL
{
    public class SQLRepo<T> : IRepository<T> where T : BaseEntity
    {
        DataContext context;
        DbSet<T> dbSet;

        public SQLRepo(DataContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbSet.AsQueryable();
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string id)
        {
            var item = Find(id);

            if (context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);
        }

        public T Find(string id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T item)
        {
            dbSet.Add(item);
        }

        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
