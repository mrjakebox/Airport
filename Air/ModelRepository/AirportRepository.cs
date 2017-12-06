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

        public AirportModel CreateModel(SqlDataReader reader)
        {
            return new AirportModel
            {
                AirportID = Convert.ToInt32(reader["AirportID"]),
                AirportName = reader["AirportName"].ToString(),
                CityName = reader["CityName"].ToString(),
                CountryName = reader["CountryName"].ToString()
            };
        }

        public async Task<bool> CreateAsync(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirportName", item.AirportName),
                new SqlParameter("@CityID", item.CityID)
            });
            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<bool> DeleteAsync(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirportID", item.AirportID));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<IEnumerable<AirportModel>> SelectListAsync()
        {
            List<AirportModel> airports = new List<AirportModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    airports.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return airports;
        }

        public IEnumerable<AirportModel> SelectListByCity(AirportModel item)
        {
            List<AirportModel> airports = new List<AirportModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelectListByCity";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CityID", item.CityID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    airports.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return airports;
        }

        public async Task<AirportModel> SelectAsync(AirportModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirportSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirportID", item.AirportID));

            SqlDataReader reader = await command.ExecuteReaderAsync();

            return CreateModel(reader);
        }

        public async Task<bool> UpdateAsync(AirportModel item)
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
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }
    }
}
