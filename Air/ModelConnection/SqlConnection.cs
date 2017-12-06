using Air.ModelRepository;
using Air.Models;
using Air.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelConnection
{
    public class SqlConnection : IConnection
    {
        private static SqlConnection _instance;
        public DbConnection DbConnection { get; }
        public static SqlConnection Instance => _instance ?? (_instance = new SqlConnection());

        private SqlConnection()
        {
            SqlConnectionStringBuilder stringConnection = new SqlConnectionStringBuilder();
            stringConnection.DataSource = @"(localdb)\MSSQLLocalDB";
            stringConnection.InitialCatalog = "Airport";
            stringConnection.UserID = Settings.Default.Username;
            stringConnection.Password = Settings.Default.Password;

            DbConnection = new System.Data.SqlClient.SqlConnection(stringConnection.ToString());
        }

        public async Task OpenAsync()
        {
            await DbConnection.OpenAsync();
            await new SqlCommand("SET IMPLICIT_TRANSACTIONS ON", (System.Data.SqlClient.SqlConnection)DbConnection).ExecuteNonQueryAsync();
        }

        public void Close()
        {
            DbConnection.Close();
        }

        public IRepository<AirlineModel> Airlines(SqlTransaction transaction) => new AirlineRepository(transaction);
        public IRepository<AirportModel> Airports(SqlTransaction transaction) => new AirportRepository(transaction);
        public IRepository<CityModel> Cities(SqlTransaction transaction) => new CityRepository(transaction);
        public IRepository<CountryModel> Countries(SqlTransaction transaction) => new CountryRepository(transaction);
        public IRepository<FlightModel> Flights(SqlTransaction transaction) => new FlightRepository(transaction);
        public IRepository<PlaneModel> Planes(SqlTransaction transaction) => new PlaneRepository(transaction);
    }
}
