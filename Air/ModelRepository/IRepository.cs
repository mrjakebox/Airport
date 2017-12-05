using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> SelectList();
        bool Create(T item);
        bool Update(T item);
        bool Delete(T item);
        T Select(int ID);
    }
}
