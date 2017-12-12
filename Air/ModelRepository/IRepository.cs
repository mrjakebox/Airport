using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        Task<ObservableCollection<T>> SelectListAsync();
        Task<ObservableCollection<T>> SelectListFormatAsync();
        Task<bool> CreateAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);
        Task<T> SelectAsync(T item);
        T CreateModel(SqlDataReader reader);
    }
}
