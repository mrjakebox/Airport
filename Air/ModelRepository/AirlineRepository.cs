using Air.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public class AirlineRepository : IRepository<AirlineModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public AirlineRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public AirlineModel CreateModel(SqlDataReader reader)
        {
            return new AirlineModel
            {
                AirlineID = Convert.ToInt32(reader["AirlineID"]),
                AirlineName = reader["AirlineName"].ToString(),
                Phone = reader["Phone"].ToString(),
                Address = reader["Address"].ToString()
            };
        }

        public async Task<bool> CreateAsync(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirlineName", item.AirlineName),
                new SqlParameter("@Phone", item.Phone),
                new SqlParameter("@Address", item.Address)
            });
            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<bool> DeleteAsync(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirlineID", item.AirlineID));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<ObservableCollection<AirlineModel>> SelectListAsync()
        {
            ObservableCollection<AirlineModel> airlines = new ObservableCollection<AirlineModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    airlines.Add(CreateModel(reader));
                }
            }
            reader.Close();
            
            return airlines;
        }

        public async Task<AirlineModel> SelectAsync(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirlineID", item.AirlineID));

            SqlDataReader reader = await command.ExecuteReaderAsync();

            return CreateModel(reader);
        }

        public async Task<bool> UpdateAsync(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@AirlineName", item.AirlineName),
                new SqlParameter("@Phone", item.Phone),
                new SqlParameter("@Address", item.Address)
            });
            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<ObservableCollection<AirlineModel>> SelectListFormatAsync()
        {
            ObservableCollection<AirlineModel> airlines = new ObservableCollection<AirlineModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineListSelectFormat";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    airlines.Add(new AirlineModel
                    {
                        AirlineID = Convert.ToInt32(reader["AirlineID"]),
                        AirlineName = reader["AirlineName"].ToString()
                    });
                }
            }
            reader.Close();

            return airlines;
        }
    }
}