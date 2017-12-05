using Air.Models;
using Air.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelConnection
{
    public interface IConnection
    {
        DbConnection DbConnection { get; }
        void Open();
        void Close();
        IRepository<AirlineModel> Airlines();
    }
}
