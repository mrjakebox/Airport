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
    public class PlaneRepository : IRepository<PlaneModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public PlaneRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public PlaneModel CreateModel(SqlDataReader reader)
        {
            return new PlaneModel
            {
                PlaneID = Convert.ToInt32(reader["PlaneID"]),
                AirlineID = Convert.ToInt32(reader["AirlineID"]),
                AirplaneModel = reader["AirplaneModel"].ToString(),
                OnboardNumber = reader["OnboardNumber"].ToString(),
                AirlineName = reader["AirlineName"].ToString()
            };
        }

        public async Task<bool> CreateAsync(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@AirplaneModel", item.AirplaneModel),
                new SqlParameter("@OnboardNumber", item.OnboardNumber)
            });
            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<bool> DeleteAsync(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@PlaneID", item.PlaneID));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<PlaneModel> SelectAsync(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@PlaneID", item.PlaneID));

            SqlDataReader reader = await command.ExecuteReaderAsync();

            return CreateModel(reader);
        }

        public async Task<ObservableCollection<PlaneModel>> SelectListAsync()
        {
            ObservableCollection<PlaneModel> planes = new ObservableCollection<PlaneModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    planes.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return planes;
        }

        public async Task<ObservableCollection<PlaneModel>> SelectListFormatAsync()
        {
            ObservableCollection<PlaneModel> planes = new ObservableCollection<PlaneModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneListSelectFormat";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    planes.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return planes;
        }

        public async Task<bool> UpdateAsync(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@PlaneID", item.PlaneID),
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@AirplaneModel", item.AirplaneModel),
                new SqlParameter("@OnboardNumber", item.OnboardNumber)
            });
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }
    }
}
