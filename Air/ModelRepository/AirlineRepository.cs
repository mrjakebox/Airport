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
    class AirlineRepository : IRepository<AirlineModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public AirlineRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public static AirlineModel CreateAirlineModel(SqlDataReader reader)
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
                new SqlParameter("@AirlineID", item.AirlineID),
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

        public IEnumerable<AirlineModel> SelectList()
        {
            List<AirlineModel> airplane = new List<AirlineModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AirlineSelect";
            command.Transaction = _transaction;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    airplane.Add(CreateAirlineModel(reader));
                }
            }
            reader.Close();

            return airplane;
        }

        public bool Update(AirlineModel item)
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
            return command.ExecuteNonQuery() == 1;
        }
    }
}
