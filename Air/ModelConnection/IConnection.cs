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
        Task OpenAsync();
        void Close();
        IRepository<AirlineModel> Airlines(SqlTransaction transaction);
        IRepository<AirportModel> Airports(SqlTransaction transaction);
        IRepository<CityModel> Cities(SqlTransaction transaction);
        IRepository<CountryModel> Countries(SqlTransaction transaction);
        IRepository<FlightModel> Flights(SqlTransaction transaction);
        IRepository<PlaneModel> Planes(SqlTransaction transaction);
    }
}
