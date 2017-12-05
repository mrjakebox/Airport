using Air.Models;
using Air.ModelRepository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Air.ModelConnection
{
    public interface IConnection
    {
        DbConnection DbConnection { get; }
        void Open();
        void Close();
        IRepository<AirlineModel> Airlines(SqlTransaction transaction);
        IRepository<AirportModel> Airports(SqlTransaction transaction);

    }
}
