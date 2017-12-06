using Air.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Air.ModelRepository
{
    public class CountryRepository : IRepository<CountryModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public CountryRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public CountryModel CreateModel(SqlDataReader reader)
        {
            return new CountryModel
            {
                CountryID = Convert.ToInt32(reader["CountryID"]),
                CountryName = reader["CountryName"].ToString()
            };
        }

        public async Task<bool> CreateAsync(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountryCreate";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryName", item.CountryName));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public async Task<bool> DeleteAsync(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountryDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", item.CountryID));

            return await Task.Run(() => command.ExecuteNonQueryAsync()) == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<CountryModel> SelectAsync(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountrySelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", item.CountryID));

            SqlDataReader reader = await command.ExecuteReaderAsync();

            return CreateModel(reader);
        }

        public async Task<IEnumerable<CountryModel>> SelectListAsync()
        {
            List<CountryModel> countries = new List<CountryModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountrySelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    countries.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return countries;
        }

        public async Task<bool> UpdateAsync(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountryUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@CountryID", item.CountryID),
                new SqlParameter("@CountryName", item.CountryName)
            });
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }
    }
}
