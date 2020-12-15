using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Find(string id);
        void Insert(T item);
        void Update(T item);
        void Delete(string id);
        IQueryable<T> Collection();
        void Commit();
    }
}
