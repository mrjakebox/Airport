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
                AirlinePhone = reader["AirlinePhone"].ToString(),
                AirlineAddress = reader["AirlineAddress"].ToString()
            };
        }

        public bool Create(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirlineName", item.AirlineName),
                new SqlParameter("@AirlinePhone", item.AirlinePhone),
                new SqlParameter("@AirlineAddress", item.AirlineAddress)
            });
            return command.ExecuteNonQuery() == 1;
        }

        public bool Delete(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirlineID", item.AirlineID));

            return command.ExecuteNonQuery() == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<IEnumerable<AirlineModel>> SelectListAsync()
        {
            List<AirlineModel> airlines = new List<AirlineModel>();

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

        public AirlineModel Select(AirlineModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@AirlineID", item.AirlineID));

            SqlDataReader reader = command.ExecuteReader();

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
                new SqlParameter("@AirlinePhone", item.AirlinePhone),
                new SqlParameter("@AirlineAddress", item.AirlineAddress)
            });
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }
    }
}
