using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        Task<IEnumerable<T>> SelectListAsync();
        bool Create(T item);
        Task<bool> UpdateAsync(T item);
        bool Delete(T item);
        T Select(T item);
        T CreateModel(SqlDataReader reader);
    }
}
