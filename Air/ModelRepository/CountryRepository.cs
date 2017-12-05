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

        public bool Create(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountryCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@CountryID", item.CountryID),
                new SqlParameter("@CountryName", item.CountryName)
            });
            return command.ExecuteNonQuery() == 1;
        }

        public bool Delete(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountryDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", item.CountryID));

            return command.ExecuteNonQuery() == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public CountryModel Select(CountryModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountrySelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", item.CountryID));

            SqlDataReader reader = command.ExecuteReader();

            return CreateModel(reader);
        }

        public IEnumerable<CountryModel> SelectList()
        {
            List<CountryModel> countries = new List<CountryModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CountrySelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    countries.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return countries;
        }

        public bool Update(CountryModel item)
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
            return command.ExecuteNonQuery() == 1;
        }
    }
}
