using Air.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace Air.ModelRepository
{
    public class FlightRepository : IRepository<FlightModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public FlightRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public FlightModel CreateModel(SqlDataReader reader)
        {
            return new FlightModel
            {
                FlightID = Convert.ToInt32(reader["FlightID"]),
                AirportID = Convert.ToInt32(reader["AirportID"]),
                PlaneID = Convert.ToInt32(reader["PlaneID"]),
                AirlineID = Convert.ToInt32(reader["AirlineID"]),
                AirlineName = reader["AirlineName"].ToString(),
                AirplaneModel = reader["AirplaneModel"].ToString(),
                AirportName = reader["AirportName"].ToString(),
                CityName = reader["CityName"].ToString(),
                CountryName = reader["CountryName"].ToString(),
                FlightType = Convert.ToBoolean(reader["FlightType"]),
                DateTimeStart = Convert.ToDateTime(reader["DateTimeStart"]),
                Duration = reader["Duration"].ToString(),
                DateTimeArrival = Convert.ToDateTime(reader["DateTimeArrival"]),
                NumOfFlights = Convert.ToInt32(reader["NumOfFlights"]),
                Periodicity = Convert.ToDateTime(reader["Periodicity"]),
                PriceEconom = Convert.ToDecimal(reader["PriceEconom"]),
                PriceBusiness = Convert.ToDecimal(reader["PriceBusiness"]),
                PriceFirst = Convert.ToDecimal(reader["PriceFirst"]),
                DateTimeStartGMT = Convert.ToDateTime(reader["DateTimeStartGMT"]),
                DateTimeArrivalGMT = Convert.ToDateTime(reader["DateTimeArrivalGMT"]),
                Status = reader["Status"].ToString()
            };
        }

        public async Task<bool> CreateAsync(FlightModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[] 
            {
                new SqlParameter("@AirportID", item.AirportID),
                new SqlParameter("@PlaneID", item.PlaneID),
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@FlightType", item.FlightType),
                new SqlParameter("@DateTimeStart", item.DateTimeStart),
                new SqlParameter("@Duration", item.Duration),
                new SqlParameter("@NumOfFlights", item.NumOfFlights),
                new SqlParameter("@Periodicity", item.Periodicity),
                new SqlParameter("@PriceEconom", item.PriceEconom),
                new SqlParameter("@PriceBusiness", item.PriceBusiness),
                new SqlParameter("@PriceFirst", item.PriceFirst)
            });

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<bool> DeleteAsync(FlightModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@FlightID", item.FlightID));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<FlightModel> SelectAsync(FlightModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelect";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@FlightID", item.FlightID),
                new SqlParameter("@AirportID", item.AirportID),
                new SqlParameter("@PlaneID", item.PlaneID),
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@FlightType", item.FlightType),
                new SqlParameter("@DateTimeStart", item.DateTimeStart),
                new SqlParameter("@Duration", item.Duration),
                new SqlParameter("@DateTimeArrival", item.DateTimeArrival),
                new SqlParameter("@DateTimeStartGMT", item.DateTimeStartGMT),
                new SqlParameter("@DateTimeArrivalGMT", item.DateTimeArrivalGMT),
                new SqlParameter("@PriceEconom", item.PriceEconom),
                new SqlParameter("@PriceBusiness", item.PriceBusiness),
                new SqlParameter("@PriceFirst", item.PriceFirst),
                new SqlParameter("@Status", item.Status)
            });

            SqlDataReader reader = await command.ExecuteReaderAsync();

            return CreateModel(reader);
        }

        public async Task<ObservableCollection<FlightModel>> SelectListAsync()
        {
            ObservableCollection<FlightModel> flights = new ObservableCollection<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public ObservableCollection<FlightModel> SelectList(DateTime endDate)
        {
            ObservableCollection<FlightModel> flights = new ObservableCollection<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));
            command.Parameters.Add(new SqlParameter("@BeginDate", DateTime.Today));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(DateTime beginDate, DateTime endDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@BeginDate", beginDate));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, DateTime endDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));
            command.Parameters.Add(new SqlParameter("@BeginDate", DateTime.Today));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, DateTime endDate, DateTime beginDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@BeginDate", beginDate));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID, DateTime endDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));
            command.Parameters.Add(new SqlParameter("@BeginDate", DateTime.Today));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID, DateTime endDate, DateTime beginDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));
            command.Parameters.Add(new SqlParameter("@BeginDate", beginDate));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID, int airportID)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));
            command.Parameters.Add(new SqlParameter("@AirportID", airportID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID, int airportID, DateTime endDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));
            command.Parameters.Add(new SqlParameter("@AirportID", airportID));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));
            command.Parameters.Add(new SqlParameter("@BeginDate", DateTime.Today));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public IEnumerable<FlightModel> SelectList(int countryID, int cityID, int airportID, DateTime endDate, DateTime beginDate)
        {
            List<FlightModel> flights = new List<FlightModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightSelectList";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", countryID));
            command.Parameters.Add(new SqlParameter("@CityID", cityID));
            command.Parameters.Add(new SqlParameter("@AirportID", airportID));
            command.Parameters.Add(new SqlParameter("@BeginDate", beginDate));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flights.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return flights;
        }

        public async Task<bool> UpdateAsync(FlightModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "FlightUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
           {
                new SqlParameter("@AirportID", item.AirportID),
                new SqlParameter("@PlaneID", item.PlaneID),
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@FlightType", item.FlightType),
                new SqlParameter("@DateTimeStart", item.DateTimeStart),
                new SqlParameter("@Duration", item.Duration),
                new SqlParameter("@NumOfFlights", item.NumOfFlights),
                new SqlParameter("@Periodicity", item.Periodicity),
                new SqlParameter("@PriceEconom", item.PriceEconom),
                new SqlParameter("@PriceBusiness", item.PriceBusiness),
                new SqlParameter("@PriceFirst", item.PriceFirst)
            });
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }

        public Task<ObservableCollection<FlightModel>> SelectListFormatAsync()
        {
            throw new NotImplementedException();
        }
    }
}
