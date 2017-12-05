using Air.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public class AirportRepository : IRepository<AirportModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public AirportRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public static AirportModel CreateAirportModel(SqlDataReader reader)
        {
            return new AirportModel
            {
                AirportID = Convert.ToInt32(reader["AirportID"]),
                AirportName = reader["AirportName"].ToString(),
                CityName = reader["CityName"].ToString(),
                CountryName = reader["CountryName"].ToString()
            };
        }

        public bool Create(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirportName", item.AirportName),
                new SqlParameter("@CityID", item.CityID),
            });
            return command.ExecuteNonQuery() == 1;
        }

        public bool Delete(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirportID", item.AirportID));

            return command.ExecuteNonQuery() == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IEnumerable<AirportModel> SelectList()
        {
            List<AirportModel> airport = new List<AirportModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    airport.Add(CreateAirportModel(reader));
                }
            }
            reader.Close();

            return airport;
        }

        public IEnumerable<AirportModel> SelectListByCity(int cityID)
        {
            List<AirportModel> airport = new List<AirportModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelectListByCity";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CityID", cityID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    airport.Add(CreateAirportModel(reader));
                }
            }
            reader.Close();

            return airport;
        }

        public AirportModel Select(int airportID)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirportID", airportID));

            SqlDataReader reader = command.ExecuteReader();

            return CreateAirportModel(reader);
        }

        public bool Update(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirportID", item.AirportID),
                new SqlParameter("@AirportName", item.AirportName),
                new SqlParameter("@CityID", item.CityID)
            });
            return command.ExecuteNonQuery() == 1;
        }
    }
}
